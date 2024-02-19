using Godot;
using System;

public class Player : Character
{
	// The downward acceleration when in the air, in meters per second squared.
	[Export]
	public int FallAcceleration = 75;

	[Export]
	public int JumpImpulse = 20;

	[Export]
	public int BounceImpulse = 16;

	[Export]
	int RunSpeed = 20;

	[Signal]
	public delegate void Hit();

	Stamina_Bar Stamina_bar = null;

	HP_Bar hp_bar = null;

	DialoguePanel DiagPan;

	Character_Animations anim;

	MoveLocation moveloc;


	public void Teleport(Vector3 pos)
	{
		GlobalTranslation = pos;
		loctomove = pos;
		NavAgent.SetTargetLocation(loctomove);
		CameraMovePivot.GetInstance().GlobalTranslation = pos;
	}

	public override void _Ready()
	{
		base._Ready();
		//Input.MouseMode = Input.MouseModeEnum.Visible;
		Control plUI = GetNode<Control>("PlayerUI");
		hp_bar = plUI.GetNode<HP_Bar>("HP_Bar");
		hp_bar.MaxValue = m_HP;
		Stamina_bar = plUI.GetNode<Stamina_Bar>("Stamina_Bar");
		Stamina_bar.MaxValue = m_Stamina;
		GetNode<AudioStreamPlayer3D>("WalkingSound").Play();
		GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused = true;
		var panels = GetTree().GetNodesInGroup("DialoguePanel");
		DiagPan = (DialoguePanel)panels[0];
		GetNode<AudioStreamPlayer3D>("TiredSound").Play();
		GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = true;
		anim = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Character_Animations>("AnimationPlayer");
		moveloc = GetNode<MoveLocation>("MoveLoc");

		NavAgent = GetNode<NavigationAgent>("NavigationAgent");

		myrid  = NavigationServer.AgentCreate();
        RID default_3d_map_rid  = GetWorld().NavigationMap;

        NavigationServer.AgentSetMap(myrid, default_3d_map_rid);
        NavigationServer.AgentSetRadius(myrid, 0.5f);
	}
	public override void _PhysicsProcess(float delta)
	{
        if (Input.IsActionPressed("Move"))
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 2000;
			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
			//if ray finds nothiong return
			if (rayar.Count == 0)
				return;
			loctomove = (Vector3)rayar["position"];
			//NavAgent.SetTargetLocation(loctomove);
			moveloc.Show();
		}
		moveloc.GlobalTranslation = loctomove;
		//Vector3 nextloc = NavAgent.GetNextLocation();
		var spd = Speed;
		var direction = loctomove - GlobalTransform.origin;
		Vector2 loc = new Vector2(loctomove.x, loctomove.z);
		
		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTransform.origin.x, GlobalTransform.origin.z));
		double stam = m_Stamina;
		

		if (Input.IsActionPressed("Run") && m_Stamina > 10 && direction != Vector3.Zero)
		{
			spd = RunSpeed;
			m_Stamina = m_Stamina - m_RunCost;
		}
		else if (!Input.IsActionPressed("Run") && m_Stamina < startingstaming)
		{
			m_Stamina = m_Stamina + m_StaminaRegen;
				
			if (direction == Vector3.Zero )
				m_Stamina = m_Stamina + m_StaminaRegen * 2;

		}
		if (dist < 1)
		{
			if (!GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused)
				GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused = true;
			anim.PlayAnimation(E_Animations.Idle);
			moveloc.Hide();	
		}
		else
		{
			direction = direction.Normalized();
			Vector3 lookloc = new Vector3(direction.x, 0, direction.z);
			GetNode<Spatial>("Pivot").LookAt(Translation - lookloc, Vector3.Up);
			if (GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused)
			{
				GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused = false;
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 0.7f;
			}
			if (Input.IsActionPressed("Run"))
			{
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 1f;
				//GetNode<AudioStreamPlayer3D>("WalkingSound").db = 5f;
			}
			anim.PlayAnimation(E_Animations.Walk);
		}
		Stamina_bar.Value = m_Stamina;

		if (m_Stamina < startingstaming / 10)
			GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = false;
		else
			GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = true;

		if (stam != m_Stamina)
			Stamina_bar.ChangeVisibility();
		// Ground velocity
		_velocity.x = direction.x * spd;
		_velocity.z = direction.z * spd;
		//_velocity.y = direction.y * spd;
		// Vertical velocity
		_velocity.y -= FallAcceleration * delta;
		// Moving the character
		_velocity = MoveAndSlide(_velocity, Vector3.Up);
		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			anim.PlayAnimation(E_Animations.Jump);
			_velocity.y += JumpImpulse;

		}
		for (int index = 0; index < GetSlideCount(); index++)
		{
			// We check every collision that occurred this frame.
			KinematicCollision collision = GetSlideCollision(index);
			// If we collide with a monster...
			if (collision.Collider is Mob mob && mob.IsInGroup("mob"))
			{
				// ...we check that we are hitting it from above.
				if (Vector3.Up.Dot(collision.Normal) > 0.1f)
				{
					// If so, we squash it and bounce.
					mob.Squash();
					_velocity.y = BounceImpulse;
				}
			}
		}
	}
	Vector3 loctomove;
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Move"))
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 2000;
			var rayar = spacestate.IntersectRay(rayor, rayend);
			//if ray finds nothiong return
			if (rayar.Count == 0)
				return;
			loctomove = (Vector3)rayar["position"];
		}
	}
	private void Die()
	{
		EmitSignal(nameof(Hit));
		QueueFree();
	}

		// We also specified this function name in PascalCase in the editor's connection window
	public void OnMobDetectorBodyEntered(Node body)
	{
		//Die();
	}
	public void OnDoorDetectorBodyEntered(Node body)
	{
		if (body is Wall)
		{
			Wall bod = (Wall)body;
			bod.Touch(this);
		}
		//loctomove = GlobalTransform.origin;
	}
	public void OnHouseDoorDetectorBodyEntered(Node body)
	{
		if (body is HouseDoor)
		{
			Wall bod = (Wall)body;
			bod.Touch(this);
			TalkText.GetInstance().Talk("'Αδειο...");
		}
		//loctomove = GlobalTransform.origin;
	}
	
		// ...

		
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

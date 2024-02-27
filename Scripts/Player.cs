using Godot;
using System;

public class Player : Character
{
	// The downward acceleration when in the air, in meters per second squared.
	

	[Signal]
	public delegate void Hit();

	Stamina_Bar Stamina_bar = null;

	HP_Bar hp_bar = null;

	DialoguePanel DiagPan;

	MoveLocation moveloc;

	bool Autowalk = false;

	bool IsRunning = false;

	ActionMenu actMen;

	[Export(PropertyHint.Layers3dPhysics)]
    public uint SelectLayer { get; set; }

	public void Teleport(Vector3 pos)
	{
		GlobalTranslation = pos;
		loctomove = pos;
		//NavAgent.SetTargetLocation(loctomove);
		CameraMovePivot.GetInstance().GlobalTranslation = pos;
	}

	public override void _Ready()
	{
		base._Ready();
		//Input.MouseMode = Input.MouseModeEnum.Visible;
		Spatial plUI = GetNode<Spatial>("PlayerUI");
		actMen = plUI.GetNode<ActionMenu>("ActionMenu");
		hp_bar = plUI.GetNode<HP_Bar>("HP_Bar");
		hp_bar.MaxValue = m_HP;
		Stamina_bar = plUI.GetNode<Stamina_Bar>("Stamina_Bar");
		Stamina_bar.MaxValue = m_Stamina;
		
		var panels = GetTree().GetNodesInGroup("DialoguePanel");
		DiagPan = (DialoguePanel)panels[0];
		

		moveloc = GetNode<MoveLocation>("MoveLoc");

		Spatial sunmoonpiv = GetNode<Spatial>("SunMoonPivot");
		MainWorld world = (MainWorld)GetParent().GetParent();
		MyWorld w = (MyWorld)GetParent();
		DayNight env = w.GetNode<WorldMap>("WorldMap").GetNode<Spatial>("Sky").GetNode<DayNight>("DayNightController");
		env.SunMoonMeshPivot = sunmoonpiv;
	}
	public override void _PhysicsProcess(float delta)
	{
		if (DayNight.IsDay())
			NightLight.LightEnergy = 0;
		else
			NightLight.LightEnergy = 1;
		if (Input.IsActionPressed("Move") || Autowalk)
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 10000;
			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
			//if ray finds nothiong return
			if (rayar.Count > 0)
			{
				Spatial ob = (Spatial)rayar["collider"];
				loctomove = (Vector3)rayar["position"];
				Vector3 norm = (Vector3)rayar["normal"];

				moveloc.Scale = new Vector3(1,1,1);
				Basis MoveLocBasis = moveloc.GlobalTransform.basis;

				var result = new Basis(norm.Cross(MoveLocBasis.z), norm, MoveLocBasis.x.Cross(norm));

				result = result.Orthonormalized();

				Transform or = moveloc.GlobalTransform;

				or.basis = result;

				moveloc.GlobalTransform = or;
				moveloc.Show();
			}
		}
		moveloc.GlobalTranslation = loctomove;

		var spd = Speed;

		var direction = loctomove - GlobalTransform.origin;

		Vector2 loc = new Vector2(loctomove.x, loctomove.z);
		
		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTransform.origin.x, GlobalTransform.origin.z));

		double stam = m_Stamina;
		
		if (IsRunning)
		{
			spd = RunSpeed;
			//m_Stamina = m_Stamina - m_RunCost;
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
			if (!IsRunning)
			{
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 0.5f;
				//GetNode<AudioStreamPlayer3D>("WalkingSound").db = 5f;
				anim.PlayAnimation(E_Animations.Walk);
			}
			else
			{
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 1f;
				anim.PlayAnimation(E_Animations.Run);
			}
				
		}
		//Stamina_bar.Value = m_Stamina;

		//if (m_Stamina < startingstaming / 10)
			//GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = false;
		//else
			//GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = true;

		//if (stam != m_Stamina)
			//Stamina_bar.ChangeVisibility();
		// Ground velocity
		_velocity.x = direction.x * spd;
		_velocity.z = direction.z * spd;
		//_velocity.y = direction.y * spd;
		// Vertical velocity
		_velocity.y -= FallAcceleration * delta;
		// Moving the character
		_velocity = MoveAndSlide(_velocity, Vector3.Up);
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor())
			{
				anim.PlayAnimation(E_Animations.Jump);
				_velocity.y += JumpImpulse;
			}
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
	

	Item selectedobj;
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Move"))
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 10000;
			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
			//if ray finds nothiong return
			if (rayar.Count == 0)
				return;
			loctomove = (Vector3)rayar["position"];
			Vector3 norm = (Vector3)rayar["normal"];
			var result = new Basis();
			result.x = norm.Cross(moveloc.GlobalTransform.basis.z);
			result.y = norm;
			result.z = moveloc.GlobalTransform.basis.x.Cross(norm);
			var scale = moveloc.GlobalTransform.basis.Scale;
			result = result.Orthonormalized();
			result.Scale = new Vector3(1, 1, 1);
			Transform or = new Transform();
			or.basis = result;
			moveloc.GlobalTransform = or;
			moveloc.Show();
		}
		if (@event.IsActionPressed("AutoWalk"))
		{
			if (Autowalk)
				Autowalk = false;
			else
				Autowalk = true;
		}
		if (@event.IsActionPressed("Run"))
		{
			if (IsRunning)
				IsRunning = false;
			else
				IsRunning = true;
		}
		if (@event.IsActionPressed("Select"))
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 10000;
			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, SelectLayer);
			//if ray finds nothiong return
			if (rayar.Count == 0)
			{
				selectedobj = null;
				actMen.Stop();
				return;
			}
			var obj = rayar["collider"];
			if (obj is Item)
			{
				selectedobj = (Item)obj;
				actMen.Start(selectedobj);
			}
			else if (obj is Character)
			{
				Character selectechar = (Character)obj;
				actMen.Start(selectechar);
			}
		}
	}
	private void Die()
	{
		EmitSignal(nameof(Hit));
		QueueFree();
	}

		// We also specified this function name in PascalCase in the editor's connection window
	public void OnHouseDoorDetectorBodyEntered(Node body)
	{
		if (body is HouseDoor)
		{
			HouseDoor bod = (HouseDoor)body;
			if (bod.Touch(this))
			{
				House h = (House)bod.GetParent();
				if (!h.HasItem())
					TalkText.GetInst().Talk("'Αδειο...");
			}
			
			
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

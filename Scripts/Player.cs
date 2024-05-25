using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
public class Player : Character
{
	
	[Export]
	Curve Consumption = null;

	[Export]
	float MaxEnergyAmmount = 100;

	

	float CurrentEnergy = 100;

	//Stamina_Bar Stamina_bar = null;

	//HP_Bar hp_bar = null;

	//DialoguePanel DiagPan;

	MoveLocation moveloc;

	bool Autowalk = false;

	public bool IsRunning = false;

	ActionMenu actMen;

	public float rpm;

	static Player instance;

	Camera DialogueCam;
	
	public static Player GetInstance()
	{
		return instance;
	}
	[Export(PropertyHint.Layers3dPhysics)]
	public uint SelectLayer { get; set; }

	public float GetCharacterBatteryCap()
	{
		return MaxEnergyAmmount;
	}
	public float GetCurrentCharacterEnergy()
	{
		return CurrentEnergy;
	}
	public void RechargeCharacter(float ammount)
	{
		CurrentEnergy += ammount;
        if (CurrentEnergy > MaxEnergyAmmount)
        {
            CurrentEnergy = MaxEnergyAmmount;
        }
	}
	public void SetEnergy(float en)
	{
		CurrentEnergy = en;
	}
	public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
    }
	public float GetCurrentEnergy()
	{
		return CurrentEnergy;
	}
	public void Teleport(Vector3 pos)
	{
		GlobalTranslation = pos;
		loctomove = pos;
		//NavAgent.SetTargetLocation(loctomove);
		//CameraMovePivot.GetInstance().GlobalTranslation = pos;
	}
	public bool HasVehicle()
	{
		return HasVecicle;
	}
	
	public override void _Ready()
	{
		base._Ready();

		instance = this;
		
		Spatial plUI = GetNode<Spatial>("PlayerUI");
		actMen = plUI.GetNode<ActionMenu>("ActionMenu");
		
		//var panels = GetTree().GetNodesInGroup("DialoguePanel");
		//DiagPan = (DialoguePanel)panels[0];

		moveloc = GetNode<MoveLocation>("MoveLoc");

		
		MainWorld world = (MainWorld)GetParent().GetParent();
		MyWorld w = (MyWorld)GetParent();
		
		DialogueCam = GetNode<Spatial>("DialogueCameraPivot").GetNode<Camera>("DialogueCamera");
		//Input.MouseMode = Input.MouseModeEnum.Visible;
		//hp_bar = plUI.GetNode<HP_Bar>("HP_Bar");
		//hp_bar.MaxValue = m_HP;
		//Stamina_bar = plUI.GetNode<Stamina_Bar>("Stamina_Bar");
		//Stamina_bar.MaxValue = m_Stamina;
	}
	public override void _PhysicsProcess(float delta)
	{
		ulong ms = OS.GetSystemTimeMsecs();
		if (DayNight.IsDay())
			NightLight.LightEnergy = 0;
		else
			NightLight.LightEnergy = 0.2f;
		if (Input.IsActionPressed("Move") || Autowalk)
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 10000;
			var rayar = new Dictionary();
			if (HasVecicle)
				rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.VehicleMoveLayer);
			else
				rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
			//if ray finds nothiong return
			if (rayar.Count > 0)
			{
				//Spatial ob = (Spatial)rayar["collider"];
				loctomove = (Vector3)rayar["position"];
				Vector3 norm = (Vector3)rayar["normal"];

				moveloc.Scale = new Vector3(1,1,1);
				moveloc.Rotation = new Vector3(0,0,0);
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

		
		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTransform.origin.x, GlobalTransform.origin.z));

		
		
		if (dist < 1)
		{
			AudioStreamPlayer3D walk = CharacterSoundManager.GetSound("Walk");
			if (!walk.StreamPaused)
				walk.StreamPaused = true;
			anim.PlayAnimation(E_Animations.Idle);
			moveloc.Hide();
			HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
			rpm = 0.05f;
		}
		else if (HasVecicle && dist < 10)
		{
			AudioStreamPlayer3D walk = CharacterSoundManager.GetSound("Walk");
			rpm = 0.05f;
			if (!walk.StreamPaused)
				walk.StreamPaused = true;
			anim.PlayAnimation(E_Animations.Idle);
			moveloc.Hide();
			HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
		}
		else if (HasVecicle && dist > 10)
		{
			if (currveh.Working)
				rpm = 1;
			else
				rpm = 0.05f;

			AudioStreamPlayer3D walk = CharacterSoundManager.GetSound("Walk");
			if (!walk.StreamPaused)
				walk.StreamPaused = true;
			anim.PlayAnimation(E_Animations.Idle);
			HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
			moveloc.Show();
		}
		else
		{
			AudioStreamPlayer3D walk = CharacterSoundManager.GetSound("Walk");
			direction = direction.Normalized();
			Vector3 lookloc = new Vector3(direction.x, 0, direction.z);
			if (!HasVecicle)
				GetNode<Spatial>("Pivot").LookAt(Translation - lookloc, Vector3.Up);

			if (walk.StreamPaused)
			{
				walk.StreamPaused = false;
				walk.PitchScale = 0.7f;
			}
			if (!IsRunning)
			{
				rpm = 0.2f;
				walk.PitchScale = 0.5f;
				//GetNode<AudioStreamPlayer3D>("WalkingSound").db = 5f;
				anim.PlayAnimation(E_Animations.Walk);
			}
			else
			{
				spd = RunSpeed;
				rpm = 0.5f;
				walk.PitchScale = 1f;
				anim.PlayAnimation(E_Animations.Run);
			}
			float heightdif = GlobalTransform.origin.y - loctomove.y ;
			float rot = heightdif / 45;
			if (rot > 0)
				HeadPivot.Rotation = new Vector3(Math.Min(rot, 0.3f) , 0.0f,0.0f);
			else
				HeadPivot.Rotation = new Vector3(Math.Max(rot, -0.5f) , 0.0f,0.0f);
		}

		
		float coons = Consumption.Interpolate(rpm) * delta;
		ConsumeEnergy(coons);
		List<Item> batteries;
		CharacterInventory.GetItemsByType(out batteries, ItemName.BATTERY);
		
		for (int i = batteries.Count() -1; i > -1; i--)
		{
			if (((Battery)batteries[i]).GetCurrentCap() < coons)
			{
				batteries.RemoveAt(i);
			}
		}
		if (batteries.Count() == 0)
		{
			
			if (CurrentEnergy <= 0)
				Kill();
		}
		else
		{
			float rechargeammount = Math.Min( GetCharacterBatteryCap() - GetCurrentCharacterEnergy() , 0.1f);
			((Battery)batteries[0]).ConsumeEnergy(rechargeammount);
			RechargeCharacter(rechargeammount);
		}
			

		//GD.Print("Consuming energy :" + coons);
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
		
		// Moving the character
		if (!HasVecicle)
		{
			_velocity.y -= FallAcceleration * delta;
			_velocity = MoveAndSlide(_velocity, Vector3.Up);
		}
		else
		{
			if (currveh.Working)
				currveh.loctomove = loctomove;
		}
		
		ulong msaf = OS.GetSystemTimeMsecs();
		if (msaf - ms > 215)
			GD.Print("Player processing took longer the 15 ms. Process time : " + (msaf - ms).ToString() + " ms");
	}
	//Handling of input
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Move"))
		{
			var spacestate = GetWorld().DirectSpaceState;
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 20000;
			var rayar = new Dictionary();
			if (HasVecicle)
				rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.VehicleMoveLayer);
			else
				rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
			//if ray finds nothiong return
			if (rayar.Count == 0)
				return;
			loctomove = (Vector3)rayar["position"];
			Vector3 norm = (Vector3)rayar["normal"];
			var result = new Basis(norm.Cross(moveloc.GlobalTransform.basis.z), norm, moveloc.GlobalTransform.basis.x.Cross(norm));

			result = result.Orthonormalized();
			result.Scale = new Vector3(1, 1, 1);
			Transform or = new Transform(result, Vector3.Zero);

			moveloc.GlobalTransform = or;
			moveloc.Show();
		}
		if (@event.IsActionPressed("jump"))
		{
			if (IsOnFloor())
			{
				anim.PlayAnimation(E_Animations.Jump);
				_velocity.y += JumpImpulse;
			}
			else if (HasVecicle)
			{
				anim.PlayAnimation(E_Animations.Jump);
				currveh.Jump();
				//_velocity.y += JumpImpulse * vehmulti;
			}
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
			if (!HasVecicle)
			{
				if (IsRunning)
					IsRunning = false;
				else
					IsRunning = true;
			}
			
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
				actMen.Stop();
				return;
			}
			Spatial obj = (Spatial)rayar["collider"];
			if (obj.GlobalTransform.origin.DistanceTo(GlobalTransform.origin) > 100)
			{
				actMen.Stop();
				return;
			}
			actMen.Start(obj);
		}
		if (@event.IsActionPressed("Inventory"))
		{
			InventoryUI inv = InventoryUI.GetInstance();
			if (inv.IsOpen)
				inv.CloseInventory();
			else
				inv.OpenInventory();
		}
	}
	public override void OnKillFieldDetectorBodyEntered(Node body)
	{
		Kill();
	}
	public override void OnVehicleBoard(Vehicle Veh)
	{
		base.OnVehicleBoard(Veh);
		IsRunning = false;
	}
	public void StartDialogue(Character character)
	{
		if (HasVehicle())
		{
			currveh.UnBoardVehicle(this);
		}
		Position3D talkpos = character.GetNode<Position3D>("TalkPosition");
		loctomove = talkpos.GlobalTranslation;
		((Spatial)DialogueCam.GetParent()).GlobalRotation = talkpos.GlobalRotation;
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeInDialogue");
		var dialogue = DialogicSharp.Start("TestTimeline");
		AddChild(dialogue);
		dialogue.Connect("timeline_end", this, "EndDialogue");
		//DialogueCam.Current = true;
	}
	public void EndDialogue(string timeline_name)
	{
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
	}
}

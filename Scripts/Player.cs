using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
public class Player : Character
{
	
	[Export]
	Curve Consumption = null;

	

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
	private void UpdateMoveLocation()
	{
		var spacestate = GetWorld().DirectSpaceState;
		Vector2 mousepos = GetViewport().GetMousePosition();
		Camera cam = GetTree().Root.GetCamera();
		Vector3 rayor = cam.ProjectRayOrigin(mousepos);
		Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 10000;
		var rayar = new Dictionary();

		rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
		//if ray finds nothiong return
		if (rayar.Count > 0)
		{
			bool ItsSea = ((CollisionObject)rayar["collider"]).GetCollisionLayerBit(8);
			if (ItsSea && !HasVecicle)
			{
				
			}
			else
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
		
	}
	bool ExpressedNoBatteries = false;
	bool ExpressedLowBattery = false;
    public override void _Process(float delta)
    {
        base._Process(delta);

		if (Input.IsActionPressed("Move") || Autowalk)
		{
			UpdateMoveLocation();
		}

		List<Item> batteries;
		CharacterInventory.GetItemsByType(out batteries, ItemName.BATTERY);

		for (int i = batteries.Count() -1; i > -1; i--)
		{
			if (((Battery)batteries[i]).GetCurrentCap() <= 0)
			{
				batteries.RemoveAt(i);
			}
		}
		if (batteries.Count() > 0)
		{
			float rechargeammount = Math.Min( GetCharacterBatteryCap() - GetCurrentCharacterEnergy() , 0.1f);
			((Battery)batteries[0]).ConsumeEnergy(rechargeammount);
			RechargeCharacter(rechargeammount);
			ExpressedNoBatteries = false;
		}
		else
		{
			if (!ExpressedNoBatteries)
			{
				TalkText.GetInst().Talk("Ξέμεινα από μπαταρίες. Πρέπει να βρώ κάπου να φωρτήσω.", this);
				ExpressedNoBatteries = true;
			}
		}

    }
    public override void _PhysicsProcess(float delta)
	{
		//ulong ms = OS.GetSystemTimeMsecs();

		base._PhysicsProcess(delta);

		if (sitting && loctomove.DistanceTo(GlobalTranslation) > 0.5)
		{
			StandUp();
		}
		if (anim.IsStanding())
			return;
		
		moveloc.GlobalTranslation = loctomove;
		var spd = Speed;

		var direction = loctomove - GlobalTransform.origin;

		
		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTransform.origin.x, GlobalTransform.origin.z));

		if (HasVecicle)
		{
			if (dist < 10)
			{
				AudioStreamPlayer3D walk = CharacterSoundManager.GetSound("Walk");
				rpm = 0.05f;
				if (!walk.StreamPaused)
					walk.StreamPaused = true;
				anim.PlayAnimation(E_Animations.Idle);
				moveloc.Hide();
				HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
			}
			else if (dist > 10)
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
		}
		else
		{
			
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
			RayCast dropcheck = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<RayCast>("RayCast");
			dropcheck.ForceUpdateTransform();
			if (!dropcheck.IsColliding())
				direction = Vector3.Zero;
			else
			{
				bool ItsSea = ((CollisionObject)dropcheck.GetCollider()).GetCollisionLayerBit(8);
				if (ItsSea)
				{
					direction = Vector3.Zero;
				}
			}
		}
		/////////////////////////////////////////////
		//battery consumption
		float coons = Consumption.Interpolate(rpm) * delta;
		ConsumeEnergy(coons);

		if (GetCurrentEnergy() < GetCharacterBatteryCap() / 10)
		{
			if (!ExpressedLowBattery)
			{
				TalkText.GetInst().Talk("Θα ξεμείνω από μπαταρία σε λίγο. Δεν νιώθω καλά.", this);
				ExpressedLowBattery = true;
			}
		}
		else
		{
			ExpressedLowBattery = false;
		}
		//List<Item> batteries;
		//CharacterInventory.GetItemsByType(out batteries, ItemName.BATTERY);
		
		//for (int i = batteries.Count() -1; i > -1; i--)
		//{
		//	if (((Battery)batteries[i]).GetCurrentCap() < coons)
		//	{
		//		batteries.RemoveAt(i);
		//	}
		//}
		//if (batteries.Count() > 0)
		//{
		//	float rechargeammount = Math.Min( GetCharacterBatteryCap() - GetCurrentCharacterEnergy() , 0.1f);
		//	((Battery)batteries[0]).ConsumeEnergy(rechargeammount);
		//	RechargeCharacter(rechargeammount);
		//}
		/////////////////////////////////////////////

		// Moving the character
		if (!HasVecicle)
		{
			_velocity.x = direction.x * spd;
			_velocity.z = direction.z * spd;
			_velocity.y -= FallAcceleration * delta;
			_velocity = MoveAndSlide(_velocity, Vector3.Up);
		}
		else
		{
			if (currveh.Working)
				currveh.loctomove = loctomove;
		}
		
		//ulong msaf = OS.GetSystemTimeMsecs();
		//if (msaf - ms > 215)
			//GD.Print("Player processing took longer the 15 ms. Process time : " + (msaf - ms).ToString() + " ms");
	}
	//Handling of input
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Move"))
		{
			UpdateMoveLocation();
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
	Character TalkingChar;
	public void StartDialogue(Character character)
	{
		if (HasVehicle())
		{
			if (!currveh.UnBoardVehicle(this))
				return;
		}
		Position3D talkpos = character.GetNode<Position3D>("TalkPosition");

		loctomove = talkpos.GlobalTranslation;
		((Spatial)DialogueCam.GetParent()).GlobalRotation = talkpos.GlobalRotation;
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeInDialogue");
		if (character.CurrentEnergy == 0)
		{
			bool HasBat = CharacterInventory.HasBatteries();
			DialogicSharp.SetVariable("HasBatteries", HasBat.ToString().ToLower());
			var dialogue = DialogicSharp.Start("UnConDialogue");
			AddChild(dialogue);
			dialogue.Connect("timeline_end", this, "EndUnconDialogue");
		}
		else
		{
			var dialogue =  DialogicSharp.Start("TestTimeline");
			AddChild(dialogue);
			dialogue.Connect("timeline_end", this, "EndDialogue");
		}
		TalkingChar = character;
		
		//DialogueCam.Current = true;
	}
	public void EndUnconDialogue(string timeline_name)
	{
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
		string saved = DialogicSharp.GetVariable("SavedCharacter");
		if (saved == "true")
		{
			
			List<Battery> bats;
			CharacterInventory.GetBatteries(out bats);

			TalkingChar.RechargeCharacter(bats[0].GetCurrentCap());
			TalkingChar.Respawn();

			CharacterInventory.DeleteItem(bats[0]);
		}
		
	}
	public void EndDialogue(string timeline_name)
	{
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
	}
}

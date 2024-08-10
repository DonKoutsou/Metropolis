using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

////////////////////////////////////////////////
/*
██████╗ ██╗      █████╗ ██╗   ██╗███████╗██████╗ 
██╔══██╗██║     ██╔══██╗╚██╗ ██╔╝██╔════╝██╔══██╗
██████╔╝██║     ███████║ ╚████╔╝ █████╗  ██████╔╝
██╔═══╝ ██║     ██╔══██║  ╚██╔╝  ██╔══╝  ██╔══██╗
██║     ███████╗██║  ██║   ██║   ███████╗██║  ██║
╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝
*/
////////////////////////////////////////////////                                      
public class Player : Character
{
	[Export]
	protected int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	protected float FallAcceleration = 75; //

	[Export]
	protected int JumpImpulse = 20;

	[Export]
	protected float RunSpeed = 20;

	////// Energy ///////
	//Consumption curve
	[Export]
	Curve Consumption = null;
	//[Export]
	//public int InventoryWeightOverride = -1;
	////// Rpm to base consumption on /////
	float rpm;
	//Node Used to show where player is moving
	MoveLocation moveloc;
	//Toggle that makes character follow cursor without having to rightclick. Keybinding [TAB]
	bool Autowalk = false;
	bool IsRunning = false;

	//Camera DialogueCam;

	static Player instance;

	public bool HasBaby = false;

	public bool BabyAlive = true;

	public bool BeingTalkedTo = false;

	public bool CanTraverseDeep = false;

	public void SetRunSpeed(int NewSpeed)
	{
		RunSpeed = NewSpeed;
	}
    public override void _EnterTree()
    {
        base._EnterTree();
		instance = this;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
		instance = null;
		
    }
	public static bool IsSpawned()
	{
		return instance != null;
	}
    public static Player GetInstance()
	{
		return instance;
	}
	//public Camera GetDialogueCamera()
	//{
	//	return DialogueCam;
	//}
	public float GetCurrentEnergy()
	{
		return CurrentEnergy;
	}
	public float GetRPM()
	{
		return rpm;
	}
	public void Teleport(Vector3 pos, Vector3 rot)
	{
		
		PhysicsServer.BodySetState(GetRid(), PhysicsServer.BodyState.Transform, Transform.Identity.Translated(pos - GlobalTranslation));
		if (HasVecicle)
			currveh.GlobalTranslation = pos;
		else
			GlobalTranslation = pos;
		loctomove = pos;
		GetNode<Spatial>("Pivot").GlobalRotation = rot;

		Vector3 rota = new Vector3(0, rot.y, 0);

		CameraPanPivot.GetInstance().GlobalRotation = rota;
	}
	
	public override void _Ready()
	{
		base._Ready();

		CharacterInventory = GetNode<Inventory>("Inventory");
		//if (InventoryWeightOverride != -1)
		//{
		//	CharacterInventory.OverrideWeight(InventoryWeightOverride);
		//}

		moveloc = GetNode<MoveLocation>("MoveLoc");
		
		//DialogueCam = GetNode<Camera>("DialogueCameraPivot/DialogueCamera");

		GetNode<MeshInstance>("Pivot/Guy/Armature/Skeleton/BabyLowpolySurface1").Visible = HasBaby;
	}
	
	private void CheckIfIdling()
	{
		if (sitting && !PlayingInstrument && HasEquippedInstrument())
		{
			IdleTimer.Start();
		}
	}
	public void ToggleLimbEffect(LimbType limb, bool toggle)
	{
		Godot.Collections.Dictionary<string, object> effects = LimbTranslator.GetLimbEffect(limb, toggle);
		foreach (KeyValuePair<string, object> ef in effects)
		{
			if (toggle)
			{
				if (ef.Value is float val)
					Set(ef.Key, (float)Get(ef.Key) * val);
				else
					Set(ef.Key, ef.Value);
			}
			else
			{
				if (ef.Value is float val)
					Set(ef.Key, (float)Get(ef.Key) / val);
				else
					Set(ef.Key, ef.Value);
			}
				
		}
	}
	public void MoveTo(Vector3 loc, bool Running = false, bool ResetLook = false)
	{
		loctomove = loc;
		IsRunning = Running;
		if (ResetLook)
		{
			Vector3 rot = GetNode<Spatial>("Pivot").Rotation;
			CameraPanPivot.GetInstance().Rotation = new Vector3(0, rot.y, 0);
		}
			
	}
	private void UpdateMoveLocation()
	{
		if (BeingTalkedTo)
			return;
		var spacestate = GetWorld().DirectSpaceState;
		float mult = OS.WindowSize.x / DViewport.GetInstance().Size.x;
		Vector2 mousepos = DViewport.GetInstance().GetMousePosition() / mult;
		Camera cam = DViewport.GetInstance().GetCamera();
		Vector3 rayor = cam.ProjectRayOrigin(mousepos);
		Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 20000;
		Dictionary rayar;

		rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, moveloc.MoveLayer);
		//if ray finds nothiong return
		if (rayar.Count > 0)
		{
			bool ItsSea = ((CollisionObject)rayar["collider"]).GetCollisionLayerBit(8);
			if (ItsSea && !HasVecicle)
				return;
			else
			{
				Vector3 pos = (Vector3)rayar["position"];
				if (!CanTraverseDeep)
				{
					if (pos.DistanceTo(Vector3.Zero) > 7 * 8000)
					{
						TalkText t = GetTalkText();
						if (!t.IsTalking())
						{
							t.Talk("Δεν μπορώ να πάω πιο βαθυιά με το μωρό μαζί μου, η ομήχλη είναι πολύ πηκνή.");
						}
						return;
					}
				}
				
				//Spatial ob = (Spatial)rayar["collider"];
				loctomove = pos;
				Vector3 norm = (Vector3)rayar["normal"];

				moveloc.Scale = new Vector3(1,1,1);
				moveloc.Rotation = new Vector3(0,0,0);
				Basis MoveLocBasis = moveloc.Transform.basis;

				var result = new Basis(norm.Cross(MoveLocBasis.z), norm, MoveLocBasis.x.Cross(norm));

				result = result.Orthonormalized();

				Transform or = moveloc.GlobalTransform;

				or.basis = result;

				moveloc.GlobalTransform = or;
				moveloc.Show();
			}
		}
	}
	public override void PlayMusic()
	{
		if (!CharacterInventory.IsLimbSlotFilled(LimbSlotType.ARM_L) || !CharacterInventory.IsLimbSlotFilled(LimbSlotType.ARM_R))
		{
			GetTalkText().Talk("Δεν μπορώ να παίξω χωρίς χέρια");
			return;
		}
		base.PlayMusic();
	}
	bool ExpressedNoBatteries = true;
	bool ExpressedLowBattery = false;
    public override void _Process(float delta)
    {
        base._Process(delta);

		CheckIfIdling();

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
			if (!ExpressedNoBatteries && !BeingTalkedTo)
			{
				GetTalkText().Talk("Ξέμεινα από μπαταρίες. Πρέπει να βρώ κάπου να φωρτήσω.");
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
		if (anim.IsStanding() || anim.IsClimbing())
			return;
		
		moveloc.GlobalTranslation = loctomove;
		float spd = Speed;

		var direction = loctomove - GlobalTranslation;

		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTranslation.x, GlobalTranslation.z));

		if (HasVecicle)
		{
			if (dist < 10)
			{
				anim.PlayAnimation(E_Animations.Idle);
				moveloc.Hide();
				HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
			}
			else if (dist > 10)
			{
				anim.PlayAnimation(E_Animations.Idle);
				HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
				moveloc.Show();
			}
			rpm = currveh.GetRPM();
		}
		else
		{
			if (dist < 1)
			{
				anim.PlayAnimation(E_Animations.Idle);
				moveloc.Hide();
				HeadPivot.Rotation = new Vector3(0.0f,0.0f,0.0f);
				rpm = 0.05f;
			}
			else
			{
				direction = direction.Normalized();
				Vector3 lookloc = new Vector3(direction.x, 0, direction.z);
				if (!HasVecicle)
					GetNode<Spatial>("Pivot").LookAt(GlobalTranslation - lookloc, Vector3.Up);

				if (!IsRunning)
				{
					rpm = 0.2f;
					anim.PlayAnimation(E_Animations.Walk);
				}
				else
				{
					spd = RunSpeed;
					rpm = 0.5f;
					anim.PlayAnimation(E_Animations.Run);
				}
				float heightdif = GlobalTranslation.y - loctomove.y ;
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
			if (!ExpressedLowBattery && !BeingTalkedTo)
			{
				GetTalkText().Talk("Θα ξεμείνω από μπαταρία σε λίγο. Δεν νιώθω καλά.");
				ScreenEffects ui = (ScreenEffects)PlayerUI.GetInstance().GetUI(PlayerUIType.SCREENEFFECTS);
				ui.PlayEffect(ScreenEffectTypes.DAMAGE);
				ExpressedLowBattery = true;
			}
		}
		else
		{
			ExpressedLowBattery = false;
		}

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
			if (currveh.IsRunning())
				currveh.UpdateMoveLoc(loctomove);
		}
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
			Autowalk = !Autowalk;
		}
		if (@event.IsActionPressed("Run"))
		{
			if (!HasVecicle)
			{
				IsRunning = !IsRunning;
			}
		}
	}
	public override void OnKillFieldDetectorBodyEntered(Node body)
	{
		Kill("Πνίξιμο");
	}
	public void OnBabyGot()
	{
		HasBaby = true;
		GetNode<MeshInstance>("Pivot/Guy/Armature/Skeleton/BabyLowpolySurface1").Visible = true;
	}
	public override void Kill(string reason = null)
	{
		base.Kill(reason);
		MyWorld.GetInstance().OnPlayerKilled(reason);
	}
	
}

using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
public class Character : KinematicBody
{
	[Signal]
	public delegate void Hit();
	[Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public int FallAcceleration = 75;

	[Export]
	public int JumpImpulse = 20;

	[Export]
	public int BounceImpulse = 16;

	[Export]
	public int RunSpeed = 20;

	//public Character_Animations anims;

	[Export]
	public float m_HP = 100;

	
	[Export]
	public float m_Stamina = 100;

	[Export]
	public float m_StaminaRegen = 0.5f;

	[Export]
	public float m_RunCost = 1;

	public float startingstaming;

	public bool m_balive = true;

	public Vector3 origposition;

	public bool m_bEnabled = false;

	public CollisionShape collider;

	public Vector3 m_velocity = Vector3.Zero;


    public Vector3 _velocity = Vector3.Zero;

	[Export]
	public DialogueLine[] lines;

	[Export]
	float InventoryWeightOverride = -1;

	public Inventory CharacterInventory;

	public SpotLight NightLight;

	public Spatial HeadPivot;

	public Vector3 loctomove;

	public Character_Animations anim;

	public bool HasVecicle = false;

	public Vehicle currveh;

	public CharacterSoundManager CharacterSoundManager;

	public override void _Ready()
	{
		if (this is Player)
		{
			CharacterInventory = GetNode<Inventory>("Inventory");
			if (InventoryWeightOverride != -1)
			{
				CharacterInventory.OverrideWeight(InventoryWeightOverride);
			}
		}

		//anims = GetNode<Character_Animations>("Character_Animations");
		collider = GetNode<CollisionShape>("CollisionShape");
		collider.Disabled = false;
		startingstaming = m_Stamina;

		CharacterSoundManager = GetNode<CharacterSoundManager>("CharacterSoundManager");

		AudioStreamPlayer3D walkingsound = CharacterSoundManager.GetSound("Walk");
		walkingsound.Play();
		walkingsound.StreamPaused = true;
		//GetNode<AudioStreamPlayer3D>("TiredSound").Play();
		//GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = true;
		
		anim = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Character_Animations>("AnimationPlayer");
		HeadPivot = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment").GetNode<Spatial>("HeadPivot");
		NightLight = HeadPivot.GetNode<SpotLight>("NightLight");
		loctomove = GlobalTranslation;

		if (this is Player)
			return;
		Island ile = (Island)GetParent();
		ile.RegisterChild(this);
	}
	public void SetData(CharacterInfo info)
	{
        Name = info.Name;
        GlobalTranslation = info.Position;
	}
	public void SetVehicle(Vehicle veh)
	{
		if (veh == null)
		{
			HasVecicle = false;
			currveh = null;
		}
		else
		{
			HasVecicle = true;
			currveh = veh;
		}
	}
	public Inventory GetCharacterInventory()
	{
		return CharacterInventory;
	}
	public bool HasLines()
	{
		return lines != null;
	}
	public DialogueLine GetLine()
	{
		return lines[0];
	}
	public override void _PhysicsProcess(float delta)
	{
		if (!m_bEnabled)
			return;
			
		if (origposition != Transform.origin)
			MoveTo(origposition);

		if (DayNight.IsDay())
			NightLight.LightEnergy = 0;
		else
			NightLight.LightEnergy = 0.2f;
	}
    public void MoveTo(Vector3 loc)
	{
        if (!m_balive)
			return;
        _velocity = Vector3.Zero;
		var cloc = GlobalTransform.origin;
		var spd = RunSpeed;
		var direction = loc - GlobalTransform.origin;
		
		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTransform.origin.x, GlobalTransform.origin.z));

		if (dist < 1)
		{
			AudioStreamPlayer3D walkingsound = CharacterSoundManager.GetSound("Walk");
			if (!walkingsound.StreamPaused)
				walkingsound.StreamPaused = true;
			anim.PlayAnimation(E_Animations.Idle);
		}
		else
		{
			AudioStreamPlayer3D walkingsound = CharacterSoundManager.GetSound("Walk");
			direction = direction.Normalized();
			Vector3 lookloc = new Vector3(direction.x, 0, direction.z);
			GetNode<Spatial>("Pivot").LookAt(Translation - lookloc, Vector3.Up);
			if (walkingsound.StreamPaused)
			{
				walkingsound.StreamPaused = false;
				walkingsound.PitchScale = 0.7f;
			}
				
		}
        _velocity.x = direction.x * spd;
		_velocity.z = direction.z * spd;
		//_velocity.y = direction.y * spd;
		// Vertical velocity
		_velocity.y -= FallAcceleration * 0.01f;
		// Moving the character
		_velocity = MoveAndSlide(_velocity, Vector3.Up);
	}
	public virtual void Start()
	{
		m_bEnabled = true;
		origposition = Transform.origin;
		//GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",false);
		//SetProcess(true);
		SetPhysicsProcess(true);
		GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
	}
	public virtual void Stop()
	{
		m_bEnabled = false;
		//GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",true);
		//SetProcess(false);
		SetPhysicsProcess(false);
		GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
	}
	private void On_Body_Entered(object body)
	{
	}
	public virtual void Damage(float ammount, Character inst)
	{
		m_HP = m_HP - ammount;
		//Push(Mathf.Rad2Deg(GetAngleTo(inst.GlobalPosition)));

		if (m_HP <= 0)
			Kill();
	}
	public bool IsAlive()
	{
		return m_balive;
	}
	public float GetHP()
	{
		return m_HP;
	}
	public float GetStamina()
	{
		return m_Stamina;
	}
	public virtual void Kill()
	{
		//GetNode<Character_Animations>("Character_Animations").ForceAnimation(E_Animations.Die);
		//GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused = true;
		m_balive = false;
		origposition = GlobalTransform.origin;
		CharacterInventory.RemoveAllItems();
		//AppliedForce = AppliedForce * 0;
		Stop();
		Die();
	}
	private void Die()
	{
		EmitSignal(nameof(Hit));
		//QueueFree();
	}
	public virtual void OnKillFieldDetectorBodyEntered(Node body)
	{
		Kill();
	}
	public void Push(float degr, float ammount = 500)
	{
		if (!m_balive)
			return;
		_velocity = Vector3.Zero;
		float adjdef = degr - 90;
		//left 
		if (adjdef < 10 && adjdef >= -170)
		{
			_velocity.x -= 1;
		}
		//right  
		if (degr > 80 || degr < -80)
		{
			_velocity.x += 1;
		}
		//up  
		if (degr > 10 && degr < 170)
		{
			_velocity.y -= 1;
		}
		//down    
		if (degr < -10 && degr > -170)
		{
			_velocity.y += 1;
		}
		//Rotation = 0;
		//AddForce(velocity * ammount, velocity * ammount);
		//AppliedForce = m_velocity * ammount;
	}
	[Signal]
    public delegate void VehicleBoardEventHandler(bool toggle, Vehicle veh);
	public virtual void OnVehicleBoard(Vehicle veh)
	{
		EmitSignal("VehicleBoardEventHandler", true, veh);
		//SetCollisionMaskBit(8, true);
	}
	public virtual void OnVehicleUnBoard(Vehicle veh)
	{
		EmitSignal("VehicleBoardEventHandler", false, veh);
		loctomove = GlobalTranslation;
		//SetCollisionMaskBit(8, false);
	}
	private void On_DialogueButton_Button_Down()
	{
		
	}
}
public class CharacterInfo
{
	public string Name;
	public Vector3 Position;
	public string SceneData;
	public Dictionary<string, object> CustomData = new Dictionary<string, object>();

	public void UpdateInfo(Character it)
	{
		Name = it.Name;
		Position = it.GlobalTranslation;
		SceneData = it.Filename;

	}
	public Dictionary<string, object>GetPackedData(out bool HasData)
	{
		HasData = false;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Position", Position);
		data.Add("Name", Name);
		data.Add("SceneData", SceneData);
		if (CustomData.Count > 0)
		{
			HasData = true;
			string[] CustomDataKeys = new string[CustomData.Count];
			object[] CustomDataValues = new object[CustomData.Count];
			int i = 0;
			foreach (KeyValuePair<string, object> Data in CustomData)
			{
				CustomDataKeys[i] = Data.Key;
				CustomDataValues[i] = Data.Value;
				i++;
			}
			data.Add("CustomDataKeys", CustomDataKeys);
			data.Add("CustomDataValues", CustomDataValues);
		}
		return data;
	}
    public void UnPackData(Resource data)
    {
        Position = (Vector3)data.Get("Position");
		Name = (string)data.Get("Name");
		SceneData = (string)data.Get("SceneData");
		Godot.Collections.Array CustomDataKeys = (Godot.Collections.Array)data.Get("CustomDataKeys");
		Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)data.Get("CustomDataValues");

		if (CustomDataKeys.Count > 0 && CustomDataValues.Count > 0)
		{
			for (int i = 0; i < CustomDataKeys.Count; i++)
			{
				CustomData.Add((string)CustomDataKeys[i], CustomDataValues[i]);
			}
		}
    }
}






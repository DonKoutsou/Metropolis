using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
public class Character : KinematicBody
{
	[Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public int FallAcceleration = 75;

	[Export]
	public int JumpImpulse = 20;

	[Export]
	public int RunSpeed = 20;

	public bool m_balive = true;

    public Vector3 _velocity = Vector3.Zero;

	[Export]
	public DialogueLine[] lines;
	
	public Inventory CharacterInventory;

	[Export]
	float MaxEnergyAmmount = 100;

	[Export]
	public string CharacterName = "Βαλάντης";

	public float CurrentEnergy = 100;

	public SpotLight NightLight;
	SpatialMaterial BulbMat;

	public Spatial HeadPivot;

	public Vector3 loctomove;

	public Character_Animations anim;

	public bool HasVecicle = false;

	public Vehicle currveh;

	public bool sitting = false;

	SittingThing chair = null;

	Position3D seat = null;

	public bool PlayingInstrument = false;

	public bool IsUncon = false;

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
	public override void _Ready()
	{

		anim = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Character_Animations>("AnimationPlayer");
		HeadPivot = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment").GetNode<Spatial>("HeadPivot");
		NightLight = HeadPivot.GetNode<SpotLight>("NightLight");
		BulbMat = (SpatialMaterial)HeadPivot.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0);
	}
	public void ToggleAllLimbs()
	{
		//Spatial skel = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton");
		for (int i = 0; i < 6; i++)
		{
			ToggleLimb((LimbType)i, false);
			//skel.GetNode<MeshInstance>(LimbTranslator.EnumToString((LimbType)i)).Visible = false;
		}
	}
	public override void _EnterTree()
	{
		base._EnterTree();
		//loctomove = GlobalTranslation;
	}
	public Character_Animations Anims()
	{
		return anim;
	}
	public void SetData(CharacterInfo info)
	{
        Name = info.Name;
        Translation = info.Position;
		CurrentEnergy = info.CurrentEnergy;
		m_balive = info.Alive;

		for (int i = 0; i < 6; i++)
		{
			SetLimbColor((LimbType)i, info.LimbColors[i]);
		}
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
    public override void _Process(float delta)
    {
        base._Process(delta);
		if (DayNight.IsDay())
		{
			NightLight.LightEnergy = 0;
			BulbMat.EmissionEnabled = false;
		}
		else
		{
			NightLight.LightEnergy = 0.2f;
			BulbMat.EmissionEnabled = true;
		}
			

		if (CurrentEnergy <= 0 && IsAlive())
		{
			Kill();
			if (HasVecicle)
			{
				currveh.ToggleMachine(false);
			}
		}
			
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
			anim.PlayAnimation(E_Animations.Idle);
		}
		else
		{
			direction = direction.Normalized();
			Vector3 lookloc = new Vector3(direction.x, 0, direction.z);
			GetNode<Spatial>("Pivot").LookAt(Translation - lookloc, Vector3.Up);
				
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
		loctomove = GlobalTransform.origin;
		SetPhysicsProcess(true);
	}
	public virtual void Stop()
	{
		SetPhysicsProcess(false);
	}
	private void On_Body_Entered(object body)
	{
	}
	public bool IsAlive()
	{
		return m_balive;
	}
	public virtual void Respawn()
	{
		m_balive = true;
		IsUncon = false;
		anim.ToggleIdle();
		Start();
	}
	public virtual void Kill(string reason = null)
	{

		m_balive = false;
		loctomove = GlobalTransform.origin;
		anim.ToggleDeath();

		Stop();
		Die();
	}
	private void Die()
	{
		//QueueFree();
	}
	public virtual void OnKillFieldDetectorBodyEntered(Node body)
	{
		Kill();
	}
	public void Sit(Position3D pos, SittingThing Sitter = null)
	{
		GlobalTranslation = pos.GlobalTranslation;
		loctomove = GlobalTranslation;
		if (Sitter != null)
		{
			anim.ToggleSitting(true);
			chair = Sitter;
			seat = pos;
			Sitter.UpdateOccupation(pos, true);
		}
		else
			anim.ToggleSitting(false);
		
		GetNode<Spatial>("Pivot").GlobalRotation = pos.GlobalRotation;
		sitting = true;
		
	}
	public void StandUp()
	{
		if (chair != null)
		{
			chair.UpdateOccupation(seat, false);
			chair = null;
			seat = null;
			
		}
		if (PlayingInstrument)
		{
			StopMusic();
		}
		anim.ToggleIdle();
		sitting = false;
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
	}
	[Signal]
    public delegate void VehicleBoardEventHandler(bool toggle, Vehicle veh);
	public virtual void OnVehicleBoard(Vehicle veh)
	{
		EmitSignal("VehicleBoardEventHandler", true, veh);
		if (HasInstrument())
			PlayMusic();
		//SetCollisionMaskBit(8, true);
	}
	public virtual void OnVehicleUnBoard(Vehicle veh)
	{
		EmitSignal("VehicleBoardEventHandler", false, veh);
		
		loctomove = GlobalTranslation;
		if (PlayingInstrument)
		{
			StopMusic();
		}
		
		//SetCollisionMaskBit(8, false);
	}
	public virtual void PlayMusic()
	{
		anim.ToggleInstrument(true);
		
		Spatial instrumentparent = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment").GetNode<Spatial>("Instrument");
		Instrument bouz = (Instrument)instrumentparent.GetChild(0);
		BoneAttachment instatatchment = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment");
		instatatchment.GetNode<RemoteTransform>("HolsterAtatchment").RemotePath = instatatchment.GetNode<RemoteTransform>("HolsterAtatchment").GetPath();
		instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").RemotePath = instrumentparent.GetPath();
		
		bouz.ToggleMusic(true);
		bouz.Connect("OnSongEnded", this, "OnSongEnded");
		PlayingInstrument = true;
	}
	public void StopMusic()
	{
		anim.ToggleInstrument(false);

		Spatial instrumentparent = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment").GetNode<Spatial>("Instrument");
		Instrument bouz = (Instrument)instrumentparent.GetChild(0);
		bouz.Disconnect("OnSongEnded", this, "OnSongEnded");
		bouz.ToggleMusic(false);
		BoneAttachment instatatchment = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment");
		instatatchment.GetNode<RemoteTransform>("HolsterAtatchment").RemotePath = instrumentparent.GetPath();
		instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").RemotePath = instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").GetPath();
		PlayingInstrument = false;
	}
	public bool HasInstrument()
	{
		return GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment").GetNode<Spatial>("Instrument").GetChildCount() > 0;
	}
	public Instrument GetInstrument()
	{
		return (Instrument)GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment").GetNode<Spatial>("Instrument").GetChild(0);
	}
	public void AddInstrument(Instrument inst)
	{
		Spatial instrumentspace = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("InstrumentAtatchment").GetNode<Spatial>("Instrument");
		inst.GetNode<CollisionShape>("CollisionShape").Disabled = true;
		//inst.RegisterOnIsland = false;
		inst.Visible = true;
		instrumentspace.AddChild(inst);
		//inst.Visible = true;
		inst.Translation = Vector3.Zero;
		inst.Rotation = Vector3.Zero;
		//inst.Owner = Owner;
	}
	/*public bool HasLimbOfType(LimbType type)
	{
		LoddedCharacter lod = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<LoddedCharacter>("Skeleton");
		string name = LimbTranslator.EnumToString(type);
		if (lod.GetCurrentLOD() == 1)
			name += "_LOD"; 
		return GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>(name).Visible;
	}*/
	public void ToggleLimb(LimbType limb, bool toggle)
	{
		LoddedCharacter lod = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<LoddedCharacter>("Skeleton");
		string limbname = LimbTranslator.EnumToString(limb);
		if (lod.GetCurrentLOD() == 1)
			limbname += "_LOD";

		GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>(limbname).Visible = toggle;
	}
	public void SetLimbColor(LimbType limb, Color colorarion)
	{
		MeshInstance limbtocolor = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>(LimbTranslator.EnumToString(limb));
		MeshInstance limbtocolorlod = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>(LimbTranslator.EnumToString(limb) + "_LOD");
		((GradientTexture)((SpatialMaterial)limbtocolor.GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, colorarion);
		((GradientTexture)((SpatialMaterial)limbtocolorlod.GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, colorarion);
	}
	public Color GetLimbColor(LimbType limb)
	{
		MeshInstance limbtocolor = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>(LimbTranslator.EnumToString(limb));
		return ((GradientTexture)((SpatialMaterial)limbtocolor.GetActiveMaterial(0)).DetailAlbedo).Gradient.GetColor(0);
	}
	public virtual void OnSongEnded(Instrument inst)
	{
		//IdleTimer.Start();
		
		StopMusic();
		//instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").ForceUpdateCache(); 

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
	public float CurrentEnergy = 0.0f;
	public bool Alive = false;
	public Dictionary<string, object> CustomData = new Dictionary<string, object>();
	public List<Color> LimbColors = new List<Color>();
	public void UpdateInfo(Character it)
	{
		Name = it.Name;
		Position = it.Translation;
		SceneData = it.Filename;
		CurrentEnergy = it.GetCurrentCharacterEnergy();
		Alive = it.m_balive;
		for (int i = 0; i < 6; i++)
		{
			LimbColors.Add(it.GetLimbColor((LimbType)i));
		}
	}
	public Dictionary<string, object>GetPackedData(out bool HasData)
	{
		HasData = false;
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{"Position", Position},
			{"Name", Name},
			{"SceneData", SceneData},
			{"Energy", CurrentEnergy},
			{"Alive", Alive}
		};
		Color[] LimbColorsAr = new Color[6];
		for (int i = 0; i < 6; i++)
		{
			LimbColorsAr[i] = LimbColors[i];
		}
		data.Add("LimbColors", LimbColorsAr);
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
		CurrentEnergy = (float)data.Get("Energy");
		Alive = (bool)data.Get("Alive");

		Godot.Color[] LimbCols = (Godot.Color[])data.Get("LimbColors");
		for (int i = 0; i < 6; i++)
		{
			LimbColors.Add(LimbCols[i]);
		}


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






using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
public class Character : KinematicBody
{
	[Export]
	protected ClothingType ClothToWear = ClothingType.HOODIE;

	[Export]
	protected Material ClothMaterial = GD.Load<Material>("res://Assets/Gen3Machine/Hoodie/HoodieMat.tres");

	[Export]
	protected string CharacterName = "Βαλάντης";

	protected bool m_balive = true;

    protected Vector3 _velocity = Vector3.Zero;

	[Export]
	float MaxEnergyAmmount = 100;
	[Export]
	public NodePath OwnedVeh = null;
	protected float CurrentEnergy = 100;

	protected SpotLight NightLight;
	protected SpatialMaterial BulbMat;

	protected Vector3 loctomove;

	protected Character_Animations anim;

	protected bool HasVecicle = false;

	protected Vehicle currveh;

	protected bool sitting = false;

	protected SittingThing chair = null;

	protected RemoteTransform seat = null;

	protected TalkText TText = null;

	protected bool PlayingInstrument = false;

	protected bool IsUncon = false;

	protected Timer IdleTimer;
	
	public override void _Ready()
	{
		TText = GetNode<TalkText>("TalkText");
		anim = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Character_Animations>("AnimationPlayer");
		NightLight = GetNode<SpotLight>("Pivot/Guy/Armature/Skeleton/BoneAttachment/HeadPivot/NightLight");
		BulbMat = (SpatialMaterial)GetNode<MeshInstance>("Pivot/Guy/Armature/Skeleton/BoneAttachment/HeadPivot/MeshInstance").GetActiveMaterial(0);
		ToggleNightLight(CustomEnviroment.IsDay());
		IdleTimer = GetNode<Timer>("IdleTimer");
		SetClothing();
	}
	public bool IsPlayerInstrument()
	{
		return PlayingInstrument;
	}
	public TalkText GetTalkText()
	{
		return TText;
	}
	public bool GetUnconState()
	{
		return IsUncon;
	}
	public Vector3 GetHeadGlobalPos()
	{
		return GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/HeadRot2Pivot").GlobalTranslation;
	}
	public void HeadLook(Vector3 Pos)
	{
		GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").HeadLookAt(Pos);
	}
	public void ResetLook()
	{
		GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").ResetHead();
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
	//Subscribing to shifting of day to turn on head lamp
	public override void _EnterTree()	{	Sky.GetEnviroment().Connect("DayShift", this, "ToggleNightLight");	}
    public override void _ExitTree()	{	Sky.GetEnviroment().Disconnect("DayShift", this, "ToggleNightLight");	}
    public Character_Animations Anims()	{	return anim;	}
	public string GetCharacterName()	{	return CharacterName;	}
	public void UpdateLocationToMove(Vector3 NewLoc) {	loctomove = NewLoc;	}
	public Vector3 GetMovingLocation()	{	return loctomove;	}
	public bool IsAlive()	{	return m_balive;	}
    public override void _Process(float delta)
    {
        base._Process(delta);
		if (CurrentEnergy <= 0 && IsAlive())
		{
			Kill();
			if (HasVecicle)
			{
				currveh.ToggleMachine(false);
			}
		}
    }
	public void ToggleNightLight(bool toggle)
	{
		if (!IsAlive())
			return;
			
		if (toggle)
			NightLight.LightEnergy = 0;
		else
			NightLight.LightEnergy = 10f;

		BulbMat.EmissionEnabled = !toggle;
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
	public virtual void Respawn()
	{
		ToggleNightLight(CustomEnviroment.IsDay());
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
	}
	public virtual void OnKillFieldDetectorBodyEntered(Node body)	{	Kill();	}
	public void Sit(RemoteTransform pos = null, SittingThing Sitter = null)
	{
		
		if (Sitter != null)
		{
			pos.RemotePath = GetPath();
			anim.ToggleSitting(true);
			chair = Sitter;
			seat = pos;
			Sitter.UpdateOccupation(pos, true);
			GetNode<Spatial>("Pivot").GlobalRotation = pos.GlobalRotation;
			UpdateLocationToMove(pos.GlobalPosition);
		}
		else
			anim.ToggleSitting();
		
		sitting = true;
		
	}
	public void StandUp()
	{
		if (chair != null)
		{
			chair.UpdateOccupation(seat, false);
			chair = null;
			seat.RemotePath = seat.GetPath();
			seat = null;
		}
		if (PlayingInstrument)
		{
			StopMusic();
		}
		IdleTimer.Stop();
		anim.ToggleIdle();
		sitting = false;
	}
	/*public void MoveTo(Vector3 loc)
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
	}*/

	//////////////////////////////////////////////////////////////////////////
	//Vehicle stuff
	[Signal]
    public delegate void VehicleBoardEventHandler(bool toggle, Vehicle veh);

	public bool IsOnVehicle()	{	return HasVecicle;	}
	public Vehicle GetVehicle()	{	return currveh;	}

	public bool GettingInVehicle = false;
	public virtual void OnVehicleBoard(Vehicle veh)
	{
		EmitSignal("VehicleBoardEventHandler", true, veh);
		if (HasEquippedInstrument())
		{
			IdleTimer.Start();
		}
		GettingInVehicle = false;
		//SetCollisionMaskBit(8, true);
	}
	public virtual void OnVehicleUnBoard(Vehicle veh)
	{
		EmitSignal("VehicleBoardEventHandler", false, veh);
		
		loctomove = GlobalTranslation;
		if (PlayingInstrument)
			StopMusic();

		IdleTimer.Stop();
		GettingInVehicle = false;
		//SetCollisionMaskBit(8, false);
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
	public bool HasVehicle()
	{
		return HasVecicle;
	}
	//////////////////////////////////////////////////////////////////////////
	//music stuff

	public virtual void PlayMusic()
	{
		anim.ToggleInstrument(true);
		
		Spatial instrumentparent = GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment/Instrument");
		Instrument bouz = (Instrument)instrumentparent.GetChild(0);
		BoneAttachment instatatchment = GetNode<BoneAttachment>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment");
		instatatchment.GetNode<RemoteTransform>("HolsterAtatchment").RemotePath = instatatchment.GetNode<RemoteTransform>("HolsterAtatchment").GetPath();
		instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").RemotePath = instrumentparent.GetPath();
		
		bouz.ToggleMusic(true);
		bouz.Connect("OnSongEnded", this, "OnSongEnded");
		PlayingInstrument = true;
	}
	public void StopMusic()
	{
		anim.ToggleInstrument(false);

		Spatial instrumentparent = GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment/Instrument");
		Instrument bouz = (Instrument)instrumentparent.GetChild(0);
		bouz.Disconnect("OnSongEnded", this, "OnSongEnded");
		bouz.ToggleMusic(false);
		BoneAttachment instatatchment = GetNode<BoneAttachment>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment");
		instatatchment.GetNode<RemoteTransform>("HolsterAtatchment").RemotePath = instrumentparent.GetPath();
		instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").RemotePath = instatatchment.GetNode<RemoteTransform>("PlayingAtatchment").GetPath();
		PlayingInstrument = false;
	}
	public bool HasEquippedItem()
	{
		return GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment/Instrument").GetChildCount() > 0;
	}
	public bool HasEquippedInstrument()
	{
		var childs = GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment/Instrument").GetChildren();
		bool anyisnt = false;
		for (int i = 0; i < childs.Count; i++)
		{
			if (childs[i] is Instrument)
			{
				anyisnt = true;
			}
		}
		return anyisnt;
	}
	public Item GetEquippedItem()
	{
		return (Item)GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment/Instrument").GetChild(0);
	}
	public void EquipItem(Item it)
	{
		Spatial instrumentspace = GetNode<Spatial>("Pivot/Guy/Armature/Skeleton/InstrumentAtatchment/Instrument");
		it.GetNode<CollisionShape>("CollisionShape").Disabled = true;
		it.RegisterOnIsland = false;
		it.Visible = true;
		instrumentspace.AddChild(it);
		it.Translation = Vector3.Zero;
		it.Rotation = Vector3.Zero;
	}
	public virtual void OnSongEnded(Instrument inst)
	{
		StopMusic();
		if (HasEquippedInstrument())
		{
			IdleTimer.Start();
		}
	}
	public void Idle_Timer_Ended()
	{
		if (GetTalkText().IsTalking())
		{
			IdleTimer.Start();
			return;
		}
		if (IsInsideTree() && HasEquippedInstrument())
			PlayMusic();
	}
	//////////////////////////////////////////////////////////////////////////
	/////Limb stuff
	public void ToggleLimb(LimbType limb, bool toggle)
	{
		LoddedCharacter lod = GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton");
		string limbname = LimbTranslator.EnumToString(limb);
		if (lod.GetCurrentLOD() == 1)
			limbname += "_LOD";

		lod.GetNode<MeshInstance>(limbname).Visible = toggle;
	}
	public void SetClothing ()
	{
		Spatial clothpar = GetNode<Spatial>("Pivot/Guy/Armature/Skeleton");

		if (ClothToWear == ClothingType.HOODIE)
		{
			clothpar.GetNode<MeshInstance>("Hoodie").Visible = true;
			clothpar.GetNode<MeshInstance>("Hoodie").MaterialOverride = ClothMaterial;
			clothpar.GetNode<MeshInstance>("Hoodie_SleeveLess").Visible = false;
		}
		else if (ClothToWear == ClothingType.HOODIE_SLEEVELESS)
		{
			clothpar.GetNode<MeshInstance>("Hoodie_SleeveLess").Visible = true;
			clothpar.GetNode<MeshInstance>("Hoodie_SleeveLess").MaterialOverride = ClothMaterial;
			clothpar.GetNode<MeshInstance>("Hoodie").Visible = false;
		}
	}
	/*public void SetLimbColor(LimbType limb, Color colorarion)
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
	}*/
	/*public bool HasLimbOfType(LimbType type)
	{
		LoddedCharacter lod = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<LoddedCharacter>("Skeleton");
		string name = LimbTranslator.EnumToString(type);
		if (lod.GetCurrentLOD() == 1)
			name += "_LOD"; 
		return GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("Armature").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>(name).Visible;
	}*/
	//////////////////////////////////////////////////////////////////////////
	
	private void On_DialogueButton_Button_Down()
	{
		
	}
	public void SetData(CharacterInfo info)
	{
        Name = info.Name;
        Translation = info.Position;
		CurrentEnergy = info.CurrentEnergy;
		m_balive = info.Alive;
		OwnedVeh = info.OwnedVeh;
		if (info.CustomData.Count > 0)
			GetNode<BaseDialogueScript>("DialogueScript").LoadSaveData(info.CustomData);

		/*for (int i = 0; i < 6; i++)
		{
			SetLimbColor((LimbType)i, info.LimbColors[i]);
		}*/
	}
	////Old stuff
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
}
public class CharacterInfo
{
	public string Name;
	public Vector3 Position;
	public string SceneData;
	public float CurrentEnergy = 0.0f;
	public bool Alive = false;
	public Dictionary<string, object> CustomData = new Dictionary<string, object>();
	//public List<Color> LimbColors = new List<Color>();
	public bool Talked = false;
	public NodePath OwnedVeh;

	//Called to fill the data
	public void UpdateInfo(NPC it)
	{
		Name = it.Name;
		Position = it.Translation;
		SceneData = it.Filename;
		OwnedVeh = it.OwnedVeh;
		CurrentEnergy = it.GetCurrentCharacterEnergy();
		Alive = it.IsAlive();
		Talked = it.Talked;
		BaseDialogueScript diag = it.GetNode<BaseDialogueScript>("DialogueScript");
		Dictionary<string, object> Savedata = diag.GetSaveData();
		foreach(KeyValuePair<string, object> data in Savedata)
		{
			if (CustomData.ContainsKey(data.Key))
				CustomData[data.Key] = data.Value;
			else
				CustomData.Add(data.Key, data.Value);
		}
		/*for (int i = 0; i < 6; i++)
		{
			LimbColors.Add(it.GetLimbColor((LimbType)i));
		}*/
	}
	//Called to get data when saving
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{"Position", Position},
			{"Name", Name},
			{"SceneData", SceneData},
			{"Energy", CurrentEnergy},
			{"Alive", Alive},
			{"Talked", Talked},
			{"OwnedVeh", OwnedVeh}
		};
		/*Color[] LimbColorsAr = new Color[6];
		for (int i = 0; i < 6; i++)
		{
			LimbColorsAr[i] = LimbColors[i];
		}
		data.Add("LimbColors", LimbColorsAr);*/
		string[] CustomDataKeys = new string[CustomData.Count];
		object[] CustomDataValues = new object[CustomData.Count];
		if (CustomData.Count > 0)
		{
			int i = 0;
			foreach (KeyValuePair<string, object> Data in CustomData)
			{
				CustomDataKeys[i] = Data.Key;
				CustomDataValues[i] = Data.Value;
				i++;
			}
			
		}
		data.Add("CustomDataKeys", CustomDataKeys);
		data.Add("CustomDataValues", CustomDataValues);
		return data;
	}
	//Called when loading data
    public void UnPackData(Resource data)
    {
        Position = (Vector3)data.Get("Position");
		Name = (string)data.Get("Name");
		SceneData = (string)data.Get("SceneData");
		CurrentEnergy = (float)data.Get("Energy");
		Alive = (bool)data.Get("Alive");
		Talked = (bool)data.Get("Talked");
		OwnedVeh = (NodePath)data.Get("OwnedVeh");
		/*Godot.Color[] LimbCols = (Godot.Color[])data.Get("LimbColors");
		for (int i = 0; i < 6; i++)
		{
			LimbColors.Add(LimbCols[i]);
		}*/
		string[] CustomDataKeys = (string[])data.Get("CustomDataKeys");
		Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)data.Get("CustomDataValues");

		if (CustomDataKeys.Count() > 0)
		{
			for (int i = 0; i < CustomDataKeys.Count(); i++)
			{
				CustomData.Add(CustomDataKeys[i], CustomDataValues[i]);
			}
		}
    }
}
public enum ClothingType
{
	HOODIE,
	HOODIE_SLEEVELESS,
	
}





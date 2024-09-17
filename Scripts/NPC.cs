using Godot;
using System;

public class NPC : Character
{
	[Export]
	public bool spawnUncon = false;
	[Export]
	public bool Sitting = true;
	[Export]
	NodePath Chair = null;
	[Export]
	bool PlayInstrument = false;
	[Export]
	PackedScene InstrumentToSpawnWith = null;
	//[Export]
    //string FirstDialogue = "TestTimeline";
	//[Export]
	//string RestDialogue = "TestTimeline";
	[Export]
	NodePath DialogueColaborator = null;
	[Export]
	public NodePath OwnedVeh = null;

	public bool Talked;

	public void DoDialogue()
	{
		if (DialogueColaborator == null )
			GetNode<BaseDialogueScript>("DialogueScript").DoDialogue(this, null);
		else
			GetNode<BaseDialogueScript>("DialogueScript").DoDialogue(this, GetNodeOrNull<NPC>(DialogueColaborator));
	}
	Node Skeleton;
	public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
    }
    public override void _Ready()
	{
		#if DEBUG

		if (Engine.EditorHint)
		{
			return;
		}
		#endif

		base._Ready();
		//GetNode<MeshInstance>("Pivot/Guy/Armature/Skeleton/BabyLowpolySurface1").Visible = false;
		if (Sitting)
		{
			SpawnSit();
		}
		if (InstrumentToSpawnWith != null)
		{
			Instrument inst = InstrumentToSpawnWith.Instance<Instrument>();
			EquipItem(inst);
		}
		
		if (PlayInstrument && HasEquippedInstrument())
		{
			PlayMusic();
		}
		
		if (spawnUncon)
		{
			//CurrentEnergy = 0;
			IsUncon = true;
			Kill();
		}


		Skeleton = GetNode("Pivot").GetNode("Guy").GetNode("Armature").GetNode("Skeleton");
		
		//Skeleton.GetNode<BoneAttachment>("InstrumentAtatchment").GetNode<RemoteTransform>("RemoteTransform").RemotePath = inst.GetPath();

		

		//RandomiseLimbs();

		Node par = GetParent();
		while (!(par is Island))
		{
            if (par == null)
				return;
			par = par.GetParent();
		}
		Island ile = (Island)par;
		ile.RegisterChild(this);
	}
	public void DespawnChar()
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "DespawnChar");
		if (OwnedVeh != null)
		{
			GetNode<Spatial>(OwnedVeh).GetNode<Vehicle>("VehicleBody").DespawnVeh();
		}
		
		QueueFree();
		Node par = GetParent();
		while (!(par is Island))
		{
            if (par == null)
				return;
			par = par.GetParent();
		}
		Island ile = (Island)par;
		ile.UnRegisterChild(this);
	}
	public void InitialSpawn()
	{
		//RandomiseLimbs();
	}
	private void SpawnSit()
	{
		if (Chair != null && GetNodeOrNull(Chair) != null)
		{
			SittingThing chair = (SittingThing)GetNode(Chair);
			Sit(chair.GetSeat(), chair);
		}
		else
		{
			SitDown();
		}
	}
	/*void RandomiseLimbs()
	{
		SetLimbColor(LimbType.ARM_L, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.ARM_R, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.LEG_L, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.LEG_R, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.N01_LEG_R, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.N01_LEG_L, LimbRandomColorProvider.GetRandomColor());
	}*/
	public void HighLightObject(bool toggle, Material OutlineMat)
    {
		if (toggle)
        {
            foreach (Node child in Skeleton.GetChildren())
			{
				if (child is MeshInstance mesh)
				{
					mesh.MaterialOverlay = OutlineMat;
				}
			}
        } 
        else
        {
            foreach (Node child in Skeleton.GetChildren())
			{
				if (child is MeshInstance mesh)
				{
					mesh.MaterialOverlay = null;
				}
			}
        }
		
    }
	public void DoAction(Player pl)
	{
		if (IsUncon)
			DialogueManager.GetInstance().ScheduleDialogue(pl, "Είναι απενεργοπηημένη, μπορώ να της δώσω μια μπάταριά.");
			//pl.GetTalkText().Talk("Είναι απενεργοπηημένη, μπορώ να της δώσω μια μπάταριά.");
		else
			DoDialogue();
		//DialogueManager.GetInstance().StartDialogue(this, DoDialogue());
	}
	public void DoAction2(Player pl)
	{
		string text = GetNode<BaseDialogueScript>("DialogueScript").Action1Done(this, pl);
		if (text != "null")
		{
			DialogueManager.GetInstance().ScheduleDialogue(this, text);
			//GetTalkText().Talk(text);
		}
	}
	public void DoAction3(Player pl)
	{
		string text = GetNode<BaseDialogueScript>("DialogueScript").Action2Done(this, pl);
		if (text != "null")
		{
			DialogueManager.GetInstance().ScheduleDialogue(this, text);
			//GetTalkText().Talk(text);
		}
	}
	public string GetActionName(Player pl)
    {
        return LocalisationHolder.GetString("Kουβέντα");
    }
	public bool ShowActionName(Player pl)
    {
        return true;
    }
	public string GetActionName2(Player pl)
    {
		return GetNode<BaseDialogueScript>("DialogueScript").GetExtraActionText();
    }
    public string GetActionName3(Player pl)
    {
		return GetNode<BaseDialogueScript>("DialogueScript").GetExtraActionText2();;
    }
    public bool ShowActionName2(Player pl)
    {
        return GetNode<BaseDialogueScript>("DialogueScript").ShouldShowExtraAction();
    }
    public bool ShowActionName3(Player pl)
    {
        return GetNode<BaseDialogueScript>("DialogueScript").ShouldShowExtraAction2();
    }
	public string GetObjectDescription()
    {
        return LocalisationHolder.GetString("Φίλος");
    }
	public override void Respawn()
	{
		m_balive = true;
		IsUncon = false;
		Start();
		if(Sitting)
		{
			anim.ToggleSitting();
		}
		else
		{
			anim.ToggleIdle();
		}
	}
}

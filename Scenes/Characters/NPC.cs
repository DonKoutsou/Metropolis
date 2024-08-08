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
			Position3D pos = new Position3D();
			AddChild(pos);
			pos.Translation = Vector3.Zero;
			pos.Rotation = Vector3.Zero;
			Sit(pos);
			pos.QueueFree();
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
	public void HighLightObject(Material OutlineMat)
    {
		foreach (Node child in Skeleton.GetChildren())
		{
			if (child is MeshInstance mesh)
			{
				mesh.MaterialOverlay = OutlineMat;
			}
		}
    }
	public void DoAction(Player pl)
	{
		DoDialogue();
		//DialogueManager.GetInstance().StartDialogue(this, DoDialogue());
	}	
	public string GetActionName(Player pl)
    {
        return "Kουβέντα";
    }
	public bool ShowActionName(Player pl)
    {
        return true;
    }
	public string GetObjectDescription()
    {
        return "Φίλος";
    }
	
}

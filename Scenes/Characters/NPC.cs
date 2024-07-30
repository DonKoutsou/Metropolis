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
	Vector3 TalkPosPos = new Vector3(0, 0.5f, 6);
	[Export]
	PackedScene InstrumentToSpawnWith = null;

	[Export]
    string FirstDialogue = "TestTimeline";
	[Export]
	string RestDialogue = "TestTimeline";

	public bool Talked;

	public string GetDialogue()
	{
		if (Talked)
			return RestDialogue;
		else
		{
			Talked = true;
			return FirstDialogue;
		}
			
	}
	Node Skeleton;

	

    public override void _Process(float delta)
    {
		#if DEBUG
		if (Engine.EditorHint)
		{
			GetNode<Position3D>("TalkPosition").Translation = TalkPosPos;
			return;
		}
		#endif
		base._Process(delta);
    }
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
		GetNode<MeshInstance>("Pivot/Guy/Armature/Skeleton/BabyLowpolySurface1").Visible = false;
		if (Sitting)
		{
			SpawnSit();
		}
		if (InstrumentToSpawnWith != null)
		{
			Instrument inst = InstrumentToSpawnWith.Instance<Instrument>();
			AddInstrument(inst);
		}
		
		if (PlayInstrument && HasInstrument())
		{
			PlayMusic();
		}
		
		if (spawnUncon)
		{
			//CurrentEnergy = 0;
			IsUncon = true;
			Kill();
		}
		GetNode<Position3D>("TalkPosition").Translation = TalkPosPos;

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
		RandomiseLimbs();
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
	void RandomiseLimbs()
	{
		SetLimbColor(LimbType.ARM_L, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.ARM_R, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.LEG_L, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.LEG_R, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.N01_LEG_R, LimbRandomColorProvider.GetRandomColor());
		SetLimbColor(LimbType.N01_LEG_L, LimbRandomColorProvider.GetRandomColor());
	}
	public void HighLightObject(bool toggle)
    {
		foreach (Node child in Skeleton.GetChildren())
		{
			if (child is MeshInstance)
			{
				ShaderMaterial Mat = (ShaderMaterial)((MeshInstance)child).MaterialOverlay;
				if (Mat != null)
				{
					Mat.SetShaderParam("enable",  toggle);
				}
				
			}
		}
    }
}

using Godot;
using System;

[Tool]
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

	Node Skeleton;

	Timer IdleTimer;

	public void Idle_Timer_Ended()
	{
		PlayMusic();
	}

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

		Node par = GetParent();
		while (!(par is Island))
		{
            if (par == null)
				return;
			par = par.GetParent();
		}
		Island ile = (Island)par;
		ile.RegisterChild(this);

		if (Sitting)
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
		if (PlayInstrument)
		{
			CallDeferred("PlayMusic");
		}
		
		if (spawnUncon)
		{
			CurrentEnergy = 0;
		}
		GetNode<Position3D>("TalkPosition").Translation = TalkPosPos;

		Skeleton = GetNode("Pivot").GetNode("Guy").GetNode("Armature").GetNode("Skeleton");

		IdleTimer = GetNode<Timer>("IdleTimer");
	}
	
	public override void OnSongEnded(Instrument inst)
	{
		base.OnSongEnded(inst);
		IdleTimer.Start();
	}
	public void HighLightObject(bool toggle)
    {
		foreach (Node child in Skeleton.GetChildren())
		{
			if (child is MeshInstance)
			{
				ShaderMaterial Mat = (ShaderMaterial)((MeshInstance)child).GetActiveMaterial(0).NextPass;
				if (Mat != null)
				{
					Mat.SetShaderParam("enable",  toggle);
				}
				
			}
		}
    }
}

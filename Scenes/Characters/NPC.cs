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
	bool PlayingInstrument = false;
	[Export]
	Vector3 TalkPosPos = new Vector3(0, 0.5f, 6);

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
		anim.ToggleInstrument(PlayingInstrument);
		GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Bouzouki>("Bouzouki").ToggleMusic(PlayingInstrument);
		
		if (spawnUncon)
		{
			CurrentEnergy = 0;
		}
		GetNode<Position3D>("TalkPosition").Translation = TalkPosPos;
	}
	#if DEBUG
    public override void _PhysicsProcess(float delta)
    {
		
		if (Engine.EditorHint)
		{
			return;
		}
        base._PhysicsProcess(delta);
    }
	#endif
	public void HighLightObject(bool toggle)
    {
		Node skel = GetNode("Pivot").GetNode("Guy").GetNode("Armature").GetNode("Skeleton");
		foreach (Node child in skel.GetChildren())
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

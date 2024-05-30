using Godot;
using System;

public class NPC : Character
{
	[Export]
	bool spawnUncon = false;
	[Export]
	bool Sitting = true;
	[Export]
	NodePath Chair = null;
	[Export]
	bool PlayingInstrument = false;

	public override void _Ready()
	{
		base._Ready();
		if (Sitting)
		{
			
			if (Chair != null)
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
	}
}

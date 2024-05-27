using Godot;
using System;

public class NPC : Character
{
	[Export]
	bool spawnUncon = false;
	[Export]
	bool Sitting = true;
	[Export]
	bool PlayingInstrument = false;

	public override void _Ready()
	{
		base._Ready();
		if (Sitting)
			anim.ToggleSitting();
		anim.ToggleInstrument(PlayingInstrument);
		GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Bouzouki>("Bouzouki").ToggleMusic(PlayingInstrument);
		
		if (spawnUncon)
		{
			CurrentEnergy = 0;
		}
	}
}

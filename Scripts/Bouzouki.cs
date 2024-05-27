using Godot;
using System;

public class Bouzouki : Item
{
	public void ToggleMusic(bool Toggle)
	{
		Visible = Toggle;
		GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Playing = Toggle;
	}
}

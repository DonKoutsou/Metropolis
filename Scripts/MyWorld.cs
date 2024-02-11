using Godot;
using System;



public class MyWorld : Spatial
{
	Player pl;
	public override void _Ready()
	{
		base._Ready();
		pl = GetNode<Player>("Player");
	}
	public void OnPlayerKilled()
	{
		StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();
		start.GameOver();
	}
	public Player GetPlayer()
	{
		return pl;
	}

};

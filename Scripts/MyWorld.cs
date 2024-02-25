using Godot;
using System;
using System.Collections.Generic;



public class MyWorld : Spatial
{
	Player pl;

	float d = 100;

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
	public void RegisterIle(Island ile)
	{
		//ile.Hide();
		AddChild(ile);
		ile.Init();
		ToggleIsland(ile, false);
		GD.Print("Spawned Island on locations: " + ile.loctospawnat);
	}
	public override void _Process(float delta)
	{
		d -= delta;
		if (d > 0)
			return;
	}
	public static void IleTransition(Island from, Island to)
	{
		GD.Print("Transitioning from : " + from.Name + " to " + to.Name);
		List<Island> closestfrom;
		from.GetClosestIles(out closestfrom);

		List<Island> closestto;
		to.GetClosestIles(out closestto);
		for (int i = 0; i < closestfrom.Count; i ++)
		{
			if (closestfrom[i] == to)
				continue;
			if (closestto.Contains(closestfrom[i]))
				continue;
			else
			{
				ToggleIsland(closestfrom[i], false);
			}
		}
		for (int i = 0; i < closestto.Count; i ++)
		{
			ToggleIsland(closestto[i], true);
		}
		GD.Print("-----------Transition Finished------------");
	}
	public static void ToggleIsland(Island ile, bool Toggle, bool affectneigh = false)
	{
		if (Toggle)
		{
			ile.EnableIsland();
		}
		else
		{
			ile.DeactivateIsland();
		}
		if (affectneigh)
		{
			List<Island> closestto;
			ile.GetClosestIles(out closestto);
			for (int i = 0; i < closestto.Count; i ++)
			{
				ToggleIsland(closestto[i], Toggle);
			}
		}
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause"))
		{
			StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();
			if (!GetTree().Paused)
			{
				GetTree().Paused = true;
				start.Pause(true);
			}
			else
			{
				GetTree().Paused = false;
				start.Pause(false);
			}
				
		}
	}
};

using Godot;
using System;
using System.Collections.Generic;



public class MyWorld : Spatial
{
	Player pl;

	float d = 1;

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
		ToggleIsland(ile, false);
		GD.Print("Spawned Island on locations: " + ile.loctospawnat);
	}
	public override void _Process(float delta)
	{
		d -= delta;
		if (d <= 0)
		{
			d = 1;
			if (ilestodissable.Count > 0)
			{
				Island ile = ilestodissable[ilestodissable.Count - 1];
				ToggleIsland(ile, false);
				ilestodissable.Remove(ile);
			}
			else if (ilestoenable.Count > 0)
			{
				Island ile = ilestoenable[ilestoenable.Count - 1];
				ToggleIsland(ilestoenable[ilestoenable.Count - 1], true);
				ilestoenable.Remove(ile);
			}
				
		}
	}
	static List<Island> ilestodissable = new List<Island>();
	static List<Island> ilestoenable = new List<Island>();
	public static void IleTransition(Island from, Island to)
	{
		GD.Print("Transitioning from : " + from.Name + " to " + to.Name);
		List<Island> closestfrom;
		WorldMap.GetClosestIles(from,out closestfrom);
		//from.GetClosestIles(out closestfrom);

		List<Island> closestto;
		WorldMap.GetClosestIles(to,out closestto);
		//to.GetClosestIles(out closestto);
		for (int i = 0; i < closestfrom.Count; i ++)
		{
			if (closestfrom[i] == to)
				continue;
			if (closestto.Contains(closestfrom[i]))
				continue;
			else
			{
				ilestodissable.Insert(ilestodissable.Count, closestfrom[i]);
				//ToggleIsland(closestfrom[i], false);
			}
		}
		for (int i = 0; i < closestto.Count; i ++)
		{
			//ToggleIsland(closestto[i], true);
			ilestoenable.Insert(ilestoenable.Count, closestto[i]);
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
			WorldMap.GetClosestIles(ile, out closestto);
			//ile.GetClosestIles(out closestto);
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

using Godot;
using System;
using System.Collections.Generic;



public class MyWorld : Spatial
{
	Player pl;

	List <Island> iles = new List<Island>();
	static List <Island> EnalbedIles = new List<Island>();
	static List <Island> DissabledIles = new List<Island>();

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
		AddChild(ile);
		iles.Insert(iles.Count, ile);
		ile.DeactivateIsland();
		DissabledIles.Insert(DissabledIles.Count, ile);
		GD.Print("Spawned Island on locations: " + ile.loctospawnat);
	}
	public override void _Process(float delta)
	{
		d -= delta;
		if (d > 0)
			return;
	}
	Island GetClosestIslandToPl()
    {
        float mindist = 999999999.0f;
		Island closestile = null;

		Vector3 plpos = pl.GlobalTransform.origin;

		foreach (Island ile in iles)
		{
			var dist = plpos.DistanceTo( ile.GlobalTransform.origin );
			if (dist < mindist)
			{
				mindist = dist; 
				closestile = ile;
			}
		}
		return closestile;
    }
	public static void IleTransition(Island from, Island to)
	{
		//if (EnalbedIles.Contains(from))
		//{
		//	EnalbedIles.Remove(from);
		//}
		GD.Print("Transitioning from : " + from.Name + " to " + to.Name);
		List<Island> closestfrom;
		from.GetClosestIles(out closestfrom);

		if (DissabledIles.Contains(to))
		{
			DissabledIles.Remove(to);
		}
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
				if (DissabledIles.Contains(closestfrom[i]))
					continue;
				ToggleIsland(closestfrom[i], false);
			}
		}
		for (int i = 0; i < closestto.Count; i ++)
		{
			if (EnalbedIles.Contains(closestto[i]))
					continue;
			ToggleIsland(closestto[i], true);
		}
		GD.Print("-----------Transition Finished------------");
	}
	public static void ToggleIsland(Island ile, bool Toggle, bool affectneigh = false)
	{
		if (Toggle)
		{
			if (EnalbedIles.Contains(ile))
				return;
			ile.EnableIsland();
			EnalbedIles.Insert(EnalbedIles.Count, ile);
			if (DissabledIles.Contains(ile))
				DissabledIles.Remove(ile);
		}
		else
		{
			if (DissabledIles.Contains(ile))
				return;
			ile.DeactivateIsland();
			DissabledIles.Insert(DissabledIles.Count, ile);
			if (EnalbedIles.Contains(ile))
				EnalbedIles.Remove(ile);
		}
		if (affectneigh)
		{
			List<Island> closestto;
			ile.GetClosestIles(out closestto);
			for (int i = 0; i < closestto.Count; i ++)
			{
				if (EnalbedIles.Contains(closestto[i]))
						continue;
				ToggleIsland(closestto[i], true);
			}
		}
	}
};

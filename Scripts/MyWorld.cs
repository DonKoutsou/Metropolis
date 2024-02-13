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
		iles.Insert(iles.Count, ile);
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
		(from).DeactivateIsland();
		DissabledIles.Insert(DissabledIles.Count, from);
		if (EnalbedIles.Contains(from))
		{
			EnalbedIles.Remove(from);
		}
		List<Island> closestfrom;
		from.GetClosestIles(out closestfrom);
		to.EnableIsland();
		EnalbedIles.Insert(EnalbedIles.Count, to);
		if (DissabledIles.Contains(to))
		{
			DissabledIles.Remove(to);
		}
		List<Island> closestto;
		to.GetClosestIles(out closestto);
		for (int i = 0; i < closestfrom.Count; i ++)
		{
			if (closestto.Contains(closestfrom[i]))
				continue;
			else
			{
				if (DissabledIles.Contains(closestfrom[i]))
					continue;
				closestfrom[i].DeactivateIsland();
				DissabledIles.Insert(DissabledIles.Count, closestfrom[i]);
				if (EnalbedIles.Contains(closestfrom[i]))
				{
					EnalbedIles.Remove(closestfrom[i]);
				}
			}	
		}
		for (int i = 0; i < closestto.Count; i ++)
		{
			if (EnalbedIles.Contains(closestto[i]))
					continue;
			closestto[i].EnableIsland();
			EnalbedIles.Insert(EnalbedIles.Count, closestto[i]);
			if (DissabledIles.Contains(closestto[i]))
			{
				DissabledIles.Remove(closestto[i]);
			}
		}
	}
};

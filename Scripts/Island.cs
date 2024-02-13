using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Island : Spatial
{
	[Export]
	bool m_bOriginalIle = false;
	public Door[] a_doors;
	List<Island> closeislands;

	List<Character> m_enem = new List<Character>();

	WorldMap map;

	bool m_enabled = false;

	bool Inited = false;

	public void RegisterChar(Character en)
	{
		if (en is Player)
			return;
		if (!m_enem.Contains(en))
		{
			m_enem.Insert(m_enem.Count(), en);
			if (m_enabled)
				en.Start();
			else
				en.Stop();
		}
	}
	public void GetClosestIles(out List<Island> closestiles)
	{
		closestiles = new List<Island>();
		for (int i = 0; i < closeislands.Count; i ++)
		{
			closestiles.Insert(i, closeislands[i]);
		}
	}
	public override void _Ready()
	{
		a_doors = new Door[4];
		var door__left = (Door)GetNode<Door>("Door_Left");
		a_doors[0] = door__left;
		var door__down = (Door)GetNode<Door>("Door_Down");
		a_doors[1] = door__down;
		var door__top = (Door)GetNode<Door>("Door_Top");
		a_doors[2] = door__top;
		var door__right = (Door)GetNode<Door>("Door_Right");
		a_doors[3] = door__right;

		map = GetParent().GetNode<WorldMap>("WorldMap");
		
		//m_enem.Insert(0, GetNode<Enemy>("Enemy"));

		if (!m_bOriginalIle)
			DeactivateIsland();
		else
			EnableIsland();
		
	}
	void Init()
	{
		Door[] doors; 
		GetIslandDoors(out doors);

		FindSiblings();

		AssignDoors();

		DissableUnusedDoors();

		var childer = GetChildren();
		//for (int i = 0; i < childer.Count; i ++)
		//{
		//	if (childer[i] is DecorationMap)
		//	{
		//		((DecorationMap)childer[i]).Dothin();
		//	}
		//}
		
		Inited = true;

		
	}

	void DissableUnusedDoors()
	{
		for (int f = 0; f < a_doors.Count(); f++)
		{
			Island ile;
			a_doors[f].GetIslandAcces(out ile);
			if (ile == null)
				a_doors[f].Hide();
		}
		
	}

	public Door GetClosestDoor(Vector3 GlobalPos)
	{
		int f = 0;
		Door closestdoor = a_doors[0];
		float closestdist = GlobalPos.DistanceTo(a_doors[0].GlobalTransform.origin);
		for (f = 0; f < a_doors.Count(); f++)
		{
			Vector3 doorloc = a_doors[f].GlobalTransform.origin;
			float dist = GlobalTransform.origin.DistanceTo(doorloc);
			if (dist > closestdist)
				continue;
			closestdoor = a_doors[f];
			closestdist = dist;
		}

		return closestdoor;


	}

	public void GetIslandDoors(out Door[] doors)
	{
		doors = new Door[4]; 
		doors = (Door[])a_doors.Clone();
	}
	void AssignDoors()
	{
		int i = 0;
		Vector2 mypos = new Vector2(GlobalTransform.origin.x, GlobalTransform.origin.z) ;
		for (i = 0; i < closeislands.Count(); i++)
		{
			Vector2 islandloc = new Vector2(closeislands[i].GlobalTransform.origin.x, closeislands[i].GlobalTransform.origin.z);

			Door closestdoor = null;
			//down
			if (islandloc.y > mypos.y)
			{
				closestdoor = a_doors[1];
			}
			//up
			if (islandloc.y < mypos.y)
			{
				closestdoor = a_doors[2];
			}
			//right
			if (islandloc.x > mypos.x)
			{
				closestdoor = a_doors[3];
			}
			//left
			if (islandloc.x < mypos.x)
			{
				closestdoor = a_doors[0];
			}
			if (closestdoor == null)
				continue;
			closestdoor.SetIslandToAccess(closeislands[i]);
		}
	}

	void ScrambleDoorDestinations()
	{
		List<Island> islands = new List<Island>();
		List <Door> doors = new List<Door>();
		int failed = 0;
		for (int f = 0; f < a_doors.Count(); f++)
		{
			Island ile = null;
			a_doors[f].GetIslandAcces(out ile);
			if (ile == null)
			{
				failed = failed + 1;
				continue;
			}
			islands.Insert(f - failed, ile);
			doors.Insert(f - failed, a_doors[f]);
		}
		for (int f = 0; f < doors.Count(); f++)
		{
			Random rnd = new Random();
			int r = rnd.Next(islands.Count);
			doors[f].SetIslandToAccess(islands[r]);
			islands.Remove(islands[r]);
		}
	}
	
	void FindSiblings()
	{
		var islands = GetTree().GetNodesInGroup("Islands");
		Island[] isnaldarray = new Island[islands.Count];
		islands.CopyTo(isnaldarray, 0);
		
		int i;
		int closei = 0;
		float mindistance = 0;

		closeislands = new List<Island>();

		Vector2 mypos = new Vector2(GlobalTransform.origin.x, GlobalTransform.origin.z);
		for (i = 0; i < isnaldarray.Count(); i++)
		{
			if (isnaldarray[i] == this)
				continue;
			
			Vector2 Islandloc = new Vector2(isnaldarray[i].GlobalTransform.origin.x, isnaldarray[i].GlobalTransform.origin.z); ;
			float dist = mypos.DistanceTo(Islandloc);

			if (mypos.x != Islandloc.x && mypos.y != Islandloc.y)
				continue;

			//GD.Print(dist);

			if (mindistance == 0)
				mindistance = dist;

			if (dist > mindistance)
				continue;
			if (dist < mindistance)
			{
				mindistance = dist;
				closeislands.Clear();
				closei = 0;
				closeislands = new List<Island>();
			}
			int f = closeislands.Count();
			closeislands.Insert(closei, isnaldarray[i]);
			closei = closei + 1;
		}
	}
	public virtual void EnableIsland()
	{
		if (!Inited)
		{
			Init();
		}
		for (int i = 0;i < closeislands.Count; i++)
			{
				if (!closeislands[i].Inited)
				{
					closeislands[i].Init();
				}
			}
		if (map.HideBasedOnState)
			Show();
		ToggleEnemies(true);
		m_enabled = true;
	}
	public void DeactivateIsland()
	{
		if (map.HideBasedOnState)
			Hide();
		ToggleEnemies(false);
		m_enabled = false;
	}
	void ToggleEnemies(bool toggle)
	{
		if (m_enem.Count == 0)
			return;
		foreach (Character enem in m_enem)
		{
			if (enem != null)
			{
				if (!toggle)
					enem.CallDeferred("Stop");
				else
					enem.CallDeferred("Start");
			}
		}
		
	}
	public override void _Process(float delta)
	{
		 
	}
}

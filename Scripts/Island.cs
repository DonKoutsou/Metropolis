using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Island : Spatial
{
	[Export]
	public bool m_bOriginalIle = false;
	public Door[] a_doors;
	List<Island> closeislands;

	List<Character> m_enem = new List<Character>();

	WorldMap map;

	bool m_enabled = false;

	public bool Inited = false;

	public Vector3 loctospawnat;

	GrassCubes grass;

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
		if (closeislands.Count == 0)
		{
			GD.Print("Island has no close islands, somehting is off");
			return;
		}
		for (int i = 0; i < closeislands.Count; i ++)
		{
			closestiles.Insert(i, closeislands[i]);
		}
	}
	public override void _Ready()
	{
		GlobalTranslation = loctospawnat;
		a_doors = new Door[4];
		var door__left = (Door)GetNode<Door>("Door_Left");
		a_doors[0] = door__left;
		var door__down = (Door)GetNode<Door>("Door_Down");
		a_doors[1] = door__down;
		var door__top = (Door)GetNode<Door>("Door_Top");
		a_doors[2] = door__top;
		var door__right = (Door)GetNode<Door>("Door_Right");
		a_doors[3] = door__right;

		grass = GetNode<GrassCubes>("Grass");
		map = GetParent().GetNode<WorldMap>("WorldMap");
	}
	public void Init()
	{

		FindSiblings();

		AssignDoors();

		DissableUnusedDoors();

		Inited = true;
	}
	void DissableUnusedDoors()
	{
		for (int f = 0; f < a_doors.Count(); f++)
		{
			Island ile;
			a_doors[f].GetIslandAcces(out ile);
			if (ile == null)
				a_doors[f].Toggle(false);
		}
	}
	public void AddCloseIle(Island ile)
	{
		if (ile == this)
			return;
		if (closeislands == null || closeislands.Contains(ile))
			return;
		closeislands.Insert(closeislands.Count, ile);
	}

	public Door GetClosestDoor(Vector3 GlobalPos)
	{
		int f = 0;
		Door closestdoor = a_doors[0];
		float closestdist = GlobalPos.DistanceTo(a_doors[0].GlobalTransform.origin);
		for (f = 0; f < a_doors.Count(); f++)
		{
			Vector3 doorloc = a_doors[f].GlobalTransform.origin;
			float dist = GlobalPos.DistanceTo(doorloc);
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
		for (i = 0; i < closeislands.Count; i++)
		{
			Vector2 islandloc = new Vector2(closeislands[i].GlobalTransform.origin.x, closeislands[i].GlobalTransform.origin.z);
			if (closeislands[i].GlobalTransform.origin.DistanceTo(GlobalTransform.origin) > 2100)
				continue;
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
			closestdoor.Toggle(true);
			closestdoor.SetIslandToAccess(closeislands[i]);
		}
	}
	public bool HasIslandInClose(Island ile)
	{
		if (closeislands == null)
			return false;
		return closeislands.Contains(ile);
	}
	void FindSiblings()
	{
		var islands = GetTree().GetNodesInGroup("Islands");
		Island[] isnaldarray = new Island[islands.Count];
		islands.CopyTo(isnaldarray, 0);
		float mindistance = 0;

		closeislands = new List<Island>();

		Vector3 mypos = GlobalTransform.origin;
		for (int i = 0; i < isnaldarray.Count(); i++)
		{
			if (isnaldarray[i] == this)
				continue;
			
			Vector3 Islandloc = isnaldarray[i].GlobalTransform.origin;
			float dist = mypos.DistanceTo(Islandloc);
			
			if (dist > 2100)
				continue;

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
				closeislands = new List<Island>();
			}
			closeislands.Insert(closeislands.Count, isnaldarray[i]);
		}
		mindistance = 0;
		List<Island> closeislands2 = new List<Island>();
		for (int i = 0; i < isnaldarray.Count(); i++)
		{
			if (isnaldarray[i] == this)
				continue;
			if (closeislands.Contains(isnaldarray[i]))
				continue;
			
			Vector3 Islandloc = isnaldarray[i].GlobalTransform.origin;
			float dist = mypos.DistanceTo(Islandloc);

			if (dist > 3000)
				continue;

			if (mypos.x == Islandloc.x && mypos.y == Islandloc.y)
				continue;

			//GD.Print(dist);

			if (mindistance == 0)
				mindistance = dist;

			if (dist > mindistance)
				continue;
			if (dist < mindistance)
			{
				mindistance = dist;
				closeislands2.Clear();
				closeislands2 = new List<Island>();
			}
			closeislands2.Insert(closeislands2.Count, isnaldarray[i]);
		}
		for (int i = 0; i < closeislands2.Count(); i++)
		{
			closeislands.Insert(closeislands.Count, closeislands2[i]);
		}
		for (int i = 0; i < closeislands.Count(); i++)
		{
			if (!closeislands[i].HasIslandInClose(this))
				closeislands[i].AddCloseIle(this);
		}
	}
	public virtual void EnableIsland()
	{
		if (m_enabled)
			return;
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
		foreach (Door d in a_doors)
		{
			d.ToggleCollisions(true);
		}
		grass.ToggleGrass(true);
		ToggleEnemies(true);
		m_enabled = true;
	}
	public void DeactivateIsland()
	{
		if (map.HideBasedOnState)
			Hide();
		foreach (Door d in a_doors)
		{
			d.ToggleCollisions(false);
		}
		ToggleEnemies(false);
		grass.ToggleGrass(false);
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
}

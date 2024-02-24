using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Island : Spatial
{
	[Export]
	public bool m_bOriginalIle = false;
	List<Island> closeislands;

	List<Character> m_enem = new List<Character>();

	WorldMap map;

	bool m_enabled = false;

	public bool Inited = false;

	public Vector3 loctospawnat;

	public float rotationtospawnwith;

	GrassCubes grass;

	Spatial Terain;

	//NavigationMeshInstance navmesh;

	Sea waterbody;
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
		//Apply rotation random
		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(rotationtospawnwith));
		//Transform.Rotated(new Vector3(0, 1, 0), rotationtospawnwith);

		//navmesh = GetNode<NavigationMeshInstance>("NavigationMeshInstance");
		Terain = GetNodeOrNull<Spatial>("HTerrain");
		waterbody = GetNodeOrNull<Sea>("SeaBed");


		grass = GetNodeOrNull<GrassCubes>("Grass");
		map = GetParent().GetNode<WorldMap>("WorldMap");
	}
	public void Init()
	{

		FindSiblings();


		Inited = true;
	}

	public void AddCloseIle(Island ile)
	{
		if (ile == this)
			return;
		if (closeislands == null || closeislands.Contains(ile))
			return;
		closeislands.Insert(closeislands.Count, ile);
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
		{
			Show();
		}
		if (Terain != null)
		{
			Terain.SetProcess(true);
		}	

		if (grass != null)
			grass.ToggleGrass(true);
		ToggleEnemies(true);
		//if (navmesh != null)
			//navmesh.Enabled = true;
		if (waterbody != null)
			waterbody.Start();
		m_enabled = true;
	}
	public void DeactivateIsland()
	{
		if (map.HideBasedOnState)
		{
			Hide();
		}
		if (Terain != null)
		{
			Terain.SetProcess(false);
		}

		ToggleEnemies(false);
		if (grass != null)
			grass.ToggleGrass(false);
		//if (navmesh != null)
			//navmesh.Enabled = false;
		if (waterbody != null)
			waterbody.Stop();
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

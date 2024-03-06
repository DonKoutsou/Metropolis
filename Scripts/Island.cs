using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Island : Spatial
{
	[Export]
	public bool m_bOriginalIle = false;

	List<Character> m_enem = new List<Character>();

	WorldMap map;

	public bool m_enabled = false;

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
	public override void _Ready()
	{
		GlobalTranslation = loctospawnat;
		//Apply rotation random
		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(rotationtospawnwith));
		Transform.Rotated(new Vector3(0, 1, 0), rotationtospawnwith);

		//navmesh = GetNode<NavigationMeshInstance>("NavigationMeshInstance");
		Terain = GetNodeOrNull<Spatial>("HTerrain");
		waterbody = GetNodeOrNull<Sea>("SeaBed");
		if (waterbody != null)
			waterbody.GlobalRotation = new Vector3 (0.0f, 0.0f, 0.0f);

		grass = GetNodeOrNull<GrassCubes>("Grass");
		map = GetParent().GetNode<WorldMap>("WorldMap");
	}
    public override void _Process(float delta)
    {
        base._Process(delta);

    }

    public virtual void EnableIsland()
	{
		if (m_enabled)
			return;

		if (map.HideBasedOnState)
		{
			Show();
		}
		if (Terain != null)
			Terain.SetProcess(true);

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
			Terain.SetProcess(false);

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

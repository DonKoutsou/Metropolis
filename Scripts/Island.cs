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

	Spatial Terain;
	Spatial TerainDetail;

	//NavigationMeshInstance navmesh;
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
		if (Terain != null)
		{
			TerainDetail = Terain.GetNodeOrNull<Spatial>("HTerrainDetailLayer");
		}
		StaticBody waterbody = GetNodeOrNull<StaticBody>("SeaBed");
		if (waterbody != null)
			waterbody.GlobalRotation = new Vector3 (0.0f, 0.0f, 0.0f);

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
		{
			Terain.SetProcess(true);
			if (TerainDetail != null)
			{
				TerainDetail.SetProcess(true);
			}
		}

		ToggleEnemies(true);
		//if (navmesh != null)
			//navmesh.Enabled = true;

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
			if (TerainDetail != null)
			{
				TerainDetail.SetProcess(false);
			}
		}
			

		ToggleEnemies(false);

		//if (navmesh != null)
			//navmesh.Enabled = false;
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

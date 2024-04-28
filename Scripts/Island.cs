using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Island : Spatial
{
	[Export]
	public bool m_bOriginalIle = false;
	[Export]
	IleType type = IleType.LAND;

	public bool Inited = false;

	private Vector3 SpawnGlobalLocation;

	private float SpawnRotation;

	List<House> Houses = new List<House>();

	List<WindGenerator> Generators = new List<WindGenerator>();
	
	public override void _Ready()
	{
		GlobalTranslation = SpawnGlobalLocation;

		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(SpawnRotation));
		Transform.Rotated(new Vector3(0, 1, 0), SpawnRotation);

		StaticBody waterbody = GetNodeOrNull<StaticBody>("SeaBed");
		if (waterbody != null)
			waterbody.GlobalRotation = new Vector3 (0.0f, 0.0f, 0.0f);
		FindHouses(this);
		FindGenerators(this);
	}
	public void SetSpawnInfo(Vector3 SpawnPos, float SpawnRot)
	{
		SpawnGlobalLocation = SpawnPos;
		SpawnRotation = SpawnRot;
	}
	public IleType GetIslandType()
	{
		return type;
	}
	public void InputData(IslandInfo data)
	{
		foreach (House hou in Houses)
		{
			foreach(HouseInfo Hnfo in data.Houses)
			{
				if (hou.Name == Hnfo.HouseName)
				{
					hou.InputData(Hnfo);
				}
			}
		}
		foreach (WindGenerator gen in Generators)
		{
			foreach(WindGeneratorInfo GenInfo in data.Generators)
			{
				if (gen.Name == GenInfo.WindGeneratorName)
				{
					gen.SetData(GenInfo);
				}
			}
		}
	}
	public void InitialSpawn(Random r)
	{
		foreach(House h in Houses)
		{
			h.StartHouse(r);
		}
	}
	private void FindHouses(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is House)
				Houses.Insert(Houses.Count, (House)child);
			else
				FindHouses(child);
		}
	}
	public void GetHouses(out List<House> hs)
	{
		hs = new List<House>();
		for (int i = 0; i < Houses.Count; i++)
		{
			hs.Insert(i, Houses[i]);
		}
	}
	private void FindGenerators(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is WindGenerator)
				Generators.Insert(Generators.Count, (WindGenerator)child);
			else
				FindGenerators(child);
		}
	}
	public void GetGenerator(out List<WindGenerator> wg)
	{
		wg = new List<WindGenerator>();
		for (int i = 0; i < Generators.Count; i++)
		{
			wg.Insert(i, Generators[i]);
		}
	}
	
}
public enum IleType
{
	ENTRANCE,
	LAND,
	EXIT,
	SEA,
	LIGHTHOUSE
}
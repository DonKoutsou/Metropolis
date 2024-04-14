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

	public bool Inited = false;

	public Vector3 loctospawnat;

	public float rotationtospawnwith;

	Spatial Terain;
	Spatial TerainDetail;

	List<House> Houses = new List<House>();

	public override void _Ready()
	{
		GlobalTranslation = loctospawnat;

		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(rotationtospawnwith));
		Transform.Rotated(new Vector3(0, 1, 0), rotationtospawnwith);

		StaticBody waterbody = GetNodeOrNull<StaticBody>("SeaBed");
		if (waterbody != null)
			waterbody.GlobalRotation = new Vector3 (0.0f, 0.0f, 0.0f);

		map = GetParent().GetNode<WorldMap>("WorldMap");
		FindHouses(this);
	}
	public void InputData(IslandInfo data)
	{
		foreach (House hou in Houses)
		{
			foreach(HouseInfo HouseInfo in data.Houses)
			{
				if (hou.Name == HouseInfo.HouseName)
				{
					hou.InputData(HouseInfo);
				}
			}
		}
	}
	public void InitIle(IslandInfo info)
	{

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
}

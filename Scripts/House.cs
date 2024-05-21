using Godot;
using System;
using System.Collections.Generic;

public class House : Spatial
{
	[Export]
	bool spawnItems = true;
	[Export]
	public PackedScene[] ItemSpawnPool;
	List<Furniture> FurnitureList = new List<Furniture>();

	StaticBody HouseExterior;
	Area HouseDoor;
	public override void _Ready()
	{
		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				FurnitureList.Insert(FurnitureList.Count, (Furniture)nd);
		}
		HouseDoor = GetNode<Area>("HouseDoor");
		HouseExterior = GetNode<StaticBody>("HouseExterior");
		Node parent = GetParent();
		
		while (!(parent is Island))
		{
			if (parent == null)
				return;
			parent = parent.GetParent();
		}
		Island ile = (Island)parent;
		ile.RegisterChild(this);
	}
	public void Touch(Node body)
	{
		Vector3 forw = HouseDoor.GlobalTransform.basis.z;
		Vector3 toOther = HouseDoor.GlobalTransform.origin - ((Spatial)body).GlobalTransform.origin;
		var thing = forw.Dot(toOther);
		if (thing > 0)
		{
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(2)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
			return;
		}
		else
		{
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(2)).ParamsCullMode = SpatialMaterial.CullMode.Front;
			return;
		}
	}
	public void StartHouse(Random random)
	{
		if (!spawnItems)
			return;
		if (ItemSpawnPool == null)
			return;

		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				FurnitureList.Insert(FurnitureList.Count, (Furniture)nd);
		}
		for (int i = 0; i < FurnitureList.Count; i++)
		{
			int start = random.Next(0, ItemSpawnPool.Length + 1);
			if (start >= ItemSpawnPool.Length)
				continue;

			FurnitureList[i].SpawnItem(ItemSpawnPool[start]);
		}
	}
	public void GetFurniture(out List<Furniture> furniture)
	{
		furniture = new List<Furniture>();
		for (int i = 0; i < FurnitureList.Count; i++)
		{
			furniture.Insert(i, FurnitureList[i]);
		}
	}
	public void InputData(HouseInfo data)
	{
		foreach (Furniture furni in FurnitureList)
		{
			foreach(FurnitureInfo Finfo in data.furni)
			{
				if (furni.Name == Finfo.FunritureName)
				{
					furni.SetData(Finfo);
				}
			}
		}
	}
}
public class HouseInfo
{
    public string HouseName;

    public List<FurnitureInfo> furni = new List<FurnitureInfo>();
    public void UpdateInfo(List<Furniture> funriture)
    {
        foreach(FurnitureInfo GInfo in furni)
        {
            Furniture f = null;
            foreach (Furniture fu in funriture)
            {
                if (fu.Name == GInfo.FunritureName)
                {
                    f = fu;
                    break;
                }
            }
            GInfo.UpdateInfo(f);

        }
    }
    public void SetInfo(string name, List<FurnitureInfo> funriture)
    {
        HouseName = name;
        for (int i = 0; i < funriture.Count; i++)
        {
            furni.Insert(i, funriture[i]);
        }
    }
}
public class FurnitureInfo
{
    public string FunritureName;
    public bool Searched;
    public bool HasItem;
    public ItemName item;
    public void UpdateInfo(Furniture furn)
    {
        Searched = furn.Searched;
    }

    public void SetInfo(string name, bool srch, bool hasI, ItemName it)
    {
        FunritureName = name;
        Searched = srch;
        HasItem = hasI;
        item = it;
    }
}
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class House : Spatial
{
	[Export]
	bool spawnItems = true;
	[Export]
	public PackedScene[] ItemSpawnPool;
	List<Furniture> FurnitureList = new List<Furniture>();

	public StaticBody HouseExterior;

	public override void _Ready()
	{
		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				FurnitureList.Insert(FurnitureList.Count, (Furniture)nd);
		}
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
	public virtual void Entered(Node body)
	{
		((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(2)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		GetNode<Occluder>("Occluder").Visible = false;
		GetNode<Occluder>("Occluder2").Visible = false;
		GetNode<Occluder>("Occluder3").Visible = false;
		GetNode<Occluder>("Occluder4").Visible = false;
		GetNode<Occluder>("Occluder5").Visible = false;
		GetNode<Occluder>("Occluder6").Visible = false;
		GetNode<Occluder>("Occluder7").Visible = false;

		
		return;

	}
	public virtual void Left(Node body)
	{
		((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(2)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		GetNode<Occluder>("Occluder").Visible = true;
		GetNode<Occluder>("Occluder2").Visible = true;
		GetNode<Occluder>("Occluder3").Visible = true;
		GetNode<Occluder>("Occluder4").Visible = true;
		GetNode<Occluder>("Occluder5").Visible = true;
		GetNode<Occluder>("Occluder6").Visible = true;
		GetNode<Occluder>("Occluder7").Visible = true;
		
		return;

	}
	public void StartHouse(Random random, out int RandomUses)
	{
		RandomUses = 0;
		if (!spawnItems)
			return;
		if (ItemSpawnPool == null)
			return;

		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
			{
				Furniture f = (Furniture)nd;
				FurnitureList.Insert(FurnitureList.Count, f);

				int start = random.Next(0, ItemSpawnPool.Length * 2);
				RandomUses ++;

				if (start >= ItemSpawnPool.Length)
					continue;

				f.SpawnItem(ItemSpawnPool[start]);
			}
		}
	}
	public void GetFurniture(out List<Furniture> furniture)
	{
		furniture = FurnitureList;
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
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();

		data.Add("Name", HouseName);
		
		GDScript FurnitureSaveScript = GD.Load<GDScript>("res://Scripts/FurnitureSaveInfo.gd");

		Resource[] FurnitureInfoobjects = new Resource[furni.Count];

		for (int i = 0; i < furni.Count; i ++)
		{
			Resource Furniinfo = (Resource)FurnitureSaveScript.New();
			Furniinfo.Call("_SetData", furni[i].GetPackedData());
			FurnitureInfoobjects[i] = Furniinfo;
		}

		data.Add("Furniture", FurnitureInfoobjects);

		return data;
	}
	public void UnPackData(Resource data)
    {
        HouseName = (string)data.Get("Name");

        Godot.Collections.Array FurnitureData = (Godot.Collections.Array)data.Get("Furniture");
		for (int i  = 0; i < FurnitureData.Count; i++)
		{
			FurnitureInfo info = new FurnitureInfo();
			info.UnPackData((Resource)FurnitureData[i]);
			furni.Add(info);
		}
    }
}


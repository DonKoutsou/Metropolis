using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class House : Spatial
{
	[Export]
	bool spawnItems = true;
	//[Export]
	//public PackedScene[] ItemSpawnPool;
	
	List<Furniture> FurnitureList = new List<Furniture>();
	List<Spatial> DecorationList = new List<Spatial>();

	[Export]
	bool HasPower = true;

	public StaticBody HouseExterior;

	public override void _Ready()
	{
		/*foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				FurnitureList.Insert(FurnitureList.Count, (Furniture)nd);
		}*/
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
		GetNode<Street_Lamp>("IndoorsLight").SetWorkingState(HasPower);
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
	[Export]
	List<PackedScene> PossibleFurni = null;
	[Export]
	List<PackedScene> PossibleDeco = null;
	[Export]
	int FurnitureAmmount = 3;
	public void StartHouse()
	{

		Spatial Furiture = GetNode<Spatial>("FurniturePlacements");
		var furniplacaments = Furiture.GetChildren();

		//pick 3 places to place furniture
		for (int i = 0; i < FurnitureAmmount; i++)
		{
			int index = RandomContainer.Next(0, furniplacaments.Count);
			Position3D place = (Position3D)furniplacaments[index];
			furniplacaments.Remove(place);

			PackedScene furnitospawn = PossibleFurni[RandomContainer.Next(0, PossibleFurni.Count)];

			Furniture furn = furnitospawn.Instance<Furniture>();
			AddChild(furn, true);
			furn.Transform = place.Transform;

			FurnitureList.Insert(i, furn);

			if (!spawnItems)
				continue;
			furn.SpawnItem(GetDrop());
		}


		Spatial decos = GetNode<Spatial>("DecorationPlacaments");
		var decoplacaments = decos.GetChildren();

		//pick 3 places to place furniture

		int dindex = RandomContainer.Next(0, decoplacaments.Count);
		Position3D decplace = (Position3D)decoplacaments[dindex];

		PackedScene decotospawn = PossibleDeco[RandomContainer.Next(0, PossibleDeco.Count)];

		Spatial dec = decotospawn.Instance<Spatial>();
		AddChild(dec, true);
		dec.Transform = decplace.Transform;

		DecorationList.Insert(0, dec);


		Furiture.QueueFree();
		/*foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
			{
				Furniture f = (Furniture)nd;
				FurnitureList.Insert(FurnitureList.Count, f);

				f.SpawnItem(GetDrop(random, out RandomUses));
			}
		}*/
	}

	[Export]
	Dictionary<int, PackedScene> SpawnPool = null;
	PackedScene GetDrop()
	{
		PackedScene Drop = null;
		int DropChance = 100;

		foreach (KeyValuePair<int, PackedScene> item in SpawnPool)
		{
			int thing = RandomContainer.Next(0,101);
			//GD.Print("Trying to spawn item : " + item.Value.ResourceName + " with chance of " + item.Key + "%, ranodm came out " + thing);
			if (thing < item.Key)
			{
				if (item.Key < DropChance)
				{
					DropChance = item.Key;
					Drop = item.Value;
				}
			}
		}
		return Drop;
	}
	public void GetFurniture(out List<Furniture> furniture)
	{
		furniture = FurnitureList;
	}
	public void GetDecorations(out List<Spatial> decos)
	{
		decos = DecorationList;
	}
	public void RespawnHouseInterion(HouseInfo data)
	{
		foreach(FurnitureInfo Finfo in data.furni)
		{
			PackedScene scene = GD.Load<PackedScene>(Finfo.SceneData);
			Furniture furn = scene.Instance<Furniture>();
			FurnitureList.Add(furn);
			AddChild(furn, true);
			furn.SetData(Finfo);
			//if (furni.Name == Finfo.FunritureName)
			//{
			//	furni.SetData(Finfo);
			//}
		}
		foreach(DecorationInfo Dinfo in data.Deco)
		{
			PackedScene scene = GD.Load<PackedScene>(Dinfo.SceneData);
			Spatial decor = scene.Instance<Spatial>();
			DecorationList.Add(decor);
			AddChild(decor, true);
			decor.Transform = Dinfo.Placement;
		}
		GetNode<Spatial>("FurniturePlacements").QueueFree();
	}
	public void InputData(HouseInfo data)
	{
		if (FurnitureList.Count == 0 || DecorationList.Count == 0)
		{
			if (data.HasInternals())
			{
				RespawnHouseInterion(data);
			return;
			}
		}
		foreach (Furniture furni in FurnitureList)
		{
			foreach(FurnitureInfo Finfo in data.furni)
			{
				//PackedScene scene = GD.Load<PackedScene>(Finfo.SceneData);
				//Furniture furn = scene.Instance<Furniture>();
				//AddChild(furn, true);
				if (furni.Name == Finfo.FunritureName)
					furni.SetData(Finfo);
				//if (furni.Name == Finfo.FunritureName)
				//{
				//	furni.SetData(Finfo);
				//}
			}
		}
		foreach (Spatial dec in DecorationList)
		{
			foreach(DecorationInfo Dinfo in data.Deco)
			{
				//PackedScene scene = GD.Load<PackedScene>(Dinfo.SceneData);
				//Spatial decor = scene.Instance<Spatial>();
				//AddChild(decor, true);
				if (dec.Name == Dinfo.Name)
					dec.Transform = Dinfo.Placement;
			}
		}
	}
}

public class HouseInfo
{
	public string HouseName;

	public List<FurnitureInfo> furni = new List<FurnitureInfo>();
	public List<DecorationInfo> Deco = new List<DecorationInfo>();
	public bool HasInternals()
	{
		return furni.Count > 0 || Deco.Count > 0;
	}
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
	public void SetInfo(string name, List<FurnitureInfo> funriture, List<DecorationInfo> decoration)
	{
		HouseName = name;
		for (int i = 0; i < funriture.Count; i++)
		{
			furni.Insert(i, funriture[i]);
		}
		for (int i = 0; i < decoration.Count; i++)
		{
			Deco.Insert(i, decoration[i]);
		}
	}
	public Dictionary<string, object>GetPackedData()
	{
		GDScript FurnitureSaveScript = GD.Load<GDScript>("res://Scripts/FurnitureSaveInfo.gd");

		Resource[] FurnitureInfoobjects = new Resource[furni.Count];

		for (int i = 0; i < furni.Count; i ++)
		{
			Resource Furniinfo = (Resource)FurnitureSaveScript.New();
			Furniinfo.Call("_SetData", furni[i].GetPackedData());
			FurnitureInfoobjects[i] = Furniinfo;
		}
		GDScript DecoSaveScript = GD.Load<GDScript>("res://Scripts/DecoSaveInfo.gd");

		Resource[] DecoInfoobjects = new Resource[Deco.Count];

		for (int i = 0; i < Deco.Count; i ++)
		{
			Resource DecoInfo = (Resource)DecoSaveScript.New();
			DecoInfo.Call("_SetData", Deco[i].GetPackedData());
			DecoInfoobjects[i] = DecoInfo;
		}

		Dictionary<string, object> data = new Dictionary<string, object>(){
			{"Name", HouseName},
			{"Furniture", FurnitureInfoobjects},
			{"Decorations", DecoInfoobjects}
		};
		
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

		Godot.Collections.Array DecoData = (Godot.Collections.Array)data.Get("Decorations");
		for (int i  = 0; i < DecoData.Count; i++)
		{
			DecorationInfo info = new DecorationInfo();
			info.UnPackData((Resource)DecoData[i]);
			Deco.Add(info);
		}
    }
}


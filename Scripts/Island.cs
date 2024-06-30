using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
[Tool]
public class Island : Spatial
{
	[Export]
	public bool m_bOriginalIle = false;
	[Export]
	IleType type = IleType.LAND;
	[Export]
	public bool KeepInstance = false;
	[Export]
	public int ImageID = 0;

	public string IslandSpecialName = null;

	public bool Inited = false;

	private Vector3 SpawnGlobalLocation;

	private float SpawnRotation;

	List<House> Houses = new List<House>();

	List<Vehicle> Vehicles = new List<Vehicle>();

	List<WindGenerator> Generators = new List<WindGenerator>();

	List<Item> Items = new List<Item>();

	List<Character> Characters = new List<Character>();
	
	public override void _Ready()
	{
		#if DEBUG
		if (Engine.EditorHint)
			return;
		#endif
		Translation = SpawnGlobalLocation;


		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(SpawnRotation));
		//Transform.Rotated(new Vector3(0, 1, 0), SpawnRotation);
		//FindHouses(this);
		//FindGenerators(this);
		//FindVehicles(this);
		StaticBody sea = GetNode<StaticBody>("SeaBed");
		sea.GlobalRotation = Vector3.Zero;
		//sea.GetNode<MeshInstance>("Sea").QueueFree();

		//FindChildren(this);
	}
	#if DEBUG
	[Export(PropertyHint.Layers3dPhysics)]
    public uint MoveLayer { get; set; }

	enum ImageRes
	{
		x16 = 16,
		x32 = 32,
		x64 = 64,
		x128 = 128,
		x256 = 256,
	}
	[Export]
	ImageRes Resolution = ImageRes.x16; 
    public Image GenerateImage()
	{
		int resolution = (int)Resolution;
		Vector2 res = new Vector2(resolution, resolution);
		int ilesize = 8000;
		Image im = new Image();
		im.Create((int)res.x, (int)res.y, true, Godot.Image.Format.Rgba8);
		int row = 0;
		int col = 0;
        float mult = ilesize/res.x;
		for (int i = 0; i < res.x * res.y; i++)
		{
			var spacestate = GetWorld().DirectSpaceState;

			Vector3 rayor = new Vector3((mult * col) - (ilesize / 2), 5000, (mult * row)  - (ilesize / 2));
			Vector3 rayend = rayor;
			rayend.y = -1000; 

			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, MoveLayer);

            if (rayar.Count == 0)
            {
                continue;
            }
			CollisionObject obj = (CollisionObject)rayar["collider"];
			//MeshInstance instance;
			//if (GetParent() is MeshInstance)
			//{
			//	instance = (MeshInstance)GetParent();
			//}
			//else
			//{
			//	instance = GetNode<MeshInstance>("MeshInstance");
			//}
			//Vector3 VectorLocalPos = instance.GlobalTransform.basis.XformInv((Vector3)rayar["position"]);
			//MeshDataTool tool = new MeshDataTool();
			//SurfaceTool tool2 = new SurfaceTool();
			//tool2.CreateFrom(instance.Mesh, 0);
			//tool.CreateFromSurface(tool2.Commit(), 0);

			//int VertexIndex = Get_Closest_Vertex(VectorLocalPos, tool);

            bool ItsSea = obj.GetCollisionLayerBit(8);

            im.Lock();
			//im.SetPixel(col, row, tool.GetVertexColor(VertexIndex));
			if (ItsSea)
			{
				im.SetPixel(col, row, new Color(r: 0, g: 0, b: 0, a: 0));
			}
            else
            {
                im.SetPixel(col, row, new Color(r: 0.83f, g: 0.7f, b: 0.49f, a: 1));
            }
            im.Unlock();
			col ++;
			if (col >= resolution)
			{
				col = 0;
				row ++;
				if (row >= resolution)
				{
					break;
				}
			}
		}
		//ImageTexture t = new ImageTexture();
		//t.CreateFromImage(im, flags:4);
        //Image = t;
		
		//im.SavePng("res://Assets/IslandPics/" + Filename.GetFile().Substr(0, Filename.GetFile().Length - 5) + ".png");
		//Image = "res://Assets/IslandPics/" + Filename.GetFile().Substr(0, Filename.GetFile().Length - 5) + ".png";
		im.ClearMipmaps();
		return im;
	}
	int Get_Closest_Vertex(Vector3 LocalPosition, MeshDataTool Tool)
	{
		float closest_dist_squared = float.MaxValue;
		int closest_vertex_index = -1;

		for (int i = 0; i < Tool.GetVertexCount(); i++)
		{
			Vector3 i_pos = Tool.GetVertex(i);
			float DistTemp = LocalPosition.DistanceSquaredTo(i_pos);
			if (DistTemp <= closest_dist_squared)
			{
				closest_dist_squared = DistTemp;
				closest_vertex_index = i;

			}
		}
		return closest_vertex_index;
	}
	#endif
	public void SetSpawnInfo(Vector3 SpawnPos, float SpawnRot, string SpecialName)
	{
		SpawnGlobalLocation = SpawnPos;
		SpawnRotation = SpawnRot;
		IslandSpecialName = SpecialName;
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
		/*foreach (Character Ch in Characters)
		{
			foreach(CharacterInfo CgarInfo in data.Characters)
			{
				if (Ch.Name == CgarInfo.Name)
				{
					Ch.SetData(CgarInfo);
				}
			}
		}*/
		List <CharacterInfo> C = new List<CharacterInfo>();

		foreach (Character Cha in Characters)
		{
			CharacterInfo myinfo = null;
			foreach(CharacterInfo ItInfo in data.Characters)
			{
				if (Cha.Name == ItInfo.Name)
				{
					myinfo = ItInfo;
				}
			}
			if( myinfo != null)
			{
				Cha.SetData(myinfo);
				C.Add(myinfo);
			}
			else
				Cha.QueueFree();
		}
		foreach(CharacterInfo inf in data.Characters)
		{
			//if (inf.removed)
				//continue;
			if (!C.Contains(inf))
			{
				PackedScene Charscene = GD.Load<PackedScene>(inf.SceneData);
				Character Char = Charscene.Instance<Character>();
				AddChild(Char);
				Char.SetData(inf);
			}
		}
		

		List <ItemInfo> I = new List<ItemInfo>();

		foreach (Item Itm in Items)
		{
			ItemInfo myinfo = null;
			foreach(ItemInfo ItInfo in data.Items)
			{
				if (Itm.Name == ItInfo.Name)
				{
					myinfo = ItInfo;
				}
			}
			if( myinfo != null)
			{
				Itm.InputData(myinfo);
				I.Add(myinfo);
			}
			else
				Itm.QueueFree();
		}
		foreach(ItemInfo inf in data.Items)
		{
			//if (inf.removed)
				//continue;
			if (!I.Contains(inf))
			{
				PackedScene Itemscene = GD.Load<PackedScene>(inf.SceneData);
				Item Item = Itemscene.Instance<Item>();
				AddChild(Item);
				Item.InputData(inf);
				Item.Name = inf.Name;
			}
		}

		List <VehicleInfo> D = new List<VehicleInfo>();

		foreach (Vehicle veh in Vehicles)
		{
			VehicleInfo myinfo = null;
			foreach(VehicleInfo Vnfo in data.Vehicles)
			{
				Spatial par = (Spatial)veh.GetParent();
				if (par.Name == Vnfo.VehName)
				{
					myinfo = Vnfo;
				}
			}
			if( myinfo != null)
			{
				//if (myinfo.removed)
				//	veh.GetParent().QueueFree();
				//else
				veh.InputData(myinfo);
				D.Add(myinfo);
			}
			else
				veh.GetParent().QueueFree();
		}
		foreach(VehicleInfo inf in data.Vehicles)
		{
			//if (inf.removed)
				//continue;
			if (!D.Contains(inf))
			{
				PackedScene vehscene = GD.Load<PackedScene>(inf.scenedata);
				Spatial veh = vehscene.Instance<Spatial>();
				Vehicle V = veh.GetNode<Vehicle>("VehicleBody");
				AddChild(veh);
				V.InputData(inf);
				veh.Name = inf.VehName;
			}
		}
	}
	public void InitialSpawn(Random r, out int RandomUses)
	{
		FindChildren(this);
		RandomUses = 0;
		int RandomUsedFromHouses = 0;
		foreach(House h in Houses)
		{
			h.StartHouse(r, out RandomUsedFromHouses);
		}
		RandomUses += RandomUsedFromHouses;
		CharacterSpawnLocations Chars = GetNode<CharacterSpawnLocations>("CharacterSpawnLocations");
		//if (Chars != null)
		//{
			
			//if (Children.Count > 0)
			//{
		if (Chars.HasChars())
		{
			var Children = Chars.GetChildren();
			foreach (Position3D pos in Children)
			{
				int Spawn = r.Next(0, 2);
				RandomUses ++;
				if (Spawn == 0)
				{
					int selection = r.Next(1, Chars.CharSpawns.Count());
					RandomUses ++;
					NPC chara = (NPC)Chars.CharSpawns[selection - 1].Instance();
					chara.Set("spawnUncon", true);
					AddChild(chara);
					chara.Translation = pos.Translation;
				}
			}
		}
			//}
		//}
		
		VehicleSpawnLocation Vehs = GetNode<VehicleSpawnLocation>("VehicleSpawnLocation");
		//if (Vehs != null)
		//{
			
			//if (VehChild.Count > 0)
			//{
		if (Vehs.HasVehicles())
		{
			var VehChild = Vehs.GetChildren();
			foreach (Position3D pos in VehChild)
			{
				int Spawn = r.Next(0, 2);
				RandomUses ++;
				if (Spawn == 0)
				{
					int selection = r.Next(1, Vehs.VehSpawns.Count());
					RandomUses ++;
					Spatial veh = (Spatial)Vehs.VehSpawns[selection - 1].Instance();
					AddChild(veh);
					
					Vehicle vehchild = veh.GetNode<Vehicle>("VehicleBody");
					vehchild.Translation = pos.Translation;
					if (vehchild.SpawnBroken)
						vehchild.OnLightDamaged();
				}
			}
		}
			//}
		//}
		
	}
	public void RegisterChild(Node child)
	{
		if (child is House house && !Houses.Contains(child))
			Houses.Insert(Houses.Count, house);
		else if (child is WindGenerator generator && !Generators.Contains(child))
			Generators.Insert(Generators.Count, generator);
		else if (child is Vehicle vehicle && !Vehicles.Contains(child))
			Vehicles.Insert(Vehicles.Count, vehicle);
		else if (child is Item item && !Items.Contains(child))
			Items.Insert(Items.Count, item);
		else if (child is Character character && !Characters.Contains(child))
			Characters.Insert(Characters.Count, character);
	}
	public void UnRegisterChild(Node child)
	{
		if (child is House house)
			Houses.Remove(house);
		else if (child is WindGenerator generator)
			Generators.Remove(generator);
		else if (child is Vehicle vehicle)
			Vehicles.Remove(vehicle);
		else if (child is Item item)
			Items.Remove(item);
		else if (child is Character character)
			Characters.Remove(character);
	}
	public void FindChildren(Node node)
	{
		//ulong ms = OS.GetSystemTimeMsecs();
		foreach (Node child in node.GetChildren())
		{
			if (child is House && !Houses.Contains(child))
				Houses.Insert(Houses.Count, (House)child);
			else if (child is WindGenerator && !Generators.Contains(child))
				Generators.Insert(Generators.Count, (WindGenerator)child);
			else if (child is Vehicle && !Vehicles.Contains(child))
				Vehicles.Insert(Vehicles.Count, (Vehicle)child);
			else if (child is Item && !Items.Contains(child))
				Items.Insert(Items.Count, (Item)child);
			else if (child is Character && !Houses.Contains(child))
				Characters.Insert(Characters.Count, (Character)child);
			else
				FindChildren(child);
		}
		//ulong msaf = OS.GetSystemTimeMsecs();
		//GD.Print("FoundChildren. Process time : " + (msaf - ms).ToString() + " ms");
	}

	public void GetHouses(out List<House> hs)
	{
		hs = new List<House>();
		for (int i = 0; i < Houses.Count; i++)
		{
			hs.Insert(i, Houses[i]);
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

	public void GetVehicles(out List<Vehicle> vhs)
	{
		vhs = new List<Vehicle>();
		for (int i = 0; i < Vehicles.Count; i++)
		{
			if (Vehicles[i] == null)
				continue;
				
			vhs.Insert(i, Vehicles[i]);
		}
	}
	public void GetItems(out List<Item> Itms)
	{
		Itms = new List<Item>();
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i] == null)
				continue;
				
			Itms.Insert(i, Items[i]);
		}
	}
	public void GetCharacters(out List<Character> Char)
	{
		Char = new List<Character>();
		for (int i = 0; i < Characters.Count; i++)
		{
			if (Characters[i] == null)
				continue;
				
			Char.Insert(i, Characters[i]);
		}
	}
}
public class IslandInfo
{
	public Island Island;
	public IleType Type;
	public Vector2 Position;
	public string SpecialName = null;
	public PackedScene IleType;
	public int ImageIndex = 0;
	public List<HouseInfo> Houses = new List<HouseInfo>();
	public List<WindGeneratorInfo> Generators = new List<WindGeneratorInfo>();
	public List<VehicleInfo> Vehicles = new List<VehicleInfo>();
	
	public List<ItemInfo> Items = new List<ItemInfo>();

	public List<CharacterInfo> Characters = new List<CharacterInfo>();
	public float RotationToSpawn;
	public bool KeepInstance;

	public IslandInfo(float rotation, PackedScene scene, Vector2 cell, string specialn)
	{
		RotationToSpawn = rotation;
		IleType = scene;
		//ImageFile = Imag;
		Position = cell;
		SpecialName = specialn;
	}
	public IslandInfo(Godot.Object data)
	{
		UnPackData(data);
	}
    public void UnPackData(Godot.Object data)
    {
        Type = (IleType)data.Get("Type");
        Position = (Vector2)data.Get("Pos");
		SpecialName = (string)data.Get("SpecialName");
        IleType = (PackedScene)data.Get("Scene");
		ImageIndex = (int)data.Get("ImageIndex");
        RotationToSpawn = (float)data.Get("Rotation");
		KeepInstance = (bool)data.Get("KeepInstance");

        Godot.Collections.Array HouseData = ( Godot.Collections.Array)data.Get("Houses");
		for (int i  = 0; i < HouseData.Count; i++)
		{
			HouseInfo info = new HouseInfo();
			info.UnPackData((Resource)HouseData[i]);
            Houses.Add(info);
		}
         Godot.Collections.Array GeneratorData = ( Godot.Collections.Array)data.Get("Generators");
        for (int i  = 0; i < GeneratorData.Count; i++)
		{
			WindGeneratorInfo info = new WindGeneratorInfo();
			info.UnPackData((Resource)GeneratorData[i]);
            Generators.Add(info);
		}
        Godot.Collections.Array VehicleData = ( Godot.Collections.Array)data.Get("Vehicles");
        for (int i  = 0; i < VehicleData.Count; i++)
		{
			VehicleInfo info = new VehicleInfo();
			info.UnPackData((Resource)VehicleData[i]);
            Vehicles.Add(info);
		}
		Godot.Collections.Array ItemData = ( Godot.Collections.Array)data.Get("Items");
        for (int i  = 0; i < ItemData.Count; i++)
		{
			ItemInfo info = new ItemInfo();
			info.UnPackData((Resource)ItemData[i]);
            Items.Add(info);
		}
		Godot.Collections.Array CharData = ( Godot.Collections.Array)data.Get("Characters");
		for (int i  = 0; i < CharData.Count; i++)
		{
			CharacterInfo info = new CharacterInfo();
			info.UnPackData((Resource)CharData[i]);
            Characters.Add(info);
		}
    }
	public Dictionary<string, object>GetPackedData()
	{
		//Houses
		GDScript HouseSaveScript = GD.Load<GDScript>("res://Scripts/HouseSaveInfo.gd");

		Resource[] HouseInfoobjects = new Resource[Houses.Count];
		for (int i = 0; i < Houses.Count; i ++)
		{
			Resource HouseInfor = (Resource)HouseSaveScript.New();
			HouseInfor.Call("_SetData", Houses[i].GetPackedData());
			HouseInfoobjects[i] = HouseInfor;
		}
		//data.Add("Houses", HouseInfoobjects);


		//generators
		GDScript GeneratorSaveScript = GD.Load<GDScript>("res://Scripts/GeneratorSaveInfo.gd");

		Resource[] GeneratorInfoobjects = new Resource[Generators.Count];

		for (int i = 0; i < Generators.Count; i ++)
		{
			Resource GeneratorInfo = (Resource)GeneratorSaveScript.New();
			GeneratorInfo.Call("_SetData", Generators[i].GetPackedData());
			GeneratorInfoobjects[i] = GeneratorInfo;
		}
		//data.Add("Generators", GeneratorInfoobjects);


		//////////to add item info gdscript path
		GDScript ItemSaveScript = GD.Load<GDScript>("res://Scripts/ItemSaveInfo.gd");

		Resource[] ItemInfoobjects = new Resource[Items.Count];

		for (int i = 0; i < Items.Count; i ++)
		{
			Resource ItemInfo = (Resource)ItemSaveScript.New();
			bool hasData;
			Dictionary<string, object> packeddata = Items[i].GetPackedData(out hasData);
			ItemInfo.Call("_SetData", packeddata, hasData);
			ItemInfoobjects[i] = ItemInfo;
		}
		//data.Add("Items", ItemInfoobjects);

		//vehicles
		GDScript VehicleSaveScript = GD.Load<GDScript>("res://Scripts/VehicleSaveInfo.gd");

		Resource[] VehicleInfoobjects = new Resource[Vehicles.Count];
		for (int i = 0; i < Vehicles.Count; i ++)
		{
			Resource Vehicleinfo = (Resource)VehicleSaveScript.New();
			Vehicleinfo.Call("_SetData", Vehicles[i].GetPackedData());
			VehicleInfoobjects[i] = Vehicleinfo;
		}
		//data.Add("Vehicles", VehicleInfoobjects);

		GDScript CharSaveScript = GD.Load<GDScript>("res://Scripts/CharacterSaveInfo.gd");

		Resource[] CharacterInfoobjects = new Resource[Characters.Count];
		for (int i = 0; i < Characters.Count; i ++)
		{
			Resource CharInfor = (Resource)CharSaveScript.New();
			bool hasdata;
			CharInfor.Call("_SetData", Characters[i].GetPackedData(out hasdata), false);
			CharacterInfoobjects[i] = CharInfor;
		}
		//data.Add("Characters", CharacterInfoobjects);

		Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "Type", Type },
            { "Pos", Position },
			{ "SpecialName", SpecialName},
			{"Scene", IleType},
			{"ImageIndex", ImageIndex},
			{"Rotation", RotationToSpawn},
			{"Houses", HouseInfoobjects},
			{"Generators", GeneratorInfoobjects},
			{"Items", ItemInfoobjects},
			{"Vehicles", VehicleInfoobjects},
			{"Characters", CharacterInfoobjects},
			{"KeepInstance", KeepInstance},
        };

		return data;
	}
    
	public void SetInfo(Island Ile)
	{
		Island = Ile;
		Type = Ile.GetIslandType();
		KeepInstance = Ile.KeepInstance;
		//SpecialName = Ile.IslandSpecialName;
		List<House> hous;
		Ile.GetHouses(out hous);
		List<WindGenerator> Gen;
		Ile.GetGenerator(out Gen);
		List<Vehicle> veh;
		Ile.GetVehicles(out veh);
		List <Item> Itms;
		Ile.GetItems(out Itms);
		List <Character> Chars;
		Ile.GetCharacters(out Chars);
		AddHouses(hous);
		AddGenerators(Gen);
		AddVehicles(veh);
		AddItems(Itms);
		AddChars(Chars);
	}
	public void AddNewVehicle(Vehicle veh)
	{
		int vehammount = Vehicles.Count;
		List <string> names = new List<string>();
		foreach (VehicleInfo d in Vehicles)
		{
			names.Add(d.VehName);
		}
		int add = 1;
		while (names.Contains("Vehicle" + (vehammount * add).ToString()))
		{
			add ++;
		}
		veh.GetParent().Name = "Vehicle" + (vehammount * add).ToString();
		VehicleInfo data = new VehicleInfo();
		data.SetInfo(veh);
		Vehicles.Insert(Vehicles.Count, data);
	}
	public void RemoveVehicle(Vehicle veh)
	{
		for(int i = 0; i < Vehicles.Count; i++)
		{
			if (Vehicles[i].VehName == veh.GetParent().Name)
			{
				VehicleInfo info = Vehicles[i];
				Vehicles.Remove(info);
				break;
			}
		}
	}
	public void AddNewCharacter(Character ch)
	{
		int Charammount = 0;
		for(int i = 0; i < Characters.Count; i++)
		{
			//if (!Vehicles[i].removed)
				Charammount += 1;
		}
		ch.Name = "Character" + (Charammount + 1).ToString();
		CharacterInfo data = new CharacterInfo();
		data.UpdateInfo(ch);
		Characters.Insert(Characters.Count, data);
	}
	public void RemoveCharacter(Character ch)
	{
		for(int i = 0; i < Characters.Count; i++)
		{
			if (Characters[i].Name == ch.GetParent().Name)
			{
				CharacterInfo info = Characters[i];
				Characters.Remove(info);
				break;
			}
		}
	}
	public void AddNewItem(Item it)
	{
		int Itemammount = 0;
		for(int i = 0; i < Items.Count; i++)
		{
			//if (!Items[i].removed)
				Itemammount += 1;
		}
		it.Name = "Item" + (Itemammount + 1).ToString();
		ItemInfo data = new ItemInfo();
		data.UpdateInfo(it);
		Items.Insert(Items.Count, data);
	}
	public void RemoveItem (Item it)
	{
		for(int i = 0; i < Items.Count; i++)
		{
			if (Items[i].Name == it.Name)
			{
				ItemInfo info = Items[i];
				Items.Remove(info);
				break;
			}
		}
	}
	public void UpdateInfo(Island island)
	{
		List<House> hous;
		island.GetHouses(out hous);
		foreach(HouseInfo HInfo in Houses)
		{
			House h = null;
			foreach (House hou in hous)
			{
				if (hou.Name == HInfo.HouseName)
				{
					h = hou;
					break;
				}
			}
			List<Furniture> funriture;
			h.GetFurniture(out funriture);
			HInfo.UpdateInfo(funriture);
		}
		List<WindGenerator> gens;
		island.GetGenerator(out gens);
		foreach(WindGeneratorInfo WGInfo in Generators)
		{
			WindGenerator g = null;
			foreach (WindGenerator gen in gens)
			{
				if (gen.Name == WGInfo.WindGeneratorName)
				{
					g = gen;
					break;
				}
			}
			WGInfo.UpdateInfo(g);
		}
		List<Item> Its;
		island.GetItems(out Its);
		foreach(ItemInfo ItInfo in Items)
		{
			Item I = null;
			foreach (Item it in Its)
			{
				if (it == null || !Godot.Object.IsInstanceValid(it))
					continue;
				if (it.Name == ItInfo.Name)
				{
					I = it;
					break;
				}
			}
			if (I == null)
			{
				continue;
			}
			ItInfo.UpdateInfo(I);
		}

		List<Vehicle> vehs;
		island.GetVehicles(out vehs);
		foreach(VehicleInfo VHInfo in Vehicles)
		{
			Vehicle v = null;
			foreach (Vehicle veh in vehs)
			{
				if (veh == null || !Godot.Object.IsInstanceValid(veh))
					continue;
				Spatial par = (Spatial)veh.GetParent();
				if (par.Name == VHInfo.VehName)
				{
					v = veh;
					break;
				}
			}
			if (v == null)
			{
				//VHInfo.removed = true;
				continue;
			}
			VHInfo.UpdateInfo(v);
		}
		List<Character> Chars;
		island.GetCharacters(out Chars);
		foreach(CharacterInfo CHInfo in Characters)
		{
			Character c = null;
			foreach (Character cha in Chars)
			{
				if (cha == null || !Godot.Object.IsInstanceValid(cha))
					continue;
				if (cha.Name == CHInfo.Name)
				{
					c = cha;
					break;
				}
			}
			if (c == null)
			{
				//VHInfo.removed = true;
				continue;
			}
			CHInfo.UpdateInfo(c);
		}
	}
	public void AddHouses(List<House> HouseToAdd)
	{
		for (int i = 0; i < HouseToAdd.Count; i++)
		{
			HouseInfo info = new HouseInfo();
			List<Furniture> furni = new List<Furniture>();
			House h = HouseToAdd[i];
			h.GetFurniture(out furni);
			List<FurnitureInfo> finfo = new List<FurnitureInfo>();
			for (int f = 0; f < furni.Count; f++)
			{
				Furniture furn = furni[f];
				FurnitureInfo inf = new FurnitureInfo();
				ItemName itn = 0;
				if (furn.HasItem())
				{
					itn = furn.GetItemName();
				}
				inf.SetInfo(furn.Name, furn.HasBeenSearched(), furn.HasItem(), itn);
				finfo.Insert(f, inf);
			}
			info.SetInfo(HouseToAdd[i].Name, finfo);
			Houses.Insert(Houses.Count, info);
		}
	}
	public void AddGenerators(List<WindGenerator> GeneratorToAdd)
	{
		for (int i = 0; i < GeneratorToAdd.Count; i++)
		{
			WindGeneratorInfo info = new WindGeneratorInfo();
			info.SetInfo(GeneratorToAdd[i].Name, GeneratorToAdd[i].GetCurrentEnergy());
			Generators.Insert(Generators.Count, info);
		}
	}
	public void AddItems(List<Item> ItemsToAdd)
	{
		for (int i = 0; i < ItemsToAdd.Count; i++)
		{
			ItemInfo info = new ItemInfo();
			info.UpdateInfo(ItemsToAdd[i]);
			Items.Insert(Items.Count, info);
		}
	}
	public void AddVehicles(List<Vehicle> VehicleToAdd)
	{
		for (int i = 0; i < VehicleToAdd.Count; i++)
		{
			VehicleInfo info = new VehicleInfo();
			info.SetInfo(VehicleToAdd[i]);
			Vehicles.Insert(Vehicles.Count, info);
		}
	}
	public void AddChars(List<Character> CharsToAdd)
	{
		for (int i = 0; i < CharsToAdd.Count; i++)
		{
			CharacterInfo info = new CharacterInfo();
			info.UpdateInfo(CharsToAdd[i]);
			Characters.Insert(Characters.Count, info);
		}
	}
	public bool IsIslandSpawned()
	{
		if (Island == null)
			return false;
		else
		{
			return Godot.Object.IsInstanceValid(Island);
		}
		
	}
}

public class WindGeneratorInfo
{
	public string WindGeneratorName;
	public float CurrentEnergy;
	public int DespawnDay = 0;
	public int Despawnhour = 0;
	public int Despawnmins = 0;
	public void UpdateInfo(WindGenerator gen)
	{
		DayNight.GetDay(out DespawnDay);
		DayNight.GetTime(out Despawnhour, out Despawnmins);
		CurrentEnergy = gen.GetCurrentEnergy();
	}
	public void SetInfo(string name, float CurEn)
	{
		WindGeneratorName = name;
		CurrentEnergy = CurEn;
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Name", WindGeneratorName);
		data.Add("CurrentEnergy", CurrentEnergy);
		data.Add("DespawnDay", DespawnDay);
		data.Add("DespawnHour", Despawnhour);
		data.Add("DespawnMins", Despawnmins);
		return data;
	}
    public void UnPackData(Resource data)
    {
        WindGeneratorName = (string)data.Get("Name");
		CurrentEnergy = (float)data.Get("CurrentEnergy");
		DespawnDay = (int)data.Get("DespawnDay");
		Despawnhour = (int)data.Get("DespawnHour");
        Despawnmins = (int)data.Get("DespawnMins");
    }
}
public class ItemInfo
{
	public string Name;
	public Vector3 Position;
	public string SceneData;
	public Dictionary<string, object> CustomData = new Dictionary<string, object>();

	public void UpdateInfo(Item it)
	{
		Name = it.Name;
		Position = it.Translation;
		SceneData = it.Filename;
		if (it is Battery)
		{
			if (CustomData.ContainsKey("CurrentEnergy"))
				CustomData["CurrentEnergy"] = ((Battery)it).GetCurrentCap();
			else
				CustomData.Add("CurrentEnergy", ((Battery)it).GetCurrentCap());
		}
		if (it is Limb)
		{
			if (CustomData.ContainsKey("LimbColor"))
				CustomData["LimbColor"] = ((Limb)it).GetColor();
			else
				CustomData.Add("LimbColor", ((Limb)it).GetColor());
		}
	}
	public Dictionary<string, object>GetPackedData(out bool HasData)
	{
		HasData = false;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Position", Position);
		data.Add("Name", Name);
		data.Add("SceneData", SceneData);
		if (CustomData.Count > 0)
		{
			HasData = true;
			string[] CustomDataKeys = new string[CustomData.Count];
			object[] CustomDataValues = new object[CustomData.Count];
			int i = 0;
			foreach (KeyValuePair<string, object> Data in CustomData)
			{
				CustomDataKeys[i] = Data.Key;
				CustomDataValues[i] = Data.Value;
				i++;
			}
			data.Add("CustomDataKeys", CustomDataKeys);
			data.Add("CustomDataValues", CustomDataValues);
		}
		return data;
	}
    public void UnPackData(Resource data)
    {
        Position = (Vector3)data.Get("Position");
		Name = (string)data.Get("Name");
		SceneData = (string)data.Get("SceneData");
		var thing = data.Get("CustomDataKeys");
		if (thing is Godot.Collections.Array)
		{
			return;
		}
		string[] CustomDataKeys = (string[])data.Get("CustomDataKeys");
		Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)data.Get("CustomDataValues");

		if (CustomDataKeys.Count() > 0 && CustomDataValues.Count > 0)
		{
			for (int i = 0; i < CustomDataKeys.Count(); i++)
			{
				CustomData.Add((string)CustomDataKeys[i], CustomDataValues[i]);
			}
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

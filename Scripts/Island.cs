using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
[Tool]
public class Island : Spatial
{
	[Export]
	IleType type = IleType.LAND;
	[Export]
	public bool KeepInstance = false;
	[Export]
	public int ImageID = 0;
	[Export]
	public string IslandName = null;

	List<Port> Ports = new List<Port>();

	public string IslandSpecialName = null;

	public Vector3 SpawnGlobalLocation;

	protected float SpawnRotation;

	List<House> Houses = new List<House>();
	List<Furniture> Furnitures = new List<Furniture>();
	List<Vehicle> Vehicles = new List<Vehicle>();

	List<WindGenerator> Generators = new List<WindGenerator>();

	List<Item> Items = new List<Item>();

	List<NPC> Characters = new List<NPC>();

	List<Breakable> Breakables = new List<Breakable>();
	bool Visited = false;
	
	
	public void SetVisited()
	{
		Visited = true;
	}
	public bool IsVisited()
	{
		return Visited;
	}
	public override void _Ready()
	{
		
		#if DEBUG
		if (Engine.EditorHint)
			return;
		#endif
		Node b = GetNodeOrNull("Bounds");
		if (b != null)
			b.QueueFree();

		Node bi = GetNodeOrNull("InnerBounds");
		if (bi != null)
			bi.QueueFree();
			
		Translation = SpawnGlobalLocation;

		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(SpawnRotation));

		StaticBody sea = GetNode<StaticBody>("SeaBed");
		sea.GlobalRotation = Vector3.Zero;


		//var kids = GetChildren();
		//foreach (Node p in kids)
		//{
		//	if (p is Port)
		//	{
		///		Ports.Add((Port)p);
		//	}
		//}
	}

	public bool HasPort()
	{
		return Ports.Count > 0;
	}
	public Port GetPort(int index)
	{
		return Ports[0];
	}
	//public List<Port> GetPorts()
	//{
	//	return Ports;
	//}
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
	Image im = null;
	int row = 0;
	int col = 0;
	Vector2 res;
	List <Vector2> seapix = null;
	float mult = 0;
	int ilesize = 12000;

	bool GeneratingImage = false;
	bool OutliningImage = false;

	ImageGenDock d;
	void GeneratePixelRow()
	{
		for (int i = 0; i < res.x; i++)
		{
			//GD.Print("Generating Pixel : X: " + row.ToString() + " Y: " + col.ToString() );
			var spacestate = GetWorld().DirectSpaceState;

			Vector3 rayor = new Vector3((mult * col) - (ilesize / 2), 5000, (mult * row)  - (ilesize / 2));
			Vector3 rayend = rayor;
			rayend.y = -5; 

			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, MoveLayer);
			Color color = new Color(r: 0, g: 0, b: 0, a: 0);
			if (rayar.Count == 0)
			{
				seapix.Add(new Vector2(col, row));
			}
			else
			{
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
				bool ITsRock = obj.GetCollisionLayerBit(10);
				
				//im.SetPixel(col, row, tool.GetVertexColor(VertexIndex));
				
				if (ItsSea)
				{
					seapix.Add(new Vector2(col, row));
				}
				else if (ITsRock)
				{
					color = new Color(r: 0.66f, g: 0.57f, b: 0.42f, a: 1);
					//int t = r.Next(0,2);
					//if (t ==0)
						//
					//else
						//color = new Color(r: 0.83f, g: 0.7f, b: 0.49f, a: 1);
				}
				else
				{
					color = new Color(r: 0.83f, g: 0.7f, b: 0.49f, a: 1);
				}
			}
			im.Lock();
			im.SetPixel(col, row, color);
			im.Unlock();
			col ++;
			if (col >= res.x)
			{
				col = 0;
				row ++;
				if (row >= res.x)
				{
					GeneratingImage = false;
					row = 0;
					col = 0;
					im.ClearMipmaps();
					im.GenerateMipmaps(true);
					d.OnImageFinished(im, this);
					//OutliningImage = true;
					break;
				}
			}
		}
	}
	void GenerateOulinePixel()
	{
		
		//for (int i = 0; i < res.x * res.y; i++)
		//{
		for (int i = 0; i < res.x; i++)
		{
			//GD.Print("Generating Outline Pixel : X: " + row.ToString() + " Y: " + col.ToString() );
			if (seapix.Contains(new Vector2(col, row)))
			{
				float minDistance = float.MaxValue;

				for (int newr = row - 8; newr <= row + 8; newr++)
				{
					for (int newc = col - 8; newc <= col + 8; newc++)
					{

						if (newr == row && newc == col)
							continue;

						if (newr < 0 || newr >= res.x || newc < 0 || newc >= res.x)
							continue;

						if (!seapix.Contains(new Vector2(newc, newr)))
						{
							float distance = Mathf.Sqrt(Mathf.Pow(newr - row, 2) + Mathf.Pow(newc - col, 2));
							if (distance < minDistance)
							{
								minDistance = distance;
							}
						}
					}
				}

				if (minDistance < float.MaxValue)
				{
					float gradientFactor = minDistance /8.0f;
					gradientFactor = Mathf.Clamp(gradientFactor, 0, 1); 

	
					//Color startColor = new Color(r: 0.76f, g: 0.9f, b: 1, a: 1);
					Color startColor = new Color(r: 0.83f, g: 0.7f, b: 0.49f, a: 1);
					//Color endColor = new Color(r: 0.2f, g: 0.4f, b: 0.8f, a: 1); 
					Color endColor = new Color(r: 0.66f, g: 0.57f, b: 0.42f, a: 1);
					Color outlineColor = startColor.LinearInterpolate(endColor, gradientFactor);

					im.Lock();
					im.SetPixel(col, row, outlineColor);
					im.Unlock();
				}
			}

			col++;
			if (col >= res.x)
			{
				col = 0;
				row++;
				if (row >= res.x)
				{
					OutliningImage = false;
					im.ClearMipmaps();
					im.GenerateMipmaps();
					d.OnImageFinished(im, this);
					break;
				}
			}
		}
		//}
		//ImageTexture t = new ImageTexture();
		//t.CreateFromImage(im, flags:4);
        //Image = t;
		
		//im.SavePng("res://Assets/IslandPics/" + Filename.GetFile().Substr(0, Filename.GetFile().Length - 5) + ".png");
		//Image = "res://Assets/IslandPics/" + Filename.GetFile().Substr(0, Filename.GetFile().Length - 5) + ".png";
		
	}
    public override void _Process(float delta)
    {
        base._Process(delta);
		if (GeneratingImage)
		{
			GeneratePixelRow();
		}
		if (OutliningImage)
		{
			GenerateOulinePixel();
		}
		
    }
    [Export]
	ImageRes Resolution = ImageRes.x16; 
    public void GenerateImage(ImageGenDock Gend)
	{
		d = Gend;
		res = new Vector2((int)Resolution, (int)Resolution);
		
		im = new Image();
		im.Create((int)res.x, (int)res.y, true, Godot.Image.Format.Rgba8);
		row = 0;
		col = 0;
        mult = ilesize/res.x;
		seapix = new List<Vector2>();
		GeneratingImage = true;
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
		Visited = data.Visited;
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
		foreach (Furniture furn in Furnitures)
		{
			foreach(FurnitureInfo Fnfo in data.Furnitures)
			{
				if (furn.Name == Fnfo.FunritureName)
				{
					furn.SetData(Fnfo);
				}
			}
		}
		foreach (Port p in Ports)
		{
			foreach(PortInfo Pnfo in data.Ports)
			{
				if (new Vector2(p.Translation.x, p.Translation.z) == Pnfo.Location)
				{
					p.Visited = Pnfo.Visited;
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
		foreach (Breakable br in Breakables)
		{
			foreach(BreakableInfo BrInfo in data.Breakables)
			{
				if (br.Name == BrInfo.BreakableName)
				{
					if (BrInfo.Destroyed)
						br.QueueFree();
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
	public void InitialSpawn()
	{
		FindChildren(this);
		foreach(House h in Houses)
		{
			h.StartHouse();
		}
		foreach(NPC c in Characters)
		{
			c.InitialSpawn();
		}
		
		/*CharacterSpawnLocations Chars = GetNode<CharacterSpawnLocations>("CharacterSpawnLocations");
		//if (Chars != null)
		//{
			
			//if (Children.Count > 0)
			//{
		if (Chars.HasChars())
		{
			for (int i = 0; i < Chars.Locations.Count(); i++)
			{
				int Spawn = r.Next(0, 2);
				RandomUses ++;
				if (Spawn == 0)
				{
					int selection = r.Next(1, Chars.CharSpawns.Count());
					RandomUses ++;
					NPC chara = (NPC)Chars.CharSpawns[selection - 1].Instance();
					//chara.Set("spawnUncon", true);
					AddChild(chara);
					chara.Translation = Chars.Locations[i];
					chara.Rotation = Chars.Rotations[i];
					chara.InitialSpawn(r);
					RandomUses += 6;
					Characters.Add(chara);
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
					Vehicles.Add(vehchild);
				}
			}
		}*/
			//}
		//}
		
	}
	public void RegisterChild(Node child)
	{
		if (child is House house && !Houses.Contains(child))
			Houses.Add(house);
		else if (child is Furniture fourn && !Furnitures.Contains(child))
			Furnitures.Add(fourn);
		else if (child is WindGenerator generator && !Generators.Contains(child))
			Generators.Add(generator);
		else if (child is Vehicle vehicle && !Vehicles.Contains(child))
			Vehicles.Add(vehicle);
		else if (child is Item item && !Items.Contains(child))
			Items.Add(item);
		else if (child is NPC character && !Characters.Contains(child))
			Characters.Add(character);
		else if (child is Port port && !Ports.Contains(child))
			Ports.Add(port);
		else if (child is Breakable br && !Breakables.Contains(child))
			Breakables.Add(br);
	}
	public void UnRegisterChild(Node child)
	{
		if (child is House house)
			Houses.Remove(house);
		else if (child is Furniture fourn)
			Furnitures.Remove(fourn);
		else if (child is WindGenerator generator)
			Generators.Remove(generator);
		else if (child is Vehicle vehicle)
			Vehicles.Remove(vehicle);
		else if (child is Item item)
			Items.Remove(item);
		else if (child is NPC character)
			Characters.Remove(character);
		else if (child is Port p)
			Ports.Remove(p);
		else if (child is Breakable br)
			Breakables.Remove(br);
	}
	public void FindChildren(Node node)
	{
		//ulong ms = OS.GetSystemTimeMsecs();
		foreach (Node child in node.GetChildren())
		{
			if (child is House ho && !Houses.Contains(child))
				Houses.Add(ho);
			if (child is Furniture furn && !Furnitures.Contains(child))
				Furnitures.Add(furn);
			else if (child is WindGenerator windg && !Generators.Contains(child))
				Generators.Add(windg);
			else if (child is Vehicle veh && !Vehicles.Contains(child))
				Vehicles.Add(veh);
			else if (child is Item item && !Items.Contains(child))
				Items.Add(item);
			else if (child is NPC cha && !Houses.Contains(child))
				Characters.Add(cha);
			else if (child is Port p && !Ports.Contains(child))
				Ports.Add(p);
			else if (child is Breakable br && !Breakables.Contains(child))
				Breakables.Add(br);
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
			hs.Add(Houses[i]);
		}
	}
	public void GetFurniture(out List<Furniture> fs)
	{
		fs = new List<Furniture>();
		for (int i = 0; i < Furnitures.Count; i++)
		{
			fs.Add(Furnitures[i]);
		}
	}
	public void GetGenerator(out List<WindGenerator> wg)
	{
		wg = new List<WindGenerator>();
		for (int i = 0; i < Generators.Count; i++)
		{
			wg.Add(Generators[i]);
		}
	}
	public void GetPorts(out List<Port> wg)
	{
		wg = new List<Port>();
		for (int i = 0; i < Ports.Count; i++)
		{
			wg.Add(Ports[i]);
		}
	}
	public void GetVehicles(out List<Vehicle> vhs)
	{
		vhs = new List<Vehicle>();
		for (int i = 0; i < Vehicles.Count; i++)
		{
			if (Vehicles[i] == null)
				continue;
				
			vhs.Add(Vehicles[i]);
		}
	}
	public void GetBreakables(out List<Breakable> brs)
	{
		brs = new List<Breakable>();
		for (int i = 0; i < Breakables.Count; i++)
		{
			if (Breakables[i] == null)
				continue;
				
			brs.Add(Breakables[i]);
		}
	}
	public void GetItems(out List<Item> Itms)
	{
		Itms = new List<Item>();
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i] == null)
				continue;
				
			Itms.Add(Items[i]);
		}
	}
	public bool HasCharacters()
	{
		return Characters.Count > 0 ;
	}
	public void GetCharacters(out List<NPC> Char)
	{
		Char = new List<NPC>();
		for (int i = 0; i < Characters.Count; i++)
		{
			if (Characters[i] == null)
				continue;
				
			Char.Add(Characters[i]);
		}
	}
	private void Player_Visited(Node body)
	{
		if (body is Player pl)
		{
			IslandInfo info = WorldMap.GetInstance().GetIleInfo(this);
			((MapUI)PlayerUI.GetInstance().GetUI(PlayerUIType.MAP)).GetGrid().SetIslandVisited(info, pl);
		}
		
	}
}
public class IslandInfo
{
	public Island Island;
	public IleType Type;
	public Vector2 Position;
	public bool HasPort;
	public List<PortInfo> Ports = new List<PortInfo>();
	public string SpecialName = null;
	public PackedScene IleType;
	public int ImageIndex = 0;
	public List<HouseInfo> Houses = new List<HouseInfo>();
	public List<FurnitureInfo> Furnitures = new List<FurnitureInfo>();
	public List<WindGeneratorInfo> Generators = new List<WindGeneratorInfo>();
	public List<BreakableInfo> Breakables = new List<BreakableInfo>();
	public List<VehicleInfo> Vehicles = new List<VehicleInfo>();
	public List<ItemInfo> Items = new List<ItemInfo>();

	public List<CharacterInfo> Characters = new List<CharacterInfo>();
	public float RotationToSpawn;
	public bool KeepInstance;
	public bool Visited;
	public IslandInfo(float rotation, PackedScene scene, Vector2 cell)
	{
		RotationToSpawn = rotation;
		IleType = scene;
		//ImageFile = Imag;
		Position = cell;
		
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
		Visited = (bool)data.Get("Visited");
		HasPort = (bool)data.Get("HasPort");

		if (HasPort)
		{
			Godot.Collections.Array PortData = ( Godot.Collections.Array)data.Get("Ports");
			for (int i  = 0; i < PortData.Count; i++)
			{
				PortInfo info = new PortInfo();
				info.UnPackData((Resource)PortData[i]);
				Ports.Add(info);
			}
		}
		

        Godot.Collections.Array HouseData = ( Godot.Collections.Array)data.Get("Houses");
		for (int i  = 0; i < HouseData.Count; i++)
		{
			HouseInfo info = new HouseInfo();
			info.UnPackData((Resource)HouseData[i]);
            Houses.Add(info);
		}
		Godot.Collections.Array FurnitureData = ( Godot.Collections.Array)data.Get("Furnitures");
		for (int i  = 0; i < FurnitureData.Count; i++)
		{
			FurnitureInfo info = new FurnitureInfo();
			info.UnPackData((Resource)FurnitureData[i]);
            Furnitures.Add(info);
		}
        Godot.Collections.Array GeneratorData = ( Godot.Collections.Array)data.Get("Generators");
        for (int i  = 0; i < GeneratorData.Count; i++)
		{
			WindGeneratorInfo info = new WindGeneratorInfo();
			info.UnPackData((Resource)GeneratorData[i]);
            Generators.Add(info);
		}
		Godot.Collections.Array BreakableData = ( Godot.Collections.Array)data.Get("Breakables");
        for (int i  = 0; i < BreakableData.Count; i++)
		{
			BreakableInfo info = new BreakableInfo();
			info.UnPackData((Resource)BreakableData[i]);
            Breakables.Add(info);
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
		GDScript PortSaveScript = GD.Load<GDScript>("res://Scripts/PortSaveInfo.gd");
		Resource[] PortInfoobjects = new Resource[Ports.Count];


		//Vector2[] Portobjects = new Vector2[Ports.Count];
		for (int i = 0; i < Ports.Count; i ++)
		{
			Resource PortInfor = (Resource)PortSaveScript.New();
			PortInfor.Call("_SetData", Ports[i].GetPackedData());
			PortInfoobjects[i] = PortInfor;
		}
		
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

		GDScript FurnitureSaveScript = GD.Load<GDScript>("res://Scripts/FurnitureSaveInfo.gd");

		Resource[] FurnitureInfoobjects = new Resource[Furnitures.Count];
		for (int i = 0; i < Furnitures.Count; i ++)
		{
			Resource FurnitureInfor = (Resource)FurnitureSaveScript.New();
			FurnitureInfor.Call("_SetData", Furnitures[i].GetPackedData());
			FurnitureInfoobjects[i] = FurnitureInfor;
		}

		//generators
		GDScript GeneratorSaveScript = GD.Load<GDScript>("res://Scripts/GeneratorSaveInfo.gd");

		Resource[] GeneratorInfoobjects = new Resource[Generators.Count];

		for (int i = 0; i < Generators.Count; i ++)
		{
			Resource GeneratorInfo = (Resource)GeneratorSaveScript.New();
			GeneratorInfo.Call("_SetData", Generators[i].GetPackedData());
			GeneratorInfoobjects[i] = GeneratorInfo;
		}


		GDScript BreakableSaveScript = GD.Load<GDScript>("res://Scripts/BreakableSaveInfo.gd");

		Resource[] BreakableInfoobjects = new Resource[Breakables.Count];

		for (int i = 0; i < Breakables.Count; i ++)
		{
			Resource BreakableInfo = (Resource)BreakableSaveScript.New();
			BreakableInfo.Call("_SetData", Breakables[i].GetPackedData());
			BreakableInfoobjects[i] = BreakableInfo;
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
			var dat = Characters[i].GetPackedData(out hasdata);
			CharInfor.Call("_SetData", dat, hasdata);
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
			{"Furnitures", FurnitureInfoobjects},
			{"Generators", GeneratorInfoobjects},
			{"Breakables", BreakableInfoobjects},
			{"Items", ItemInfoobjects},
			{"Vehicles", VehicleInfoobjects},
			{"Characters", CharacterInfoobjects},
			{"KeepInstance", KeepInstance},
			{"Visited", Visited},
			{"HasPort", HasPort},
			{"Ports", PortInfoobjects}
        };

		return data;
	}
    
	public void SetInfo(Island Ile)
	{
		Island = Ile;
		Type = Ile.GetIslandType();
		KeepInstance = Ile.KeepInstance;
		Visited = Ile.IsVisited();
		HasPort = Ile.HasPort();
		SpecialName = Ile.IslandName;
		//SpecialName = Ile.IslandSpecialName;
		List<House> hous;
		Ile.GetHouses(out hous);
		List<Furniture> fourns;
		Ile.GetFurniture(out fourns);
		List<WindGenerator> Gen;
		Ile.GetGenerator(out Gen);
		List<Vehicle> veh;
		Ile.GetVehicles(out veh);
		List <Item> Itms;
		Ile.GetItems(out Itms);
		List <NPC> Chars;
		Ile.GetCharacters(out Chars);
		List <Breakable> Breaks;
		Ile.GetBreakables(out Breaks);

		AddHouses(hous);
		AddFurnitures(fourns);
		AddGenerators(Gen);
		AddVehicles(veh);
		AddItems(Itms);
		AddChars(Chars);
		AddBreakables(Breaks);

		if (HasPort)
		{
			List <Port> Ports;
			Ile.GetPorts(out Ports);
			AddPorts(Ports);
		}
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
	public void AddNewCharacter(NPC ch)
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
		Visited = island.IsVisited();
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

		List<Furniture> furs;
		island.GetFurniture(out furs);
		foreach(FurnitureInfo FInfo in Furnitures)
		{
			Furniture f = null;
			foreach (Furniture furn in furs)
			{
				if (furn.Name == FInfo.FunritureName)
				{
					f = furn;
					break;
				}
			}
			FInfo.UpdateInfo(f);
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

		List<Breakable> breaks;
		island.GetBreakables(out breaks);
		foreach(BreakableInfo BRInfo in Breakables)
		{
			Breakable br = null;
			foreach (Breakable breakab in breaks)
			{
				if (breakab == null || !Godot.Object.IsInstanceValid(breakab))
					continue;
				if (breakab.Name == BRInfo.BreakableName)
				{
					br = breakab;
					break;
				}
			}
			BRInfo.UpdateInfo(br == null);
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
		List<NPC> Chars;
		island.GetCharacters(out Chars);
		foreach(CharacterInfo CHInfo in Characters)
		{
			NPC c = null;
			foreach (NPC cha in Chars)
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
			List<Furniture> furni;
			House h = HouseToAdd[i];
			h.GetFurniture(out furni);
			List<FurnitureInfo> finfo = new List<FurnitureInfo>();
			for (int f = 0; f < furni.Count; f++)
			{
				Furniture furn = furni[f];
				FurnitureInfo inf = new FurnitureInfo();
				string itn = string.Empty;
				if (furn.HasItem())
				{
					itn = furn.GetItemName();
				}
				inf.SetInfo(furn.Name, furn.HasBeenSearched(), furn.HasItem(), itn, furn.Filename, furn.Transform);
				finfo.Add(inf);
			}

			List<Spatial> Decos;
			h.GetDecorations(out Decos);
			List<DecorationInfo> dinfo = new List<DecorationInfo>();
			for (int f = 0; f < Decos.Count; f++)
			{
				Spatial dec = Decos[f];
				DecorationInfo inf = new DecorationInfo();

				inf.SetInfo(dec.Filename, dec.Transform, dec.Name);
				dinfo.Add(inf);
			}


			info.SetInfo(HouseToAdd[i].Name, finfo, dinfo);
			Houses.Add(info);
		}
	}
	public void AddFurnitures(List<Furniture> FurnitureToAdd)
	{
		for (int i = 0; i < FurnitureToAdd.Count; i++)
		{
			FurnitureInfo info = new FurnitureInfo();
			string itn = string.Empty;
			if (FurnitureToAdd[i].HasItem())
			{
				itn = FurnitureToAdd[i].GetItemName();
			}
			info.SetInfo(FurnitureToAdd[i].Name, FurnitureToAdd[i].HasBeenSearched(), FurnitureToAdd[i].HasItem(), itn, FurnitureToAdd[i].Filename, FurnitureToAdd[i].Transform);
			Furnitures.Add(info);
		}
	}
	public void AddGenerators(List<WindGenerator> GeneratorToAdd)
	{
		for (int i = 0; i < GeneratorToAdd.Count; i++)
		{
			WindGeneratorInfo info = new WindGeneratorInfo();
			info.SetInfo(GeneratorToAdd[i].Name, GeneratorToAdd[i].GetCurrentEnergy());
			Generators.Add(info);
		}
	}
	public void AddBreakables(List<Breakable> BreakablesToAdd)
	{
		for (int i = 0; i < BreakablesToAdd.Count; i++)
		{
			BreakableInfo info = new BreakableInfo();
			info.SetInfo(BreakablesToAdd[i].Name, false);
			Breakables.Add(info);
		}
	}	
	public void AddItems(List<Item> ItemsToAdd)
	{
		for (int i = 0; i < ItemsToAdd.Count; i++)
		{
			ItemInfo info = new ItemInfo();
			info.UpdateInfo(ItemsToAdd[i]);
			Items.Add(info);
		}
	}
	public void AddVehicles(List<Vehicle> VehicleToAdd)
	{
		for (int i = 0; i < VehicleToAdd.Count; i++)
		{
			VehicleInfo info = new VehicleInfo();
			info.SetInfo(VehicleToAdd[i]);
			Vehicles.Add(info);
		}
	}
	public void AddChars(List<NPC> CharsToAdd)
	{
		for (int i = 0; i < CharsToAdd.Count; i++)
		{
			CharacterInfo info = new CharacterInfo();
			info.UpdateInfo(CharsToAdd[i]);
			Characters.Add(info);
		}
	}
	public void AddPorts(List<Port> PortsToAdd)
	{
		for (int i = 0; i < PortsToAdd.Count; i++)
		{
			PortInfo info = new PortInfo();
			info.SetInfo(new Vector2 (PortsToAdd[i].Translation.x, PortsToAdd[i].Translation.z), PortsToAdd[i].Visited);
			Ports.Add(info);
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
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{"Name", WindGeneratorName},
			{"CurrentEnergy", CurrentEnergy},
			{"DespawnDay", DespawnDay},
			{"DespawnHour", Despawnhour},
			{"DespawnMins", Despawnmins},
		};
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

public enum IleType
{
	MOTHERCITY,
	LAND,
	EXIT,
	SEA,
	LIGHTHOUSE,
	WALL,
}

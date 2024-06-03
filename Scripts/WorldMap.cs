using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;


public class WorldMap : TileMap
{
	[Export]
	public PackedScene[] scenestospawn;

	[Export]
	public PackedScene[] Eventscenestospawn;

	List <PackedScene> loadedscenes = new List<PackedScene>();

	[Export]
	public PackedScene Entrytospawn;

	[Export]
	public PackedScene Exittospawn;

	[Export]
	public PackedScene Sea;

	[Export]
	public PackedScene LightHouse;

	[Export]
	public PackedScene Μαχαλάς;

	[Export]
	int CellSizeOverride = 8000;

	[Export]
	PackedScene IntroScene = null;
	[Signal]
	public delegate void OnTransitionEventHandler(Island ile);

	[Export]
	List<string> LightHouseNames = null;

	List <float> rots = new List<float>{0f, 90f, 180f, -90f};

	//TO BE SAVED
	//id of cells to be changed to events
	List <int> RandomisedEntryID = new List<int>();

	//TO BE SAVED
	public int currentile;

	//TO BE SAVED
	int ΜαχαλάςEntryID;

	//TO BE SAVED
	//id of exit
	int ExitID = 0;

	//TO BE SAVED
	List <Vector2> OrderedCells = new List<Vector2>();

	//TO BE SAVED
	Random random;
	int RandomTimes = 0;

	//TO BE SAVED
	Vector2 CurrentTile;

	//TO BE SAVED
	static Dictionary<Vector2, IslandInfo> ilemap = new Dictionary<Vector2, IslandInfo>();

	//TO BE SAVED
	bool finishedspawning = false;

	

	IslandInfo entry;

	static WorldMap Instance;

	public Player pl;

	public Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("RandomisedEntryID", RandomisedEntryID);
		data.Add("currentile", currentile);
		data.Add("ΜαχαλάςEntryID", ΜαχαλάςEntryID);
		data.Add("ExitID", ExitID);
		Vector2[] OrdC = new Vector2[OrderedCells.Count];
		OrderedCells.CopyTo(OrdC);
		data.Add("OrderedCells", OrdC);
		//data.Add("random", random);
		data.Add("CurrentTile", CurrentTile);
		data.Add("RandomTimes", RandomTimes);
		data.Add("Seed", Settings.GetGameSettings().Seed);
		//Dictionary<Vector2, Godot.Object> ilemapObj = new Dictionary<Vector2, Godot.Object>();

		GDScript IleSaveScript = GD.Load<GDScript>("res://Scripts/IleSaveInfo.gd");

		Vector2[] IleVectors = new Vector2[ilemap.Count];
		Resource[] Iles = new Resource[ilemap.Count];
		int i = 0;
		foreach(KeyValuePair<Vector2, IslandInfo> entry in ilemap)
		{
			Resource IleSaveInfo = (Resource)IleSaveScript.New();
			IleSaveInfo.Call("_SetData", entry.Value.GetPackedData());
			IleVectors[i]= entry.Key;
			Iles[i] = IleSaveInfo;
			i +=1;
		}
		data.Add("ilemap", Iles);
		data.Add("ilemapVectors", IleVectors);
		data.Add("finishedspawning", finishedspawning);
		return data;
	}
	
	public void LoadSaveData(Resource data)
	{
		finishedspawning = (bool)data.Get("finishedspawning");
		CurrentTile = (Vector2)data.Get("CurrentTile");
		RandomTimes = (int)data.Get("RandomTimes");


		int seed = (int)data.Get("Seed");
		Settings.GetGameSettings().Seed = seed;

		random = new Random(seed);

		for (int i = 0; i < RandomTimes; i++)
		{
			random.NextDouble();
		}

		currentile = (int)data.Get("currentile");
		ΜαχαλάςEntryID = (int)data.Get("MahalasEntryID");
		ExitID = (int)data.Get("ExitID");

		Vector2[] cells = (Vector2[])data.Get("OrderedCells");
		foreach (Vector2 cell in cells)
		{
			OrderedCells.Add(cell);
		}

		int[] RandomisedEntryIDArray = (int[])data.Get("RandomisedEntryID");
		foreach (int cell in RandomisedEntryIDArray)
		{
			RandomisedEntryID.Add(cell);
		}

		//random = data.random;
		Vector2[] IleVectors = (Vector2[])data.Get("ilemapvectors");
		Godot.Collections.Array Iles = (Godot.Collections.Array)data.Get("ilemap");
		for (int i = 0; i < Iles.Count; i++)
		{
			IslandInfo info = new IslandInfo();
			info.UnPackData((Resource)Iles[i]);
			ilemap.Add(IleVectors[i], info);
		}
	}
	public static WorldMap GetInstance()
	{
		return Instance;
	}
	public override void _Ready()
	{
		Instance = this;

		//MapGrid.GetInstance().InitMap();

		int seed = Settings.GetGameSettings().Seed;

		random = new Random(seed);
		
			
		CellSize = new Vector2(CellSizeOverride, CellSizeOverride);
		
		//ulong ms = OS.GetSystemTimeMsecs();
		
		//ulong msaf = OS.GetSystemTimeMsecs();
		//GD.Print("Aranging cells took : " + (msaf - ms).ToString() + " ms");
		for (int i = 0; i < scenestospawn.Count(); i++)
		{
			loadedscenes.Insert(i, scenestospawn[i]);
		}
		//ms = OS.GetSystemTimeMsecs();
		
		//msaf = OS.GetSystemTimeMsecs();
		//GD.Print("Initialising map grid took : " + (msaf - ms).ToString() + " ms");
	}
	public void Init(bool LoadSave)
	{
		if (LoadSave)
		{
			var ResourceLoaderSafe = ResourceLoader.Load("res://Scripts/safe_resource_loader.gd") as Script;
			Resource save = (Resource)ResourceLoaderSafe.Call("load", "user://SavedGame.tres");
			if (save == null)
			{
				ArrangeCellsBasedOnDistance();
				MapGrid.GetInstance().InitMap();
				return;
			}
			int[] Date = (int[])save.Get("Date");
			DayNight.GetInstance().SetTime(Date[0], Date[1], Date[2]);
			LoadSaveData(save);
			IslandInfo CurIle;
			ilemap.TryGetValue(WorldToMap(CurrentTile), out CurIle);
			//((MyWorld)GetParent()).ToggleIsland(CurIle, true, true);
			Intro intro = SpawnIntro(CurIle);
			intro.LoadStop((Vector3)save.Get("playerlocation"));
			Player pl = Player.GetInstance();

			//setting player energy
			Player.GetInstance().SetEnergy((float)save.Get("playerenergy"));

			//loading inventory
			Inventory inv = pl.GetCharacterInventory();
			Godot.Collections.Array Items = (Godot.Collections.Array)save.Get("InventoryContents");
			inv.LoadSavedInventory(Items);

			MapGrid.GetInstance().InitMap();

			//returning data to inventory map
			Vector2[] MapGridVectorData = (Vector2[])save.Get("MapGridVectors");
			int[] MapGridTypeData = (int[])save.Get("MapGridTypes");

			bool HasVehicle = (bool)save.Get("playerHasVehicle");
			if (HasVehicle)
			{
				string vehname = (string)save.Get("VehicleName");
				bool MachineState = (bool)save.Get("VehicleState");
				bool WingState = (bool)save.Get("WingState");
				bool LightState = (bool)save.Get("LightState");


				Vehicle veh = GetCurrentIsland().GetNode<Spatial>(vehname).GetNode<Vehicle>("VehicleBody");
				veh.BoardVehicle(pl);
				pl.SetVehicle(veh);

				veh.ToggleMachine(MachineState);
				veh.ToggleWings(WingState);
				veh.ToggleLights(LightState);
			}

			

			Dictionary<Vector2, int> MapGridData = new Dictionary<Vector2, int>();

			for (int i = 0; i < MapGridVectorData.Count(); i++)
			{
				MapGridData.Add(MapGridVectorData[i], MapGridTypeData[i]);
			}
			MapGrid.GetInstance().LoadSaveData(MapGridData);

		}
		else
		{
			ArrangeCellsBasedOnDistance();
			MapGrid.GetInstance().InitMap();
		}
	}
	public Vector2 GetCurrentTile()
	{
		return WorldToMap(CurrentTile);
	}
	public Island GetCurrentIsland()
	{
		IslandInfo CurIle = null;
		ilemap.TryGetValue(WorldToMap(CurrentTile), out CurIle);
		if (CurIle == null)
			return null;
		return CurIle.ile;
	}
	//IslandInfo IleToSave;
	public override void _Process(float delta)
	{
		//ulong ms = OS.GetSystemTimeMsecs();
		if (!finishedspawning)
		{
			SaveIsland(GenerateIsland());

			if (currentile == LoadingScreen.GetWaitTime())
			{
				SpawnIntro();
			}
			currentile += 1;
			if (currentile >= OrderedCells.Count)
				finishedspawning = true; 
		}
		
		CheckForTransition();
		//ulong msaf = OS.GetSystemTimeMsecs();
		//if (msaf - ms > 10)
			//GD.Print("World map processing took longer the 10 ms. Process time : " + (msaf - ms).ToString() + " ms. Island scene =  " + scene);
	}
	//spawning and
	IslandInfo GenerateIsland()
	{
		//ulong ms = OS.GetSystemTimeMsecs();

		//Get Cell id
		int id = GetCell((int)OrderedCells[currentile].x, (int)OrderedCells[currentile].y);
		//Cell cords
		Vector2 cell = OrderedCells[currentile];

		string SpecialName;

		//Scene using ID from cell
		PackedScene ilescene = GetSceneToSpawn(id, out SpecialName);
		
		//Start the Data saving
		IslandInfo ileinfo = new IslandInfo();
		ilemap.Add(OrderedCells[currentile], ileinfo);
		//Spawndata to be used when spawning
		float rot;
		int index = random.Next(rots.Count);
		RandomTimes++;
		rot = rots[index];

		ileinfo.rottospawn = rot;
		ileinfo.IleType = ilescene;
		ileinfo.pos = cell;
		ileinfo.SpecialName = SpecialName;
		
		SpawnIsland(ileinfo);   

		//SaveEntry
		if (id == 0)
			entry = ileinfo;


		//ulong msaf = OS.GetSystemTimeMsecs();
		//if (msaf - ms > 10)
			//GD.Print("Island Generated. Process time : " + (msaf - ms).ToString() + " ms. Island scene =  " + ilescene.ResourcePath);

		return ileinfo;
	}
	public void SaveIsland(IslandInfo iletosave)
	{
		//ulong ms = OS.GetSystemTimeMsecs();

		Island ile = iletosave.ile;

		int RandomUses;

		ile.InitialSpawn(random, out RandomUses);

		RandomTimes += RandomUses;

		iletosave.SetInfo(ile);

		ile.QueueFree();

		MapGrid.GetInstance().UpdateIleInfo(iletosave.pos, iletosave.Type);

		//ulong msaf = OS.GetSystemTimeMsecs();
		//GD.Print("Island Saved. Process time : " + (msaf - ms).ToString() + " ms");
	}
	public Intro SpawnIntro(IslandInfo info = null)
	{
		IslandInfo start;
		if (info != null)
			start = info;
		else
			start = entry;

		MyWorld.GetInstance().ToggleIsland(start, true, true);
			
		Island island = start.ile;

		Intro intr = (Intro)IntroScene.Instance();

		EmitSignal("OnTransitionEventHandler", island);

		island.AddChild(intr);
		
		intr.Translation = Vector3.Zero;
		intr.GlobalRotation = Vector3.Zero;

		CurrentTile = new Vector2 (island.GlobalTransform.origin.x ,island.GlobalTransform.origin.z);
		return intr;
		//List<IslandInfo> closestto2;
		//GetClosestIles(entry,out closestto2, 2);
		//foreach(IslandInfo info in closestto2)
		   //MapGrid.GetInstance().OnIslandVisited(info);
	}
	private void CheckForTransition()
	{
		if (pl == null)
			return;
		Vector2 plpos = new Vector2(pl.GlobalTransform.origin.x, pl.GlobalTransform.origin.z);
		if (plpos.DistanceTo(CurrentTile) > CellSize.x/2)
		{
			Vector2 curt = FindClosestIslandPosition(plpos);

			if (CurrentTile != curt)
			{
				IslandInfo ileinf = null;
				ilemap.TryGetValue(WorldToMap(CurrentTile), out ileinf);
				IslandInfo ileinfto = null;
				ilemap.TryGetValue(WorldToMap(curt), out ileinfto);
				CurrentTile = curt;
				if (pl.HasVehicle())
				{
					Vehicle veh = pl.currveh;
					
					ileinf.RemoveVehicle(veh);
					veh.ReparentVehicle(ileinf.ile ,ileinfto.ile);
					ileinfto.AddNewVehicle(veh);
				}
				
				MyWorld.IleTransition(ileinf, ileinfto);
				
				EmitSignal("OnTransitionEventHandler", ileinfto.ile);
			}
		}
	}
	public IslandInfo GetCurrentIleInfo()
	{
		IslandInfo info = null;
		ilemap.TryGetValue(WorldToMap(CurrentTile), out info);
		return info;
	}
	public Island SpawnIsland(IslandInfo info)
	{
		Island Ile = (Island)info.IleType.Instance();

		Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

		Vector2 postoput = MapToWorld(new Vector2(info.pos.x, info.pos.y));

		postoput += CellSize / 2;
		pos.x = postoput.x;
		pos.z = postoput.y;
		Ile.SetSpawnInfo(pos, info.rottospawn, info.SpecialName);

		info.ile = Ile;

		MyWorld.GetInstance().AddChild(Ile);
		
		return Ile;
	}
	public Island ReSpawnIsland(IslandInfo info)
	{
		Island Ile = (Island)info.IleType.Instance();

		Vector3 pos;

		Vector2 postoput = MapToWorld(new Vector2(info.pos.x, info.pos.y));

		postoput += CellSize / 2;
		pos.x = postoput.x;
		pos.z = postoput.y;
		pos.y = 0.0f;
		Ile.SetSpawnInfo(pos, info.rottospawn, info.SpecialName);

		info.ile = Ile;


		MyWorld.GetInstance().AddChild(Ile);
		//Ile.InputData(info);
		return Ile;
	}
	void ArrangeCellsBasedOnDistance()
	{
		//arange all the cells by distance from center of world put in OrderedCells array
		var cells = GetUsedCells();
		
		OrderedCells.Insert(0, (Vector2)cells[0]);
		List<int> absolutesums = new List<int>(cells.Count);
		absolutesums.Add((int)(Math.Abs(OrderedCells[0].x) + Math.Abs(OrderedCells[0].y)));

		for (int x = 1; x < cells.Count; x++)
		{
			Vector2 cellArray = (Vector2)cells[x];
			int ind = (int)(Math.Abs(cellArray.x) + Math.Abs(cellArray.y));

			//Vector2 closest = OrderedCells[0];
			int closestind = Math.Abs(absolutesums.BinarySearch(ind)) - 1;
			closestind = Math.Max(0, closestind);
			OrderedCells.Insert(closestind, cellArray);

			absolutesums.Insert(closestind, ind);
		}
		//produce indexes of where tiles events will be placed on
		
		for (int i = 0; i < Eventscenestospawn.Count(); i++)
		{
			int SpawnIndex = random.Next(0, OrderedCells.Count);
			RandomTimes++;
			RandomisedEntryID.Insert(i, SpawnIndex);
		}
		// μαχαλάς randomise
		var lighthousecells = GetUsedCellsById(4);
		int RandomLightHouseIndex = random.Next(0, lighthousecells.Count);
		RandomTimes++;
		Vector2 Μαχαλάςpalcement = (Vector2)lighthousecells[RandomLightHouseIndex];
		ΜαχαλάςEntryID = OrderedCells.IndexOf(Μαχαλάςpalcement);
		//exit randomise
		var exitcells = GetUsedCellsById(2);
		int RandomExitIndex = random.Next(0, exitcells.Count);
		RandomTimes++;
		Vector2 Exitpalcement = (Vector2)exitcells[RandomExitIndex];
		ExitID = OrderedCells.IndexOf(Exitpalcement);
	}
	
	//takes in cell position gives out global transforms of closest island
	Vector2 FindClosestIslandPosition(Vector2 pos)
	{
		float dist = 999999999;
		Vector2 closest = Vector2.Zero;
		foreach(KeyValuePair<Vector2, IslandInfo> entry in ilemap)
		{
			Vector2 ilepos = MapToWorld(new Vector2(entry.Value.pos.x, entry.Value.pos.y));
			ilepos += CellSize / 2;
			
			float Itdist = ilepos.DistanceTo(pos);
			if (dist > Itdist)
			{
				closest = ilepos;
				dist = Itdist;
			}
		}
		return closest;
	}
	public void GetClosest(Vector2 pos, out List<Vector2> closeIles, int dist = 2)
	{
		closeIles = new List<Vector2>();
		Vector3 transform = new Vector3(pos.x, 0, pos.y);

		int ammount = 0;
		for (int i = 0; i < dist; i ++)
			ammount += ammount + 8;

		int offset = CellSizeOverride * dist;
		float row = transform.x - offset;
		float collumn = transform.z - offset;
		for (int i = 0; i < ammount + 1; i++)
		{
			closeIles.Insert(closeIles.Count, new Vector2(row, collumn));
			row += CellSizeOverride;
			if (row > transform.x + offset)
			{
				row = transform.x - offset;
				collumn += CellSizeOverride;
				if (collumn > transform.z + offset)
					break;

			}
		}
	}
	public void GetClosestIles(IslandInfo info, out List<IslandInfo> closeIles, int dist = 2)
	{
		
		closeIles = new List<IslandInfo>();
		Vector3 transform = new Vector3(info.pos.x, 0, info.pos.y);

		int ammount = 0;
		for (int i = 0; i < dist; i ++)
			ammount += ammount + 8;

		int offset = 1 * dist;
		float row = transform.x - offset;
		float collumn = transform.z - offset;
		for (int i = 0; i < ammount + 1; i++)
		{
			if (new Vector2(row, collumn) != info.pos)
			{
				IslandInfo ile;
				ilemap.TryGetValue(new Vector2(row, collumn), out ile);
				closeIles.Insert(closeIles.Count, ile);
			}
			
			row += 1;
			if (row > transform.x + offset)
			{
				row = transform.x - offset;
				collumn += 1;
				if (collumn > transform.z + offset)
					break;
			}
		}
	}
	
   
	PackedScene GetSceneToSpawn(int type, out string SpecialName)
	{
		PackedScene scene = null;
		SpecialName = null;
		//0 entry
		if (type == 0)
		{
			SpecialName = "Μητρόπολη";
			scene = Entrytospawn;
		}
			

		//1 random or event
		else if (type == 1)
		{
			if (RandomisedEntryID.Contains(currentile))
				scene = Eventscenestospawn[RandomisedEntryID.IndexOf(currentile)];
			else
			{
				scene = loadedscenes[random.Next(0, loadedscenes.Count)];
				RandomTimes++;
			}
				

			SpecialName = "Νησί";
		}
		//2 exit
		else if (type == 2)
		{
			if (currentile == ExitID)
				scene =  Exittospawn;
			else
			{
				RandomTimes++;
				scene = loadedscenes[random.Next(0, loadedscenes.Count)];
			}
				
			SpecialName = "Νησί";
		}
		//3 sea
		else if (type == 3)
		{
			scene =  Sea;
			SpecialName = "Νησί";
		}
			
		else if (type == 4)
		{
			if (currentile == ΜαχαλάςEntryID)
			{
				SpecialName = "Μαχαλάς";
				scene =  Μαχαλάς;
			}
			else
			{
				if (LightHouseNames != null && LightHouseNames.Count() > 0)
				{
					SpecialName = LightHouseNames[LightHouseNames.Count() - 1];
					LightHouseNames.Remove(SpecialName);
				}
				else
				{
					SpecialName = "Μαχαλάς";
				}
				scene = LightHouse;
			}	
		}
			
		return scene;
	}
	public void SyncSeas()
	{
		var seas = GetTree().GetNodesInGroup("Sea");
		float animstage = Poseidon.GetInstance().GetAnimStage();
		foreach (Node sea in seas)
		{
			sea.GetNode<AnimationPlayer>("AnimationPlayer").Seek(animstage);
			sea.GetNode<AnimationPlayer>("AnimationPlayer").Play();
		}
	}
}




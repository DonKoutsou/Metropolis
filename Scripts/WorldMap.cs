using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;

public class WorldMap : TileMap
{
	[Export]
	string[] ScenesToSpawnLocs = null;
	//[Export]
	//public PackedScene[] scenestospawn;

	[Export]
	string[] Eventscenestospawn = null;

	//List <PackedScene> loadedscenes = new List<PackedScene>();

	[Export]
	string Entrytospawn = null;

	[Export]
	string Exittospawn = null;
	[Export]
	string WallToSpawn = null;
	[Export]
	string EventWall = null;

	[Export]
	string[] SeaVariations = null;

	[Export]
	string LightHouse = null;

	//[Export]
	//public PackedScene Μαχαλάς;

	[Export]
	int CellSizeOverride = 8000;

	[Export]
	string IntroScene = null;

	[Signal]
	public delegate void OnTransitionEventHandler(Island ile);

	[Export]
	List<string> LightHouseNames = null;

	List <int> RandomisedEntryID = new List<int>();

	List<Vector2> UnlockedLightHouses = new List<Vector2>();

	// one of the lighthouses will become the 2nd town
	//int ΜαχαλάςEntryID;

	int WallEventID;
	//random stuff
	//Random random;
	//int RandomTimes = 0;
	//////////////////////

	// loc of island player is at atm
	Vector2 CurrentTile;

	/// index of next island to be spawned
	public int IslandSpawnIndex;
	/// islands to be spawned ordered by distance form center of map
	List <Vector2> OrderedCells = new List<Vector2>();

	// map of all islands
	static Dictionary<Vector2, IslandInfo> ilemap = new Dictionary<Vector2, IslandInfo>();

	// once all islands in map have been generated this will be true
	bool finishedspawning = false;

	Vector2 entry;
	//TO BE SAVED
	//id of exit
	int ExitID = 0;
	Vector2 Exitpalcement;
	static WorldMap Instance;

	public bool HasSpawningFinished()
	{
		return finishedspawning;
	}

	public int GetIslandCount()
	{
		return OrderedCells.Count;
	}

    public override void _ExitTree()
    {
        base._ExitTree();
		Instance = null;
		ilemap.Clear();
    }
    public override void _EnterTree()
    {
        base._EnterTree();

		Instance = this;
		ilemap = new Dictionary<Vector2, IslandInfo>();
    }
	public void UnlockLightHouse(IslandInfo Info)
	{
		UnlockedLightHouses.Add(Info.Position);
		if (UnlockedLightHouses.Count == 4)
		{
			Player pl = Player.GetInstance();
			pl.CanTraverseDeep = true;
			DialogueManager.GetInstance().ScheduleDialogue(pl, "Και οι 4 φάροι είναι αναμένοι, μπορώ να φτάσω την Μητρόπολη τώρα.");
			//pl.GetTalkText().Talk("Και οι 4 φάροι είναι αναμένοι, μπορώ να φτάσω την Μητρόπολη τώρα.");
		}
	}
	public bool IsLightHouseUnlocked(IslandInfo Info)
	{
		return UnlockedLightHouses.Contains(Info.Position);
	}
	public int GetUnlockedLightHouseCount()
	{
		return UnlockedLightHouses.Count;
	}
    public Dictionary<string, object> GetSaveData()
	{
		/*if (IleToSave != null)
		{
			SaveIsland();
			IleToSave = null;

			IslandSpawnIndex += 1;
			if (IslandSpawnIndex >= OrderedCells.Count)
				finishedspawning = true; 
		}


		Vector2[] OrdC = new Vector2[OrderedCells.Count];
		OrderedCells.CopyTo(OrdC);*/


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
		Vector2[] UnlockedLs = new Vector2[UnlockedLightHouses.Count];

		if (UnlockedLightHouses.Count > 0)
		{
			int v = 0;			
			foreach (Vector2 unLockedLight in UnlockedLightHouses)
			{
				UnlockedLs[v] = unLockedLight;
				v +=1;
			}
		}

		Dictionary<string, object> data = new Dictionary<string, object>(){
			{"RandomisedEntryID" , RandomisedEntryID},
			{"currentile", IslandSpawnIndex},
			//{"ΜαχαλάςEntryID", ΜαχαλάςEntryID},
			{"ExitID", ExitID},
			{"Exitpalcement", Exitpalcement},
			{"EventWallID", WallEventID},
			//{"OrderedCells", OrdC},
			{"CurrentTile", CurrentTile},
			{"RandomTimes", RandomContainer.GetState()},
			{"Seed", Settings.GetGameSettings().Seed},
			{"UnlockedLightHouses", UnlockedLs},
			{"ilemap", Iles},
			{"ilemapVectors", IleVectors},
			{"finishedspawning", finishedspawning}
		};
		return data;
	}
	public string GetExitDirection()
	{
		string direction = GetCompassDirection(CurrentTile, Exitpalcement);
		return direction;
	}
	public static void Main()
    {
        Vector2 vector1 = new Vector2(1, 0); // Example vector
        Vector2 vector2 = new Vector2(0, 1); // Example vector

        string direction = GetCompassDirection(vector1, vector2);
        Console.WriteLine($"The direction from vector1 to vector2 is: {direction}");
    }

    public static string GetCompassDirection(Vector2 vector1, Vector2 vector2)
    {
        float angle = CalculateAngle(vector1, vector2);

        return AngleToCompassDirection(angle);
    }

    private static float CalculateAngle(Vector2 vector1, Vector2 vector2)
    {
        float dotProduct = vector1.x * vector2.x + vector1.y * vector2.y;
        float magnitude1 = Mathf.Sqrt(vector1.x * vector1.x + vector1.y * vector1.y);
        float magnitude2 = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);

        float cosTheta = dotProduct / (magnitude1 * magnitude2);

        float angleRadians = Mathf.Acos(cosTheta);

        float angleDegrees = angleRadians * (180 / Mathf.Pi);

        if (vector1.x * vector2.y - vector1.y * vector2.x < 0)
        {
            angleDegrees = 360 - angleDegrees;
        }
        
        return angleDegrees;
    }

    private static string AngleToCompassDirection(float angle)
    {
		string dir = string.Empty;
        if (angle >= 337.5 || angle < 22.5)
            dir = "Βόρεια";
        else if (angle >= 22.5 && angle < 67.5)
            dir = "Βορειοανατολικά";
        else if (angle >= 67.5 && angle < 112.5)
            dir = "Ανατολικά";
        else if (angle >= 112.5 && angle < 157.5)
            dir = "Νοτιοανατολικά";
        else if (angle >= 157.5 && angle < 202.5)
            dir = "Νότια";
        else if (angle >= 202.5 && angle < 247.5)
            dir = "Νοτιοδυτικά";
        else if (angle >= 247.5 && angle < 292.5)
            dir = "Δυτικά";
        else if (angle >= 292.5 && angle < 337.5)
            dir = "Βορειοδυτικά";
        else
            dir = "Unknown";
		return LocalisationHolder.GetString(dir);
    }
	public void LoadSaveData(Resource data)
	{
		finishedspawning = (bool)data.Get("finishedspawning");
		if (finishedspawning)
			SetProcess(false);
		CurrentTile = (Vector2)data.Get("CurrentTile");

		int seed = (int)data.Get("Seed");
		Settings.GetGameSettings().Seed = seed;

		RandomContainer.LoadState((int)data.Get("RandomTimes"), seed);

		IslandSpawnIndex = (int)data.Get("currentile");
		//ΜαχαλάςEntryID = (int)data.Get("MahalasEntryID");
		Exitpalcement = (Vector2)data.Get("Exitpalcement");
		WallEventID = (int)data.Get("EventWallID");
		ExitID = (int)data.Get("ExitID");

		/*Vector2[] cells = (Vector2[])data.Get("OrderedCells");
		foreach (Vector2 cell in cells)
		{
			OrderedCells.Add(cell);
		}*/

		int[] RandomisedEntryIDArray = (int[])data.Get("RandomisedEntryID");
		foreach (int cell in RandomisedEntryIDArray)
		{
			RandomisedEntryID.Add(cell);
		}

		//random = data.random;
		Vector2[] IleVectors = (Vector2[])data.Get("ilemapvectors");
		Godot.Collections.Array Iles = (Godot.Collections.Array)data.Get("ilemap");

		MapGrid grid = ((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid();
		for (int i = 0; i < Iles.Count; i++)
		{
			IslandInfo info = new IslandInfo((Resource)Iles[i]);
			//info.UnPackData((Resource)Iles[i]);
			ilemap.Add(IleVectors[i], info);

			//Godot.Image im = new Godot.Image();
			ImageTexture tex = new ImageTexture();
			tex.CreateFromImage(IslandImageHolder.GetInstance().Images[info.ImageIndex]);
			//tex.Load(ile.Image);

			string name = info.SpecialName;
			//MapGrid.GetInstance().UpdateIleInfo(info.Position, info.Type, info.HasPort, info.Ports, - info.RotationToSpawn, tex, info.SpecialName);
			grid.UpdateIleInfo(info.Position, info.Visited, - info.RotationToSpawn, tex, name);
		}

		grid.FrameMap();
		
		Vector2[] UnlockedLs = (Vector2[])data.Get("UnlockedLightHouses");

		for (int i = 0; i < UnlockedLs.Count(); i++)
		{
			UnlockedLightHouses.Add(UnlockedLs[i]);
		}

	}
	public static WorldMap GetInstance()
	{
		return Instance;
	}
	public override void _Ready()
	{
		int seed = Settings.GetGameSettings().Seed;
		
		RandomContainer.OnGameStart(seed);

		CellSize = new Vector2(CellSizeOverride, CellSizeOverride);

		/*for (int i = 0; i < ScenesToSpawnLocs.Count(); i++)
		{

			loadedscenes.Insert(i, ScenesToSpawnLocs[i]);
		}*/
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
				((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid().InitMap();
				return;
			}
			int[] Date = (int[])save.Get("Date");
			Sky.GetEnviroment().SetTime(Date[0], Date[1], Date[2]);

			((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid().InitMap();

			LoadSaveData(save);
			IslandInfo CurIle;
			ilemap.TryGetValue(WorldToMap(CurrentTile), out CurIle);
			foreach (KeyValuePair<Vector2, IslandInfo> entry in ilemap)
			{
				if (entry.Value.KeepInstance == true)
				{
					Island ile = SpawnIsland(entry.Value);
					
					MyWorld.GetInstance().ToggleIsland(entry.Value, true, false);
					MyWorld.GetInstance().ToggleIsland(entry.Value, false, false);
					ile.InputData(entry.Value);
				}
			}
			//((MyWorld)GetParent()).ToggleIsland(CurIle, true, true);
			Intro intro = SpawnIntro(CurIle);
			intro.LoadStop((Vector3)save.Get("playerlocation"));
			Player pl = Player.GetInstance();

			Sky.GetEnviroment().UpdatePlayerDistance(Math.Max(Math.Abs(CurIle.Position.x), Math.Abs(CurIle.Position.y)) / 11);
			
			//PlayerUI.OnMenuToggled(false);
			Control c = pl.GetNodeOrNull<Control>("Tutorial");
			if (c != null)
				c.QueueFree();

			Vector2[] UnlockedLs = (Vector2[])save.Get("UnlockedLightHouses");

			if (UnlockedLs.Count() == 4)
			{
				pl.CanTraverseDeep = true;
			}
			//setting player energy
			pl.SetEnergy((float)save.Get("PlayerEnergy"));
			pl.BabyAlive = (bool)save.Get("BabyAlive");
			if ((bool)save.Get("HasBaby"))
			{
				pl.OnBabyGot();
			}
			else
			{
				DepartureSystemPosition.GetInstance().SpawnSystem();
			}
				

			//loading inventory
			Inventory inv = pl.GetCharacterInventory();
			Godot.Collections.Array Items = (Godot.Collections.Array)save.Get("InventoryContents");
			inv.LoadSavedInventory(Items);

			bool HasVehicle = (bool)save.Get("PlayerHasVehicle");
			if (HasVehicle)
			{
				string vehname = (string)save.Get("VehicleName");
				//bool MachineState = (bool)save.Get("VehicleState");
				//bool WingState = (bool)save.Get("WingState");
				//bool LightState = (bool)save.Get("LightState");


				Vehicle veh = GetCurrentIsland().GetNode<Spatial>(vehname).GetNode<Vehicle>("VehicleBody");
				veh.BoardVehicle(pl);
				pl.SetVehicle(veh);
				//veh.PlayerOwned = true;
				//veh.ToggleMachine(MachineState);
				//veh.ToggleWings(WingState);
				//veh.ToggleLights(LightState);
			}

			//Vector2[] DelJobs =  (Vector2[])save.Get("DeliverJobs");
			//int activeDJob = (int)save.Get("ActiveDeliveryJob");
			//GlobalJobManager jm = GlobalJobManager.GetInstance();
			//jm.LoadDeliverJobs(DelJobs, activeDJob);
			//jm.SetJobAmm(DelJobs.Count());
		}
		else
		{
			ArrangeCellsBasedOnDistance();
			((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid().InitMap();
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
		return CurIle.Island;
	}
	//public Island TempSpawn(IslandInfo i)
	//{

	//}
	public IslandInfo GetRandomIle(int MinDist, int MaxDist)
	{
		List<IslandInfo> iles = new List<IslandInfo>();
		foreach (KeyValuePair<Vector2, IslandInfo> i in ilemap)
		{
			Vector2 k = i.Key;
			float dist = Math.Max(k.x, k.y);
			if (dist > MinDist && dist < MaxDist)
			{
				iles.Add(i.Value);
			}
		}
		int index = RandomContainer.Next(0, iles.Count - 1);

		return iles[index];
	}
	public Vector2 GlobalToMap(Vector3 pos)
	{
		return WorldToMap(new Vector2(pos.x, pos.z));
	}
	public IslandInfo GetRandomLightHouse(int MinDist, int MaxDist)
	{
		List<IslandInfo> iles = new List<IslandInfo>();
		foreach (KeyValuePair<Vector2, IslandInfo> i in ilemap)
		{
			if (i.Value.Type != IleType.LIGHTHOUSE)
				continue;
			Vector2 k = i.Key;
			float dist = Math.Max(k.x, k.y);
			if (dist > MinDist && dist < MaxDist)
			{
				iles.Add(i.Value);
			}
		}
		if (iles.Count == 0)
			return null;
		int index = RandomContainer.Next(0, iles.Count - 1);

		return iles[index];
	}
	IslandInfo IleToSave;
	float d = 1;
	public override void _PhysicsProcess(float delta)
	{
		d -= delta;
		if (d > 0)
			return;
			
		d = 1;
		CheckForTransition();
	}
    public override void _Process(float delta)
    {
		if (IleToSave == null)
		{
			GenerateIsland(IslandSpawnIndex);
		}
    }
    public IslandInfo GetIle(Vector2 pos)
	{
		return ilemap[pos];
	}
	void GenerateIsland(int tilenum)
	{
		//Get Cell id
		
		int id = GetCell((int)OrderedCells[IslandSpawnIndex].x, (int)OrderedCells[IslandSpawnIndex].y);
		//Cell cords
		Vector2 cell = OrderedCells[IslandSpawnIndex];

		string SpecialName;

		//Scene using ID from cell
		PackedScene ilescene = ResourceLoader.Load<PackedScene>(GetSceneToSpawn(id, out SpecialName));


		//Start the Data saving
		//Spawndata to be used when spawning
		float rot;
		rot = RandomContainer.Next(0, 360);
		
		IslandInfo ileinfo = new IslandInfo(rot, ilescene, cell, SpecialName);

		
		//SaveEntry
		if (id == 0)
			entry = cell;

		IleToSave = ileinfo;
		ilemap.Add(OrderedCells[tilenum], ileinfo);

		CallDeferred("ThreadedInstance");

		return;
	}

	public void ThreadedInstance()
	{
		Island instancedile = (Island)IleToSave.IleType.Instance();
		CallDeferred("AddSpawnInfo", instancedile);
	}
	public void AddSpawnInfo(Island ile)
	{
		if (!ile.RotateIle)
			IleToSave.RotationToSpawn = 0;
			
		Vector2 postoput = MapToWorld(new Vector2(IleToSave.Position.x, IleToSave.Position.y));

		postoput += CellSize / 2;

		ile.SetSpawnInfo(new Vector3(postoput.x, 0, postoput.y), IleToSave.RotationToSpawn, IleToSave.SpecialName);

		IleToSave.Island = ile;
		IleToSave.ImageIndex = ile.ImageID;

		CallDeferred("DoInitialSpawn");
	}
	public void DoInitialSpawn()
	{
		IslandInfo ilei = IleToSave;

		IslandSpawnIndex += 1;
		if (IslandSpawnIndex == OrderedCells.Count)
		{
			finishedspawning = true; 
			//loadedscenes.Clear();
			SetProcess(false);
		}

		Island ile = ilei.Island;

		ile.InitialSpawn();

		CallDeferred("SaveIsland");
		if (finishedspawning == true)
		{
			SpawnIntro();
			Sky.GetEnviroment().UpdatePlayerDistance(Math.Max(Math.Abs(ilemap[entry].Position.x), Math.Abs(ilemap[entry].Position.y)) / 11);
			((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid().FrameMap();
		}
	}
	public void SaveIsland()
	{
		IslandInfo ilei = IleToSave;
		Island ile = ilei.Island;

		ilei.SetInfo(ile);

		IleToSave = null;

		DespawnIle(ilei.Island, ilei.KeepInstance);

		//CallDeferred("AddMapData", ilei.Position, ilei.Type, ilei.HasPort, ilei.Ports, ilei.RotationToSpawn, ile.ImageID, ilei.SpecialName);
		string name = ile.IslandName;
		//if (ile.UnlockName)
			//name = ile.IslandName;

		ImageTexture tex = new ImageTexture();
		tex.CreateFromImage(IslandImageHolder.GetInstance().Images[ile.ImageID]);

		((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid().UpdateIleInfo(ilei.Position, ilei.Visited, - ilei.RotationToSpawn, tex, name);
	}
	void DespawnIle(Island ile, bool KeepInstance)
	{
		if (KeepInstance)
		{
			ile.Visible = false;
			if (ile.GetParent() != null)
				ile.GetParent().RemoveChild(ile);
		}
		else
		{
			ile.QueueFree();
		}
	}
	public Intro SpawnIntro(IslandInfo info = null)
	{
		IslandInfo start;
		if (info != null)
			start = info;
		else
			start = ilemap[entry];

		
		

		//MyWorld.IleTransition(start);
		MyWorld.GetInstance().ToggleIsland(start, true, true);
				
		Island island = start.Island;

		//MyWorld.GetInstance().Translation = - new Vector3(info.Position.x * CellSize.x, 0, info.Position.y * CellSize.y);

		//MapGrid.GetInstance().SetIslandVisited(start);

		Intro intr = (Intro)ResourceLoader.Load<PackedScene>(IntroScene).Instance();

		EmitSignal("OnTransitionEventHandler", island);

		GetParent().AddChild(intr);
		
		intr.GlobalTranslation = island.GlobalTranslation;
		intr.GlobalRotation = island.GlobalRotation;

		CurrentTile = new Vector2 (island.GlobalTranslation.x ,island.GlobalTranslation.z);

		//GlobalJobManager.GetInstance().OnNewDay();

		//OS.VsyncEnabled = true;
		return intr;
	}
	private void CheckForTransition()
	{
		if (!Player.IsSpawned())
			return;
		Player pl = Player.GetInstance();
		Vector3 pos = pl.GlobalTranslation + (-MyWorld.GetInstance().Translation);
		Vector2 plpos = new Vector2(pos.x, pos.z);
		if (plpos.DistanceTo(CurrentTile) > CellSize.x/2)
		{
			Vector2 curt = FindClosestIslandPosition(plpos);

			if (CurrentTile != curt)
			{
				IslandInfo ileinf;
				ilemap.TryGetValue(WorldToMap(CurrentTile), out ileinf);
				IslandInfo ileinfto;
				ilemap.TryGetValue(WorldToMap(curt), out ileinfto);
				CurrentTile = curt;
				if (pl.HasVehicle())
				{
					Vehicle veh = pl.GetVehicle();
					veh.ReparentVehicle(ileinfto.Island);
					ileinfto.AddNewVehicle(veh);
				}
				
				MyWorld.GetInstance().IleTransition(ileinfto);
				
				EmitSignal("OnTransitionEventHandler", ileinfto.Island);
			}
		}
	}
	public IslandInfo GetCurrentIleInfo()
	{
		IslandInfo info;
		ilemap.TryGetValue(WorldToMap(CurrentTile), out info);
		return info;
	}
	public IslandInfo GetIleInfo(Island ile)
	{
		IslandInfo info = null;
		Vector2 ilekey = new Vector2 (ile.SpawnGlobalLocation.x, ile.SpawnGlobalLocation.z);
		ilekey -= CellSize / 2;
		ilekey = WorldToMap(ilekey);
		info = ilemap[ilekey];
		return info;
	}
	public Island SpawnIsland(IslandInfo info)
	{
		Island Ile = (Island)info.IleType.Instance();

		Vector2 postoput = MapToWorld(new Vector2(info.Position.x, info.Position.y));

		postoput += CellSize / 2;

		Ile.SetSpawnInfo(new Vector3(postoput.x, 0, postoput.y), info.RotationToSpawn, info.SpecialName);

		info.Island = Ile;
		info.ImageIndex = Ile.ImageID;
		if (UnlockedLightHouses.Contains(info.Position))
		{
			LightHouse lhouse = Ile.GetNode<LightHouse>("Spatial/LightHouse");
			lhouse.Enabled = true;
		}
		return Ile;
	}
	public Island ReSpawnIsland(IslandInfo info)
	{
		Island Ile = (Island)info.IleType.Instance();

		Vector2 postoput = MapToWorld(new Vector2(info.Position.x, info.Position.y));

		postoput += CellSize / 2;

		Ile.SetSpawnInfo(new Vector3(postoput.x, 0, postoput.y), info.RotationToSpawn, info.SpecialName);

		info.Island = Ile;
		
		if (UnlockedLightHouses.Contains(info.Position))
		{
			LightHouse lhouse = Ile.GetNode<LightHouse>("Spatial/LightHouse");
			lhouse.ToggeLightHouse(true);
		}

		MyWorld.GetInstance().AddChild(Ile);
		return Ile;
	}
	void ArrangeCellsBasedOnDistance()
	{
		//arange all the cells by distance from center of world put in OrderedCells array
		var cells = GetUsedCells();
		
		OrderedCells.Insert(0, (Vector2)cells[0]);
		List<int> absolutesums = new List<int>(cells.Count){
			(int)(Math.Abs(OrderedCells[0].x) + Math.Abs(OrderedCells[0].y))
		};

		for (int x = 1; x < cells.Count; x++)
		{
			Vector2 cellArray = (Vector2)cells[x];
			int ind = (int)(Math.Abs(cellArray.x) + Math.Abs(cellArray.y));

			int closestind = Math.Abs(absolutesums.BinarySearch(ind)) - 1;
			closestind = Math.Max(0, closestind);
			OrderedCells.Insert(closestind, cellArray);

			absolutesums.Insert(closestind, ind);
		}
		//produce indexes of where tiles events will be placed on
		
		for (int i = 0; i < Eventscenestospawn.Count(); i++)
		{
			int SpawnIndex = RandomContainer.Next(0, OrderedCells.Count);
			while (RandomisedEntryID.Contains(SpawnIndex) || SpawnIndex > OrderedCells.Count * 0.63f || SpawnIndex < 9 || GetCell((int)OrderedCells[SpawnIndex].x, (int)OrderedCells[SpawnIndex].y) != 1)
			{
				SpawnIndex = RandomContainer.Next(0, OrderedCells.Count);
			}
			RandomisedEntryID.Insert(i, SpawnIndex);
		}

		// μαχαλάς randomise
		//var lighthousecells = GetUsedCellsById(4);
		//int RandomLightHouseIndex = RandomContainer.Next(0, lighthousecells.Count);
		//Vector2 Μαχαλάςpalcement = (Vector2)lighthousecells[RandomLightHouseIndex];
		//ΜαχαλάςEntryID = OrderedCells.IndexOf(Μαχαλάςpalcement);

		//wall even randomise

		var Wallcells = GetUsedCellsById(5);
		int RandomWallIndex = RandomContainer.Next(0, Wallcells.Count);
		Vector2 WallEventpalcement = (Vector2)Wallcells[RandomWallIndex];

		//while loop to avoid adding it to corners
		while (Math.Abs(WallEventpalcement.x) == Math.Abs(WallEventpalcement.y))
		{
			RandomWallIndex = RandomContainer.Next(0, Wallcells.Count);
			WallEventpalcement = (Vector2)Wallcells[RandomWallIndex];
		}
		WallEventID = OrderedCells.IndexOf(WallEventpalcement);

		//exit randomise
		var exitcells = GetUsedCellsById(2);
		int RandomExitIndex = RandomContainer.Next(0, exitcells.Count);
		Exitpalcement = (Vector2)exitcells[RandomExitIndex];
		ExitID = OrderedCells.IndexOf(Exitpalcement);
	}
	
	//takes in cell position gives out global transforms of closest island
	Vector2 FindClosestIslandPosition(Vector2 pos)
	{
		float dist = 999999999;
		Vector2 closest = Vector2.Zero;
		foreach(KeyValuePair<Vector2, IslandInfo> entry in ilemap)
		{
			Vector2 posi = entry.Value.Position;
			Vector2 ilepos = MapToWorld(new Vector2(posi.x, posi.y));
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
		Vector2 pos = info.Position;
		Vector3 transform = new Vector3(pos.x, 0, pos.y);

		int ammount = 0;
		for (int i = 0; i < dist; i ++)
			ammount += ammount + 8;

		int offset = 1 * dist;
		float row = transform.x - offset;
		float collumn = transform.z - offset;
		for (int i = 0; i < ammount + 1; i++)
		{
			if (new Vector2(row, collumn) != pos)
			{
				IslandInfo ile;
				ilemap.TryGetValue(new Vector2(row, collumn), out ile);
				if (ile != null)
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
	//
    // Summary:
	
	/// <summary>
	/// 0 = entry /
	///	1 = random or event /
	///	2 = exit /
	///	3 = sea /
	///	4 = lighthouse /
	/// </summary>
	/// <param name="type"></param>
	/// <param name="SpecialName"></param>
	/// <returns></returns>
	string GetSceneToSpawn(int type, out string SpecialName)
	{
		string scene = null;
		SpecialName = "No_Name";
		
		switch (type)
		{
			case 0:
			{
				scene = Entrytospawn;
				break;
			}
			case 1:
			{
				if (RandomisedEntryID.Contains(IslandSpawnIndex))
					scene = Eventscenestospawn[RandomisedEntryID.IndexOf(IslandSpawnIndex)];
				else
				{
					scene = ScenesToSpawnLocs[RandomContainer.Next(0, ScenesToSpawnLocs.Count())];
				}
				break;
			}
			case 2:
			{
				if (IslandSpawnIndex == ExitID)
					scene =  Exittospawn;
				else
				{
					scene = ScenesToSpawnLocs[RandomContainer.Next(0, ScenesToSpawnLocs.Count())];
				}
				break;
			}
			case 3:
			{
				scene =  SeaVariations[RandomContainer.Next(0, SeaVariations.Count())];
				break;
			}
			case 4:
			{

				if (LightHouseNames != null && LightHouseNames.Count() > 0)
				{
					SpecialName = LightHouseNames[LightHouseNames.Count() - 1];
					LightHouseNames.Remove(SpecialName);
				}

				scene = LightHouse;
				//}
				break;
			}
			case 5:
			{
				if (IslandSpawnIndex == WallEventID)
				{
					scene =  EventWall;
				}
				else
				{
					scene =  WallToSpawn;
				}
				break;
				
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




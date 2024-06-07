using Godot;
using System;
using System.Collections.Generic;

public class SaveLoadManager : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	static SaveLoadManager Instance;
	public override void _Ready()
	{
		Instance = this;
	}
	public static SaveLoadManager GetInstance()
	{
		return Instance;
	}
	public void SaveGame()
	{
		GDScript SaveGD = GD.Load<GDScript>("res://Scripts/saved_game.gd");
		Godot.Object save = (Godot.Object)SaveGD.New();
		

		WorldMap map = WorldMap.GetInstance();
		MyWorld world = MyWorld.GetInstance();

		if (world == null)
			return;

		

		Player pl = Player.GetInstance();
		if (pl == null)
			return;

		bool HasVecicle = pl.HasVecicle;
		Dictionary<string, object> pldata = new Dictionary<string, object>(){
			
		};
		pldata.Add("playerHasVehicle", HasVecicle);
		if (HasVecicle)
		{
			Vehicle veh = pl.currveh;
			pldata.Add("VehicleName", veh.GetParent().Name);
			pldata.Add("VehicleState", veh.IsRunning());
			pldata.Add("WingState", veh.HasDeployedWings());
			pldata.Add("LightState", veh.LightCondition());
			pldata.Add("PlayerLocation", ((Spatial)veh.GetParent()).GlobalTranslation);
		}
		else
			pldata.Add("PlayerLocation", pl.GlobalTranslation);

		foreach (IslandInfo ile in world.GetActiveIles())
		{
			world.ToggleIsland(ile, false, false);
		}
		Dictionary<string, object> data = map.GetSaveData();
		
		foreach (KeyValuePair<string, object> dat in pldata)
		{
			data.Add(dat.Key, dat.Value);
		}
		
		data.Add("PlayerEnergy", pl.GetCurrentEnergy());

		int day, hour, mins;
		DayNight.GetDay(out day);
		DayNight.GetTime(out hour, out mins);

		int[] Date = new int[3];
		Date[0] = day;
		Date[1] = hour;
		Date[2] = mins;

		data.Add("Date", Date);


		Dictionary<Vector2, int> MapData = MapGrid.GetInstance().GetSaveData();
		Vector2[] MapGridVectorData = new Vector2[MapData.Count];
		int[] MapGridTypeData = new int[MapData.Count];
		int i = 0;
		foreach (KeyValuePair<Vector2 , int> kvp in MapData)
		{
			MapGridVectorData[i] = kvp.Key;
			MapGridTypeData[i] = kvp.Value;
			i++;

		}
		data.Add("MapGridVectors", MapGridVectorData);
		data.Add("MapGridTypes", MapGridTypeData);

		GDScript InventorySaveGD = GD.Load<GDScript>("res://Scripts/InventoryItemInfo.gd");
		
		
		List<Item> InventoryContents = new List<Item>();
		pl.GetCharacterInventory().GetContents(out InventoryContents);
		
		Resource[] Inventorydata = new Resource[InventoryContents.Count];
		int v = 0;
		foreach(Item it in InventoryContents)
		{
			Resource Inventorysave = (Resource)InventorySaveGD.New();
			Dictionary<string, object> Itemdata = new Dictionary<string, object>();
			string[] Keys = new string[4];
			object[] Values = new object[4];
			Itemdata.Add("SceneData", it.Filename);
			bool HasData = false;
			if (it is Battery)
			{
				Keys[0] = "CurrentEnergy";
				Values[0] = ((Battery)it).GetCurrentCap();
				HasData = true;
			}
			Itemdata.Add("CustomDataKeys", Keys);
			Itemdata.Add("CustomDataValues", Values);
			Inventorysave.Call("_SetData", Itemdata, HasData);
			Inventorydata[v] = Inventorysave;
			v ++;
		}
		data.Add("InventoryContents", Inventorydata);
		save.Call("_SetData", data);
		/*save.RandomisedEntryID = (List <int>)data["RandomisedEntryID"];
		save.currentile = (int)data["currentile"];
		save.ΜαχαλάςEntryID = (int)data["ΜαχαλάςEntryID"];
		save.ExitID = (int)data["ExitID"];
		save.OrderedCells = (List <Vector2> )data["OrderedCells"];
		save.random = (Random)data["random"];
		save.CurrentTile = (Vector2)data["CurrentTile"];
		save.ilemap = (Dictionary<Vector2, IslandInfo>)data["ilemap"];
		save.finishedspawning = (bool)data["finishedspawning"];
		save.playerlocation = pl.GlobalTranslation;
		save.playerenergy = pl.GetCurrentEnergy();*/
		

		//File savef = new File();
		ResourceSaver.Save("user://SavedGame.tres", (Resource)save);
	}
}
/*public class SavedGame : Resource
{
	[Export]
	public List <int> RandomisedEntryID { get; set; }

	[Export]
	public int currentile { get; set; }

	[Export]
	public int ΜαχαλάςEntryID { get; set; }

	[Export]
	public int ExitID { get; set; }

	[Export]
	public List <Vector2> OrderedCells { get; set; }

	[Export]
	public Random random { get; set; }

	[Export]
	public Vector2 CurrentTile { get; set; }

	[Export]
	public Dictionary<Vector2, IslandInfo> ilemap { get; set; }

	[Export]
	public bool finishedspawning { get; set; }

	[Export]
	public Vector3 playerlocation  { get; set; }
	[Export]
	public float playerenergy { get; set; }
}  */

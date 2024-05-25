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

		foreach (IslandInfo ile in world.GetActiveIles())
		{
			world.ToggleIsland(ile, false, false);
		}

		Player pl = Player.GetInstance();

		Dictionary<string, object> data = map.GetSaveData();
		data.Add("PlayerLocation", pl.GlobalTranslation);
		data.Add("PlayerEnergy", pl.GetCurrentEnergy());
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

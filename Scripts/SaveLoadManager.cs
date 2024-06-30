using Godot;
using System;
using System.Collections.Generic;

public class SaveLoadManager : Control
{
	static SaveLoadManager Instance;
	public override void _Ready()
	{
		Instance = this;
	}
	public static SaveLoadManager GetInstance()
	{
		return Instance;
	}
	public void ClearSaves()
	{
		 Directory dir = new Directory();

		dir.Remove("user://SavedGame.tres");
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


		GDScript InventorySaveGD = GD.Load<GDScript>("res://Scripts/InventoryItemInfo.gd");
		
		
		List<Item> InventoryContents;
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
			if (it is Limb)
			{
				Keys[0] = "LimbColor";
				Values[0] = ((Limb)it).GetColor();
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
		

		//File savef = new File();
		ResourceSaver.Save("user://SavedGame.tres", (Resource)save);
	}
}

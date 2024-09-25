using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
		GameOverTrigger.ToggleTriggers(false);
		
		GDScript SaveGD = GD.Load<GDScript>("res://Scripts/saved_game.gd");
		Godot.Object save = (Godot.Object)SaveGD.New();

		Player pl = Player.GetInstance();

		bool HasVecicle = pl.HasVehicle();
		Dictionary<string, object> pldata = new Dictionary<string, object>(){
			{"PlayerHasVehicle", HasVecicle},
			{"PlayerEnergy", pl.GetCurrentEnergy()},
			{"BabyAlive", pl.BabyAlive},
			{"HasBaby", pl.HasBaby}

		};
		if (HasVecicle)
		{
			Vehicle veh = pl.GetVehicle();
			pldata.Add("VehicleName", veh.GetParent().Name);
			//pldata.Add("VehicleState", veh.IsRunning());
			//pldata.Add("WingState", veh.HasDeployedWings());
			//pldata.Add("LightState", veh.LightCondition());
			pldata.Add("PlayerLocation", ((Spatial)veh.GetParent()).GlobalTranslation);
		}
		else
			pldata.Add("PlayerLocation", pl.GlobalTranslation);


		WorldMap map = WorldMap.GetInstance();
		MyWorld world = MyWorld.GetInstance();

		foreach (IslandInfo ile in world.GetActiveIles())
		{
			world.ToggleIsland(ile, false, false);
		}

		Dictionary<string, object> data = map.GetSaveData();
		
		foreach (KeyValuePair<string, object> dat in pldata)
		{
			data.Add(dat.Key, dat.Value);
		}

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
			string[] Keys;
			object[] Values;
			Itemdata.Add("SceneData", it.Filename);
			it.GetCustomData(out Keys, out Values);
			Itemdata.Add("CustomDataKeys", Keys);
			Itemdata.Add("CustomDataValues", Values);
			Inventorysave.Call("_SetData", Itemdata);
			Inventorydata[v] = Inventorysave;
			v ++;
		}
		data.Add("InventoryContents", Inventorydata);

		/*GlobalJobManager man = GlobalJobManager.GetInstance();
		


		List <Job> jobs = man.GetJobs();
		List <Job> Ajobs = man.GetAssignedJobs();
		List <Job> DelJobs = new List<Job>();
		int activeDJob = -1;

		foreach (Job j in jobs)
		{
			if (j is DeliverJob)
			{
				DelJobs.Add(j);
			}
		}
		Vector2[] DelArr = new Vector2[DelJobs.Count];
		for (int i = 0; i < DelJobs.Count(); i++)
		{
			if (Ajobs.Contains(DelJobs[i]))
				activeDJob = i;
;			DelArr[i] = DelJobs[i].GetLocation();
		}
		data.Add("DeliverJobs", DelArr);
		data.Add("ActiveDeliveryJob", activeDJob);*/
		save.Call("_SetData", data);
		

		//File savef = new File();
		ResourceSaver.Save("user://SavedGame.tres", (Resource)save);
	}
}

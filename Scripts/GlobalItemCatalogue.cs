using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class GlobalItemCatalogue : Node
{
	[Export]
	string[] GlobalItemDirList = null;
    //[Export]
	//PackedScene[] GlobalItemListConfiguration = null;
    [Export]
    string[] CharacterCataluge = null;
    
    [Export]
    string[] VehicleCatalogue = null;

    Dictionary<ItemName, Dictionary<string, PackedScene>> GlobalItemList = new Dictionary<ItemName, Dictionary<string, PackedScene>>();

    static GlobalItemCatalogue Instance;

    public static GlobalItemCatalogue GetInstance()
	{
		return Instance;
	}
    public override void _Ready()
	{
		base._Ready();
		//pl = GetNode<Player>("Player");
		foreach (string pair in GlobalItemDirList)
		{
			PackedScene sc = ResourceLoader.Load<PackedScene>(pair);
			Item it = sc.Instance<Item>();
			
			string key = it.GetItemName();
			
			if (GlobalItemList.ContainsKey(it.ItemType))
			{
				Dictionary<string, PackedScene> dic = GlobalItemList[it.ItemType];
				dic.Add(key, sc);
			}
			else
			{
				Dictionary<string, PackedScene> dic = new Dictionary<string, PackedScene>(){
				{key, sc}
				};
				GlobalItemList.Add(it.ItemType, dic);
			}
			
			it.QueueFree();
		}
		Instance = this;
	}
	public PackedScene GetRandomCharacterFromCatalogue()
	{
		PackedScene character = null;

		if (CharacterCataluge.Count() > 0)
		{
			character = ResourceLoader.Load<PackedScene>(CharacterCataluge[RandomContainer.Next(0, CharacterCataluge.Count())]);
		}
		return character;
	}
	public PackedScene GetRandomVehicleFromCatalogue()
	{
		PackedScene veh = null;

		if (VehicleCatalogue.Count() > 0)
		{
			veh = ResourceLoader.Load<PackedScene>(VehicleCatalogue[RandomContainer.Next(0, VehicleCatalogue.Count())]);
		}
		return veh;
	}
	public PackedScene GetItemByName(string name)
	{
		PackedScene path = null;
		//var lookup = GlobalItemList.ToLookup(kvp => (int)name, kvp => kvp.Value);
		foreach (KeyValuePair<ItemName, Dictionary<string, PackedScene>> thing in GlobalItemList)
		{
			Dictionary<string, PackedScene> thang = thing.Value;
			foreach (KeyValuePair<string, PackedScene> thang2 in thang)
			{
				if (thang2.Key == name)
				{
					path = thang2.Value;
				}
			}
		}
		if (path == null)
		{
			GD.Print("Trying to get item with name : " + name + " | and failed.");
		}
		return path;
	}
    public PackedScene GetItemByType(ItemName name, string stname = "Null")
	{
		Dictionary<string, PackedScene> thang = GlobalItemList[name];
		
		PackedScene thing;

		if (thang.Count > 1)
		{
			thing = thang[stname];
		}
		else
		{
			thing = thang.ElementAt(0).Value;
		}
		return thing;
	}
}

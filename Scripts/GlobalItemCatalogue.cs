using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class GlobalItemCatalogue : Node
{
    [Export]
	PackedScene[] GlobalItemListConfiguration = null;

    [Export]
    public PackedScene[] CharacterCataluge = new PackedScene[0];
    
    [Export]
    public PackedScene[] VehicleCatalogue = new PackedScene[0];

    static List<KeyValuePair<string, PackedScene>> GlobalItemList = new List<KeyValuePair<string, PackedScene>>();

    static GlobalItemCatalogue Instance;

    public static GlobalItemCatalogue GetInstance()
	{
		return Instance;
	}
    public override void _Ready()
	{
		base._Ready();
		//pl = GetNode<Player>("Player");
		foreach (PackedScene pair in GlobalItemListConfiguration)
		{
			Item it = pair.Instance<Item>();
			
			string key = it.GetItemName();
			GlobalItemList.Add(new KeyValuePair<string, PackedScene>(key, pair));
			it.Free();
		}
		Instance = this;
	}
    public static PackedScene GetItemByType(string name)
	{
		PackedScene path = null;
		//var lookup = GlobalItemList.ToLookup(kvp => (int)name, kvp => kvp.Value);
		foreach (KeyValuePair<string, PackedScene> thing in GlobalItemList)
		{
			if (thing.Key == name)
			{
				path = thing.Value;
			}
		}
		return path;
	}
}

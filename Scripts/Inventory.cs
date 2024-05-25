using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class Inventory : Spatial
{
    [Signal]
    public delegate void On_Item_Added(Item item);

    [Signal]
    public delegate void On_Item_Removed(Item item);

    List<Item> InventoryContents;

    [Export]
    float Capacity = 10;
    [Export]
    public PackedScene[] StartingItems;
    float currentweight = 0;
    InventoryUI ui;

    public override void _Ready()
    {
        ui = InventoryUI.GetInstance();
        ui.CallDeferred("OnPlayerSpawned", (Player)GetParent());
        InventoryContents = new List<Item>();
        if (StartingItems == null)
            return;
        for (int i = 0; i < StartingItems.Count(); i ++)
        {
            Item it = (Item)StartingItems[i].Instance();
            InsertItem(it);
        }
    }

    public void LoadSavedInventory(Godot.Collections.Array items)
    {
        DeleteContents();
        foreach(var It in items)
        {
            PackedScene it = GD.Load<PackedScene>((string)((Resource)It).Get("SceneData"));
            Item newItem = it.Instance<Item>();
            InsertItem(newItem);
        }
    }
    public bool IsEmpty()
    {
        return currentweight <= 0;
    }
    public bool InsertItem(Item item)
    {
        if (currentweight + item.GetInventoryWeight() > Capacity)
        {
            return false;
        }

        InventoryContents.Insert(InventoryContents.Count, item);
        currentweight += item.GetInventoryWeight();
        var parent = item.GetParent();
        if (parent != null)
            parent.RemoveChild(item);
        AddChild(item);
        item.Hide();
        //EmitSignal(nameof(On_Item_Added), item);
        item.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",true);
        //ui.UpdateInventory();
        return true;
    }
    public bool RemoveItem(Item item)
    {
        InventoryContents.Remove(item);
        currentweight -= item.GetInventoryWeight();

        RemoveChild(item);
        IslandInfo info = WorldMap.GetInstance().GetCurrentIleInfo();
        Island ile = info.ile;
        ile.AddChild(item);
        ile.RegisterChild(item);
        info.AddNewItem(item);
        //WorldMap.GetInstance().AddChild(item);
        item.Show();
        Transform loc = ((Character)GetParent()).GlobalTransform;
        loc.origin.y += 1;
        //loc.origin.z += 1;
        item.GlobalTransform = loc;
        item.Translation = new Vector3(item.Translation.x, item.Translation.y, item.Translation.z + 1);
        //EmitSignal(nameof(On_Item_Removed), item);
        item.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
        
        ui.UpdateInventory();
        return true;
    }
    public void DeleteItem(Item item)
    {
        InventoryContents.Remove(item);
        currentweight -= item.GetInventoryWeight();
        RemoveChild(item);
        item.QueueFree();
    }
    public void RemoveAllItems()
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            RemoveItem(InventoryContents[i]);
        }
    }
    public void DeleteContents()
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            DeleteItem(InventoryContents[i]);
        }
    }
    public void GetContents(out List<Item> Items)
    {
        Items = new List<Item>();
        for (int i = InventoryContents.Count() - 1; i > -1; i--)
        {
            Items.Insert(Items.Count(), InventoryContents[i]);
        }
    }
    public void GetItemsByType(out List<Item> Items, ItemName Type)
    {
        Items = new List<Item>();
        for (int i = 0; i < InventoryContents.Count; i++)
        {
            if (InventoryContents[i].GetItemType() == (int)Type)
                Items.Insert(Items.Count(), InventoryContents[i]);
        }
    }
    public float GetAvailableCapacity()
    {
        return Capacity - currentweight;
    }
    public float GetCurrentWeight()
    {
        return currentweight;
    }
    public float GetMaxCap()
    {
        return Capacity;
    }
    public bool OverrideWeight(float NewWeight)
    {
        if (currentweight > NewWeight)
        {
            return false;
        }
        Capacity = NewWeight;
        return true;
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
		Position = it.GlobalTranslation;
		SceneData = it.Filename;
		if (it is Battery)
		{
			CustomData.Add("CurrentEnergy", ((Battery)it).GetCurrentCap());
		}
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Position", Position);
		data.Add("Name", Name);
		data.Add("SceneData", SceneData);
		if (CustomData.Count > 0)
		{
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
		string[] CustomDataKeys = (string[])data.Get("CustomDataKeys");
		object[] CustomDataValues = (object[])data.Get("CustomDataKeys");

		if (CustomDataKeys.Count() > 0 && CustomDataValues.Count() > 0)
		{
			for (int i = 0; i < CustomDataKeys.Count(); i++)
			{
				CustomData.Add(CustomDataKeys[i], CustomDataValues[i]);
			}
		}
    }
}
}

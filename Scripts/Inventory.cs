using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public string[] StartingItems;

    float currentweight = 0;
    InventoryUI ui;

    public override void _Ready()
    {
        ui = (InventoryUI)GetChild(0);
        InventoryContents = new List<Item>();
        if (StartingItems == null)
            return;
        for (int i = 0; i < StartingItems.Count(); i ++)
        {
            var itemToSpawn = GD.Load<PackedScene>(StartingItems[i]);
            Item it = (Item)itemToSpawn.Instance();
            InsertItem(it);
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
        ui.UpdateInventory();
        return true;
    }
    public bool RemoveItem(Item item)
    {
        InventoryContents.Remove(item);
        currentweight -= item.GetInventoryWeight();
        var worlds = GetTree().GetNodesInGroup("World");
        MyWorld world = (MyWorld)worlds[0];
        RemoveChild(item);
        world.AddChild(item);
        item.Show();
        item.GlobalTransform = ((Character)GetParent()).Transform;
        //EmitSignal(nameof(On_Item_Removed), item);
        item.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",false);
        ui.UpdateInventory();
        return true;
    }
    public void RemoveAllItems()
    {
        for (int i = 0; i < InventoryContents.Count; i++)
        {
            RemoveItem(InventoryContents[i]);
        }
    }
    public void GetContents(out List<Item> Items)
    {
        Items = new List<Item>();
        for (int i = 0; i < InventoryContents.Count; i++)
        {
            Items.Insert(i, InventoryContents[i]);
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
}

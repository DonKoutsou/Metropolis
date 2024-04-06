using Godot;
using System;

public class Item : RigidBody
{
    [Export]
    float ItemWeight = 0;

    [Export]
    public ItemName ItemType;

    [Export]
    public Texture ItemIcon = null;

    [Export]
    string ItemName = "Item";

    [Export]
    string ItemDesc = "Quifsa";

    public string GetItemName()
    {
        return ItemName;
    }
    public string GetItemDesc()
    {
        return ItemDesc;
    }
    public int GetItemType()
    {
        return (int)ItemType;
    }
    public override void _Ready()
    {
        GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
    }
    public Texture GetIconTexture()
    {
        return ItemIcon;
    }
    public float GetInventoryWeight()
    {
        return ItemWeight;
    }
}

public enum ItemName
{
    DRAHMA,
    ROPE,
    BATTERY,
}

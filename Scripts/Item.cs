using Godot;
using System;

public class Item : RigidBody
{
    [Export]
    float ItemWeight = 0;

    [Export]
    public ItemName ItemType;

    [Export]
    Texture ItemIcon;
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
    BRONZE_COIN,
    SILVER_COIN,
    GOLD_COIN,
}

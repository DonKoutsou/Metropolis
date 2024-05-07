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
	public string ItemDesc = "Quifsa";

	[Export]
	public bool stackable = true;

	public string GetItemName()
	{
		return ItemName;
	}
	public virtual string GetItemDesc()
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
	public void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", toggle);
    }

}

public enum ItemName
{
	DRAHMA,
	ROPE,
	BATTERY,
	CLOCK,
	COMPASS,
	MAP,
	LIMB,
}

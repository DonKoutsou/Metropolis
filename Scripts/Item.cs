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
	string ItemPickupActionName = null;

	[Export]
	string ItemName = "Item";

	[Export]
	public string ItemDesc = "Quifsa";

	[Export]
	public bool stackable = true;
	[Export]
	public bool RegisterOnIsland = true;

	public string GetItemName()
	{
		return ItemName;
	}
	public string GetItemPickUpText()
	{
		return ItemPickupActionName;
	}
	public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
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
		//GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
		if (RegisterOnIsland)
		{
			Node par = GetParent();
			if (par is Inventory || par is Furniture || par is Character)
				return;
			while (!(par is Island))
			{
				if (par == null)
					return;
				par = par.GetParent();
			}
			Island ile = (Island)par;
			ile.RegisterChild(this);
		}
		
	}
	public Texture GetIconTexture()
	{
		return ItemIcon;
	}
	public float GetInventoryWeight()
	{
		return ItemWeight;
	}
	public virtual void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", toggle);
    }
	public void InputData(ItemInfo data)
	{
		Translation = data.Position;
		Name = data.Name;
		if (this is Battery battery)
		{
			float cap = (float)data.CustomData["CurrentEnergy"];
			battery.SetCurrentCap(cap);
		}
		if (this is Limb limb)
		{
			Color cap = (Color)data.CustomData["LimbColor"];
			limb.SetColor(cap);
		}
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
	GUITAR,
}

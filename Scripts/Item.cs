using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Item : RigidBody
{
	//[Export]
	//int ItemWeight = 0;

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
	public virtual string GetItemDesc()
	{
		return ItemDesc;
	}
	public ItemName GetItemType()
	{
		return ItemType;
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
	//public int GetInventoryWeight()
	//{
		//return ItemWeight;
	//}
	public virtual void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable", toggle);
    }
	public void InputData(ItemInfo data)
	{
		Translation = data.Position;
		Name = data.Name;
		if (this is Battery battery)
		{
			float cap = (float)data.CustomData["CurrentEnergy"];
			battery.SetCurrentCap(cap);
			float cond = (float)data.CustomData["CurrentCondition"];
			battery.SetCurrentCondition(cond);
		}
		if (this is Toolbox box)
		{
			float cap = (float)data.CustomData["CurrentSupplies"];
			box.SetCurrentSupplies(cap);
		}
		if (this is Limb limb)
		{
			Color cap = (Color)data.CustomData["LimbColor"];
			limb.SetColor(cap);
		}
	}

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
		Position = it.Translation;
		SceneData = it.Filename;
		if (it is Battery bat)
		{
			if (CustomData.ContainsKey("CurrentEnergy"))
				CustomData["CurrentEnergy"] = bat.GetCurrentCap();
			else
				CustomData.Add("CurrentEnergy", bat.GetCurrentCap());
			if (CustomData.ContainsKey("CurrentCondition"))
				CustomData["CurrentCondition"] = bat.GetCondition();
			else
				CustomData.Add("CurrentCondition", bat.GetCondition());
		}
		if (it is Toolbox box)
		{
			if (CustomData.ContainsKey("CurrentSupplies"))
				CustomData["CurrentSupplies"] = box.GetCurrentSupplyAmmount();
			else
				CustomData.Add("CurrentSupplies", box.GetCurrentSupplyAmmount());
		}
		if (it is Limb l)
		{
			if (CustomData.ContainsKey("LimbColor"))
				CustomData["LimbColor"] = l.GetColor();
			else
				CustomData.Add("LimbColor", l.GetColor());
		}
	}
	public Dictionary<string, object>GetPackedData(out bool HasData)
	{
		HasData = false;
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{"Position", Position},
			{"Name", Name},
			{"SceneData", SceneData}
		};
		if (CustomData.Count > 0)
		{
			HasData = true;
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
		var thing = data.Get("CustomDataKeys");
		if (thing is Godot.Collections.Array)
		{
			return;
		}
		string[] CustomDataKeys = (string[])data.Get("CustomDataKeys");
		Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)data.Get("CustomDataValues");

		if (CustomDataKeys.Count() > 0 && CustomDataValues.Count > 0)
		{
			for (int i = 0; i < CustomDataKeys.Count(); i++)
			{
				CustomData.Add((string)CustomDataKeys[i], CustomDataValues[i]);
			}
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
	BOUZOUKI,
	TOOLBOX,
}

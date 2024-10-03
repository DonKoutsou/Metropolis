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
	protected string ItemName = "Item";

	[Export]
	public string ItemDesc = "Quifsa";

	[Export]
	public bool stackable = true;
	[Export]
	public bool RegisterOnIsland = true;

	public virtual void GetCustomData(out string[] Keys, out object[] Values)
	{
		Keys = new string[0];
		Values = new object[0];
	}

	public virtual string GetItemName()
	{
		return LocalisationHolder.GetString(ItemName);
	}
	public virtual string GetInventoryItemName()
	{
		return ItemName;
	}
	public virtual string GetItemDesc()
	{
		return LocalisationHolder.GetString(ItemDesc);
	}
	public ItemName GetItemType()
	{
		return ItemType;
	}
	public virtual void OnItemPickedUp()
	{

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
	public virtual void HighLightObject(bool toggle, Material OutlineMat)
    {
		if (toggle)
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
    }
	public void DoAction(Player pl)
	{
		if (!pl.GetCharacterInventory().InsertItem(this))
		{
			DialogueManager.GetInstance().ForceDialogue(pl, "Δέν έχω χώρο.");
			//pl.GetTalkText().Talk("Δέν έχω χώρο.");
		}
	}
	public string GetActionName(Player pl)
    {
        return "Πάρε";
    }
	public bool ShowActionName(Player pl)
    {
        return true;
    }
	public string GetActionName2(Player pl)
    {
		return "Null.";
    }
    public string GetActionName3(Player pl)
    {
		return "Null.";
    }
    public bool ShowActionName2(Player pl)
    {
        return false;
    }
    public bool ShowActionName3(Player pl)
    {
        return false;
    }
	public virtual void InputData(ItemInfo data)
	{
		Translation = data.Position;
		Name = data.Name;
	}
	public string GetObjectDescription()
    {
        return GetItemName();
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

		string[] Keys;
		object[] Values;

		it.GetCustomData(out Keys,out Values);

		if (Keys.Count() > 0)
		{
			for (int i = 0; i < Keys.Count(); i++)
			{
				CustomData.Add(Keys[i], Values[i]);
			}
		}
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{"Position", Position},
			{"Name", Name},
			{"SceneData", SceneData}
		};
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
	CASSETTE,
	VINYL,
	LOG,
	BOOK,
	BLUEPRINT,
	MEMORY_STICK,
	DOSIER,
	EXPLOSIVE,
	PAINTCAN,
	BLOOD_VIAL,
	WEAPON,
	KEYCARD,
	SHEET_MUSIC,
	SINNER_NOTE,
	LOCKPICK,
}

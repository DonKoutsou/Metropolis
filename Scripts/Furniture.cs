using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Furniture : Spatial
{
	[Export]
	public string FurnitureDescription;

	public Item StashedItem;

	public bool Searched = false;

	//public override void _Ready()
	//{
	//}
	public void SpawnItem(PackedScene it)
	{
		Item itemToDrop = (Item)it.Instance();
		//AddChild(itemToDrop);
		//itemToDrop.GlobalTranslation = GlobalTransform.origin;
		StashedItem = itemToDrop;
		//StashedItem.Hide();
	}
	public void RespawnItem(PackedScene it)
	{
		if (StashedItem != null)
			StashedItem.QueueFree();
		
		Item itemToDrop = (Item)it.Instance();
		AddChild(itemToDrop);
		itemToDrop.Translation = Vector3.Zero;
		StashedItem = itemToDrop;
		StashedItem.Hide();
	}
	public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
    }
	public bool HasItem()
	{
		return StashedItem != null;
	}
	public void Search(out Item founditem)
	{
		founditem = StashedItem;
		Searched = true;
	}
	public bool HasBeenSearched()
	{
		return Searched;
	}
	public ItemName GetItemName()
	{
		return StashedItem.ItemType;
	}
	public void SetData(FurnitureInfo info)
	{
		Searched = info.Searched;
		if (!Searched)
		{
			if (info.HasItem)
			{
				RespawnItem(MyWorld.GetItemByType(info.item));
			}
		}
	}
	public void HighLightObject(bool toggle)
    {
		if (Searched)
        	((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable",  false);
		else
			((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable",  toggle);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
public class FurnitureInfo
{
	public string FunritureName;
	public bool Searched;
	public bool HasItem;
	public ItemName item;
	public void UpdateInfo(Furniture furn)
	{
		Searched = furn.Searched;
	}

	public void SetInfo(string name, bool srch, bool hasI, ItemName it)
	{
		FunritureName = name;
		Searched = srch;
		HasItem = hasI;
		item = it;
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();

		data.Add("Name", FunritureName);
		data.Add("Searched", Searched);
		data.Add("HasItem", HasItem);
		data.Add("ItemType", (int)item);
		return data;
	}
	public void UnPackData(Godot.Object data)
    {
        FunritureName = (string)data.Get("Name");
		Searched = (bool)data.Get("Searched");
		HasItem = (bool)data.Get("HasItem");
		item = (ItemName)data.Get("ItemType");
    }
}

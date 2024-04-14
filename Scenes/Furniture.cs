using Godot;
using System;

public class Furniture : Spatial
{
	[Export]
	public string FurnitureDescription;

	public Item StashedItem;

	public bool Searched = false;

	//public override void _Ready()
	//{
	//}
	public void SpawnItem(string it)
	{
		var itemToSpawn = GD.Load<PackedScene>(it);
		Item itemToDrop = (Item)itemToSpawn.Instance();
		AddChild(itemToDrop);
		itemToDrop.GlobalTranslation = GlobalTransform.origin;
		StashedItem = itemToDrop;
		StashedItem.Hide();
	}
	public void RespawnItem(string it)
	{
		if (StashedItem != null)
			StashedItem.QueueFree();
		
		var itemToSpawn = GD.Load<PackedScene>(it);
		Item itemToDrop = (Item)itemToSpawn.Instance();
		AddChild(itemToDrop);
		itemToDrop.GlobalTranslation = GlobalTransform.origin;
		StashedItem = itemToDrop;
		StashedItem.Hide();
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
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Godot;
using System;

public class Furniture : Spatial
{
	[Export]
	public string FurnitureDescription;

	Item Item;

	bool Searched = false;

	//public override void _Ready()
	//{
	//}
	public void SpawnItem(string it)
	{
		var itemToSpawn = GD.Load<PackedScene>(it);
		Item itemToDrop = (Item)itemToSpawn.Instance();
		AddChild(itemToDrop);
		itemToDrop.GlobalTranslation = GlobalTransform.origin;
		Item = itemToDrop;
		Item.Hide();
	}
	public bool HasItem()
	{
		return Item != null;
	}
	public void Search(out Item founditem)
	{
		founditem = Item;
		Searched = true;
	}
	public bool HasBeenSearched()
	{
		return Searched;
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

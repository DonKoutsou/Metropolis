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
	public void SpawnItem(PackedScene it)
	{
		Item itemToDrop = (Item)it.Instance();
		AddChild(itemToDrop);
		itemToDrop.GlobalTranslation = GlobalTransform.origin;
		StashedItem = itemToDrop;
		StashedItem.Hide();
	}
	public void RespawnItem(PackedScene it)
	{
		if (StashedItem != null)
			StashedItem.QueueFree();
		
		Item itemToDrop = (Item)it.Instance();
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

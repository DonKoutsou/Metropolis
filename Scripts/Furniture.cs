using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Furniture : Spatial
{
	[Export]
	public string FurnitureDescription;

	[Export]
	List<NodePath> ToHighLight = new List<NodePath>();

	public Item StashedItem;

	bool Searched = false;

	//public override void _Ready()
	//{
	//}
	public bool GetIsSearched()
	{
		return Searched;
	}
	public void SpawnItem(PackedScene it)
	{
		if (it == null)
			return;
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
	public bool HasItem()
	{
		return StashedItem != null;
	}
	public void Search(out Item founditem)
	{
		PlayerSearchAnim();
		founditem = StashedItem;
		Searched = true;
	}
	private void PlayerSearchAnim()
	{
		AnimationPlayer anim = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		if (anim != null)
		{
			anim.Play("Open");
		}
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
		Transform = info.Placement;
		Name = info.FunritureName;
		if (!Searched)
		{
			if (info.HasItem)
			{
				RespawnItem(MyWorld.GetItemByType(info.item));
			}
		}
		else
			PlayerSearchAnim();
	}
	public void HighLightObject(bool toggle)
    {
		if (Searched)
		{
			foreach (NodePath path in ToHighLight)
			{
				((ShaderMaterial)GetNode<MeshInstance>(path).MaterialOverlay).SetShaderParam("enable",  false);
			}
		}
		else
		{
			foreach (NodePath path in ToHighLight)
			{
				((ShaderMaterial)GetNode<MeshInstance>(path).MaterialOverlay).SetShaderParam("enable",  toggle);
			}
		}
			
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
public class DecorationInfo
{
	public string SceneData;
	public Transform Placement;
	public string Name;

	public void SetInfo(string SceneDt, Transform placement, string nam)
	{
		SceneData = SceneDt;
		Placement = placement;
		Name = nam;
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>(){
			{"SceneData", SceneData},
			{"Placement", Placement},
			{"Name", Name}
		};
		return data;
	}
	public void UnPackData(Godot.Object data)
    {
		SceneData = (string)data.Get("SceneData");
		Placement = (Transform)data.Get("Placement");
		Name = (string)data.Get("Name");
    }
}
public class FurnitureInfo
{
	public string FunritureName;
	public string SceneData;
	public Transform Placement;
	public bool Searched;
	public bool HasItem;
	public ItemName item;
	public void UpdateInfo(Furniture furn)
	{
		Searched = furn.GetIsSearched();
	}

	public void SetInfo(string name, bool srch, bool hasI, ItemName it, string SceneDt, Transform placement)
	{
		FunritureName = name;
		Searched = srch;
		HasItem = hasI;
		item = it;
		SceneData = SceneDt;
		Placement = placement;
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>(){
			{"Name", FunritureName},
			{"Searched", Searched},
			{"HasItem", HasItem},
			{"ItemType", (int)item},
			{"SceneData", SceneData},
			{"Placement", Placement}
		};
		return data;
	}
	public void UnPackData(Godot.Object data)
    {
        FunritureName = (string)data.Get("Name");
		Searched = (bool)data.Get("Searched");
		HasItem = (bool)data.Get("HasItem");
		item = (ItemName)data.Get("ItemType");
		SceneData = (string)data.Get("SceneData");
		Placement = (Transform)data.Get("Placement");
    }
}

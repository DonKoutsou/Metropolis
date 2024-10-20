using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Furniture : StaticBody
{
	[Export]
	public string FurnitureDescription;

	[Export]
	List<NodePath> ToHighLight = new List<NodePath>();
	[Export]
	PackedScene ItemToSpawnWith = null;

	public Item StashedItem;

	[Export]
	bool Searched = false;

	public override void _Ready()
	{
		if (ItemToSpawnWith != null)
		{
			SpawnItem(ItemToSpawnWith);
		}
		Node parent = GetParent();
		
		while (!(parent is Island))
		{
			if (parent == null || parent is House)
				return;
			parent = parent.GetParent();
		}
		Island ile = (Island)parent;
		ile.RegisterChild(this);
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
		itemToDrop.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",true);
		if (itemToDrop is Battery bat)
		{
			bat.SetCurrentCap(0);
			//bat.SetCurrentCondition(RandomContainer.Next(0, 100));
		}
		if (itemToDrop is DrahmaStack d)
		{
			d.SetAmmount(RandomContainer.Next(3, 8));
		}
	}
	public bool HasItem()
	{
		return StashedItem != null;
	}
	public void Search(out Item founditem)
	{
		PlayerSearchAnim();
		if (StashedItem is Book b)
		{
			int volume = BookVolumeHolder.GetRandomUnfoundVolume(b.GetSeries());
			b.SetVoluemeNumber(volume);
			BookVolumeHolder.OnVolumeFound(b.GetSeries(), volume);
		}
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
		AudioStreamPlayer3D Sound = GetNodeOrNull<AudioStreamPlayer3D>("AudioStreamPlayer3D");
		if (Sound != null)
		{
			Sound.Play();
		}
	}
	public bool HasBeenSearched()
	{
		return Searched;
	}
	public string GetItemName()
	{
		return StashedItem.GetItemName();
	}
	public void SetSearched(bool IsSearched)
	{
		Searched = IsSearched;
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
				RespawnItem(GlobalItemCatalogue.GetInstance().GetItemByName(info.item));
			}
			else
			{
				Searched = true;
				PlayerSearchAnim();
			}
				
		}
		else
			PlayerSearchAnim();
	}
	public void HighLightObject(bool toggle, Material OutlineMat)
    {
		if (Searched)
		{
			foreach (NodePath path in ToHighLight)
			{
				GetNode<MeshInstance>(path).MaterialOverlay = null;
			}
		}
		else
		{
			foreach (NodePath path in ToHighLight)
			{
				if (toggle)
					GetNode<MeshInstance>(path).MaterialOverlay = OutlineMat;
				else
					GetNode<MeshInstance>(path).MaterialOverlay = null;
			}
		}
    }
	public void DoAction(Player pl)
	{
		Item foundit;
		Search(out foundit);
		ActionTracker.OnActionDone("FurnitureLoot");
		if (foundit != null)
		{
			pl.GetCharacterInventory().InsertItem(foundit);
			DialogueManager.GetInstance().ForceDialogue(pl, "ItmFoundDiag");
			//pl.GetTalkText().Talk(foundit.GetItemPickUpText());
		}
		else
			DialogueManager.GetInstance().ForceDialogue(pl, "Άδειο...");
			//pl.GetTalkText().Talk("Άδειο...");
	}
	public string GetActionName(Player pl)
    {
		return "Ψάξε";
    }
	public bool ShowActionName(Player pl)
    {
        return !HasBeenSearched();
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
	public string GetObjectDescription()
    {
		string desc;
		if (HasBeenSearched())
			desc = "SearchedDiag";
		else
			desc = FurnitureDescription;
        return desc;
    }
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
	public string item;
	public void UpdateInfo(Furniture furn)
	{
		Searched = furn.HasBeenSearched();
	}

	public void SetInfo(string name, bool srch, bool hasI, string it, string SceneDt, Transform placement)
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
			{"ItemName", item},
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
		item = (string)data.Get("ItemName");
		SceneData = (string)data.Get("SceneData");
		Placement = (Transform)data.Get("Placement");
    }
}

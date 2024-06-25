using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class Inventory : Spatial
{
    [Signal]
    public delegate void On_Item_Added(Item item);

    [Signal]
    public delegate void On_Item_Removed(Item item);

    List<Item> InventoryContents;

    [Export]
    float Capacity = 10;
    [Export]
    public PackedScene[] StartingItems;
    float currentweight = 0;
    InventoryUI ui;

    Character CharacterOwner;

    public override void _Ready()
    {
        ui = InventoryUI.GetInstance();
        ui.CallDeferred("OnPlayerSpawned", (Player)GetParent());
        
        InventoryContents = new List<Item>();
        CharacterOwner = (Character)GetParent();
        CharacterOwner.ToggleLimb(LimbType.ARM_L, false);
        CharacterOwner.ToggleLimb(LimbType.ARM_R, false);
        CharacterOwner.ToggleLimb(LimbType.LEG_L, false);
        CharacterOwner.ToggleLimb(LimbType.LEG_R, false);
        CharacterOwner.ToggleLimb(LimbType.N01_LEG_R, false);
        CharacterOwner.ToggleLimb(LimbType.N01_LEG_L, false);
        if (StartingItems == null)
            return;
        for (int i = 0; i < StartingItems.Count(); i ++)
        {
            Item it = (Item)StartingItems[i].Instance();
            InsertItem(it);
        }
    }
    public bool HasBatteries()
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            if (InventoryContents[i] is Battery)
            {
                if (((Battery)InventoryContents[i]).GetCurrentCap() > 10)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void GetBatteries(out List<Battery> batter)
    {
        batter = new List<Battery>();
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            if (InventoryContents[i] is Battery)
            {
                if (((Battery)InventoryContents[i]).GetCurrentCap() > 10)
                {
                    batter.Add((Battery)InventoryContents[i]);
                }
            }
        }
    }
    public void LoadSavedInventory(Godot.Collections.Array items)
    {
        DeleteContents();
        foreach(var It in items)
        {
            Resource res = (Resource)It;
            PackedScene it = GD.Load<PackedScene>((string)res.Get("SceneData"));
            Item newItem = it.Instance<Item>();
            if (newItem is Battery)
            {
                Array CustomDataKeys = (Array)res.Get("CustomDataKeys");
		        Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)res.Get("CustomDataValues");
                for (int i = 0; i < CustomDataKeys.Length; i++)
                {
                    if ((string)CustomDataKeys.GetValue(i) == "CurrentEnergy")
                    {
                        ((Battery)newItem).SetCurrentCap((float)CustomDataValues[i]);
                    }
                    if ((string)CustomDataKeys.GetValue(i) == "LimbColor")
                    {
                        ((Limb)newItem).SetColor((Color)CustomDataValues[i]);
                    }
                }
            }
            InsertItem(newItem);
        }
    }
    public bool IsEmpty()
    {
        return currentweight <= 0;
    }
    public bool InsertItem(Item item)
    {
        if (currentweight + item.GetInventoryWeight() > Capacity)
        {
            return false;
        }
        var parent = item.GetParent();
        if (parent != null)
            parent.RemoveChild(item);
        currentweight += item.GetInventoryWeight();
        if (item is Instrument && !CharacterOwner.HasInstrument())
        {
            CharacterOwner.AddInstrument((Instrument)item);
            return true;
        }
        else
        {
            if (item is Limb && !CharacterOwner.HasLimbOfType(((Limb)item).GetLimbType()))
            {
                Limb limb = (Limb)item;
                CharacterOwner.ToggleLimb(limb.GetLimbType(), true);
                CharacterOwner.SetLimbColor(limb.GetLimbType(), limb.GetColor());
                limb.Equiped = true;
            }
                
            AddChild(item);
            item.Hide();
            item.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",true);
        }

        WorldMap map = WorldMap.GetInstance();
        if (map != null)
        {
            IslandInfo ileinfo = WorldMap.GetInstance().GetCurrentIleInfo();
            ileinfo.RemoveItem(item);
            ileinfo.Island.UnRegisterChild(item);
        }
        

        //if (item.GetItemType() == (int)ItemName.ROPE)
        //{
        //    MeshInstance rope = pl.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment2").GetNode<MeshInstance>("Rope");
        //    rope.Show();
        //}
        InventoryContents.Add(item);
        //EmitSignal(nameof(On_Item_Added), item);
        
        //ui.UpdateInventory();
        return true;
    }
    public bool RemoveItem(Item item)
    {
        InventoryContents.Remove(item);
        currentweight -= item.GetInventoryWeight();
        if (item is Instrument && ((Instrument)item).IsPlaying())
            CharacterOwner.OnSongEnded((Instrument)item);
           
        if (item is Limb && ((Limb)item).Equiped)
        {
            Limb limb = (Limb)item;
            CharacterOwner.ToggleLimb(limb.GetLimbType(), false);
            limb.Equiped = false;
        }
        item.GetParent().RemoveChild(item);
        
        WorldMap map = WorldMap.GetInstance();
        if (map != null)
        {
            IslandInfo info = WorldMap.GetInstance().GetCurrentIleInfo();
            Island ile = info.Island;
            ile.AddChild(item);
            ile.RegisterChild(item);
            info.AddNewItem(item);
        }
        else
        {
            CharacterOwner.GetParent().AddChild(item);
        }
        
        //WorldMap.GetInstance().AddChild(item);
        item.Show();
        Vector3 loc = ((Character)GetParent()).GlobalTranslation;
        loc.y += 1;
        //loc.origin.z += 1;
        item.GlobalTranslation= loc;
        item.Translation = new Vector3(item.Translation.x, item.Translation.y, item.Translation.z + 1);
        //EmitSignal(nameof(On_Item_Removed), item);
        item.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
        
        ui.UpdateInventory();
        return true;
    }
    public void DeleteItem(Item item)
    {
        InventoryContents.Remove(item);
        currentweight -= item.GetInventoryWeight();
        RemoveChild(item);
        item.QueueFree();
    }
    public void RemoveAllItems()
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            RemoveItem(InventoryContents[i]);
        }
    }
    public void DeleteContents()
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            DeleteItem(InventoryContents[i]);
        }
    }
    public void GetContents(out List<Item> Items)
    {
        Items = new List<Item>();
        for (int i = InventoryContents.Count() - 1; i > -1; i--)
        {
            Items.Add(InventoryContents[i]);
        }
        if (CharacterOwner.HasInstrument())
            Items.Add(CharacterOwner.GetInstrument());
    }
    public void GetItemsByType(out List<Item> Items, ItemName Type)
    {
        Items = new List<Item>();
        for (int i = 0; i < InventoryContents.Count; i++)
        {
            if (InventoryContents[i].GetItemType() == (int)Type)
                Items.Add(InventoryContents[i]);
        }
    }
    public float GetAvailableCapacity()
    {
        return Capacity - currentweight;
    }
    public float GetCurrentWeight()
    {
        return currentweight;
    }
    public float GetMaxCap()
    {
        return Capacity;
    }
    public bool OverrideWeight(float NewWeight)
    {
        if (currentweight > NewWeight)
        {
            return false;
        }
        Capacity = NewWeight;
        return true;
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
		Position = it.GlobalTranslation;
		SceneData = it.Filename;
		if (it is Battery)
		{
			CustomData.Add("CurrentEnergy", ((Battery)it).GetCurrentCap());
		}
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Position", Position);
		data.Add("Name", Name);
		data.Add("SceneData", SceneData);
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
		string[] CustomDataKeys = (string[])data.Get("CustomDataKeys");
		object[] CustomDataValues = (object[])data.Get("CustomDataKeys");

		if (CustomDataKeys.Count() > 0 && CustomDataValues.Count() > 0)
		{
			for (int i = 0; i < CustomDataKeys.Count(); i++)
			{
				CustomData.Add(CustomDataKeys[i], CustomDataValues[i]);
			}
		}
    }
}
}

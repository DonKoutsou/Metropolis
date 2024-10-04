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

    //[Export]
    //int Capacity = 10;
    [Export]
    public PackedScene[] StartingItems;
    //[Export]
    //int StartingCurrency = 0;
    //int currentweight = 0;
    InventoryUI ui;

    public Player CharacterOwner;

    

    public override void _Ready()
    {
        InventoryContents = new List<Item>();
        CharacterOwner = (Player)GetParent();
        CharacterOwner.ToggleAllLimbs();
        //CharacterOwner.ToggleLimb(LimbType.ARM_R, false);
        //CharacterOwner.ToggleLimb(LimbType.LEG_L, false);
        //CharacterOwner.ToggleLimb(LimbType.LEG_R, false);
        //CharacterOwner.ToggleLimb(LimbType.N01_LEG_R, false);
        //CharacterOwner.ToggleLimb(LimbType.N01_LEG_L, false);
        //if (StartingCurrency > 0)
        //{
        //    PackedScene d = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Drahma.tscn");
        //    for (int i = 0; i < StartingCurrency; i++)
        //    {
        //        Item it = (Item)d.Instance();
        //        InsertItem(it);
        //    }
        //}

        ui = (InventoryUI)PlayerUI.GetUI(PlayerUIType.INVENTORY);
        
        if (StartingItems == null)
            return;
        for (int i = 0; i < StartingItems.Count(); i ++)
        {
            Item it = (Item)StartingItems[i].Instance();
            InsertItem(it, false);
        }

        
    }

    //LIMB stuff
    List<Limb> EquippedLimbs = new List<Limb>();

    public Limb GetEquippedLimb(LimbSlotType t)
    {
        Limb retlimp = null;
        foreach (Limb l in EquippedLimbs)
        {
            if (l.GetSlotType() == t)
            {
                retlimp = l;
                break;
            }
        }
        return retlimp;
    }
    public bool IsLimbSlotFilled(LimbSlotType SlotType)
    {
        bool SlotFilled = false;

         foreach (Limb l in EquippedLimbs)
        {
            if (l.GetSlotType() == SlotType)
            {
                SlotFilled = true;
                break;
            }
        }
        return SlotFilled;
    }
    public bool IsLimbEquipped(Limb l)
    {
        return EquippedLimbs.Contains(l);
    }
    public void EquipLimp(Limb l)
    {
        CharacterOwner.ToggleLimb(l.GetLimbType(), true);
        CharacterOwner.ToggleLimbEffect(l.GetLimbType(), true);
        //CharacterOwner.SetLimbColor(l.GetLimbType(), l.GetColor());
        EquippedLimbs.Add(l);
    }
    public void UnEquipLimp(Limb l)
    {
        CharacterOwner.ToggleLimb(l.GetLimbType(), false);
        CharacterOwner.ToggleLimbEffect(l.GetLimbType(), false);
        EquippedLimbs.Remove(l);
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
    public bool HasItemOfType(ItemName type)
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            if (InventoryContents[i].GetItemType() == type)
            {
                return true;
            }
        }
        return false;
    }
    public bool HasAnyOfItems(ItemName[] types)
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            if (types.Contains(InventoryContents[i].GetItemType()))
            {
                return true;
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
        DeleteContents(false);
        //CharacterOwner = (Character)GetParent();
        //CharacterOwner.ToggleAllLimbs();
        foreach(var It in items)
        {
            Resource res = (Resource)It;
            PackedScene it = GD.Load<PackedScene>((string)res.Get("SceneData"));
            Item newItem = it.Instance<Item>();
            if (newItem is Battery bat)
            {
                string[] CustomDataKeys = (string[])res.Get("CustomDataKeys");
		        Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)res.Get("CustomDataValues");
                for (int i = 0; i < CustomDataKeys.Count(); i++)
                {
                    if ((string)CustomDataKeys[i] == "CurrentEnergy")
                    {
                        bat.SetCurrentCap((float)CustomDataValues[i]);
                    }
                    //if ((string)CustomDataKeys.GetValue(i) == "CurrentCondition")
                    //{
                    //    bat.SetCurrentCondition((float)CustomDataValues[i]);
                    //}
                }
            }
            /*if (newItem is Toolbox box)
            {
                Array CustomDataKeys = (Array)res.Get("CustomDataKeys");
		        Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)res.Get("CustomDataValues");
                for (int i = 0; i < CustomDataKeys.Length; i++)
                {
                    if ((string)CustomDataKeys.GetValue(i) == "CurrentSupplies")
                    {
                        box.SetCurrentSupplies((float)CustomDataValues[i]);
                    }
                }
            }
                else if (newItem is Limb l)
            {
                Array CustomDataKeys = (Array)res.Get("CustomDataKeys");
		        Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)res.Get("CustomDataValues");
                for (int i = 0; i < CustomDataKeys.Length; i++)
                {
                    if ((string)CustomDataKeys.GetValue(i) == "LimbColor")
                    {
                        l.SetColor((Color)CustomDataValues[i]);
                    }
                }
                l.RandomiseColor = false;
            }*/
            else if (newItem is PaintCan p)
            {
                string[] CustomDataKeys = (string[])res.Get("CustomDataKeys");
		        Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)res.Get("CustomDataValues");
                for (int i = 0; i < CustomDataKeys.Count(); i++)
                {
                    if (CustomDataKeys[i] == "CanColor")
                    {
                        p.SetColor((Color)CustomDataValues[i]);
                    }
                }
                p.RandomiseColor = false;
            }
            else if (newItem is Book b)
            {
                string[] CustomDataKeys = (string[])res.Get("CustomDataKeys");
		        Godot.Collections.Array CustomDataValues = (Godot.Collections.Array)res.Get("CustomDataValues");
                for (int i = 0; i < CustomDataKeys.Count(); i++)
                {
                    if (CustomDataKeys[i] == "VolumeNumber")
                    {
                        b.SetVoluemeNumber((int)CustomDataValues[i]);
                        BookVolumeHolder.OnVolumeFound(b.GetSeries(), (int)CustomDataValues[i]);
                    }
                }
            }
            InsertItem(newItem, false);
        }
    }
    //public bool IsEmpty()
    //{
    //    return currentweight <= 0;
    //}
    public bool InsertItem(Item item, bool ShowNotif = true)
    {
        //int itW = item.GetInventoryWeight();
        //if (currentweight + itW > Capacity)
        //{
        //    return false;
        //}
        var parent = item.GetParent();
        if (parent != null)
            parent.RemoveChild(item);
        //currentweight += itW;
        
        WorldMap map = WorldMap.GetInstance();
        if (map != null)
        {
            IslandInfo ileinfo = WorldMap.GetInstance().GetCurrentIleInfo();
            ileinfo.RemoveItem(item);
            ileinfo.Island.UnRegisterChild(item);
        }
        if (item is Instrument || item.ItemType == ItemName.WEAPON)
        {
            if (!CharacterOwner.HasEquippedItem())
            {
                CharacterOwner.EquipItem(item);
                InventoryContents.Add(item);
            }
            else
            {
                PlaceInInventory(item);
            }
        }
        else if (item is DrahmaStack d)
        {
            Item[] drahmas = d.DecomposeStack();
            for (int i = 0; i < drahmas.Count(); i++)
            {
                PlaceInInventory(drahmas[i]);
            }
        }
        else
        {
            PlaceInInventory(item);
            //if (item is Limb limb && !CharacterOwner.HasLimbOfType(limb.GetLimbType()))
            if (item is Limb limb && !IsLimbSlotFilled(limb.GetSlotType()))
            {
                EquipLimp(limb);
            }
        }
        //if (item.GetItemType() == (int)ItemName.ROPE)
        //{
        //    MeshInstance rope = pl.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment2").GetNode<MeshInstance>("Rope");
        //    rope.Show();
        //}
        
        item.OnItemPickedUp();

        if (ShowNotif)
            ui.OnItemAdded(item);
            
        //EmitSignal(nameof(On_Item_Added), item);
        
        //ui.UpdateInventory();
        return true;
    }
    private void PlaceInInventory(Item i)
    {
        AddChild(i);
        i.Hide();
        i.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",true);
        InventoryContents.Add(i);
}
    public void ChangeEquippedItem(Item it)
    {
        Item currentIt = CharacterOwner.GetEquippedItem();
        if (currentIt == it)
            return;
        currentIt.GetParent().RemoveChild(currentIt);
        AddChild(currentIt);
        currentIt.Visible = false;
        RemoveChild(it);
        CharacterOwner.EquipItem(it);
    }
    public bool RemoveItem(Item item, bool RegisterToIsle = true, bool ShowNotif = true)
    {
        InventoryContents.Remove(item);
        //currentweight -= item.GetInventoryWeight();
        if (item is Instrument && ((Instrument)item).IsPlaying())
            CharacterOwner.OnSongEnded((Instrument)item);
           
        if (item is Limb l && EquippedLimbs.Contains(l))
        {
            UnEquipLimp(l);
            
        }
        item.GetParent().RemoveChild(item);
        
        WorldMap map = WorldMap.GetInstance();
        if (map != null)
        {
            IslandInfo info = WorldMap.GetInstance().GetCurrentIleInfo();
            Island ile = info.Island;
            ile.AddChild(item);
            if (RegisterToIsle)
            {
                ile.RegisterChild(item);
                info.AddNewItem(item);
            }
                
        }
        else
        {
            CharacterOwner.GetParent().AddChild(item);
        }

        item.Show();
        Vector3 loc = ((Character)GetParent()).GlobalTranslation;
        loc.y += 1;
        item.GlobalTranslation= loc;
        item.Translation = new Vector3(item.Translation.x, item.Translation.y, item.Translation.z + 1);

        item.GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
        
        ui.UpdateInventory();

        if (ShowNotif)
            ui.OnItemRemoved(item);

        return true;
    }
    public void DeleteItem(Item item, bool ShowNotif = true)
    {
        InventoryContents.Remove(item);
        //currentweight -= item.GetInventoryWeight();
        if (item is Instrument && ((Instrument)item).IsPlaying())
            CharacterOwner.OnSongEnded((Instrument)item);
           
        if (item is Limb l && EquippedLimbs.Contains(l))
        {
            UnEquipLimp(l);
            
        }
        item.GetParent().RemoveChild(item);
        if (ShowNotif)
            ui.OnItemRemoved(item);
        item.QueueFree();
    }
    public void RemoveAllItems()
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            RemoveItem(InventoryContents[i]);
        }
    }
    public void DeleteContents(bool ShowNotif = true)
    {
        for (int i = InventoryContents.Count - 1; i > -1; i--)
        {
            DeleteItem(InventoryContents[i], ShowNotif);
        }
    }
    public void GetContents(out List<Item> Items)
    {
        Items = new List<Item>();
        for (int i = InventoryContents.Count() - 1; i > -1; i--)
        {
            Items.Add(InventoryContents[i]);
        }
        //if (CharacterOwner.HasInstrument())
            //Items.Add(CharacterOwner.GetInstrument());
    }
    public void GetItemsByType(out List<Item> Items, ItemName[] Types)
    {
        Items = new List<Item>();
        for (int i = 0; i < InventoryContents.Count; i++)
        {
            if (Types.Contains(InventoryContents[i].GetItemType()))
                Items.Add(InventoryContents[i]);
        }
    }
    //public float GetAvailableCapacity()
    //{
    //    return Capacity - currentweight;
    //}
    //public float GetCurrentWeight()
    //{
    //    return currentweight;
    //}
    //public float GetMaxCap()
    //{
    //    return Capacity;
    //}
    //public bool OverrideWeight(int NewWeight)
    //{
    //    if (currentweight > NewWeight)
    //    {
    //        return false;
    //    }
    //    Capacity = NewWeight;
    //    return true;
    //}
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
		if (it is Battery bat)
		{
			CustomData.Add("CurrentEnergy", bat.GetCurrentCap());
            //CustomData.Add("CurrentCondition", bat.GetCondition());
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

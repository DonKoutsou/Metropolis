using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryUI : Control
{
    Inventory Inv;

    Player pl;
    List <InventoryUISlot> slots = new List<InventoryUISlot>();
    static InventoryUI inst;
    public bool IsOpen = false;

    RichTextLabel Capacity;
    float MaxLoad = 0;

    RichTextLabel Description;
    RichTextLabel WeightText;
    RichTextLabel ItemName;

    InventoryUISlot FocusedSlot;

    Panel ItemOptionPanel;

    bool showingDesc = false;

    Item ShowingDescSample = null;

    bool hascompass = false;
    bool hasmap = false;

    bool ShowingCompass = false;
    bool ShowingMap = false;

    ProgressBar CharacterBatteryCharge;
    
    Compass comp;
    MapGrid map;
    ProgressBar CharacterRPM;
    public static InventoryUI GetInstance()
    {
        return inst;
    }
    public override void _Ready()
    {
        inst = this;
        Inv = (Inventory)GetParent();
        pl = (Player)Inv.GetParent();
        comp = Compass.GetInstance();
        map = MapGrid.GetInstance();
        MaxLoad = Inv.GetMaxCap();
        float currentload = Inv.GetCurrentWeight();
        Capacity = GetNode<Panel>("CapPanel").GetNode<RichTextLabel>("CapAmmount");
        Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        GridContainer gr = GetNode<GridContainer>("GridContainer");
        int childc = gr.GetChildCount();
        CharacterBatteryCharge = GetNode<Panel>("BatteryPanel").GetNode<ProgressBar>("CharacterBatteryCharge");
        CharacterRPM = GetNode<Panel>("BatteryPanel").GetNode<ProgressBar>("RPMAmount");
        CharacterBatteryCharge.MaxValue = pl.GetCharacterBatteryCap();
        CharacterBatteryCharge.Value = pl.GetCurrentCharacterEnergy();
        Description = GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("Description");
        WeightText =  GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("WeightText");
        ItemName = GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("ItemName");
        ItemOptionPanel = GetNode<Panel>("ItemOptionPanel");
        ((Control)Description.GetParent()).Hide();
        Hide();

        for (int i = 0; i < childc; i ++)
        {
            slots.Insert(i, (InventoryUISlot)gr.GetChild(i));
        }
        SetProcess(false);
        //CallDeferred("UpdateInventory");
    }
    public void OpenInventory()
    {
        UpdateInventory();
        Show();
        IsOpen = true;
        SetProcess(true);
    }
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
        if (d > 0)
            return;
        d = 0.5f;

        UpdateInventory();
    }
    public void UpdateInventory()
    {
        if (!ShowingMap)
        {
            List<Item> Items = new List<Item>();
            Inv.GetContents(out Items);
            List<int> itemcount = new List<int>();
            hascompass = false;
            hasmap = false;
            for (int i = 0; i < slots.Count(); i++)
            {
                Item sample = null;
                int ammount = 0;
                
                for (int v = Items.Count() - 1; v > -1; v --)
                {
                    if (sample == null)
                        sample = Items[v];
                    if (Items[v].GetItemType() == 4)
                        hascompass = true;
                    if (Items[v].GetItemType() == 5)
                        hasmap = true;
                    
                    if (Items[v].GetItemType() == sample.GetItemType())
                    {
                        ammount += 1;
                        Items.RemoveAt(v);
                    }
                    if (!sample.stackable)
                        break;
                }
                itemcount.Insert(i, ammount);
                slots[i].SetItem(sample, ammount);
                if (FocusedSlot == slots[i])
                    slots[i].Toggle(true);
                else
                    slots[i].Toggle(false);
            }
            //if (FocusedSlot != null)
                //ItemOptionPanel.Show();
            //else
                //ItemOptionPanel.Hide();
            float currentload = Inv.GetCurrentWeight();
            Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        }

        CharacterBatteryCharge.Value = pl.GetCurrentCharacterEnergy();
        CharacterRPM.Value = pl.rpm * 100;
        if (showingDesc)
            Description.BbcodeText = "[center]" + ShowingDescSample.GetItemDesc();
        if (!hascompass)
            ShowingCompass = false;
        if (!hasmap)
            ShowingMap = false;
        if (ShowingCompass)
        {
            comp = Compass.GetInstance();
            //if (comp != null)
                comp.ToggleCompass(true);
        }
        else
        {
            comp = Compass.GetInstance();
            //if (comp != null)
                comp.ToggleCompass(false);
        }
        if (ShowingMap)
        {
            map = MapGrid.GetInstance();
            //if (map != null)
                map.ToggleMap(true);
        }
        else
        {
            map = MapGrid.GetInstance();
            //if (map != null)
                map.ToggleMap(false);
        }
    }
    public void CloseInventory()
    {
        WarpMouse(GetViewport().Size/2);
        Hide();
        FocusedSlot = null;
        IsOpen = false;
        SetProcess(false);
        comp = Compass.GetInstance();
        //if (comp != null)
            comp.ToggleCompass(false);
        
    }
    public void ItemHovered(Item it)
    {
        if (it == null)
            return;
        showingDesc = true;
        ShowingDescSample = it;
        ((Control)Description.GetParent()).Show();
        Description.BbcodeText = "[center]" + it.GetItemDesc();
        ItemName.BbcodeText = "[center]" + it.GetItemName();
        WeightText.BbcodeText = "[center]Βάρος: " + ShowingDescSample.GetInventoryWeight();
    }
    public void ItemUnHovered(Item it)
    {
        ((Control)Description.GetParent()).Hide();
        showingDesc = false;
        ShowingDescSample = null;
    }
    public void SetFocused(InventoryUISlot slot)
    {
        FocusedSlot = slot;
    }
    public void UnFocus(InventoryUISlot slot)
    {
        if (slot == FocusedSlot)
            FocusedSlot = null;
    }
    private void On_Drop_Button_Down()
    {
        if (FocusedSlot == null)
        {
            TalkText.GetInst().Talk("Πρέπει να διαλέξω κάτι για να αφήσω.", pl);
            return;
        }
            
        Inv.RemoveItem(FocusedSlot.item);
        FocusedSlot = null;
    }
    private void On_Compass_Button_Down()
    {
        if (!hascompass)
        {
            TalkText.GetInst().Talk("Δεν έχω πυξήδα.", pl);
            return;
        }
            
        if (!ShowingCompass)
            ShowingCompass = true;
        else
            ShowingCompass = false;
    }
    private void On_Map_Button_Down()
    {
        if (!hasmap)
        {
            TalkText.GetInst().Talk("Δεν έχω χάρτη.", pl);
            return;
        }
        if (!ShowingMap)
            ShowingMap = true;
        else
            ShowingMap = false;
    }
    
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryUI : Control
{
    [Export]
    string NoMapText = "Δεν έχω χάρτη...";
    [Export]
    string NoCompassText = "Δεν έχω πυξήδα...";
    [Export]
    string NoSelectionOnDropText = "Πρέπει να διαλέξω κάτι για να αφήσω...";

    Inventory Inv;

    Player pl;

    List <InventoryUISlot> slots = new List<InventoryUISlot>();

    public bool IsOpen = false;

    float MaxLoad = 0;
    Panel DescPan;

    RichTextLabel Capacity;
    RichTextLabel Description;
    RichTextLabel WeightText;
    RichTextLabel ItemName;

    InventoryUISlot FocusedSlot;

    //Panel ItemOptionPanel;

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

    static InventoryUI Instance;

    public override void _EnterTree()
    {
        base._EnterTree();
        Instance = this;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        Instance = null;
    }
    public override void _Ready()
    {
        Capacity = GetNode<Panel>("CapPanel").GetNode<RichTextLabel>("CapAmmount");
        
        GridContainer gr = GetNode<GridContainer>("GridContainer");
        int childc = gr.GetChildCount();
        CharacterBatteryCharge = GetNode<Panel>("BatteryPanel").GetNode<ProgressBar>("CharacterBatteryCharge");
        CharacterRPM = GetNode<Panel>("BatteryPanel").GetNode<ProgressBar>("RPMAmount");
        
        DescPan = GetNode<Panel>("DescriptionPanel");
        Description = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("Description");
        WeightText =  DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("WeightText");
        ItemName = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("ItemName");
        //ItemOptionPanel = GetNode<Panel>("ItemOptionPanel");
        DescPan.Hide();
        Hide();

        for (int i = 0; i < childc; i ++)
        {
            slots.Insert(i, (InventoryUISlot)gr.GetChild(i));
        }
        SetProcess(false);
    }
    public void OnPlayerSpawned(Player play)
    {
        pl = play;
        Inv = pl.GetNode<Inventory>("Inventory");
        MaxLoad = Inv.GetMaxCap();
        float currentload = Inv.GetCurrentWeight();
        Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        CharacterBatteryCharge.MaxValue = pl.GetCharacterBatteryCap();
        CharacterBatteryCharge.Value = pl.GetCurrentCharacterEnergy();
        comp = GetNode<Compass>("CompassUI");
        map = MapGrid.GetInstance();
    }
    static public InventoryUI GetInstance()
    {
        return Instance;
    }
    public void OpenInventory()
    {
        UpdateInventory();
        Show();
        IsOpen = true;
        SetProcess(true);
    }
    public void CloseInventory()
    {
        WarpMouse(GetViewport().Size/2);
        Hide();
        FocusedSlot = null;
        IsOpen = false;
        SetProcess(false);
        comp.ToggleCompass(false);
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

        comp.ToggleCompass(ShowingCompass);

        map.ToggleMap(ShowingMap);

        GetNode<Panel>("ItemOptionPanel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Button>("DropButton").Visible = !ShowingMap;
    }
    
    public void ItemHovered(Item it)
    {
        if (it == null)
            return;
        showingDesc = true;
        ShowingDescSample = it;
        DescPan.Show();
        Description.BbcodeText = "[center]" + it.GetItemDesc();
        ItemName.BbcodeText = "[center]" + it.GetItemName();
        WeightText.BbcodeText = "[center]Βάρος: " + ShowingDescSample.GetInventoryWeight();
    }
    public void ItemUnHovered(Item it)
    {
        DescPan.Hide();
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
            TalkText.GetInst().Talk(NoSelectionOnDropText, pl);
            return;
        }
            
        Inv.RemoveItem(FocusedSlot.item);
        FocusedSlot = null;
    }
    
    private void On_Compass_Button_Down()
    {
        if (!hascompass)
        {
            TalkText.GetInst().Talk(NoCompassText, pl);
            return;
        }
        if (!ShowingCompass)
            ShowingCompass = true;
        else
            ShowingCompass = false;
    }
    
    public void On_Map_Button_Down()
    {
        if (!hasmap)
        {
            TalkText.GetInst().Talk(NoMapText, pl);
            return;
        }
        if (!ShowingMap)
        {
            ShowingMap = true;
            map.FrameMap();
        }
        else
        {
            ShowingMap = false;
            
        }
            
    }
    
}

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
    Panel JobPan;
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
    Panel CharacterRPM;

    public void ConfigureJob(Job j)
    {
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/TaskName").BbcodeText = "[center]" + j.GetJobName();
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Location").BbcodeText = "[center]" + string.Format("Προορισμός : X = {0} - Y = {1}", j.GetLocation().x, j.GetLocation().y);
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Description").BbcodeText = "[center]" + string.Format("Μεταφορά προμηθειών στον φάρο {0}", j.GetOwnerName());
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Reward").BbcodeText = "[center]" + string.Format("Αμοιβή : {0} Δραχμές", j.GetRewardAmmount());
    }
    public override void _Ready()
    {
        Capacity = GetNode<Panel>("CapPanel").GetNode<RichTextLabel>("CapAmmount");
        
        GridContainer gr = GetNode<GridContainer>("GridContainer");
        int childc = gr.GetChildCount();
        CharacterBatteryCharge = GetNode<Panel>("BatteryPanel").GetNode<ProgressBar>("CharacterBatteryCharge");
        CharacterRPM = GetNode<Panel>("BatteryPanel").GetNode<Panel>("RPMAmount");
        
        DescPan = GetNode<Panel>("DescriptionPanel");
        JobPan = GetNode<Panel>("JobPanel");
        Description = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("Description");
        WeightText =  DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("WeightText");
        ItemName = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("ItemName");
        //ItemOptionPanel = GetNode<Panel>("ItemOptionPanel");
        DescPan.Hide();
        JobPan.Hide();
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
            List<Item> Items;
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
        float rpm = pl.rpm;
        if (rpm > 0.66f)
            CharacterRPM.Modulate = new Color(1,0,0);
        else if (rpm > 0.33f)
            CharacterRPM.Modulate = new Color(1,1,0);
        else
            CharacterRPM.Modulate = new Color(0,1,0);


        if (showingDesc)
            Description.BbcodeText = "[center]" + ShowingDescSample.GetItemDesc();

        if (!hascompass)
            ShowingCompass = false;
        if (!hasmap)
            ShowingMap = false;

        comp.ToggleCompass(ShowingCompass);

        map.ToggleMap(ShowingMap);

        GetNode<Panel>("ItemOptionPanel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Button>("DropButton").Visible = !ShowingMap;
        bool selectinginst = FocusedSlot != null && FocusedSlot.item is Instrument;
        bool selectinglimb = FocusedSlot != null && FocusedSlot.item is Limb;
        GetNode<Panel>("ItemOptionPanel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Button>("SwitchButton").Visible = selectinginst;
        GetNode<Panel>("ItemOptionPanel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Button>("SwitchLimbButton").Visible = selectinglimb;
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
    private void On_Instrument_Button_Down()
    {
        if (FocusedSlot == null)
            return;
        if (!Inv.CharacterOwner.HasInstrument())
            Inv.CharacterOwner.AddInstrument((Instrument)FocusedSlot.item);
        else
            Inv.ChangeInstrument((Instrument)FocusedSlot.item);
    }
    private void On_Limb_Button_Down()
    {
        if (FocusedSlot == null)
            return;
        Limb l = (Limb)FocusedSlot.item;
        Character owner = Inv.CharacterOwner;
        if (!Inv.IsLimbSlotFilled(l.GetSlotType()))
        {
            owner.ToggleLimb(l.GetLimbType(), true);
            owner.SetLimbColor(l.GetLimbType(), l.GetColor());
            Inv.EquipLimp(l);
        }
        else
        {
            Limb ltoremove = Inv.GetEquippedLimb(l.GetSlotType());
            Inv.UnEquipLimp(ltoremove);
            Inv.EquipLimp(l);
        }

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
            if (GlobalJobManager.GetInstance().HasJobAssigned())
                JobPan.Visible = true;
            map.FrameMap();
        }
        else
        {
            ShowingMap = false;
            JobPan.Visible = false;
        }
            
    }
    
}

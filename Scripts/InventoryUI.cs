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

    //float MaxLoad = 0;
    Panel DescPan;
    Panel JobPan;
    //RichTextLabel Capacity;
    RichTextLabel Description;
    //RichTextLabel WeightText;
    RichTextLabel ItemName;

    InventoryUISlot FocusedSlot;

    //Panel ItemOptionPanel;

    bool showingDesc = false;

    Item ShowingDescSample = null;

    bool hascompass = false;
    bool hasmap = false;
    bool hastoolbox = false;
    bool ShowingCompass = false;
    bool ShowingMap = false;

    //ProgressBar CharacterBatteryCharge;

    AnimationPlayer Anim;
    
    Compass comp;
    MapGrid map;
    //Panel CharacterRPM;

    int currentpage = 0;
    int maxpage = 0;
    public void ConfigureJob(string JobName, Vector2 Jobloc, string JobOwner, int RewardAmmount)
    {
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/TaskName").BbcodeText = "[center]" + JobName;
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Location").BbcodeText = "[center]" + string.Format("Προορισμός : X = {0} - Y = {1}", Jobloc.x, Jobloc.y);
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Description").BbcodeText = "[center]" + string.Format("Μεταφορά προμηθειών στον φάρο {0}", JobOwner);
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Reward").BbcodeText = "[center]" + string.Format("Αμοιβή : {0} Δραχμές", RewardAmmount);
    }
    public override void _Ready()
    {
        //Capacity = GetNode<RichTextLabel>("InventoryContainer/Inventory/CapPanel/CapAmmount");
        Anim = GetNode<AnimationPlayer>("InventoryContainer/InventoryAnimation");
        GridContainer gr = GetNode<GridContainer>("InventoryContainer/Inventory/GridContainer");
        int childc = gr.GetChildCount();
        //CharacterBatteryCharge = GetNode<ProgressBar>("InventoryContainer/Inventory/BatteryPanel/CharacterBatteryCharge");
        //CharacterRPM = GetNode<Panel>("InventoryContainer/Inventory/BatteryPanel/RPMAmount");
        
        DescPan = GetNode<Panel>("InventoryContainer/Inventory/DescriptionPanel");
        JobPan = GetNode<Panel>("InventoryContainer/Inventory/JobPanel");
        Description = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("Description");
        //WeightText =  DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("WeightText");
        ItemName = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("ItemName");
        //ItemOptionPanel = GetNode<Panel>("ItemOptionPanel");
        DescPan.Hide();
        JobPan.Hide();
        Hide();

        for (int i = 0; i < childc; i ++)
        {
            InventoryUISlot sl = (InventoryUISlot)gr.GetChild(i);
            slots.Insert(i, sl);
            sl.Connect("Hovered", this, "ItemHovered");
            sl.Connect("Focused", this, "SetFocused");
        }
        SetProcess(false);
    }
    public void OnPlayerSpawned(Player play)
    {
        pl = play;
        Inv = pl.GetNode<Inventory>("Inventory");
        //MaxLoad = Inv.GetMaxCap();
        //float currentload = Inv.GetCurrentWeight();
        //Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        //CharacterBatteryCharge.MaxValue = pl.GetCharacterBatteryCap();
        //CharacterBatteryCharge.Value = pl.GetCurrentCharacterEnergy();
        comp = GetNode<Compass>("InventoryContainer/Inventory/CompassUI");
        map = MapGrid.GetInstance();
        Show();
    }
    private void ToggleInventory()
    {
        if (!IsOpen)
            OpenInventory();
        else
            CloseInventory();
    }
    public void OpenInventory()
    {
        if (Anim.IsPlaying())
            return;
        Anim.Play("MenuOpen");
        PlayerUI.OnMenuToggled(true);
        UpdateInventory();
        //Show();
        IsOpen = true;
        SetProcess(true);
    }
    public void CloseInventory()
    {
        if (Anim.IsPlaying())
            return;
        Anim.Play("MenuClose");
        PlayerUI.OnMenuToggled(false);
        WarpMouse(GetViewport().Size/2);
        //Hide();
        FocusedSlot = null;
        IsOpen = false;
        SetProcess(false);
        //comp.ToggleCompass(false);
    }
    float d = 0.1f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
        if (d > 0)
            return;
        d = 0.1f;

        UpdateInventory();
    }
    public void UpdateInventory()
    {
        if (!ShowingMap)
        {
            List<Item> Items;
            Inv.GetContents(out Items);

            Dictionary <string, int> itemcatalogue = new Dictionary<string, int>();
            Dictionary <string, Item> itemcat2 = new Dictionary<string, Item>();

            hascompass = false;
            hasmap = false;
            hastoolbox = false;

            for (int v = Items.Count() - 1; v > -1; v --)
            {
                if (Items[v] is Limb l && Inv.IsLimbEquipped(l))
                    continue;
                if (!itemcat2.ContainsKey(Items[v].GetItemName()))
                    itemcat2.Add(Items[v].GetItemName(), Items[v]);

                if (itemcatalogue.ContainsKey(Items[v].GetItemName()))
                {
                    itemcatalogue[Items[v].GetItemName()] += 1;
                }
                else
                {
                    itemcatalogue.Add(Items[v].GetItemName(), 1);
                }

                if (Items[v].GetItemType() ==  global::ItemName.COMPASS)
                    hascompass = true;
                else if (Items[v].GetItemType() == global::ItemName.MAP)
                    hasmap = true;
                else if (Items[v].GetItemType() == global::ItemName.TOOLBOX)
                    hastoolbox = true;

            }
            maxpage = itemcatalogue.Count / 12;
            GetNode<Control>("InventoryContainer/Inventory/CapPanel2/InventoryPage").Visible = maxpage > 0;
            int slottofill = 0;
            int currentit = 0;
            
           
            int min = 0;
            for (int i = 0; i < currentpage; i++)
                min += 11;

            int max = min + 11;


            foreach(KeyValuePair<string, int> it in itemcatalogue)
            {
                if (currentit > max)
                    break;
                if (currentit < min)
                {
                    currentit ++;
                    continue;
                }
                if (!itemcat2[it.Key].stackable)
                {
                    for (int i = 0; i < it.Value; i++)
                    {
                        if (currentit > max)
                            break;
                        slots[slottofill].SetItem(itemcat2[it.Key], 1);
                        currentit ++;
                        slottofill++;
                    }
                }
                else
                {
                    slots[slottofill].SetItem(itemcat2[it.Key], it.Value);
                    currentit ++;
                    slottofill++;
                }
                
            }
            

            for (int i = 0; i < slots.Count(); i++)
            {
                //itemcount.Insert(i, ammount);
                if (i >= slottofill)
                    slots[i].SetItem(null, 0);
                if (FocusedSlot == slots[i])
                    slots[i].Toggle(true);
                else
                    slots[i].Toggle(false);
            }
            //if (FocusedSlot != null)
                //ItemOptionPanel.Show();
            //else
                //ItemOptionPanel.Hide();
            //float currentload = Inv.GetCurrentWeight();
            //Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        }

        //CharacterBatteryCharge.Value = pl.GetCurrentCharacterEnergy();
        //float rpm = pl.rpm;
        //if (rpm > 0.66f)
        //    CharacterRPM.Modulate = new Color(1,0,0);
        //else if (rpm > 0.33f)
        //    CharacterRPM.Modulate = new Color(1,1,0);
        //else
        //    CharacterRPM.Modulate = new Color(0,1,0);


        if (showingDesc)
            Description.BbcodeText = "[center]" + ShowingDescSample.GetItemDesc();

        if (!hascompass)
            ShowingCompass = false;
        if (!hasmap)
            ShowingMap = false;


        

        comp.ToggleCompass(ShowingCompass);

        map.ToggleMap(ShowingMap);

        
        GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/DropButton").Visible = !ShowingMap;
        bool selectinginst = FocusedSlot != null && FocusedSlot.item is Instrument;
        bool selectingbat = FocusedSlot != null && FocusedSlot.item is Battery;
        bool selectinglimb = FocusedSlot != null && FocusedSlot.item is Limb;
        //GetNode<Button>("ItemOptionPanel/HBoxContainer/RepairButton").Visible = selectinginst && hastoolbox || selectingbat && hastoolbox;
        GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/RepairButton").Visible = selectingbat && hastoolbox;
        GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/SwitchButton").Visible = selectinginst;
        GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/SwitchLimbButton").Visible = selectinglimb;

        GetNode<Control>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/CompassButton").Visible = hascompass;
        GetNode<Control>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/MapButton").Visible = hasmap;
    }
    private void PageForw()
    {
        if (currentpage == maxpage)
            return;
        currentpage ++;
        GetNode<RichTextLabel>("InventoryContainer/Inventory/CapPanel2/InventoryPage/w").BbcodeText = "[center]" + currentpage;
    }
    private void PageBack()
    {
        if (currentpage == 0)
            return;

        
        currentpage --;
        GetNode<RichTextLabel>("InventoryContainer/Inventory/CapPanel2/InventoryPage/w").BbcodeText = "[center]" + currentpage;
    }
    public void ItemHovered(Item it, bool t)
    {
        if (t)
        {
            if (it == null)
            return;
            showingDesc = true;
            ShowingDescSample = it;
            DescPan.Show();
            Description.BbcodeText = "[center]" + it.GetItemDesc();
            ItemName.BbcodeText = "[center]" + it.GetItemName();
            //WeightText.BbcodeText = "[center]Βάρος: " + ShowingDescSample.GetInventoryWeight();
        }
        else
        {
            DescPan.Hide();
            showingDesc = false;
            ShowingDescSample = null;
        }
    }
    //public void ItemUnHovered(Item it, bool t)
    //{
        
        
    //}
    public void SetFocused(bool t, InventoryUISlot slot)
    {
        if (t)
            FocusedSlot = slot;
        else
        {
            if (slot == FocusedSlot)
                FocusedSlot = null;
        }
    }
    //public void UnFocus(InventoryUISlot slot)
    //{
        
    //}
    
    private void On_Repair_Button_Down()
    {
        List<Item> toolboxes;
        Inv.GetItemsByType(out toolboxes, global::ItemName.TOOLBOX);
        Battery ittorepair = (Battery)FocusedSlot.item;
        if (ittorepair.GetCondition() == 100)
            return;
        float AmmountToRepair = 100 - ittorepair.GetCondition();
        for (int i = 0; i < toolboxes.Count; i++)
        {
            Toolbox t = (Toolbox)toolboxes[i];
            t.RepaiItem(ittorepair);
            if (ittorepair.GetCondition() == 100)
                break;
        }
    }
    private void On_Drop_Button_Down()
    {
        if (FocusedSlot == null)
        {
            pl.GetTalkText().Talk(NoSelectionOnDropText);
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
            pl.GetTalkText().Talk(NoCompassText);
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
            pl.GetTalkText().Talk(NoMapText);
            return;
        }
        if (!ShowingMap)
        {
            ShowingMap = true;
            //if (GlobalJobManager.GetInstance().HasJobAssigned())
                //JobPan.Visible = true;
            //map.FrameMap();
        }
        else
        {
            ShowingMap = false;
            JobPan.Visible = false;
        }
            
    }
    
}

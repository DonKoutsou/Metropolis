using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryUI : Control
{
    //[Export]
    //string NoMapText = "Δεν έχω χάρτη...";
    [Export]
    string NoCompassText = "Δεν έχω πυξήδα...";
    //[Export]
    //string NoSelectionOnDropText = "Πρέπει να διαλέξω κάτι για να αφήσω...";

    Inventory Inv;

    Player Playr;

    List <InventoryUISlot> slots = new List<InventoryUISlot>();

    public bool IsOpen = false;

    //float MaxLoad = 0;
    PanelContainer DescPan;
    //Panel JobPan;
    //RichTextLabel Capacity;
    RichTextLabel Description;
    //RichTextLabel WeightText;
    RichTextLabel ItemName;

    InventoryUISlot FocusedSlot;

    //Panel ItemOptionPanel;

    bool showingDesc = false;

    Item ShowingDescSample = null;

    bool hascompass = false;
    //bool hasmap = false;
    //bool hastoolbox = false;
    bool ShowingCompass = false;
    //bool ShowingMap = false;

    ProgressBar CharacterBatteryCharge;

    AnimationPlayer Anim;
    
    Compass comp;
    MapGrid map;
    Panel CharacterRPM;

    int currentpage = 0;
    int maxpage = 0;

    AudioStreamPlayer SoundOpen;
    AudioStreamPlayer SoundClose;
    InventoryItemInOutNotification ItemNotif;
    /*public void ConfigureJob(string JobName, Vector2 Jobloc, string JobOwner, int RewardAmmount)
    {
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/TaskName").BbcodeText = "[center]" + JobName;
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Location").BbcodeText = "[center]" + string.Format("Προορισμός : X = {0} - Y = {1}", Jobloc.x, Jobloc.y);
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Description").BbcodeText = "[center]" + string.Format("Μεταφορά προμηθειών στον φάρο {0}", JobOwner);
        JobPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Reward").BbcodeText = "[center]" + string.Format("Αμοιβή : {0} Δραχμές", RewardAmmount);
    }*/
    public override void _Ready()
    {
        //Capacity = GetNode<RichTextLabel>("InventoryContainer/Inventory/CapPanel/CapAmmount");
        Anim = GetNode<AnimationPlayer>("InventoryContainer/InventoryAnimation");
        GridContainer gr = GetNode<GridContainer>("InventoryContainer/Inventory/GridContainer");
        int childc = gr.GetChildCount();
        CharacterBatteryCharge = GetNode<ProgressBar>("InventoryContainer/Inventory/BatteryPanel/CharacterBatteryCharge");
        CharacterRPM = GetNode<Panel>("InventoryContainer/Inventory/BatteryPanel/RPMAmount");
        
        DescPan = GetNode<PanelContainer>("InventoryContainer/Inventory/ItemRender/DescriptionPanel");
        //JobPan = GetNode<Panel>("InventoryContainer/Inventory/JobPanel");
        Description = DescPan.GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Panel/ScrollContainer/Description");
        //WeightText =  DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("WeightText");
        ItemName = DescPan.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<RichTextLabel>("ItemName");
        //ItemOptionPanel = GetNode<Panel>("ItemOptionPanel");
        DescPan.Hide();

        ItemNotif = GetNode<InventoryItemInOutNotification>("InventoryContainer/InventoryItemInOutNotification");
        //JobPan.Hide();

        SoundOpen = GetNode<AudioStreamPlayer>("InventoryContainer/Inventory/InvOpen");
        SoundClose = GetNode<AudioStreamPlayer>("InventoryContainer/Inventory/InvClose");

        for (int i = 0; i < childc; i ++)
        {
            InventoryUISlot sl = (InventoryUISlot)gr.GetChild(i);
            slots.Insert(i, sl);
            sl.Connect("Hovered", this, "ItemHovered");
            sl.Connect("Focused", this, "SetFocused");
        }
        SetProcess(false);

        GetNode<Control>("InventoryContainer/Inventory").Visible = false;
    }
    public void OnItemAdded(Item it)
    {
        ItemNotif.OnItemAddedToInv(it);
    }
    public void OnItemRemoved(Item it)
    {
        ItemNotif.OnItemRemovedFromInv(it);
    }
    public void PlayerToggle(Player pl)
    {
		bool toggle = pl != null;
        Visible = toggle;
		if (toggle)
		{
            Playr = pl;
            Inv = pl.GetNode<Inventory>("Inventory");
            CharacterBatteryCharge.MaxValue = pl.GetCharacterBatteryCap();
            CharacterBatteryCharge.Value = pl.GetCurrentCharacterEnergy();
            comp = GetNode<Compass>("InventoryContainer/Inventory/CompassUI");
            map = ((MapUI)PlayerUI.GetUI(PlayerUIType.MAP)).GetGrid();
		}
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
        IsOpen = true;
        SetProcess(true);
        SoundOpen.Play();
        GetNode<Control>("InventoryContainer/Inventory").Visible = true;
        if (ControllerInput.IsUsingController())
        {
            slots[0].GetNode<Button>("ItemIcon").GrabFocus();
        }
        
    }
    public void CloseInventory()
    {
        if (Anim.IsPlaying())
            return;
        Anim.Play("MenuClose");
        PlayerUI.OnMenuToggled(false);
        WarpMouse(GetViewport().Size/2);
        IsOpen = false;
        SetProcess(false);

        SoundClose.Play();

        if (FocusedSlot != null)
            SetFocused(false, FocusedSlot);
    }
    private void CloseAnimStoped(string anim)
    {
        if (anim == "MenuClose")
        {
            GetNode<Control>("InventoryContainer/Inventory").Visible = false;
        }
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
        //if (!ShowingMap)
        //{
        List<Item> Items;
        Inv.GetContents(out Items);

        Dictionary <string, int> itemcatalogue = new Dictionary<string, int>();
        Dictionary <string, Item> itemcat2 = new Dictionary<string, Item>();


        hascompass = false;
        //hasmap = false;
        //hastoolbox = false;
        int Itamm = 0;

        for (int v = Items.Count() - 1; v > -1; v --)
        {
            if (Items[v] is Limb l && Inv.IsLimbEquipped(l))
                continue;
            if (!itemcat2.ContainsKey(Items[v].GetInventoryItemName()))
                itemcat2.Add(Items[v].GetInventoryItemName(), Items[v]);

            if (itemcatalogue.ContainsKey(Items[v].GetInventoryItemName()))
            {
                itemcatalogue[Items[v].GetInventoryItemName()] += 1;
            }
            else
            {
                itemcatalogue.Add(Items[v].GetInventoryItemName(), 1);
            }

            Itamm ++;

            if (Items[v].GetItemType() ==  global::ItemName.COMPASS)
                hascompass = true;
            //else if (Items[v].GetItemType() == global::ItemName.MAP)
                //hasmap = true;
            //else if (Items[v].GetItemType() == global::ItemName.TOOLBOX)
                //hastoolbox = true;

        }
        maxpage = Itamm / 12;
        bool PageBool = maxpage > 0;
        GetNode<Button>("InventoryContainer/Inventory/CapPanel2/InventoryPage/PageFront").Disabled = !PageBool;
        GetNode<Button>("InventoryContainer/Inventory/CapPanel2/InventoryPage/PageBack").Disabled = !PageBool;
        int slottofill = 0;
        int currentit = 0;

        int min = 0;
        for (int i = 0; i < currentpage; i++)
            min += 12;

        int max = min + 11;


        foreach(KeyValuePair<string, int> it in itemcatalogue)
        {
            if (itemcat2[it.Key] is Limb l && Inv.IsLimbEquipped(l))
                continue;

            if (currentit > max)
                break;
            if (!itemcat2[it.Key].stackable)
            {
                for (int i = 0; i < it.Value; i++)
                {
                    if (currentit > max)
                        break;
                    if (currentit >= min)
                    {
                        slots[slottofill].SetItem(itemcat2[it.Key], 1);
                        slottofill++;
                    }
                    currentit ++;
                }
            }
            else
            {
                if (currentit >= min)
                {
                    slots[slottofill].SetItem(itemcat2[it.Key], it.Value);
                    slottofill++;
                }
                currentit ++;
            }
        }

        for (int i = 0; i < slots.Count(); i++)
        {
            if (i >= slottofill)
                slots[i].SetItem(null, 0);

        }

            //float currentload = Inv.GetCurrentWeight();
            //Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        //}

        CharacterBatteryCharge.Value = Playr.GetCurrentCharacterEnergy();
        float rpm = Playr.GetRPM();
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
        //if (!hasmap)
            //ShowingMap = false;

        comp.ToggleCompass(ShowingCompass);

        //map.ToggleMap(ShowingMap);

        
        bool selectinginst = FocusedSlot != null && (FocusedSlot.item is Instrument);
        //bool selecingwp = FocusedSlot != null && FocusedSlot.item.ItemType == global::ItemName.WEAPON;
        //bool selectingbat = FocusedSlot != null && FocusedSlot.item is Battery;
        bool selectinglimb = FocusedSlot != null && FocusedSlot.item is Limb;

        //GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/DropButton").Visible = !ShowingMap;
        //GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/RepairButton").Visible = selectingbat && hastoolbox;
        GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/SwitchButton").Visible = selectinginst ;
        GetNode<Button>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/SwitchLimbButton").Visible = selectinglimb;
        GetNode<Control>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/CompassButton").Visible = hascompass;
        //GetNode<Control>("InventoryContainer/Inventory/ItemOptionPanel/HBoxContainer/MapButton").Visible = hasmap;
    }
    private void PageForw()
    {
        if (currentpage == maxpage)
            return;
        currentpage ++;
        GetNode<RichTextLabel>("InventoryContainer/Inventory/CapPanel2/InventoryPage/w").BbcodeText = "[center]" + currentpage;
        if (FocusedSlot != null)
            SetFocused(false, FocusedSlot);
    }
    private void PageBack()
    {
        if (currentpage == 0)
            return;

        currentpage --;
        GetNode<RichTextLabel>("InventoryContainer/Inventory/CapPanel2/InventoryPage/w").BbcodeText = "[center]" + currentpage;
        if (FocusedSlot != null)
            SetFocused(false, FocusedSlot);
    }
    public void ItemHovered(Item it, bool t)
    {
        if (t)
        {
            
            //WeightText.BbcodeText = "[center]Βάρος: " + ShowingDescSample.GetInventoryWeight();
        }
        else
        {
            
            
        }
    }
    public void SetFocused(bool t, InventoryUISlot slot)
    {
        if (slot.item == null)
        {
            slot.Toggle(false);
            
            return;
        }
            

        if (FocusedSlot != null)
            FocusedSlot.Toggle(false);
        
        FocusedSlot = slot;
        if (t)
        {
            MeshInstance meshi = slot.item.GetNode<MeshInstance>("MeshInstance");
            
            
            GetNode<ItemPreviewPivot>("InventoryContainer/Inventory/ItemRender/Panel2/ViewportContainer/ItemPreviewViewport/ItemPreviewPivot").Start(meshi);

            showingDesc = true;
            ShowingDescSample = slot.item;
            DescPan.Show();
            Description.BbcodeText = "[center]" + slot.item.GetItemDesc();
            ItemName.BbcodeText = "[center]" + LocalisationHolder.GetString(slot.item.GetItemName());
        }
        else
        {
            DescPan.Hide();
            showingDesc = false;
            ShowingDescSample = null;
            FocusedSlot = null;
            GetNode<ItemPreviewPivot>("InventoryContainer/Inventory/ItemRender/Panel2/ViewportContainer/ItemPreviewViewport/ItemPreviewPivot").Stop();
        }
        slot.Toggle(t);
    }
    /*private void On_Repair_Button_Down()
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
    }*/
    /*private void On_Drop_Button_Down()
    {
        if (FocusedSlot == null)
        {
            //pl.GetTalkText().Talk(NoSelectionOnDropText);
            DialogueManager.GetInstance().ForceDialogue(pl, NoSelectionOnDropText);
            return;
        }
        Inv.RemoveItem(FocusedSlot.item);
        SetFocused(false, FocusedSlot);
    }*/
    private void On_Instrument_Button_Down()
    {
        if (FocusedSlot == null)
            return;
        if (!Inv.CharacterOwner.HasEquippedInstrument())
            Inv.CharacterOwner.EquipItem(FocusedSlot.item);
        else
            Inv.ChangeEquippedItem(FocusedSlot.item);
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
            //owner.SetLimbColor(l.GetLimbType(), l.GetColor());
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
            DialogueManager.GetInstance().ForceDialogue(Playr, NoCompassText);
            //pl.GetTalkText().Talk(NoCompassText);
            return;
        }
        if (!ShowingCompass)
            ShowingCompass = true;
        else
            ShowingCompass = false;
    }
    
    /*public void On_Map_Button_Down()
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
            //JobPan.Visible = false;
        }
            
    }*/
    
}

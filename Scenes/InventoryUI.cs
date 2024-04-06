using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryUI : Control
{
    Inventory Inv;
    List <InventoryUISlot> slots = new List<InventoryUISlot>();
    static InventoryUI inst;
    public bool IsOpen = false;

    RichTextLabel Capacity;
    float MaxLoad = 0;

    RichTextLabel Description;
    RichTextLabel ItemName;

    InventoryUISlot FocusedSlot;

    Panel ItemOptionPanel;
    public static InventoryUI GetInstance()
    {
        return inst;
    }
    public override void _Ready()
    {
        inst = this;
        Inv = (Inventory)GetParent();
        MaxLoad = Inv.GetMaxCap();
        float currentload = Inv.GetCurrentWeight();
        Capacity = GetNode<Panel>("CapPanel").GetNode<RichTextLabel>("CapAmmount");
        Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
        GridContainer gr = GetNode<GridContainer>("GridContainer");
        int childc = gr.GetChildCount();

        Description = GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("Description");
        ItemName = GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("ItemName");
        ItemOptionPanel = GetNode<Panel>("ItemOptionPanel");
        ((Control)Description.GetParent()).Hide();
        Hide();

        for (int i = 0; i < childc; i ++)
        {
            slots.Insert(i, (InventoryUISlot)gr.GetChild(i));
        }
    }
    public void OpenInventory()
    {
        Show();
        IsOpen = true;
        UpdateInventory();
    }
    public void UpdateInventory()
    {
        List<Item> Items = new List<Item>();
        Inv.GetContents(out Items);
        List<int> itemcount = new List<int>();
        for (int i = 0; i < slots.Count(); i++)
        {
            Item sample = null;
            int ammount = 0;
            
            for (int v = Items.Count(); v > 0; v --)
            {
                if (sample == null)
                    sample = Items[v - 1];
                if (Items[v - 1].GetItemType() == sample.GetItemType())
                {
                    ammount += 1;
                    Items.RemoveAt(v - 1);
                }
            }
            itemcount.Insert(i, ammount);
            slots[i].SetItem(sample, ammount);
            if (FocusedSlot == slots[i])
                slots[i].Toggle(true);
            else
                slots[i].Toggle(false);
        }
        if (FocusedSlot != null)
            ItemOptionPanel.Show();
        else
            ItemOptionPanel.Hide();
        float currentload = Inv.GetCurrentWeight();
        Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
    }
    public void CloseInventory()
    {
        Hide();
        FocusedSlot = null;
        IsOpen = false;
    }
    public void ItemHovered(Item it)
    {
        if (it == null)
            return;
        ((Control)Description.GetParent()).Show();
        Description.BbcodeText = "[center]" + it.GetItemDesc();
        ItemName.BbcodeText = "[center]" + it.GetItemName();
    }
    public void SetFocused(InventoryUISlot slot)
    {
        FocusedSlot = slot;
        UpdateInventory();
    }
    public void UnFocus(InventoryUISlot slot)
    {
        if (slot == FocusedSlot)
            FocusedSlot = null;
        UpdateInventory();
    }
    public void ItemUnHovered(Item it)
    {
        ((Control)Description.GetParent()).Hide();
    }
    private void On_Drop_Button_Down()
    {
        Inv.RemoveItem(FocusedSlot.item);
    }
    
}

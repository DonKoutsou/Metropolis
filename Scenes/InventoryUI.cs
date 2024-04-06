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
        GridContainer gr = (GridContainer)GetChild(2);
        int childc = gr.GetChildCount();
        Description = GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("Description");
        ItemName = GetNode<Panel>("DescriptionPanel").GetNode<RichTextLabel>("ItemName");
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
        }
        float currentload = Inv.GetCurrentWeight();
        Capacity.BbcodeText = string.Format("[center]{0}/{1}", currentload, MaxLoad);
    }
    public void CloseInventory()
    {
        Hide();
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
    public void ItemUnHovered(Item it)
    {
        ((Control)Description.GetParent()).Hide();
    }
}

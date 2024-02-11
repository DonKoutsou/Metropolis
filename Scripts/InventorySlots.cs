using Godot;
using System;
using System.Collections.Generic;

public class InventorySlots : Control
{
	List<InventoryUISlot> Slots;

	Player pl;
	public override void _Ready()
	{
		Slots = new List<InventoryUISlot>();
		var children = GetChildren();
		for (int i = 0; i < children.Count; i++)
		{
			Slots.Insert(Slots.Count, (InventoryUISlot)children[i]);
		}
		pl = (Player)GetParent().GetParent();

		/*Item[] Items;

		pl.GetCharacterInventory().GetContents(out Items);

		int currentslot = 0;

		for (int i = 0; i < Items.Length; i++)
		{
			if (Slots[currentslot].item.ItemType == Items[i].ItemType)
			{
				Slots[currentslot].SetAmmount(Slots[currentslot].m_ammount + 1);
				currentslot += 1;
				continue;
			}
			Slots[currentslot].SetItem(Items[i]);
		}*/
	}
	private void On_Item_Added(Item item)
	{
		for (int i = 0; i < Slots.Count; i++)
		{
            if (Slots[i].item == null)
            {
                Slots[i].SetItem(item);
			    break;
            }
			if (Slots[i].item.ItemType == item.ItemType)
			{
				Slots[i].SetAmmount(Slots[i].m_ammount + 1);
				break;
			}
			
		}
	}
    private void On_Item_Removed(Item item)
    {
        for (int i = 0; i < Slots.Count; i++)
		{
            if (Slots[i].item == null)
            {
                continue;
            }
			if (Slots[i].item.ItemType == item.ItemType)
			{
                if (Slots[i].m_ammount == 1)
                {
                    Slots[i].SetAmmount(0);
                    Hide();
                    Slots[i].item = null;
                }
				Slots[i].SetAmmount(Slots[i].m_ammount - 1);
				break;
			}
		}
    }
}




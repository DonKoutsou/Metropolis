using Godot;
using System;

public class InventoryUISlot : Control
{
    public Button ItemIcon;
    public RichTextLabel Ammount;

    public int m_ammount;

    public Item item;

    InventoryUI UI;

    
    public override void _Ready()
    {
        ItemIcon = GetNode<Button>("ItemIcon");
        Ammount = GetNode<RichTextLabel>("Ammount");
        UI = (InventoryUI)(GetParent().GetParent());
    }
    public void SetItem(Item it, int ammount = 0)
    {
        if (it == null)
        {
            item = null;
            SetAmmount(ammount);
            AddItemTexture(null);
            //ItemIcon.Text = "";
        }
        else
        {
            item = it;
            SetAmmount(ammount);
            AddItemTexture(it.GetIconTexture());
            //ItemIcon.Text = it.GetItemName();
        }
    }
    public void AddItemTexture(Texture text)
    {
        ItemIcon.Icon = text;
    }
    public void SetAmmount(int ammount)
    {
        m_ammount = ammount;
        Ammount.Text = ammount.ToString();
        if (ammount == 0)
            Ammount.Hide();
        else
            Ammount.Show();
    }
    private void On_Mouse_Entered()
    {
        UI.ItemHovered(item);
    }
    private void On_Mouse_Exited()
    {
        UI.ItemUnHovered(item);
    }
}

using Godot;
using System;
using System.Threading;

public class InventoryUISlot : Control
{
    public Button ItemIcon;
    public RichTextLabel Ammount;

    public int m_ammount;

    public Item item;

    InventoryUI UI;

    ProgressBar Rbar;
    
    public override void _Ready()
    {
        ItemIcon = GetNode<Button>("ItemIcon");
        Ammount = GetNode<RichTextLabel>("Ammount");
        UI = (InventoryUI)(GetParent().GetParent());
        Rbar = GetNode<ProgressBar>("ItemResourceBar");

    }
    public void SetItem(Item it, int ammount = 0)
    {
        if (it == null)
        {
            item = null;
            SetAmmount(ammount);
            AddItemTexture(null);
            ItemIcon.MouseFilter = MouseFilterEnum.Ignore;
            Rbar.Hide();
        }
        else
        {
            item = it;
            SetAmmount(ammount);
            AddItemTexture(it.GetIconTexture());
            ItemIcon.MouseFilter = MouseFilterEnum.Stop;
            if (it.GetItemType() == (int)ItemName.BATTERY)
            {
                Battery bat = (Battery)it;
                Rbar.Value = bat.GetCurrentCap();
                Rbar.Show();
            }
            else
                Rbar.Hide();
        }
    }
    public void AddItemTexture(Texture text)
    {
        ItemIcon.Icon = text;
    }
    public void SetAmmount(int ammount)
    {
        m_ammount = ammount;
        Ammount.Text = "x" + ammount.ToString();
        if (ammount <= 1)
            Ammount.Hide();
        else
            Ammount.Show();
    }
    public void Toggle(bool pressed)
    {
        ItemIcon.Pressed = pressed;
    }
    private void On_Mouse_Entered()
    {
        UI.ItemHovered(item);
    }
    private void On_Mouse_Exited()
    {
        UI.ItemUnHovered(item);
    }
    private void ItemIcon_Toggled(bool toggled)
    {
        if (toggled)
        {
            UI.SetFocused(this);
        }
        else
        {
            UI.UnFocus(this);
        }
            
    }
}

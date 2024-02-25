using Godot;
using System;

public class InventoryUISlot : Control
{
    public Sprite ItemIcon;
    public RichTextLabel Ammount;

    public int m_ammount;

    public Item item;
    public override void _Ready()
    {
        Hide();
        ItemIcon = GetNode<Sprite>("ItemIcon");
        Ammount = GetNode<RichTextLabel>("Ammount");
    }
    public void SetItem(Item it)
    {
        item = it;
        SetAmmount(1);
        AddItemTexture(it.GetIconTexture());
        if (it == null)
            Hide();
        else
            Show();
    }
    public void AddItemTexture(Texture text)
    {
        ItemIcon.Texture = text;
    }
    public void SetAmmount(int ammount)
    {
        m_ammount = ammount;
        Ammount.Text = ammount.ToString();
    }
}

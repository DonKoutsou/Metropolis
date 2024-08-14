using Godot;
using System;
using System.Threading;

public class InventoryUISlot : Control
{
    [Signal]
    public delegate void Hovered(Item it, bool t);

    [Signal]
    public delegate void Focused(bool t, InventoryUISlot slot);

    public Button ItemIcon;
    public RichTextLabel Ammount;

    public int m_ammount;

    public Item item;

    //InventoryUI UI;

    ProgressBar Rbar;

    Panel LimbColor;

    ConditionPanel Condition;
    
    public override void _Ready()
    {
        ItemIcon = GetNode<Button>("ItemIcon");
        Ammount = GetNode<RichTextLabel>("Ammount");
        //UI = (InventoryUI)GetParent().GetParent().GetParent();
        Rbar = GetNode<ProgressBar>("ItemResourceBar");
        LimbColor = GetNode<Panel>("LimbColor");
        Condition = GetNode<ConditionPanel>("ConditionPanel");
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
            LimbColor.Hide();
            Condition.Hide();
        }
        else
        {
            item = it;
            SetAmmount(ammount);
            AddItemTexture(it.GetIconTexture());
            ItemIcon.MouseFilter = MouseFilterEnum.Stop;
            if (it is Battery)
            {
                Battery bat = (Battery)it;
                Rbar.Value = bat.GetCurrentCap();
                Rbar.MaxValue = bat.GetCapacity();
                //Condition.SetCondition(bat.GetCondition());
                Rbar.Show();
                Condition.Hide();
                LimbColor.Hide();
            }
            else if (it is Limb limbitem)
            {
                //((StyleBoxFlat)LimbColor.GetStylebox("panel")).BgColor = limbitem.GetColor();
                LimbColor.Hide();
                Rbar.Hide();
                Condition.Hide();
            }
            else if (it is PaintCan paintc)
            {
                ((StyleBoxFlat)LimbColor.GetStylebox("panel")).BgColor = paintc.GetColor();
                LimbColor.Show();
                Rbar.Hide();
                Condition.Hide();
            }
            else
            {   
                LimbColor.Hide();
                Rbar.Hide();
                Condition.Hide();
            }
                
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
        EmitSignal("Hovered", item, true);
        
        //UI.ItemHovered(item);
    }
    private void On_Mouse_Exited()
    {
        EmitSignal("Hovered", item, false);

        //UI.ItemUnHovered(item);
    }
    private void ItemIcon_Toggled(bool toggled)
    {
        EmitSignal("Focused",toggled, this);

        /*if (toggled)
        {
            UI.SetFocused(this);
        }
        else
        {
            UI.UnFocus(this);
        }*/ 
    }
}

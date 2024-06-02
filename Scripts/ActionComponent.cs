using Godot;
using System;

public class ActionComponent : Node
{
    [Export]
    public float ActionDistance = 30;
    [Export]
    string PickupActionFunciton = null;
    [Export]
    string IntActionFunciton = null;
    [Export]
    string Int2ActionFunciton = null;
    public override void _Ready()
    {
        
    }
    public void OnActionPressed(int index, Player pl, bool CallOnPlayer)
    {
        if (index == 0)
            GetParent().Call(PickupActionFunciton);
        if (index == 1)
            GetParent().Call(IntActionFunciton);
        if (index == 2)
            GetParent().Call(Int2ActionFunciton);
    }
}

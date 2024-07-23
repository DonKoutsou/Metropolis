using Godot;
using System;

public class ActionComponent : Spatial
{
    [Export]
    public float ActionDistance = 30;
    [Export]
    string PickupActionFunciton = null;
    [Export]
    string IntActionFunciton = null;
    [Export]
    string Int2ActionFunciton = null;

    [Export]
    bool AskOwnerForPosition = false;

    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        if (AskOwnerForPosition)
            return (Vector3)GetParent().Call("GetActionPos", PlayerPos);
        else
            return GlobalTranslation;
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

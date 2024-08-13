using Godot;
using System;

public class ActionComponent : Spatial
{
    [Export]
    public float ActionDistance = 30;

    [Export]
    bool AskOwnerForPosition = false;

    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        if (AskOwnerForPosition)
            return (Vector3)GetParent().Call("GetActionPos", PlayerPos);
        else
            return GlobalTranslation;
    }
}

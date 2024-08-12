using Godot;
using System;

public class DoorOpenerSignaler : Spatial
{
    [Export]
    NodePath NodeToSignal = null;

    [Export]
    string FunctionToCall = null;
    
    private void OnDoorOpened()
    {
        GetNode(NodeToSignal).Call(FunctionToCall);
    }
}

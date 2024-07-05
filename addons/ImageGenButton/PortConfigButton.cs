using Godot;
using System;

[Tool]
public class PortConfigButton : Button
{
    [Signal]
    public delegate void OnButtonClicked();
    public override void _EnterTree()
    {
        Connect("pressed", this, "clicked");
    }

    public void clicked()
    {
        EmitSignal("OnButtonClicked");
    }
}


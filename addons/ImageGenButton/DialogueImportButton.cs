using Godot;
using System;

[Tool]
public class DialogueImportButton : Button
{
    [Signal]
    public delegate void OnButtonClicked();
    public override void _EnterTree()
    {
        Connect("pressed", this, "clicked");
    }
    public override void _ExitTree()
    {
        Disconnect("pressed", this, "clicked");
    }
    public void clicked()
    {
        EmitSignal("OnButtonClicked");
    }
}

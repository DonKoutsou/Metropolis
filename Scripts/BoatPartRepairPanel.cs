using Godot;
using System;

public class BoatPartRepairPanel : Control
{
    [Signal]
    public delegate void OnPartRepaired(string Name);
    public void SetData(string PartName, int PartState)
    {
        GetNode<Label>("Panel/PartName").Text = PartName + "\n" + PartState + " / 100";
    }
    private void On_Repair_button_down()
    {
        EmitSignal("OnPartRepaired", Name);
    }
}

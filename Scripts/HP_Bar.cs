using Godot;
using System;

public class HP_Bar : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
        Hide();
        if (GetParent() is Character)
            Value = ((Character)GetParent()).GetHP();
        else
            Value = ((Player)GetParent().GetParent()).GetHP();
    }
    public  void ChangeVisibility()
    {
        Show();
        GetNode<Timer>("VisibilityTimerHP").Start(); 
    }
    public void TurnOff()
    {
        Hide();
    }
}

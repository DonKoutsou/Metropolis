using Godot;
using System;

public class Stamina_Bar : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
        Hide();
        Value = ((Player)GetParent().GetParent()).GetStamina();
    }
    public  void ChangeVisibility()
    {
        Show();
        GetNode<Timer>("VisibilityTimerStamina").Start(); 
    }
    public void TurnOff()
    {
        Hide();
    }
}

using Godot;
using System;

public class AchievementIcon : Control
{
    [Signal]
    public delegate void OnSelected(AchievementIcon ic);
    private void OnIconSelected()
    {
        EmitSignal("OnSelected", this);
        ((StyleBoxFlat)GetNode<Panel>("Panel").GetStylebox("panel")).BorderColor = new Color(0.43f, 0.72f, 0.94f);
        
    }
    private void OnIconUnselected()
    {
        ((StyleBoxFlat)GetNode<Panel>("Panel").GetStylebox("panel")).BorderColor = new Color(0, 0, 0);
    }
}

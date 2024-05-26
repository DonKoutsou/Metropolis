using Godot;
using System;

public class CityNameUI : Control
{
    static Label lab;
    static AnimationPlayer Apl;
    public override void _Ready()
    {
        base._Ready();
        lab = GetNode<Label>("Label2");
        Apl = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public static void ShowName(string Name)
    {
        lab.Text = Name;
        Apl.Play("Show");
    }
}

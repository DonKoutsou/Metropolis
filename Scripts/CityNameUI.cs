using Godot;
using System;

public class CityNameUI : Control
{
    [Export]
    NodePath AnimationNode = null;
    Label lab;
    AnimationPlayer Apl;
    public override void _Ready()
    {
        base._Ready();
        lab = GetNode<Label>("Label2");
        Apl = GetNode<AnimationPlayer>(AnimationNode);
    }
    public void ShowName(string Name)
    {
        lab.Text = Name;
        Apl.Play("Show");
    }
}

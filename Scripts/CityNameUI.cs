using Godot;
using System;

public class CityNameUI : Control
{
    [Export]
    NodePath AnimationNode = null;
    Label lab;
    AnimationPlayer Apl;
    AudioStreamPlayer sound;
    public override void _Ready()
    {
        base._Ready();
        lab = GetNode<Label>("Label2");
        Apl = GetNode<AnimationPlayer>(AnimationNode);
        sound = GetNode<AudioStreamPlayer>("CityNotifSound");
        Visible = false;
    }
    public void PlayerToggle(Player pl)
    {
        Visible = pl != null;
    }
    public void ShowName(string Name)
    {
        lab.Text = Name;
        Apl.Play("Show");
        sound.Play();

    }
}

using Godot;
using System;
using System.Collections.Generic;

public class UISoundManager : Node
{
    [Export]
    AudioStream HoverSound = null;

    [Export]
    AudioStream ClickSound = null;

    AudioStreamPlayer[] Players;
    public override void _Ready()
    {
        Players = new AudioStreamPlayer[2];

        AudioStreamPlayer HoverPlayer = new AudioStreamPlayer()
        {
            Stream = HoverSound,
            Bus = "UI"
        };

        AddChild(HoverPlayer);

        Players[0] = HoverPlayer;

        AudioStreamPlayer ClickPlayer = new AudioStreamPlayer()
        {
            Stream = ClickSound,
            Bus = "UI"
        };

        AddChild(ClickPlayer);

        Players[1] = ClickPlayer;

        var Buttons = GetTree().GetNodesInGroup("Buttons");

        for (int i = 0; i < Buttons.Count; i++)
        {
            Control b = (Control)Buttons[i];
            b.Connect("mouse_entered", this, "OnButtonHovered");
            b.Connect("focus_entered", this, "OnButtonHovered");
            if (b is Button)
                b.Connect("button_down", this, "OnButtonPressed");
        }
    }
    private void OnButtonHovered()
    {
        Players[0].Play();
    }
    private void OnButtonPressed()
    {
        Players[1].Play();
    }
}

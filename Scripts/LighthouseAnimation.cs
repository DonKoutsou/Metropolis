using Godot;
using System;

public class LighthouseAnimation : AnimationPlayer
{
    public override void _Ready()
    {
        CurrentAnimation = "Light";
        Play();
    }
}

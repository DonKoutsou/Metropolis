using Godot;
using System;

public class Kolopoustas : KinematicBody
{
    [Export]
    string AnimToPlay = null;
    public override void _Ready()
    {
        AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim.CurrentAnimation = AnimToPlay;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

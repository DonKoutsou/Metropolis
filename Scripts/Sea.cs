using Godot;
using System;


public class Sea : MeshInstance
{
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation = "Water_Move";
        GetNode<AnimationPlayer>("AnimationPlayer").Play();
    }
}

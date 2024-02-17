using Godot;
using System;


public class Sea : StaticBody
{
    AnimationPlayer anim;
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim.CurrentAnimation = "Water_Move";
        GetNode<AnimationPlayer>("AnimationPlayer").Play();
    }
    public void Start()
    {
        anim.Play();
    }
    public void Stop()
    {
        anim.Stop();
    }
}

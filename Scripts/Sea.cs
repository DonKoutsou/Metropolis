using Godot;
using System;


public class Sea : StaticBody
{
    AnimationPlayer anim;
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim.CurrentAnimation = "RESET";
    }
    private void _on_Sea_visibility_changed()
    {
        if (Visible)
            anim.Play();
        else
            anim.Stop();
    }
}

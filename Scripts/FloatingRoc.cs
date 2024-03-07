using Godot;
using System;

public class FloatingRoc : Spatial
{
    AnimationPlayer anim;
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim.CurrentAnimation = "Dis";
    }
    private void _on_Rock_visibility_changed()
    {
        if (Visible)
            anim.Play();
        else
            anim.Stop();
    }
}

using Godot;
using System;

public class CameraAnimationPlayer : AnimationPlayer
{
    static CameraAnimationPlayer instance;

    public static CameraAnimationPlayer GetInstance()
    {
        return instance;
    }
    public override void _Ready()
    {
        instance = this;
    }
    public void FadeIn()
    {
        Play("FadeIn");
    }

}

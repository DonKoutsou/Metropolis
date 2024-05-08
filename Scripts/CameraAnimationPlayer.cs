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
        SetProcess(false);
    }
    public void PlayAnim(string name)
    {
        PlaybackSpeed = 1;
        Play(name);
    }
    public void FadeIn(int spd)
    {
        PlaybackSpeed = 1.0f / spd;
        Play("FadeIn");
    }
    public void FadeOut(int spd)
    {
        PlaybackSpeed = 1.0f / spd;
        Play("FadeOut");
    }
    public void FadeInOut(int spd)
    {
        PlaybackSpeed = 1.0f / spd;
        Play("FadeOut");
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (IsPlaying())
        {
            return;
        }

        Play("FadeIn");

        SetProcess(false);
    }

}

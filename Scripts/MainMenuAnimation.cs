using Godot;
using System;

public class MainMenuAnimation : AnimationPlayer
{
    public override void _Ready()
    {
        Play("Start");
        SetProcess(false);
    }
    public void FadeIn()
    {
        Play("Stop");
    }
    public void FadeOut()
    {
        Play("Start");
    }
    public void FadeInOut()
    {
        Play("Stop");
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (IsPlaying())
        {
            return;
        }
        StartingScreen screen = (StartingScreen)GetParent().GetParent();

        screen.StartGame();

        Play("Start");

        SetProcess(false);
    }
}

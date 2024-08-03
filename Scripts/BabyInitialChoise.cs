using Godot;
using System;

public class BabyInitialChoise : Control
{
    [Signal]
    public delegate void BabyPicked(bool P);

    bool picked = false;
    private void BabyTaken()
    {
        if (picked)
            return;
        EmitSignal("BabyPicked", true);
        picked = true;
        pl.Play("Stop");

        SetProcess(true);
    }
    private void BabyLeft()
    {
        if (picked)
            return;
        EmitSignal("BabyPicked", false);
        picked = true;
        pl.Play("Stop");

        SetProcess(true);
    }
    AnimationPlayer pl;
    public override void _Ready()
    {
        pl = GetNode<AnimationPlayer>("AnimationPlayer");
        pl.Play("Start");
        SetProcess(false);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (!pl.IsPlaying())
            QueueFree();
    }
}

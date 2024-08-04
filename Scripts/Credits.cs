using Godot;
using System;

public class Credits : Control
{
    [Signal]
    public delegate void OnCreditsEnded();
    public override void _Ready()
    {
        Hide();
    }
    public void PlayCredits()
    {
        Show();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Credit Roll");
    }
    private void Credits_Finished(string anim)
    {
        Hide();
        EmitSignal("OnCreditsEnded");
    }
}

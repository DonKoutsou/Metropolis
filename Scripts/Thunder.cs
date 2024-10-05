using Godot;
using System;

public class Thunder : Spatial
{
    public void InputData(AudioStream ThunderSound)
    {
        GetNode<AudioStreamPlayer>("Thunder").Stream = ThunderSound;
    }
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("ThunderStutter");
    }
    private void ThunderFinished()
    {
        QueueFree();
    }
}

using Godot;
using System;

public class Intro : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Intro");
        GetNode<Camera>("Camera").MakeCurrent();
    }
    public override void _Process(float delta)
    {
        if (GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
            return;
        Stop();
    }
    private void On_Skip_Button_Down()
    {
        Stop();
    }
    private void Stop()
    {
        MyWorld.GetInstance().SpawnPlayer(SpawnPosition.GetInstance().GlobalTranslation);
        Particles part = GetNode<Particles>("Particles");
        RemoveChild(part);
        Player.GetInstance().AddChild(part);
        part.Translation = Vector3.Zero;
        QueueFree();
    }
}

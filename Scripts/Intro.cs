using Godot;
using System;

public class Intro : Spatial
{

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

        QueueFree();
    }
}

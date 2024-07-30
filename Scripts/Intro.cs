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
        Player pl = MyWorld.GetInstance().SpawnPlayer(SpawnPosition.GetInstance().GlobalTranslation);
        WorldMap map = WorldMap.GetInstance();
        WorldParticleManager man = GetNode<WorldParticleManager>("WorldParticleManager");
        RemoveChild(man);
        map.AddChild(man);
        pl.GetNode<RemoteTransform>("WorldParticleRemoteTransform").RemotePath = man.GetPath();
        man.GlobalRotation = Vector3.Zero;
        PlayerCamera.GetInstance().Current = true;
        QueueFree();
    }
    public void LoadStop(Vector3 spawnpos)
    {
        Player pl = MyWorld.GetInstance().SpawnPlayer(spawnpos);
        WorldMap map = WorldMap.GetInstance();
        WorldParticleManager man = GetNode<WorldParticleManager>("WorldParticleManager");
        RemoveChild(man);
        map.AddChild(man);
        pl.GetNode<RemoteTransform>("WorldParticleRemoteTransform").RemotePath = man.GetPath();
        man.GlobalRotation = Vector3.Zero;
        PlayerCamera.GetInstance().Current = true;
        QueueFree();
    }
}

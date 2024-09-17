using Godot;
using System;

public class Intro : Spatial
{

    public override void _Ready()
    {
        
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Intro");
        GetNode<Camera>("Camera").MakeCurrent();

        UniversalLodManager.GetInstance().UpdateCamera(GetNode<Camera>("Camera"));
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
        Player pl = MyWorld.GetInstance().SpawnPlayer(SpawnPosition.GetInstance().GlobalTranslation, SpawnPosition.GetInstance().GlobalRotation);
        WorldMap map = WorldMap.GetInstance();
        WorldParticleManager man = GetNode<WorldParticleManager>("WorldParticleManager");
        RemoveChild(man);
        map.AddChild(man);
        pl.GetNode<RemoteTransform>("WorldParticleRemoteTransform").RemotePath = man.GetPath();

        DialogueManager.GetInstance().ScheduleDialogue(pl, LocalisationHolder.GetString("Ένα από τα σκάφη επέζησε. Τι μπορεί να έχει μέσα;..."));
        //pl.GetTalkText().Talk("Ένα από τα σκάφη επέζησε. Τι μπορεί να έχει μέσα;...");
        pl.MoveTo(PodPosition.GetInstance().GlobalTranslation, true, true);
        man.GlobalRotation = Vector3.Zero;
        PlayerCamera.GetInstance().Current = true;
        DepartureSystemPosition.GetInstance().SpawnSystem();
        QueueFree();
    }
    public void LoadStop(Vector3 spawnpos)
    {
        Player pl = MyWorld.GetInstance().SpawnPlayer(spawnpos, Vector3.Zero);
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

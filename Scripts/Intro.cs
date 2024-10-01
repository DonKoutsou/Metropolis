using Godot;
using System;

public class Intro : Spatial
{

    public override void _Ready()
    {
        
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Intro");
        GetNode<Camera>("Camera").MakeCurrent();

        UniversalLodManager.GetInstance().UpdateCamera(GetNode<Camera>("Camera"));

        GetNode<Label>("Camera/LoadingScreen/Button").Text = LocalisationHolder.GetString(GetNode<Label>("Camera/LoadingScreen/Button").Text);
        GetNode<Label>("Camera/LoadingScreen/Label").Text = LocalisationHolder.GetString(GetNode<Label>("Camera/LoadingScreen/Label").Text);

        Sky.OnGameStart();
    }
    public override void _Process(float delta)
    {
        if (GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
            return;
        Stop();
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("SkipCinematic"))
		{
			Stop();
		}
    }
    private void Stop()
    {
        Player pl = MyWorld.GetInstance().SpawnPlayer(SpawnPosition.GetInstance().GlobalTranslation, SpawnPosition.GetInstance().GlobalRotation, true);
        Sky.OnPlayerSpawned(pl);
        DialogueManager.GetInstance().ScheduleDialogue(pl, "Ένα από τα σκάφη επέζησε. Τι μπορεί να έχει μέσα;...");
        //pl.GetTalkText().Talk("Ένα από τα σκάφη επέζησε. Τι μπορεί να έχει μέσα;...");
        pl.MoveTo(PodPosition.GetInstance().GlobalTranslation, true, true);
        PlayerCamera.GetInstance().Current = true;
        DepartureSystemPosition.GetInstance().SpawnSystem();
        QueueFree();
    }
    public void LoadStop(Vector3 spawnpos)
    {
        Player pl = MyWorld.GetInstance().SpawnPlayer(spawnpos, Vector3.Zero);
        Sky.OnPlayerSpawned(pl);
        PlayerCamera.GetInstance().Current = true;
        QueueFree();
    }
}

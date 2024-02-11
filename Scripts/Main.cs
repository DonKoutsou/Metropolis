using Godot;
using System;

public class Main : StaticBody
{
    [Export]
    public PackedScene MobScene;

    public override void _Ready()
    {
        GD.Randomize();
        GetNode<Control>("UserInterface/Retry").Hide();
    }
    public void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Mob mob = (Mob)MobScene.Instance();

        // Choose a random location on the SpawnPath.
        // We store the reference to the SpawnLocation node.
        var mobSpawnLocation = GetNode<PathFollow>("SpawnPath/SpawnLocation");
        // And give it a random offset.
        float rand = GD.Randf();
        mobSpawnLocation.UnitOffset = rand;

        mob.Initialize(mobSpawnLocation.Translation, GetNode<Player>("Player"));

        AddChild(mob);

        mob.Connect(nameof(Mob.Squashed), GetNode<ScoreLabel>("UserInterface/ScoreLabel"), nameof(ScoreLabel.OnMobSquashed));

    }
    public void OnPlayerHit()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Control>("UserInterface/Retry").Show();
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
        {
            // This restarts the current scene.
            GetTree().ReloadCurrentScene();
        }
    }
}

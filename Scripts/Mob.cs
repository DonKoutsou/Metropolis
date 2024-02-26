using Godot;
using System;

public class Mob : Character
{
    [Export]
    public int MinSpeed = 10;
    // Maximum speed of the mob in meters per second
    [Export]
    public int MaxSpeed = 18;

    [Signal]
    public delegate void Squashed();


    public override void _PhysicsProcess(float delta)
    {
        var pls = GetTree().GetNodesInGroup("player");
        if (pls.Count == 0)
            return;
        Player pl = (Player)pls[0];
        var orig = pl.Transform.origin;
        MoveTo(orig);
        _velocity.y -= FallAcceleration * delta;
        MoveAndSlide(_velocity);
    }
    public void Initialize(Vector3 startPosition, Player player)
    {

    }
    public void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }
    public void Squash()
    {
        EmitSignal(nameof(Squashed));
        QueueFree();
    }
}

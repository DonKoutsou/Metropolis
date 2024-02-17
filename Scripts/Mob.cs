using Godot;
using System;

public class Mob : Character
{
    [Export]
    public int MinSpeed = 10;
    // Maximum speed of the mob in meters per second
    [Export]
    public int MaxSpeed = 18;
    [Export]
    public int FallAcceleration = 75;
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
        // We position the mob and turn it so that it looks at the player.
        LookAtFromPosition(startPosition, player.Transform.origin, Vector3.Up);
        // And rotate it randomly so it doesn't move exactly toward the player.
        //RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));
        // We calculate a random speed.
        //float randomSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);
        // We calculate a forward velocity that represents the speed.
        //_velocity = Vector3.Forward * randomSpeed;
        // We then rotate the vector based on the mob's Y rotation to move in the direction it's looking
        //_velocity = _velocity.Rotated(Vector3.Up, Rotation.y);

        NavAgent = GetNode<NavigationAgent>("NavigationAgent");
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

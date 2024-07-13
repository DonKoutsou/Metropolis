using Godot;
using System;

public class PortWorker : NPC
{
    [Export]
    NodePath PortPath = null;

    Port p;

    public override void _Ready()
    {
        base._Ready();
        p = GetNode<Port>(PortPath);
        p.RegisterWorker(this);
    }
    public Port GetPort()
    {
        return p;
    }
}

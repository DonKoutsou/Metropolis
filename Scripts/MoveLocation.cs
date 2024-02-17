using Godot;
using System;

public class MoveLocation : Spatial
{
    [Export(PropertyHint.Layers3dPhysics)]
    public uint MoveLayer { get; set; }
}

using Godot;
using System;

public class MapTile : Control
{
    public int type;
    public override void _Ready()
    {
        base._Ready();
        Modulate = new Color(1,1,1,0);
    }
}

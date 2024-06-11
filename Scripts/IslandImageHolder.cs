using Godot;
using System;

public class IslandImageHolder : Node
{
    [Export]
    public Image[] Images = null;
    static IslandImageHolder instance;      
    public override void _Ready()
    {
        instance = this;
    }
    static public IslandImageHolder GetInstance()
    {
        return instance;
    }
}

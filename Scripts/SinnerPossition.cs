using Godot;
using System;

public class SinnerPossition : Position3D
{
    static SinnerPossition instance;
    public override void _Ready()
    {
        instance = this;
    }
    public static SinnerPossition GetInstance()
    {
        return instance;
    }
}

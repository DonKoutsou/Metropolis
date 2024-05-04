using Godot;
using System;

public class PlayerCamera : Camera
{
    public override void _Ready()
    {
        Fov = Settings.GetGameSettings().FOVOverride;
    }
}

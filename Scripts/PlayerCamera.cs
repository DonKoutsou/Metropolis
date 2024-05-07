using Godot;
using System;

public class PlayerCamera : Camera
{
    static PlayerCamera instance;


    public static PlayerCamera GetInstance()
    {
        return instance;
    }
    public override void _Ready()
    {
        instance = this;
        Fov = Settings.GetGameSettings().FOVOverride;
    }
}

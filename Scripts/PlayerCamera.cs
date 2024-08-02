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
    public override void _EnterTree()
    {
        base._EnterTree();
        UniversalLodManager.GetInstance().UpdateCamera(this);
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        UniversalLodManager.GetInstance().UpdateCamera(null);
    }
}

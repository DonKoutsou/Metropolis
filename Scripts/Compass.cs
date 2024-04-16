using Godot;
using System;

public class Compass : Spatial
{
    Position3D pivot;
    static Compass instance;
    public override void _Ready()
    {
        pivot = GetNode<Position3D>("CompassNeedlePivot");
        instance = this;
        ToggleCompass(false);
    }
    public static Compass GetInstance()
    {
        return instance;
    }
    public void ToggleCompass(bool toggle)
    {
        if (toggle)
        {
            Show();
            SetProcess(true);
        }
        else
        {
            Hide();
            SetProcess(false);
        }
    }
  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector3 globalrot = GlobalRotation;
        if (globalrot.y != 0)
            pivot.Rotation = new Vector3(pivot.Rotation.x, -globalrot.y, pivot.Rotation.z);
    }
}

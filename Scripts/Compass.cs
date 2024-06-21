using Godot;
using System;

public class Compass : Control
{
    //Position3D pivot;
    public override void _Ready()
    {
        //pivot = GetNode<Position3D>("CompassNeedlePivot");
        ToggleCompass(false);
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
        PlayerCamera cam = PlayerCamera.GetInstance();
        if (cam == null || !Godot.Object.IsInstanceValid(cam))
            return;
        GetNode<Sprite>("Sprite2").RotationDegrees = Mathf.Rad2Deg(cam.GlobalRotation.y);
        //Vector3 globalrot = GlobalRotation;
        //if (globalrot.y != 0)
            //pivot.Rotation = new Vector3(pivot.Rotation.x, -globalrot.y, pivot.Rotation.z);
    }
}

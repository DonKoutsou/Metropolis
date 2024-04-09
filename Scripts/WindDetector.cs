using Godot;
using System;

public class WindDetector : MeshInstance
{

  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public bool WindBehind = false;
    public bool BoatFacingWind = false;
    public override void _PhysicsProcess(float delta)
    {
        Vector3 newrot = new Vector3(0, Mathf.Deg2Rad(-360 - DayNight.GetWindDirection()), 0);
        GlobalRotation = newrot;
        if (Mathf.Rad2Deg(Rotation.y) < 90 && Mathf.Rad2Deg(Rotation.y) > -90)
            BoatFacingWind = true;
        else
            BoatFacingWind = false;
    }
}

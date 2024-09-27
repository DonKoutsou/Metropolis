using Godot;
using System;

public class WindDetector : MeshInstance
{

  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public bool WindBehind = false;
    public bool BoatFacingWind = false;
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        Vector3 newrot = new Vector3(0, Mathf.Deg2Rad(-360 - CustomEnviroment.GetWindDirection()), 0);
        GlobalRotation = newrot;
        if (Mathf.Rad2Deg(Rotation.y) < 90 && Mathf.Rad2Deg(Rotation.y) > -90)
            BoatFacingWind = true;
        else
            BoatFacingWind = false;
    }
}

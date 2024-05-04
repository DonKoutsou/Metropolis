using Godot;
using System;

public class Rain : Spatial
{

    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        Vector3 rot = Rotation;
        float rotf = 45 * (DayNight.GetWindStr() - 50) / 100;
        rot.x = Mathf.Deg2Rad( - rotf );
        Rotation = rot;
    }
}

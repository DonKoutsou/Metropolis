using Godot;
using System;

public class Clouds : Spatial
{
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        Vector3 org = ((Spatial)GetParent()).GlobalTransform.origin;
        GlobalTranslation = new Vector3 (org.x, 1700, org.z);
    }
}

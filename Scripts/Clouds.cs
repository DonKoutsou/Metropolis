using Godot;
using System;

public class Clouds : Spatial
{
    public override void _Process(float delta)
    {
        base._Process(delta);
        Vector3 org = GlobalTransform.origin;
        GlobalTranslation = new Vector3 (org.x, 900, org.z);
    }
}

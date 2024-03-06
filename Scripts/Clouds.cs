using Godot;
using System;

public class Clouds : Spatial
{
    public override void _Process(float delta)
    {
        base._Process(delta);
        Vector3 org = ((Spatial)GetParent()).GlobalTransform.origin;
        GlobalTranslation = new Vector3 (org.x, 1700, org.z);
    }
}

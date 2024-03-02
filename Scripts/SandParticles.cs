using Godot;
using System;

public class SandParticles : Particles
{

    public override void _Ready()
    {
        Emitting = true;
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        Vector3 org = GlobalTransform.origin;
        GlobalTranslation = new Vector3 (org.x, 50, org.z);
    }

}

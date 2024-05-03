using Godot;
using System;
using System.Collections.Generic;

public class WorldParticleManager : Spatial
{
    List<Particles> WorldParticles = new List<Particles>();
    public override void _Ready()
    {
        var children = GetChildren();
        foreach (Node Child in children)
        {
            if (Child is Particles)
                WorldParticles.Add((Particles)Child);
        }
    }
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;

        Vector3 org = GlobalTransform.origin;
        GlobalTranslation = new Vector3 (org.x, 50, org.z);
        float winddir = DayNight.GetWindDirection();
        float windstr = DayNight.GetWindStr();
        float rot = Mathf.Deg2Rad(-360 - winddir);
        foreach (Particles particle in WorldParticles)
        {
            particle.SpeedScale = windstr * 0.01f;
            particle.Lifetime = ((100 - windstr) * 0.5f) + 20;
        }
        GlobalRotation = new Vector3(0, rot, 0);
    }
}

using Godot;
using System;

public class SandParticles : Particles
{
    float d = 0.5f;
    Spatial Parent;
    public override void _Ready()
    {
        Parent = (Spatial)GetParent();  
        Emitting = true;
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        Vector3 org = Parent.GlobalTransform.origin;
        Parent.GlobalTranslation = new Vector3 (org.x, 50, org.z);
        float winddir = DayNight.GetWindDirection();
        float windstr = DayNight.GetWindStr();
        float rot = Mathf.Deg2Rad(-360 - winddir);
        SpeedScale = windstr * 0.01f;
        Lifetime = ((100 - windstr) * 0.5f) + 20;
        Parent.GlobalRotation = new Vector3(0, rot, 0);
    }

}

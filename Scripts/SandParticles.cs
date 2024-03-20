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
        float winddir = DayNight.GetWindDirection();
        float windstr = DayNight.GetWindStr();
        float rot = Mathf.Deg2Rad(-360 - (winddir));
        SpeedScale = windstr * 0.05f;
        Lifetime = ((100 - windstr) * 0.5f) + 20;
        GlobalRotation = new Vector3(0.0f, rot, 0.0f);
    }

}

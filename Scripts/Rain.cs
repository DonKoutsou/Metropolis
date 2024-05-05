using Godot;
using System;

public class Rain : Spatial
{
    [Export]
    float MaxRainParticleAmmount = 5000;
    [Export]
    Curve PitchScaleCurve = null;
    [Export]
    Curve SoundScaleCurve = null;
    Particles RainPart;
    AudioStreamPlayer3D RainSound;
    float d = 0.5f;

    public override void _Ready()
    {
        base._Ready();
        RainPart = GetNode<Particles>("Clouds2");
        RainSound = GetNode<AudioStreamPlayer3D>("RainSound");
    }
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
        float multi = DayNight.GetRainStr() / 100;
        int particleammount = (int)(MaxRainParticleAmmount * multi);
        if (particleammount < 1)
            RainPart.Amount = 1;
        else
            RainPart.Amount = particleammount;

        RainSound.PitchScale = PitchScaleCurve.Interpolate(multi);
        RainSound.UnitDb = SoundScaleCurve.Interpolate(multi);
    }
}

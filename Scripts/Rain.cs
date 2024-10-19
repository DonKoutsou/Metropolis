using Godot;
using System;

public class Rain : Spatial
{
    [Export]
    Curve PitchScaleCurve = null;
    [Export]
    Curve SoundScaleCurve = null;
    Particles RainPart;

    float d = 0.5f;

    public override void _Ready()
    {
        base._Ready();
        RainPart = GetNode<Particles>("Clouds2");
        SetPhysicsProcess(false);
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        //Vector3 rot = RainPart.Rotation;
        //float rotf = 45 * (DayNight.GetWindStr() - 50) / 100;
        //rot.x = Mathf.Deg2Rad( rotf );
        //RainPart.Rotation = rot;
        AudioStreamPlayer RainSound = WorldSoundManager.GetSound("Rain");
        float multi = CustomEnviroment.GetRainStr() / 100;
        if (multi < 0.1)
        {
            RainPart.Emitting = false;
            RainSound.Playing = false;
        }
        else
        {
             RainPart.Emitting = true;
             if (!RainSound.Playing)
                RainSound.Playing = true;
             RainSound.PitchScale = PitchScaleCurve.Interpolate(multi);
             RainSound.VolumeDb = SoundScaleCurve.Interpolate(multi);
        }
        //int particleammount = (int)(MaxRainParticleAmmount * multi);
        //if (particleammount < 1)
            //RainPart.Amount = 1;
        //else
            //RainPart.Amount = particleammount;

        
        
        
    }
}

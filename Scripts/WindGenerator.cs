using Godot;
using System;
public class WindGenerator : StaticBody
{
    [Export]
    private float EnergyCapacity = 100;
    [Export]
    private float CurrentEnergy;
    [Export]
    private float EnergyPerWindStreangth = 0.1f;
    AnimationPlayer anim;
    AnimationPlayer anim2;
    AnimationPlayer anim3;
    static Random rand = new Random(69420);

    public float GetCurrentEnergy()
    {
        return CurrentEnergy;
    }
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
    }
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        anim3 = GetNode<AnimationPlayer>("AnimationPlayer3");
        anim.CurrentAnimation = "Blade_Rot";
        anim2.CurrentAnimation = "Blade_Rot";
        anim3.CurrentAnimation = "LookDir";
        anim.Advance(rand.Next(0, 5000) / 1000);
        anim.PlaybackSpeed = rand.Next(2500, 3000) / 1000;
        anim3.Stop();
        anim2.Advance(rand.Next(0, 5000) / 1000);
        anim2.PlaybackSpeed = rand.Next(2500, 3000) / 1000;
        Spatial rotorpivot = GetNode<Spatial>("Rotor_Pivot");
        rotorpivot.LookAt(new Vector3(rotorpivot.GlobalTransform.origin.x, rotorpivot.GlobalTransform.origin.y, rotorpivot.GlobalTransform.origin.z + 1), Vector3.Up);
        if (!Visible)
            SetProcess(false);
    }
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        float winddir = DayNight.GetWindDirection();
        float windstr = DayNight.GetWindStr();
        Spatial rotorpivot = GetNode<Spatial>("Rotor_Pivot");
        float rot = winddir;
        Vector3 originalRotation = GlobalRotation;
        rot += Mathf.Rad2Deg(originalRotation.y) + 180;
        if (rot > 360)
            rot = rot - 360;
        anim3.Seek(rot / 36, true);
        //rotorpivot.GlobalRotation = new Vector3(0.0f, rot, 0.0f);
        float animspeed = windstr * 0.03f;
        float energy = EnergyPerWindStreangth * (windstr/100);
        float scale = Scale.x;
 
        anim.PlaybackSpeed = animspeed - (scale - 1);
        anim2.PlaybackSpeed = animspeed - (scale - 1);
        
        if (CurrentEnergy + energy < EnergyCapacity)
        {
            CurrentEnergy += energy;
        }
    }
    private void _on_Generator_visibility_changed()
    {
        if (Visible)
        {
            anim.Play();
            anim2.Play();
            SetProcess(true);
        }
        
        else
        {
            anim.Stop();
            anim2.Stop();
            SetProcess(false);
        }
    }
    public void SetData(WindGeneratorInfo info)
	{
		CurrentEnergy = info.CurrentEnergy;
	}
}

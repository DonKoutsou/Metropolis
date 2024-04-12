using Godot;
using System;

public class FireplaceLight : StaticBody
{
    [Export]
    public bool State = false;
    Random rand = new Random();

    OmniLight light;
    Particles part;
    public override void _Ready()
    {
        light = GetNode<OmniLight>("FireplaceLight");
        part = GetNode<Particles>("Particles");

        if (!State)
        {
            light.LightEnergy = 0.0f;
            part.Emitting = false;
            SetPhysicsProcess(false);
            GetNode<AudioStreamPlayer3D>("FireplaceSound").Stop();
        }
        else
        {
            GetNode<AudioStreamPlayer3D>("FireplaceSound").Play();
            //part.Emitting = true;
            //SetProcess(true);
        }
        
    }
    public void ToggleFileplace()
    {
        if (State)
        {
            State = false;
            light.LightEnergy = 0.0f;
            part.Emitting = false;
            GetNode<AudioStreamPlayer3D>("FireplaceSound").Stop();
            SetProcess(false);
        }
        else
        {
            State = true;
            GetNode<AudioStreamPlayer3D>("FireplaceSound").Play();
            part.Emitting = true;
            SetProcess(true);
        }
    }
    float d = 0.05f;
    public override void _Process(float delta)
    {
        d -= delta;
		if (d <= 0)
		{
			d = 0.05f;
            double sample = rand.NextDouble();
            double scaled = (sample * 1.2) + 0.6;
            light.LightEnergy = (float)scaled;
            //double scaled1 = (sample * 0.5) + -0.5;
            //double scaled2 = (sample * 0.5) + -0.5;
            //light.Translation = new Vector3( (float)scaled1, 1.3f, (float)scaled2);
        }
    }
}

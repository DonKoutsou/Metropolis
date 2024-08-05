using Godot;
using System;

public class FireplaceLight : StaticBody
{
    [Export]
    public bool State = false;
    Random rand = new Random();

    OmniLight light;
    Particles part;
    bool covered = false;
    public override void _Ready()
    {
        light = GetNode<OmniLight>("FireplaceLight");
        part = GetNode<Particles>("Particles");

        if (!State)
        {
            light.LightEnergy = 0.0f;
            part.Emitting = false;
            SetProcess(false);
            GetNode<AudioStreamPlayer3D>("FireplaceSound").Stop();
        }
        else
        {
            GetNode<AudioStreamPlayer3D>("FireplaceSound").Play();
            part.Emitting = true;
            SetProcess(true);
        }
        RayCast CoverCast = GetNode<RayCast>("RoofRayCast");
        CoverCast.ForceRaycastUpdate();
        covered = CoverCast.IsColliding();
        CoverCast.QueueFree();
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
            if (DayNight.GetRainStr() > 10 && !covered)
                ToggleFileplace();
            //double scaled1 = (sample * 0.5) + -0.5;
            //double scaled2 = (sample * 0.5) + -0.5;
            //light.Translation = new Vector3( (float)scaled1, 1.3f, (float)scaled2);
        }
    }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
        ToggleFileplace();
    }
    public string GetActionName(Player pl)
    {
        string actiontex;
        if (State)
            actiontex = "Σβήσε.";
        else
            actiontex = "Άναψε.";

        return actiontex;
    }
    public bool ShowActionName(Player pl)
    {
        return true;
    }
    public string GetObjectDescription()
    {
        return "Με ένα εκρηκτικό θα μπορούσα να το σπάσω";
    }
}

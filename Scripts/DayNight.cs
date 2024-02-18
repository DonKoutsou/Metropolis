using Godot;
using System;

public class DayNight : WorldEnvironment
{
    [Export]
    float[] timebrightness;

    [Export]
    float[] SunRotation;

    [Export]
    int startinghour;

    int currenthour;
    float currentmins;

    Time_UI UI;

    DirectionalLight sun;
    public override void _Process(float delta)
    {
        currentmins += delta * 10;
        if (currentmins > 60)
        {
            currentmins = 0;
            currenthour += 1;
            if (currenthour > 23)
            {
                currenthour = 0;
            }
        }
        if (UI != null)
            UI.UpdateTime(currenthour, currentmins);
        else
            UI = Time_UI.GetInstance();

        var minval = (currentmins-0)/(60-0);
        var brightness = timebrightness[currenthour] + (minval / 10);
        var sunrot = SunRotation[currenthour];
        Environment.AdjustmentBrightness = brightness;
        sun.RotationDegrees = new Vector3(sunrot, 0, 0);
        sun.LightEnergy = brightness;

        GD.Print(brightness);
        GD.Print(sunrot);
    }
    public override void _Ready()
    {
        base._Ready();
        currenthour = startinghour;
        sun = GetParent().GetNode<DirectionalLight>("Sun");
    }

}

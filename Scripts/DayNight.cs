using Godot;
using System;

public class DayNight : WorldEnvironment
{

    [Export]
    Curve brightnesscurve = null;

    [Export]
    Curve sunbrightnesscurve = null;

    [Export]
    Curve moonbrightnesscurve = null;

    [Export]
    Curve softlightnesscurve = null;
    [Export]
    Curve sunrotcurve = null;

    [Export]
    Curve sunRcolorcurve = null;

    [Export]
    Curve sunGcolorcurve = null;

    [Export]
    Curve sunBcolorcurve = null;

    [Export]
    Curve moonRcolorcurve = null;

    [Export]
    Curve moonGcolorcurve = null;

    [Export]
    Curve moonBcolorcurve = null;

   // [Export]
    //Curve moonRcolorcurve;

    //[Export]
   // Curve moonGcolorcurve;

    //[Export]
    //Curve moonBcolorcurve;

    [Export]
    int startinghour = 10;

    float currenthour;
    float currentmins;

    Time_UI UI;

    DirectionalLight sun;
    DirectionalLight moon;

    static bool day = false;

    public Spatial SunMoonMeshPivot;

    float d = 0.1f;
    public override void _Process(float delta)
    {
        d -= delta;
		if (d > 0)
            return;
		d = 0.1f;
        currentmins += d;
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
        var hourval = (currenthour + minval)/24;
        //var currentval = hourval + (minval / 10);
        var brightness = brightnesscurve.Interpolate(hourval);
        var sunbrightness = sunbrightnesscurve.Interpolate(hourval);
        var moonbrightness = moonbrightnesscurve.Interpolate(hourval);
        var softlight = softlightnesscurve.Interpolate(hourval);
        var sunrot = sunrotcurve.Interpolate(hourval);
        
        //var brightness = timebrightness[currenthour] + (minval / 10);
        //var sunrot = SunRotation[(int)currenthour];
        Environment.BackgroundEnergy = brightness;
        Environment.AmbientLightEnergy = softlight;
        sun.LightEnergy = sunbrightness;
        moon.LightEnergy = moonbrightness;
        Color newsuncol = new Color(sunRcolorcurve.Interpolate(hourval) , sunGcolorcurve.Interpolate(hourval), sunBcolorcurve.Interpolate(hourval));
        Color newmooncol = new Color(moonRcolorcurve.Interpolate(hourval) , moonGcolorcurve.Interpolate(hourval), moonBcolorcurve.Interpolate(hourval));
        sun.LightColor = newsuncol;
        
        moon.LightColor = newmooncol;
        if (sunrot > 180)
        {
            sun.Show();
            day = true;
            sunrot = -(180 - (sunrot - 180));
            Environment.FogSunColor = newsuncol;
            //Environment.FogColor = newsuncol;
            Environment.AmbientLightColor = newsuncol;
            Environment.BackgroundColor = newsuncol.LinearInterpolate(new Color (0.0f, 0.0f,0.0f), 1- sunbrightness);
            Environment.FogSunAmount = 0.6f;
            moon.Hide();
        }
        else
        {
            sun.Hide();
            day = false;
            moon.Show();
            Environment.FogSunColor = newmooncol;
            //Environment.FogColor = newmooncol;
            Environment.AmbientLightColor = newmooncol;
            Environment.BackgroundColor = newmooncol.LinearInterpolate(new Color (0.0f, 0.0f,0.0f), 1- moonbrightness);
            Environment.FogSunAmount = 0.05f;
        }
        var moonrot = -(180 - sunrot);
        if (SunMoonMeshPivot != null)
        {
            SunMoonMeshPivot.RotationDegrees = new Vector3(sunrot, 0, 0);
        }  
        sun.RotationDegrees = new Vector3(sunrot, 0, 0);
        moon.RotationDegrees = new Vector3(moonrot, 0, 0);
        //GD.Print(brightness);
        //GD.Print(sunrot);
    }
    public static bool IsDay()
    {
        return day;
    }
    public override void _Ready()
    {
        base._Ready();
        currenthour = startinghour;
        sun = GetParent().GetNode<DirectionalLight>("Sun");
        moon = GetParent().GetNode<DirectionalLight>("Moon");
    }

}

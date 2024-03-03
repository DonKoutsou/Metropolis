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

    static int timeprogmultiplier = 1;

    float currenthour;
    float currentmins;

    Time_UI UI;

    DirectionalLight sun;
    DirectionalLight moon;

    static bool day = false;

    public Spatial SunMoonMeshPivot;

    

    public override void _PhysicsProcess(float delta)
    {
        
        currentmins += 0.016f * timeprogmultiplier;
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
        var sunrot = 0.0f;
        if (hourval > 0.85f)
            sunrot = sunrotcurve.Interpolate(hourval - 0.85f);
        else
            sunrot = sunrotcurve.Interpolate(hourval + 0.15f);

        Environment.BackgroundEnergy = brightness;
        

        Color newsuncol = new Color(sunRcolorcurve.Interpolate(hourval) , sunGcolorcurve.Interpolate(hourval), sunBcolorcurve.Interpolate(hourval));
        Color newmooncol = new Color(moonRcolorcurve.Interpolate(hourval) , moonGcolorcurve.Interpolate(hourval), moonBcolorcurve.Interpolate(hourval));
        sun.LightColor = newsuncol;

        moon.LightColor = newmooncol;
        if (sunrot > 190 && sunrot < 350)
        {
            sun.Show();
            sun.LightEnergy = sunbrightness;
            day = true;
            Environment.FogSunColor = newsuncol;
            Environment.FogColor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(newsuncol, sunbrightness);
            Environment.AmbientLightColor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(newsuncol, sunbrightness);
            Environment.BackgroundColor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(newsuncol, sunbrightness);
            Environment.FogSunAmount = 0.3f;
            Environment.AmbientLightEnergy = softlight;
            moon.Hide();
        }
        else if (sunrot < 170 && sunrot > 10)
        {
            sun.Hide();
            day = false;
            moon.Show();
            Environment.FogSunColor = newmooncol;
            Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(newmooncol, moonbrightness);

            Environment.FogColor = backgroundcolor;

            Environment.AmbientLightColor = backgroundcolor;
            
            Environment.BackgroundColor = backgroundcolor;

            Environment.AmbientLightEnergy = softlight;

            Environment.FogSunAmount = 0.05f;

            moon.LightEnergy = moonbrightness * 0.003f;
        }
        else
        {
            sun.Show();
            moon.Show();
            
            float multi = 0;
            Color combination = new Color (0.0f, 0.0f,0.0f);

            float bright = 0;
            float fogsun = 0;
            if (sunrot > 170 && sunrot < 190)
            {
                multi = (float)Math.Round((sunrot - 170) / 20, 4);

                
                bright = Mathf.Lerp(moonbrightness, sunbrightness, multi);
                fogsun = Mathf.Lerp(0.05f, 0.3f, multi);
                Color mix = newmooncol.LinearInterpolate(newsuncol , multi);
                
                sun.LightEnergy = Mathf.Lerp(0.0f, sunbrightness, multi);
                moon.LightEnergy = Mathf.Lerp((moonbrightness * 0.003f), 0.0f, multi);
                //if (multi > 0.5f)
                //{
                //    sun.Show();
                //    moon.Hide();
                //}
                //else
                //{
                //    sun.Hide();
                //    moon.Show();
                //}
                combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, bright);
                Environment.FogSunColor = mix;
            }
            else
            {
                if (sunrot > 190)
                {
                    float rot = 10 - (360 - sunrot);
                    multi = (float)Math.Round((rot / 20), 4);
                }
                else
                {
                    multi = (float)Math.Round((sunrot + 10) / 20, 4);
                }

                bright = Mathf.Lerp(sunbrightness, moonbrightness, multi);
                fogsun = Mathf.Lerp(0.3f, 0.05f, multi);
                Color mix = newsuncol.LinearInterpolate(newmooncol , multi);
                combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, bright);
                Environment.FogSunColor = mix;
                sun.LightEnergy = Mathf.Lerp(sunbrightness, 0.0f, multi);
                moon.LightEnergy = Mathf.Lerp(0.0f, (moonbrightness * 0.003f), multi);
                //if (multi > 0.5f)
               // {
                //    sun.Hide();
                //    moon.Show();
                //}
                //else
                //{
                //    sun.Show();
                //    moon.Hide();
                //}
            }

            Environment.FogColor = combination;

            Environment.AmbientLightColor = combination;
            
            Environment.BackgroundColor = combination;

            Environment.AmbientLightEnergy = softlight;

            Environment.FogSunAmount = fogsun;
        }
        var moonrot = 0.0f;
        if (sunrot < 180)
            moonrot = 180 + sunrot;
        else
            moonrot = -(180 - sunrot);
        if (SunMoonMeshPivot != null)
        {
            SunMoonMeshPivot.RotationDegrees = new Vector3(sunrot, 0, 0);
        }
        sun.RotationDegrees = new Vector3(sunrot, 0, 0);
        moon.RotationDegrees = new Vector3(moonrot, 0, 0);

    }
    public static bool IsDay()
    {
        return day;
    }
    public static void UpdateTimeProgression(int time)
    {
        timeprogmultiplier = time;
    }
    public override void _Ready()
    {
        base._Ready();
        currenthour = startinghour;
        sun = GetParent().GetNode<DirectionalLight>("Sun");
        moon = GetParent().GetNode<DirectionalLight>("Moon");
        timeprogmultiplier = Settings.GetGameSettings().TimeProgression;
        Environment.FogEnabled = true;
    }

}

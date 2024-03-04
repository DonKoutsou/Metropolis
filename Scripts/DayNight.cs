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
                currenthour = 0;
        }
        if (UI != null)
            UI.UpdateTime(currenthour, currentmins);
        else
            UI = Time_UI.GetInstance();

        var minval = (currentmins-0)/(60-0);

        var hourval = (currenthour + minval)/24;

        var brightness = brightnesscurve.Interpolate(hourval);

        var sunbrightness = sunbrightnesscurve.Interpolate(hourval);

        var moonbrightness = moonbrightnesscurve.Interpolate(hourval);

        var softlight = softlightnesscurve.Interpolate(hourval);

        float sunrot;

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

            moon.Hide();

            sun.LightEnergy = sunbrightness;

            Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(newsuncol, sunbrightness);

            day = true;

            Environment.FogSunColor = newsuncol;

            Environment.FogColor = backgroundcolor;

            Environment.AmbientLightColor = backgroundcolor;

            Environment.BackgroundColor = backgroundcolor;

            Environment.FogSunAmount = 0.3f;

            Environment.AmbientLightEnergy = softlight;
        }
        else if (sunrot < 170 && sunrot > 10)
        {
            sun.Hide();
            moon.Show();

            day = false;
            
            moon.LightEnergy = moonbrightness;

            
            Environment.FogSunColor = newmooncol;

            Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(newmooncol, moonbrightness);

            Environment.FogColor = backgroundcolor;

            Environment.AmbientLightColor = backgroundcolor;
            
            Environment.BackgroundColor = backgroundcolor;

            Environment.AmbientLightEnergy = softlight;

            Environment.FogSunAmount = 0.05f;

            moon.LightEnergy = moonbrightness;
        }
        else
        {
            sun.Show();
            moon.Show();
            
            float multi;

            Color combination;

            float bright;

            float fogsun;

            float SunLightEnergy = 0.0f;

            float MoonLightEnergy = 0.0f;

            Color mix;

            if (sunrot > 170 && sunrot < 190)
            {
                multi = (float)Math.Round((sunrot - 170) / 20, 4);
                
                bright = Mathf.Lerp(moonbrightness, sunbrightness, multi);

                fogsun = Mathf.Lerp(0.05f, 0.3f, multi);

                mix = newmooncol.LinearInterpolate(newsuncol , multi);

                SunLightEnergy = Mathf.Lerp(0.0f, sunbrightness, multi);
                
                MoonLightEnergy = Mathf.Lerp((moonbrightness), 0.0f, multi);

                combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, bright);
            }
            else
            {
                if (sunrot > 190)
                {
                    float rot = 10 - (360 - sunrot);
                    multi = (float)Math.Round((rot / 20), 4);
                }
                else
                    multi = (float)Math.Round((sunrot + 10) / 20, 4);
                    
                bright = Mathf.Lerp(sunbrightness, moonbrightness, multi);

                fogsun = Mathf.Lerp(0.3f, 0.05f, multi);

                mix = newsuncol.LinearInterpolate(newmooncol , multi);
                
                SunLightEnergy = Mathf.Lerp(sunbrightness, 0.0f, multi);
                
                MoonLightEnergy = Mathf.Lerp(0.0f, (moonbrightness), multi);

                combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, bright);
            }
            
            sun.LightEnergy = (float)Math.Round(SunLightEnergy, 4);

            moon.LightEnergy = (float)Math.Round(MoonLightEnergy, 4);

            Environment.FogColor = combination;

            Environment.AmbientLightColor = combination;
            
            Environment.BackgroundColor = combination;

            Environment.AmbientLightEnergy = softlight;

            Environment.FogSunAmount = fogsun;

            Environment.FogSunColor = mix;
        }
        float moonrot;

        if (sunrot < 180)
            moonrot = 180 + sunrot;
        else
            moonrot = -(180 - sunrot);

        if (SunMoonMeshPivot != null)
        {
            SunMoonMeshPivot.RotationDegrees = new Vector3(sunrot, 0, 0);
            SunMoonMeshPivot.GlobalTranslation = new Vector3(SunMoonMeshPivot.GlobalTransform.origin.x, 0, SunMoonMeshPivot.GlobalTransform.origin.z);
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

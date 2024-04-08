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


    //WindDirPer10Days
    [Export]
    Curve WindDirCurve = null;
    //WindStPer10Days
    [Export]
    Curve WindStreangthCurve = null;

    float currentDay;

    float currenthour;
    float currentmins;

    static public float WindDir = 0;
    static public float WindStreangth = 100;

    DirectionalLight sun;
    DirectionalLight moon;

    static bool day = false;

    public Spatial SunMoonMeshPivot;

    static public float GetWindDirection()
    {
        return WindDir;
    }
    static public float GetWindStr()
    {
        return WindStreangth;
    }
    
    private void UpdateWind()
    {
        float value = currentDay + (((currenthour  + (currentmins / 60))/ 24));
        while (value > 10)
            value -= 10;
        WindDir = WindDirCurve.Interpolate(value/ 10);
        WindStreangth = WindStreangthCurve.Interpolate(value/ 10);
    }
    private void UpdateCurveValues()
    {
        MinuteValue = (currentmins-0)/(60-0);

        HourValue = (currenthour + MinuteValue)/24;

        Brighness = brightnesscurve.Interpolate(HourValue);

        SunBrightness = sunbrightnesscurve.Interpolate(HourValue);

        MoonBrightness = moonbrightnesscurve.Interpolate(HourValue);

        Softlight = softlightnesscurve.Interpolate(HourValue);

        if (HourValue > 0.85f)
            SunRot = sunrotcurve.Interpolate(HourValue - 0.85f);
        else
            SunRot = sunrotcurve.Interpolate(HourValue + 0.15f);

        SunColor = new Color(sunRcolorcurve.Interpolate(HourValue) , sunGcolorcurve.Interpolate(HourValue), sunBcolorcurve.Interpolate(HourValue));

        MoonColor = new Color(moonRcolorcurve.Interpolate(HourValue) , moonGcolorcurve.Interpolate(HourValue), moonBcolorcurve.Interpolate(HourValue));
    }
    private void ToggleDay(int Phase)
    {
        if (Phase == 0)
        {
            sun.Show();
            moon.Hide();
            day = true;
        }
        if (Phase == 1)
        {
            sun.Hide();
            moon.Show();
            day = false;
        }
        if (Phase == 2)
        {
            sun.Show();
            moon.Show();
        }
    }
    private void CalculateDay(out Color FogColor, out Color FogSunColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    {
        if (!day)
            ToggleDay(0);

        sun.LightEnergy = SunBrightness;

        Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(SunColor, SunBrightness);

        FogSunColor = SunColor;

        FogColor = backgroundcolor;

        AmbientLightColor = backgroundcolor;

        BackgroundColor = backgroundcolor;

        AmbientLightEnergy = Softlight;

        //Environment.FogSunAmount = 0.3f;
    }
    private void CalculateNight(out Color FogColor, out Color FogSunColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    {
        if (day)
            ToggleDay(1);
        
        moon.LightEnergy = MoonBrightness;

        FogSunColor = MoonColor;

        Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(MoonColor, MoonBrightness);

        FogColor = backgroundcolor;

        AmbientLightColor = backgroundcolor;
        
        BackgroundColor = backgroundcolor;

        AmbientLightEnergy = Softlight;

        //Environment.FogSunAmount = 0.05f;
    }
    private void CalculateTransition(out Color FogColor, out Color FogSunColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    {
        sun.Show();
        moon.Show();
            
        float multi;

        Color combination;

        float bright;

        //float fogsun;

        float SunLightEnergy = 0.0f;

        float MoonLightEnergy = 0.0f;

        Color mix;

        if (SunRot > 170 && SunRot < 190)
        {
            multi = (float)Math.Round((SunRot - 170) / 20, 4);
            if (multi > 0.5f)
                day = true;
            else
                day = false;
            
            bright = Mathf.Lerp(MoonBrightness, SunBrightness, multi);

            //fogsun = Mathf.Lerp(0.05f, 0.3f, multi);

            mix = MoonColor.LinearInterpolate(SunColor , multi);

            SunLightEnergy = Mathf.Lerp(0.0f, SunBrightness, multi);
            
            MoonLightEnergy = Mathf.Lerp(MoonBrightness, 0.0f, multi);

            combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, bright);
        }
        else
        {
            if (SunRot > 190)
            {
                float rot = 10 - (360 - SunRot);
                multi = (float)Math.Round((rot / 20), 4);
            }
            else
                multi = (float)Math.Round((SunRot + 10) / 20, 4);
            
            if (multi > 0.5f)
                day = false;
            else
                day = true;
            bright = Mathf.Lerp(SunBrightness, MoonBrightness, multi);

            //fogsun = Mathf.Lerp(0.3f, 0.05f, multi);

            mix = SunColor.LinearInterpolate(MoonColor , multi);
            
            SunLightEnergy = Mathf.Lerp(SunBrightness, 0.0f, multi);
            
            MoonLightEnergy = Mathf.Lerp(0.0f, MoonBrightness, multi);

            combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, bright);
        }
        
        sun.LightEnergy = (float)Math.Round(SunLightEnergy, 4);

        moon.LightEnergy = (float)Math.Round(MoonLightEnergy, 4);

        FogColor = combination;

        AmbientLightColor = combination;
        
        BackgroundColor = combination;

        AmbientLightEnergy = Softlight;

        //Environment.FogSunAmount = fogsun;

        FogSunColor = mix;
    }
    private void UpdateSunMoonPlacament()
    {
        float moonrot;

        if (SunRot < 180)
            moonrot = 180 + SunRot;
        else
            moonrot = -(180 - SunRot);

        //if (SunMoonMeshPivot != null)
        //{
        SunMoonMeshPivot.RotationDegrees = new Vector3(SunRot, 0, 0);
        SunMoonMeshPivot.GlobalTranslation = new Vector3(SunMoonMeshPivot.GlobalTransform.origin.x, 0, SunMoonMeshPivot.GlobalTransform.origin.z);
        //}
        sun.RotationDegrees = new Vector3(SunRot, 0, 0);

        moon.RotationDegrees = new Vector3(moonrot, 0, 0);
    }
    private void UpdateTime()
    {
        currentmins += 0.016f * timeprogmultiplier;
        
        if (currentmins > 60)
        {
            currentmins = 0;

            currenthour += 1;

            if (currenthour > 23)
            {
                currentDay += 1;
                currenthour = 0;
            }
                
        }
    }
    //updating values
    float MinuteValue;
    float HourValue;
    float Brighness;
    float SunBrightness;
    float MoonBrightness;
    float Softlight;
    float SunRot;
    Color SunColor;
    Color MoonColor;
    public override void _PhysicsProcess(float delta)
    {
        UpdateTime();
        
        UpdateWind();

        UpdateCurveValues();

        Environment.BackgroundEnergy = Brighness;

        sun.LightColor = SunColor;

        moon.LightColor = MoonColor;
        ///
        /////
        Color FogColor;

        Color FogSunColor;

        Color AmbientLightColor;

        Color BackgroundColor;

        float AmbientLightEnergy;
        //////
        ///


        float SunPlacament = SunRot - 180;

        if (SunPlacament > 10)
        {
            CalculateDay(out FogColor, out FogSunColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
        }
        else if (SunPlacament < -10)
        {
            CalculateNight(out FogColor, out FogSunColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
        }
        else
        {
            CalculateTransition(out FogColor, out FogSunColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
        }
        Environment.FogSunColor = FogSunColor;

        Environment.FogColor = FogColor;

        Environment.AmbientLightColor = AmbientLightColor;

        Environment.BackgroundColor = BackgroundColor;

        Environment.AmbientLightEnergy = AmbientLightEnergy;

        //Environment.FogSunAmount = 0.3f;
        UpdateSunMoonPlacament();

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

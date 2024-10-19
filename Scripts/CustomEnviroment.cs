using Godot;
using System;

public class CustomEnviroment : WorldEnvironment
{
    [Export]
    Curve AutoExposureCurve = null;
    [Export]
    Curve brightnesscurve = null;

    [Export]
    Curve sunbrightnesscurve = null;

    [Export]
    Curve moonbrightnesscurve = null;

    [Export]
    Curve softlightnesscurve = null;
    [Export]
    Curve SunGodRayCurve = null;

    [Export]
    Curve MoonGodRayCurve = null;
    [Export]
    Curve sunrotcurve = null;
    float ThunderMulti;
    Random ThunderRand = new Random();
    private const double lambda = 5.0; // Average number of strikes per minute
    [Export]
    Gradient SunColorGradient = null;

    [Export]
    Gradient MoonColorGradient = null;

    [Export]
    int startinghour = 10;

    static int timeprogmultiplier = 1;
    //WindDirPer10Days
    [Export]
    Curve WindDirCurve = null;
    //WindStPer10Days
    [Export]
    Curve WindStreangthCurve = null;

    [Export]
    Curve RainStreangthCurve = null;

    [Export]
    Curve FogDirstanceCurve = null;

    [Export]
    Color FogDistantColor = new Color(0.8f, 0.6f, 0.38f);

    float CurrentDistance = 0;

    public override void _Ready()
    {
        base._Ready();
        
        currenthour = startinghour;

        Random rand = new Random(SettingsPanel.Instance.Seed);
        currentDay = rand.Next(0, 10);
        timeprogmultiplier = SettingsPanel.Instance.TimeProgression;
        
        sun = GetParent().GetNode<DirectionalLight>("Sun");
        moon = GetParent().GetNode<DirectionalLight>("Moon");
        SunGodRays = sun.GetNode("GodRays");
        MoonGodRays = moon.GetNode("GodRays");
        //timeprogmultiplier = Settings.GetGameSettings().TimeProgression;
        Environment.FogEnabled = true;
        SetPhysicsProcess(false);
    }

    public void UpdatePlayerDistance(float newdist)
    {
        int lighthouseamm = WorldMap.GetInstance().GetUnlockedLightHouseCount();
        if (lighthouseamm == 4)
        {
            CurrentDistance = 0;
            return;
        }
         SceneTreeTween tw = CreateTween();
        tw.TweenProperty(this, "CurrentDistance", newdist, 10);
        //CurrentDistance = newdist;
        if (newdist >= 0.6)
        {
            DialogueManager.GetInstance().ForceDialogue(Player.GetInstance(), "TooDeepFog");
            //Player.GetInstance().GetTalkText().Talk("Η ομίχλη γίνεται πολύ πυκνή για το μωρό, ίσος να μην πάω πιο βαθιά...");
        }
    }

    [Signal]
    public delegate void DayShift(bool day);
    //[Signal]
    //public delegate void DayEventHandler();

    //[Signal]
    //public delegate void NightEventHandler();

    static int currentDay;

    static int currenthour;
    static float currentmins;

    static float WindDir = 0;
    static float WindStreangth = 100;

    static float Raintreangth = 100;

    DirectionalLight sun;
    Node SunGodRays;
    DirectionalLight moon;
    Node MoonGodRays;
    static int daystage = -1;

    public SunMoonPivot SunMoonMeshPivot;

    //Time_UI UI;

    static public float GetWindDirection()
    {
        return WindDir;
    }
    static public float GetWindStr()
    {
        return WindStreangth;
    }
    static public float GetRainStr()
    {
        return Raintreangth;
    }
    static public void GetTime(out int hour, out int mins)
    {
        hour = currenthour; mins = (int)currentmins;
    }
    static public void GetDay(out int day)
    {
        day = currentDay;
    }
    private void UpdateWind()
    {
        float value = currentDay + ((currenthour  + (currentmins / 60))/ 24);
        while (value > 10)
            value -= 10;
        WindDir = WindDirCurve.Interpolate(value/ 10);
        WindStreangth = WindStreangthCurve.Interpolate(value/ 10);
    }
    private void UpdateRain()
    {
        float value = currentDay + ((currenthour  + (currentmins / 60))/ 24);
        while (value > 10)
            value -= 10;
        Raintreangth = RainStreangthCurve.Interpolate(value/ 10);
        //Raintreangth = 0;
    }
    private void UpdateCurveValues()
    {
        MinuteValue = currentmins/60;

        HourValue = (currenthour + MinuteValue)/24;

        Brighness = brightnesscurve.Interpolate(HourValue);

        AutoExposure = AutoExposureCurve.Interpolate(HourValue);
        SunBrightness = sunbrightnesscurve.Interpolate(HourValue);

        MoonBrightness = moonbrightnesscurve.Interpolate(HourValue);

        Softlight = softlightnesscurve.Interpolate(HourValue);

        if (HourValue > 0.85f)
            SunRot = sunrotcurve.Interpolate(HourValue - 0.85f);
        else
            SunRot = sunrotcurve.Interpolate(HourValue + 0.15f);

        SunColor = SunColorGradient.Interpolate(HourValue);
        //SunColor = new Color(sunRcolorcurve.Interpolate(HourValue) , sunGcolorcurve.Interpolate(HourValue), sunBcolorcurve.Interpolate(HourValue));
        SunGodRayBrightness = SunGodRayCurve.Interpolate(HourValue);
        MoonColor = MoonColorGradient.Interpolate(HourValue);
        //MoonColor = new Color(moonRcolorcurve.Interpolate(HourValue) , moonGcolorcurve.Interpolate(HourValue), moonBcolorcurve.Interpolate(HourValue));
        MoonGodRayBrightness = MoonGodRayCurve.Interpolate(HourValue);


        ThunderMulti = WorldParticleManager.Instance.ThunderLight;
        //ThunderMulti = ThunderCurve.Interpolate(MinuteValue);
        //if (ThunderMulti > 1)
        //{
        //    WorldParticleManager.PlayThunder();
        //}
    }
    private int PoissonRandom(double mean)
    {
        double L = Math.Exp(-mean);
        int k = 0;
        double p = 1.0;

        do
        {
            k++;
            p *= ThunderRand.NextDouble();
        }
        while (p > L);

        return k - 1;
    }
    private void ToggleDay(int Phase)
    {
        if (Phase == 0)
        {
            EmitSignal("DayShift", true);
            sun.Show();
            moon.Hide();
            
            SunMoonMeshPivot.GetNode<MeshInstance>("Sun").Show();
            SunMoonMeshPivot.GetNode<MeshInstance>("Moon").Hide();
        }
        if (Phase == 1)
        {
            EmitSignal("DayShift", false);
            sun.Hide();
            moon.Show();
            moon.ShadowEnabled = true;

            SunMoonMeshPivot.GetNode<MeshInstance>("Sun").Hide();
            SunMoonMeshPivot.GetNode<MeshInstance>("Moon").Show();
        }
        if (Phase == 2)
        {
            sun.Show();
            moon.Show();
            moon.ShadowEnabled = false;

            SunMoonMeshPivot.GetNode<MeshInstance>("Sun").Show();
            SunMoonMeshPivot.GetNode<MeshInstance>("Moon").Show();
        }
        daystage = Phase;
    }
    //private void CalculateDay(out Color FogColor, out Color FogSunColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    //private void CalculateDay(out Color FogColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    private void CalculateDay(out Color AmbientLightColor, out float AmbientLightEnergy)
    {
        if (daystage != 0)
            ToggleDay(0);

        sun.LightEnergy = SunBrightness;

        Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(SunColor, Softlight);

        //FogSunColor = SunColor;

        //FogColor = backgroundcolor;

        AmbientLightColor = backgroundcolor;

        //BackgroundColor = backgroundcolor;

        AmbientLightEnergy = Softlight;

        //Environment.FogSunAmount = 0.3f;
    }
    //private void CalculateNight(out Color FogColor, out Color FogSunColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    //private void CalculateNight(out Color FogColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    private void CalculateNight(out Color AmbientLightColor, out float AmbientLightEnergy)
    {
        if (daystage != 1)
            ToggleDay(1);
        
        moon.LightEnergy = MoonBrightness;

        //FogSunColor = MoonColor;

        Color backgroundcolor = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(MoonColor, Softlight);

        //FogColor = backgroundcolor;

        AmbientLightColor = backgroundcolor;
        
        //BackgroundColor = backgroundcolor;

        AmbientLightEnergy = Softlight;

        //Environment.FogSunAmount = 0.05f;
    }
    //private void CalculateTransition(out Color FogColor, out Color FogSunColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy)
    //private void CalculateTransition(out Color FogColor, out Color AmbientLightColor, out Color BackgroundColor, out float AmbientLightEnergy).
    private void CalculateTransition(out Color AmbientLightColor, out float AmbientLightEnergy)
    {
        //sun.Show();
        //moon.Show();
        if (daystage != 2)
            ToggleDay(2);
        float multi;

        Color combination;

        //float bright;

        //float fogsun;

        float SunLightEnergy;

        float MoonLightEnergy;

        Color mix;

        if (SunRot > 170 && SunRot < 190)
        {
            multi = (float)Math.Round((SunRot - 170) / 20, 4);
            //if (multi > 0.5f)
                //day = true;
            //else
                //day = false;
            
           //bright = Mathf.Lerp(MoonBrightness, SunBrightness, multi);

            //fogsun = Mathf.Lerp(0.05f, 0.3f, multi);
            mix = MoonColor.LinearInterpolate(SunColor , multi);

            SunLightEnergy = Mathf.Lerp(0.0f, SunBrightness, multi);
            
            MoonLightEnergy = Mathf.Lerp(MoonBrightness, 0.0f, multi);

            combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, Softlight);
        }
        else
        {
            if (SunRot > 190)
            {
                float rot = 10 - (360 - SunRot);
                multi = (float)Math.Round(rot / 20, 4);
            }
            else
                multi = (float)Math.Round((SunRot + 10) / 20, 4);
            
            //if (multi > 0.5f)
                //day = false;
            //else
                //day = true;
            //bright = Mathf.Lerp(SunBrightness, MoonBrightness, multi);

            //fogsun = Mathf.Lerp(0.3f, 0.05f, multi);
            
            mix = SunColor.LinearInterpolate(MoonColor , multi);
            
            SunLightEnergy = Mathf.Lerp(SunBrightness, 0.0f, multi);
            
            MoonLightEnergy = Mathf.Lerp(0.0f, MoonBrightness, multi);

            combination = new Color (0.0f, 0.0f,0.0f).LinearInterpolate(mix, Softlight);
        }
        
        sun.LightEnergy = (float)Math.Round(SunLightEnergy, 4);

        moon.LightEnergy = (float)Math.Round(MoonLightEnergy, 4);

        //FogColor = combination;

        AmbientLightColor = combination;
        
        //BackgroundColor = combination;

        AmbientLightEnergy = Softlight;

        //FogSunColor = mix;
    }
    private void UpdateSunMoonPlacament()
    {
        float moonrot;

        if (SunRot < 180)
            moonrot = 180 + SunRot;
        else
            moonrot = -(180 - SunRot);

        if (SunMoonMeshPivot != null)
        {
            SunMoonMeshPivot.RotationDegrees = new Vector3(SunRot, -90, 0);
            SunMoonMeshPivot.GlobalTranslation = new Vector3(SunMoonMeshPivot.GlobalTransform.origin.x, 0, SunMoonMeshPivot.GlobalTransform.origin.z);
        }
        sun.RotationDegrees = new Vector3(SunRot, -90, 0);

        moon.RotationDegrees = new Vector3(moonrot, -90, 0);
    }
    private void UpdateTime(float delta)
    {
        currentmins += delta * timeprogmultiplier;
        
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
    public void UpdateThunder(float deltaTime)
    {
        // Calculate expected strikes for this time step
        double expectedStrikes = deltaTime / 60.0 * lambda;

        // Generate the actual number of strikes using Poisson distribution
        int strikes = PoissonRandom(expectedStrikes);

        for (int i = 0; i < strikes; i++)
        {
            WorldParticleManager.Instance.PlayThunder(); // Handle individual lightning strikes
        }
    }
    //updating values
    float MinuteValue;
    float HourValue;
    float Brighness;
    float AutoExposure;
    float SunBrightness;
    float MoonBrightness;
    float Softlight;
    float SunRot;
    Color SunColor;
    float SunGodRayBrightness;
    Color MoonColor;
    float MoonGodRayBrightness;

    float d = 0.1f;
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    
        d -= delta;

        if (d > 0)
            return;
        d = 0.1f;

        UpdateTime(d);

        UpdateThunder(d);

        UpdateWind();

        UpdateRain();

        UpdateCurveValues();

        Environment.BackgroundEnergy = Brighness;

        Environment.AutoExposureScale = AutoExposure;

        sun.LightColor = SunColor;

        SunGodRays.Set("exposure", SunGodRayBrightness);

        moon.LightColor = MoonColor;

        MoonGodRays.Set("exposure", MoonGodRayBrightness);

        if (SunMoonMeshPivot != null)
        {
            SunMoonMeshPivot.SetSunColor(SunColor);
            SunMoonMeshPivot.SetMoonColor(MoonColor);
        }
        ///
        /////
        //Color FogColor;

        //Color FogSunColor;

        Color AmbientLightColor;

        //Color BackgroundColor;

        float AmbientLightEnergy;
        //////
        ///

        if (SunRot > 190 && SunRot < 350)
        {
            //CalculateDay(out FogColor, out FogSunColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
            //CalculateDay(out FogColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
            CalculateDay(out AmbientLightColor, out AmbientLightEnergy);
        }
        else if (SunRot < 170 && SunRot > 10)
        {
            //CalculateNight(out FogColor, out FogSunColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
            //CalculateNight(out FogColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
            CalculateNight(out AmbientLightColor, out AmbientLightEnergy);
        }
        else
        {
            //CalculateTransition(out FogColor, out FogSunColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
            //CalculateTransition(out FogColor, out AmbientLightColor, out BackgroundColor, out AmbientLightEnergy);
            CalculateTransition(out AmbientLightColor, out AmbientLightEnergy);
        }
       //Environment.FogSunColor = FogSunColor;

       Color fogdistc = new Color(0.1f,0.1f,0.1f).LinearInterpolate(FogDistantColor, AmbientLightEnergy);

        float tmult = Mathf.Clamp(ThunderMulti, 1, 2);
        //Environment.FogColor = FogColor.LinearInterpolate(fogdistc, CurrentDistance);
        Environment.FogColor = new Color(Mathf.Clamp(fogdistc.r * tmult, 0, 1), Mathf.Clamp(fogdistc.g * tmult, 0, 1), Mathf.Clamp(fogdistc.b * tmult, 0, 1));

        Environment.FogDepthEnd = FogDirstanceCurve.Interpolate(CurrentDistance) * 1000;

        Environment.AmbientLightColor = AmbientLightColor;

        Environment.BackgroundColor = fogdistc;

        Environment.BackgroundEnergy = AmbientLightEnergy + ThunderMulti;

        Environment.AmbientLightEnergy = AmbientLightEnergy + (ThunderMulti / 5);

        Environment.FogSunColor = fogdistc;

        //Environment.FogSunAmount = 0.3f;
        UpdateSunMoonPlacament();
    }
    public static bool IsDay()
    {
        return daystage == 0;
    }
    public static void UpdateTimeProgression(int time)
    {
        timeprogmultiplier = time;
    }
    public static void ProgressTime(int days = 0, int hours = 0, int mins = 0)
    {
        CameraAnimationPlayer.GetInstance().FadeInOut(1);
        currentmins += mins;
        while (currentmins > 60)
        {
            currenthour += 1;
            currentmins -= 60;
        }
        currenthour += hours;
        while (currenthour > 23)
        {
            currentDay += 1;
            currenthour -= 24;
        }
        currentDay += days;
    }
    public void SetTime(int Day, int Hours, int Mins)
    {
        currentDay = Day;
        currenthour = Hours;
        currentmins = Mins;
    }
    public static void MinsToTime(int ammout, out int days, out int hours, out int mins)
    {
        days = 0;
        hours = 0;
        mins = 0;
        while (ammout > 1440)
        {
            days += 1;
            ammout -= 1440;
        }
        while (ammout > 60)
        {
            hours += 1;
            ammout -= 60;
        }
        mins = ammout;
    }
    

}

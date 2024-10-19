using Godot;
using System;
using System.Collections.Generic;
public class SettingsPanel : Control
{
    [Export]
	public int ViewDistance = 2;
    
	[Export]
	public int FOVOverride = 30;
    [Export]
    public int TimeProgression { get; private set; }
    [Export]
    public int Seed = 0;
    [Export]
    Godot.Environment Startscreenenv = null;
    [Export]
    Godot.Environment gameenv = null;
    [Signal]
    public delegate void OnSettingsClosed();

    public static SettingsPanel Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;

        UpdateTimeProgression();
        var index = OS.GetDatetime();

        LoadSettingsFromFile();
		
		int thing = (int)index["hour"] + (int)index["minute"] + (int)index["second"];
        Random rand = new Random(thing);
		Seed = rand.Next(0, 99999);
        DViewport v = DViewport.GetInstance();
        GetNode<CheckBox>("Panel/GridContainer/Full_Screen_Check").SetPressedNoSignal(OS.WindowFullscreen);
        GetNode<CheckBox>("Panel/GridContainer/VSync_Check").SetPressedNoSignal(OS.VsyncEnabled);
        GetNode<CheckBox>("Panel/GridContainer/FXAA_Check").SetPressedNoSignal(v.Fxaa);
        GetNode<CheckBox>("Panel/GridContainer/SSAO_Check").SetPressedNoSignal(gameenv.SsaoEnabled);

        GetNode<OptionButton>("Panel/GridContainer/MSAAChoice").Selected = GetMSAAValue(v.Msaa);
        GetNode<OptionButton>("Panel/GridContainer/MaxFPSChoice").Selected = GetFPSValue(Engine.TargetFps);
        GetNode<OptionButton>("Panel/GridContainer/ResolutionChoice").Selected = GetResolutionValue(v.Size);
        
        GetNode<Slider>("Panel/GridContainer/BrightnessSlider").Value = gameenv.AdjustmentBrightness;
        GetNode<Slider>("Panel/GridContainer/ContrastSlider").Value = gameenv.AdjustmentContrast;
        GetNode<Slider>("Panel/GridContainer/SaturationSlider").Value = gameenv.AdjustmentSaturation;
        GetNode<Slider>("Panel/GridContainer/SoundSlider").Value = AudioServer.GetBusVolumeDb(0);
        Visible = false;
    }
    public void SaveSettingsToFile()
    {
        DViewport v = DViewport.GetInstance();

        GDScript SaveGD = GD.Load<GDScript>("res://Scripts/Saved_Settings.gd");
		Godot.Object save = (Godot.Object)SaveGD.New();

		Dictionary<string, object> data = new Dictionary<string, object>(){
			{"FullScreen", OS.WindowFullscreen},
            {"Vsync", OS.VsyncEnabled},
            {"AAFXAA", v.Fxaa},
            {"SSAO", gameenv.SsaoEnabled},
            {"MSAA", GetMSAAValue(v.Msaa)},
            {"MaxFPS", GetFPSValue(Engine.TargetFps)},
            {"Resolution", GetResolutionValue(DViewport.GetInstance().Size)},
            {"Brightness", gameenv.AdjustmentBrightness},
            {"Contrast", gameenv.AdjustmentContrast},
            {"Saturation", gameenv.AdjustmentSaturation},
            {"Sound", AudioServer.GetBusVolumeDb(0)}
		};
        save.Call("_SetData", data);

		//File savef = new File();
		ResourceSaver.Save("user://SavedSettings.tres", (Resource)save);
    }
    public void LoadSettingsFromFile()
    {
        var ResourceLoaderSafe = ResourceLoader.Load("res://Scripts/safe_resource_loader.gd") as Script;
        Resource save = (Resource)ResourceLoaderSafe.Call("load", "user://SavedSettings.tres");
        if (save == null)
        {
            return;
        }
        OS.WindowFullscreen = (bool)save.Get("FullScreen");
        OS.VsyncEnabled = (bool)save.Get("Vsync");

        gameenv.SsaoEnabled = (bool)save.Get("SSAO");

        Update_FXAA((bool)save.Get("AAFXAA"));
        Update_MSAA((int)save.Get("MSAA"));
        UpdateMaxFPS((int)save.Get("MaxFPS"));
        Update_Resolution((int)save.Get("Resolution"));
        Update_Brightness((float)save.Get("Brightness"));
        Update_Contrast((float)save.Get("Contrast"));
        Update_Saturation((float)save.Get("Saturation"));
        Update_Sound((float)save.Get("Sound"));
    }
    private void Update_Vsync(bool T)
    {
        OS.VsyncEnabled = T;
    }
    private void Update_FXAA(bool T)
    {
        DViewport vp = DViewport.GetInstance();
        vp.Fxaa = T;
    }
    private void Update_SSAO(bool T)
    {
        gameenv.SsaoEnabled = T;
        Startscreenenv.SsaoEnabled = T;
    }
    
    private void Update_FullScreen(bool T)
	{
		OS.WindowFullscreen = T;
        if (!T)
        {
            OS.WindowSize = new Vector2(1920, 1080);
            var actual_size = OS.GetRealWindowSize(); 
            var centered = new Vector2(OS.GetScreenSize().x / 2 - actual_size.x / 2, OS.GetScreenSize().y / 2 - actual_size.y / 2);
            OS.WindowPosition = centered;
        }
	}
    private void Update_Brightness(float v)
    {
        gameenv.AdjustmentBrightness = v;
        Startscreenenv.AdjustmentBrightness = v;
    }
    private void Update_Contrast(float v)
    {
        gameenv.AdjustmentContrast = v;
        Startscreenenv.AdjustmentContrast = v;
    }
    private void Update_Saturation(float v)
    {
        gameenv.AdjustmentSaturation = v;
        Startscreenenv.AdjustmentSaturation = v;
    }
    private void Update_Sound(float v)
    {
        AudioServer.SetBusVolumeDb(0, v);
    }
    private void UpdateMaxFPS(int p)
    {
        switch (p)
        {
            case 0:
            {
                Engine.TargetFps = 30;
                break;
            }
            case 1:
            {
                Engine.TargetFps = 60;
                break;
            }
            case 2:
            {
                Engine.TargetFps = 75;
                break;
            }
            case 3:
            {
                Engine.TargetFps = 120;
                break;
            }
        }
    }
    private void Update_InputDevice(int p)
    {
        switch (p)
        {
            case 0:
            {
                ControllerInput.ToggleController(false);
                break;
            }
            case 1:
            {
                if (Input.GetConnectedJoypads().Count == 0)
                {
                    GetNode<OptionButton>("Panel/GridContainer/MSAA_Option2").Selected = 0;
                    break;
                }
                ControllerInput.ToggleController(true);
                break;
            }
        }
    }
    private void Update_MSAA(int p)
    {
        Viewport.MSAA set = Viewport.MSAA.Disabled;
        switch (p)
        {
            case 0:
            {
                set = Viewport.MSAA.Disabled;
                break;
            }
            case 1:
            {
                set = Viewport.MSAA.Msaa2x;
                break;
            }
            case 2:
            {
                set = Viewport.MSAA.Msaa4x;
                break;
            }
            case 3:
            {
                set = Viewport.MSAA.Msaa8x;
                break;
            }
            case 4:
            {
                set = Viewport.MSAA.Msaa16x;
                break;
            }
        }
        var vps = GetTree().GetNodesInGroup("3DVP");
        foreach(Viewport vp in vps)
        {
            vp.Msaa = set;
        }
    }
    private void Update_Resolution(int p)
    {
        Vector2 Size = Vector2.Zero;
        switch (p)
        {
            case 0:
            {
                Size = new Vector2(3840, 2160);
                break;
            }
            case 1:
            {
                Size = new Vector2(1920,1080);
                break;
            }
            case 2:
            {
                Size = new Vector2(1280,720);
                break;
            }
            case 3:
            {
                Size = new Vector2(960,540);
                break;
            }
            case 4:
            {
                Size = new Vector2(480,270);
                break;
            }
        }
        var vps = GetTree().GetNodesInGroup("3DVP");
        foreach(Viewport vp in vps)
        {
            vp.Size = Size;
        }
    }
    
    private void ClearAchievements()
    {
        ActionTracker.ClearActions();
        AchievementManager.Instance.ClearAchievements();
    }
    private void Close()
    {
        SaveSettingsToFile();
        EmitSignal("OnSettingsClosed");
        Visible = false;
    }
    private void UpdateFOV()
	{
		if (!StartingScreen.IsGameRunning())
			return;
		Camera	cam = GetTree().Root.GetCamera();
		if (cam != null)
			cam.Fov = FOVOverride;
	}
	public void IncreaseTimeProgression()
	{
		TimeProgression += 1;
		UpdateTimeProgression();
	}
	public void DecreaseTimeProgression()
	{
		if (TimeProgression == 1)
			return;
		TimeProgression -= 1;
		UpdateTimeProgression();
	}
	private void IncreaseFOV()
	{
		if (FOVOverride == 90)
			return;
		FOVOverride += 1;
		UpdateFOV();
	}
	private void DecreaseFOV()
	{
		if (FOVOverride == 25)
			return;
		FOVOverride -= 1;
		UpdateFOV();
	}
	private void IncreaseViewDistance()
	{
		ViewDistance += 1;
		UpdateViewDistance();
	}
	private void DecreaseViewDistance()
	{
		if (ViewDistance == 2)
			return;
		ViewDistance -= 1;
		UpdateViewDistance();
	}
    private void UpdateViewDistance()
	{
		GetNode<Panel>("ViewDistanceSetting").GetNode<RichTextLabel>("ViewDistanceNumber").BbcodeText = "[center]" + ViewDistance.ToString();
	}
	private void UpdateTimeProgression()
	{
		CustomEnviroment.UpdateTimeProgression(TimeProgression);
		//GetNode<Panel>("TimeMultiplierSetting").GetNode<RichTextLabel>("TimeProgressionNumber").BbcodeText = "[center]" + TimeProgression.ToString();
	}
    private void On_SeedText_changed()
	{
		TextEdit text = GetParent().GetNode<TextEdit>("Panel/VBoxContainer/SeedSetting/HBoxContainer/SeedText");
		string newseedtext = text.Text;

		if (newseedtext == string.Empty)
		{
			text.Text = 0.ToString();
			text.CursorSetColumn(1);
			return;
		}

		if (!newseedtext.IsValidInteger())
		{
			text.Text = Seed.ToString();
			return;
		}
		
		if (newseedtext.Contains("\n"))
		{
			text.Text = newseedtext.Replace("\n", String.Empty);
			text.CursorSetColumn(newseedtext.Length - 1);
		}
			
		if (newseedtext.Contains("0"))
		{
			newseedtext = newseedtext.TrimStart('0');
			if (newseedtext == string.Empty)
			{
				text.Text = 0.ToString();
				text.CursorSetColumn(1);
				return;
			}
			text.Text = newseedtext;
			text.CursorSetColumn(newseedtext.Length);
		}

		int newseed = text.Text.ToInt();
		
		if (newseed > 100000)
		{
			text.Text = Seed.ToString();
			text.CursorSetColumn(Seed.ToString().Length);
			return;
		}
		if (newseed < 0)
		{
			text.Text = 0.ToString();
			text.CursorSetColumn(1);
			return;
		}
		Seed = text.Text.ToInt();
	}
    public int GetMSAAValue(Viewport.MSAA msatype)
    {
        int type = -1;
        switch (msatype)
        {
            case Viewport.MSAA.Disabled:
            {
                type = 0;
                break;
            }
            case Viewport.MSAA.Msaa2x:
            {
                type = 1;
                break;
            }
            case Viewport.MSAA.Msaa4x:
            {
                type = 2;
                break;
            }
            case Viewport.MSAA.Msaa8x:
            {
                type = 3;
                break;
            }
            case Viewport.MSAA.Msaa16x:
            {
                type = 4;
                break;
            }
        }
        return type;
    }
    public int GetFPSValue(int TargetFPS)
    {
        int value = -1;
        switch (TargetFPS)
        {
            case 30:
            {
                value = 0;
                break;
            }
            case 60:
            {
                value = 1;
                break;
            }
            case 75:
            {
                value = 2;
                break;
            }
            case 120:
            {
                value = 3;
                break;
            }
        }
        return value;
    }
    private int GetResolutionValue(Vector2 res)
    {
        int resvalue = -1;
        switch (res.x)
        {
            case 3840:
            {
                resvalue = 0;
                break;
            }
            case 1920:
            {
                resvalue = 1;
                break;
            }
            case 1280:
            {
                resvalue = 2;
                break;
            }
            case 960:
            {
                resvalue = 3;
                break;
            }
            case 480:
            {
                resvalue = 4;
                break;
            }
        }
        return resvalue;
    }
}

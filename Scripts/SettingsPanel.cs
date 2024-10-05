using Godot;
using System;

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
		
		int thing = (int)index["hour"] + (int)index["minute"] + (int)index["second"];
        Random rand = new Random(thing);
		Seed = rand.Next(0, 99999);
        DViewport v = DViewport.GetInstance();
        GetNode<CheckBox>("Panel/GridContainer/Full_Screen_Check").SetPressedNoSignal(OS.WindowFullscreen);
        GetNode<CheckBox>("Panel/GridContainer/VSync_Check").SetPressedNoSignal(OS.VsyncEnabled);
        GetNode<CheckBox>("Panel/GridContainer/FXAA_Check").SetPressedNoSignal(v.Fxaa);
        GetNode<CheckBox>("Panel/GridContainer/SSAO_Check").SetPressedNoSignal(gameenv.SsaoEnabled);
        switch (v.Msaa)
        {
            case Viewport.MSAA.Msaa2x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAAChoice").Selected = 0;
                break;
            }
            case Viewport.MSAA.Msaa4x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAAChoice").Selected = 1;
                break;
            }
            case Viewport.MSAA.Msaa8x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAAChoice").Selected = 2;
                break;
            }
            case Viewport.MSAA.Msaa16x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAAChoice").Selected = 3;;
                break;
            }
        }
        switch (Engine.TargetFps)
        {
            case 30:
            {
                GetNode<OptionButton>("Panel/GridContainer/MaxFPSChoice").Selected = 0;
                break;
            }
            case 60:
            {
                GetNode<OptionButton>("Panel/GridContainer/MaxFPSChoice").Selected = 1;
                break;
            }
            case 75:
            {
                GetNode<OptionButton>("Panel/GridContainer/MaxFPSChoice").Selected = 2;
                break;
            }
            case 120:
            {
                GetNode<OptionButton>("Panel/GridContainer/MaxFPSChoice").Selected = 3;
                break;
            }
        }
        GetNode<Slider>("Panel/GridContainer/BrightnessSlider").Value = gameenv.AdjustmentBrightness;
        GetNode<Slider>("Panel/GridContainer/ContrastSlider").Value = gameenv.AdjustmentContrast;
        GetNode<Slider>("Panel/GridContainer/SaturationSlider").Value = gameenv.AdjustmentSaturation;
        Visible = false;
    }
    private void Update_Vsync(bool T)
    {
        OS.VsyncEnabled = T;
    }
    private void Update_FXAA(bool T)
    {
        DViewport v = DViewport.GetInstance();
        v.Fxaa = T;
        ItemPreviewViewport vp = ItemPreviewViewport.GetInstance();
        v.Fxaa = T;
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
        ItemPreviewViewport vp = ItemPreviewViewport.GetInstance();
        DViewport v = DViewport.GetInstance();
        switch (p)
        {
            case 0:
            {
                v.Msaa = Viewport.MSAA.Disabled;
                vp.Msaa = Viewport.MSAA.Disabled;
                break;
            }
            case 1:
            {
                v.Msaa = Viewport.MSAA.Msaa2x;
                vp.Msaa = Viewport.MSAA.Msaa2x;
                break;
            }
            case 2:
            {
                v.Msaa = Viewport.MSAA.Msaa4x;
                vp.Msaa = Viewport.MSAA.Msaa4x;
                break;
            }
            case 3:
            {
                v.Msaa = Viewport.MSAA.Msaa8x;
                vp.Msaa = Viewport.MSAA.Msaa8x;
                break;
            }
            case 4:
            {
                v.Msaa = Viewport.MSAA.Msaa16x;
                vp.Msaa = Viewport.MSAA.Msaa16x;
                break;
            }
        }
    }
    private void Update_Resolution(int p)
    {
        DViewport v = DViewport.GetInstance();
        ItemPreviewViewport vp = ItemPreviewViewport.GetInstance();
        switch (p)
        {
            case 0:
            {
                v.Size = new Vector2(3840, 2160);
                vp.Size = new Vector2(3840, 2160);
                break;
            }
            case 1:
            {
                v.Size = new Vector2(1920,1080);
                vp.Size = new Vector2(1920,1080);
                break;
            }
            case 2:
            {
                v.Size = new Vector2(1280,720);
                vp.Size = new Vector2(1280,720);
                break;
            }
            case 3:
            {
                v.Size = new Vector2(960,540);
                vp.Size = new Vector2(960,540);
                break;
            }
            case 4:
            {
                v.Size = new Vector2(480,270);
                vp.Size = new Vector2(480,270);
                break;
            }
        }
    }
    private void ClearAchievements()
    {
        ActionTracker.ClearActions();
        AchievementManager.Instance.ClearAchievements();
    }
    private void Close()
    {
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
}

using Godot;
using System;

public class SettingsPanel : Control
{
    [Export]
    Godot.Environment Startscreenenv = null;
    [Export]
    Godot.Environment gameenv = null;
    [Signal]
    public delegate void OnSettingsClosed();
    public override void _Ready()
    {
        DViewport v = DViewport.GetInstance();
        GetNode<CheckBox>("Panel/GridContainer/Full_Screen_Check").SetPressedNoSignal(OS.WindowFullscreen);
        GetNode<CheckBox>("Panel/GridContainer/VSync_Check").SetPressedNoSignal(OS.VsyncEnabled);
        GetNode<CheckBox>("Panel/GridContainer/FXAA_Check").SetPressedNoSignal(v.Fxaa);
        GetNode<CheckBox>("Panel/GridContainer/SSAO_Check").SetPressedNoSignal(gameenv.SsaoEnabled);
        switch (v.Msaa)
        {
            case Viewport.MSAA.Msaa2x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAA_Option").Selected = 0;
                break;
            }
            case Viewport.MSAA.Msaa4x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAA_Option").Selected = 1;
                break;
            }
            case Viewport.MSAA.Msaa8x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAA_Option").Selected = 2;
                break;
            }
            case Viewport.MSAA.Msaa16x:
            {
                GetNode<OptionButton>("Panel/GridContainer/MSAA_Option").Selected = 3;;
                break;
            }
        }
        switch (Engine.TargetFps)
        {
            case 30:
            {
                GetNode<OptionButton>("Panel/GridContainer/FPS_Choice").Selected = 0;
                break;
            }
            case 60:
            {
                GetNode<OptionButton>("Panel/GridContainer/FPS_Choice").Selected = 1;
                break;
            }
            case 75:
            {
                GetNode<OptionButton>("Panel/GridContainer/FPS_Choice").Selected = 2;
                break;
            }
            case 120:
            {
                GetNode<OptionButton>("Panel/GridContainer/FPS_Choice").Selected = 3;
                break;
            }
        }
        GetNode<Slider>("Panel/GridContainer/HSlider").Value = gameenv.AdjustmentBrightness;
        GetNode<Slider>("Panel/GridContainer/HSlider2").Value = gameenv.AdjustmentContrast;
        GetNode<Slider>("Panel/GridContainer/HSlider3").Value = gameenv.AdjustmentSaturation;
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
        ((AchievementManager)PlayerUI.GetInstance().GetUI(PlayerUIType.ACHIEVEMENT)).ClearAchievements();
    }
    private void Close()
    {
        EmitSignal("OnSettingsClosed");
        Visible = false;
    }
}

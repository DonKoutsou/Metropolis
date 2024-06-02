using Godot;
using System;
////////////////////////////////////////////////////////////////////////////////////
/*
 ██████╗██╗  ██╗███████╗ █████╗ ████████╗    ███╗   ███╗███████╗███╗   ██╗██╗   ██╗
██╔════╝██║  ██║██╔════╝██╔══██╗╚══██╔══╝    ████╗ ████║██╔════╝████╗  ██║██║   ██║
██║     ███████║█████╗  ███████║   ██║       ██╔████╔██║█████╗  ██╔██╗ ██║██║   ██║
██║     ██╔══██║██╔══╝  ██╔══██║   ██║       ██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║   ██║
╚██████╗██║  ██║███████╗██║  ██║   ██║       ██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╔╝
 ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝                                                                                    
*/
////////////////////////////////////////////////////////////////////////////////////
public class CheatMenu : Control
{
	Player pl;

	CameraMovePivot CamMove;

	CameraZoomPivot CamZoom;

	bool Showing = false;

	bool HasSpeed = false;

	bool HasInfCam = false;

	bool ShowingFPS = false;

	Label FpsCounter;
	private void On_FastRun_Button_Down()
	{
		if (HasSpeed)
		{
			pl.RunSpeed = 30;
			HasSpeed = false;
		}
		else
		{
			HasSpeed = true;
			pl.RunSpeed = 1000;
		}
		
	}
	private void On_InfCamera_Button_Down()
	{
		if (HasInfCam)
		{
			CamMove.MaxDist = 150.0f;
			CamZoom.MaxDist = 150.0f;
			//CamMove.Frame();
			CamZoom.Frame();
			HasInfCam = false;
		}
		else
		{
			CamMove.MaxDist = 9999999.0f;
			CamZoom.MaxDist = 9999999.0f;
			HasInfCam = true;
		}
		
	}
	private void On_FPS_Button_Down()
	{
		if (ShowingFPS)
		{
			SetProcess(false);
			ShowingFPS = false;
			FpsCounter.Hide();
		}
		else
		{
			SetProcess(true);
			ShowingFPS = true;
			FpsCounter.Show();
		}
	}
	
	
	public void Start()
	{
		Show();
		Showing = true;
		MouseFilter = MouseFilterEnum.Stop;
	}
	public void Stop()
	{
		Hide();
		Showing = false;
		MouseFilter = MouseFilterEnum.Ignore;
	}
	public override void _Ready()
	{
		if (!OS.HasFeature("editor"))
		{
			SetProcessInput(false);
			SetProcess(false);
			return;
		}
		pl = (Player)GetParent().GetParent();
		CamMove = pl.GetNode<CameraMovePivot>("CameraMovePivot");
		CameraPanPivot pan = CamMove.GetNode<CameraPanPivot>("CameraPanPivot");
		CamZoom = pan.GetNode<SpringArm>("SpringArm").GetNode<CameraZoomPivot>("CameraZoomPivot");
		FpsCounter = GetNode<Label>("FPS_Counter");
	}
    public override void _Process(float delta)
    {
        base._Process(delta);
		FpsCounter.Text = Engine.GetFramesPerSecond().ToString();
    }

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("CheatMenu"))
		{
			if (!Showing)
				Start();
			else
				Stop();
		}
	}
}






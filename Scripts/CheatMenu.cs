using Godot;
using System;

public class CheatMenu : Control
{

	Player pl;

	CameraMovePivot CamMove;

	CameraZoomPivot CamZoom;

	bool Showing = false;

	bool HasSpeed = false;

	bool HasInfCam = false;
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
			pl.RunSpeed = 300;
		}
		
	}
	private void On_InfCamera_Button_Down()
	{
		if (HasInfCam)
		{
			CamMove.MaxDist = 150.0f;
			CamZoom.MaxDist = 150.0f;
			CamMove.Frame();
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
		pl = (Player)GetParent().GetParent();
		CamMove = pl.GetNode<CameraMovePivot>("CameraMovePivot");
		CameraPanPivot pan = CamMove.GetNode<CameraPanPivot>("CameraPanPivot");
		CamZoom = pan.GetNode<CameraZoomPivot>("CameraZoomPivot");
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






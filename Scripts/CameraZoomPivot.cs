using Godot;
using System;



public class CameraZoomPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	Camera cam;

    public override void _Ready()
	{
		cam = GetNode<Camera>("Camera");
    }
	public override void _Input(InputEvent @event)
	{
		Vector3 prevpos = Translation;
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (cam.Translation.y > 150)
				return;
			cam.Translation = new Vector3(cam.Translation.x, cam.Translation.y * 1.1f, cam.Translation.z * 1.1f);
		}
		if (@event.IsActionPressed("ZoomIn"))
		{
			if (cam.Translation.y < 10)
				return;
			cam.Translation = new Vector3(cam.Translation.x, cam.Translation.y * 0.90f, cam.Translation.z * 0.90f);
		}
		if (@event.IsActionPressed("FrameCamera"))
		{
			cam.Translation = new Vector3(0, 20, 20);
		}
		if (MyCamera.IsClipping())
			cam.Translation = prevpos;
	}
	
	
	
	
}

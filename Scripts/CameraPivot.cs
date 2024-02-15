using Godot;
using System;

public class CameraPivot : Position3D
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
		if (@event is InputEventMouseMotion)
		{
			if (!Input.IsActionPressed("CamPan"))
				return;
			Vector3 prevrot = GlobalRotation;
			Vector3 rot = new Vector3(GlobalRotation.x - ((InputEventMouseMotion)@event).Relative.y * 0.001f, GlobalRotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.001f, GlobalRotation.z ) ;
			
			GlobalRotation = rot;

			if (cam.GlobalTransform.origin.y < 1)
				GlobalRotation = prevrot;
			//screen
			//Input.WarpMousePosition(new Vector2(0, 0));
			//RotateObjectLocal(Vector3.Up, ((InputEventMouseMotion)@event).Relative.x * 0.001f);
		}
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (cam.Translation.y > 300)
				return;
			cam.Translation = new Vector3(cam.Translation.x, cam.Translation.y * 1.1f, cam.Translation.z * 1.1f);
		}
		if (@event.IsActionPressed("ZoomIn"))
		{
			if (cam.Translation.y < 10)
				return;
			cam.Translation = new Vector3(cam.Translation.x, cam.Translation.y * 0.90f, cam.Translation.z * 0.90f);
		}
	}
}

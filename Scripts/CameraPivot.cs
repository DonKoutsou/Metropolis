using Godot;
using System;

public class CameraPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).
    public override void _Ready()
	{
		//Input.MouseMode = Input.MouseModeEnum.Captured;
        /*var spd = Speed;
        var direction = Vector3.Zero;
		if (Input.IsActionPressed("Move_Right"))
		{
			direction.x += 1;
		}

		if (Input.IsActionPressed("Move_Left"))
		{
			direction.x -= 1;
		}

		if (Input.IsActionPressed("Move_Down"))
		{
			direction.z += 1f;
		}

		if (Input.IsActionPressed("Move_Up"))
		{
            direction.z -= 1f;
		}
        Vector3 pos = GlobalTransform.origin;
        pos = pos + direction;
        GlobalTranslation = pos;*/
    }
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			if (!Input.IsActionPressed("CamPan"))
				return;
			Vector3 rot = new Vector3(GlobalRotation.x, GlobalRotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.001f, GlobalRotation.z ) ;
			GlobalRotation = rot;
			//screen
			//Input.WarpMousePosition(new Vector2(0, 0));
			//RotateObjectLocal(Vector3.Up, ((InputEventMouseMotion)@event).Relative.x * 0.001f);
		}
		if (@event.IsActionPressed("ZoomOut"))
		{
			Camera cam =  GetNode<Camera>("Camera");

			cam.Translation = new Vector3(cam.Translation.x, cam.Translation.y * 1.1f, cam.Translation.z * 1.1f);
			
		}
		if (@event.IsActionPressed("ZoomIn"))
		{
			Camera cam =  GetNode<Camera>("Camera");
			cam.Translation = new Vector3(cam.Translation.x, cam.Translation.y * 0.90f, cam.Translation.z * 0.90f);
		}
	}
}

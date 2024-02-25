using Godot;
using System;

public class CameraPanPivot : Position3D
{
    Camera cam;
    public override void _Ready()
	{
		cam = GetTree().Root.GetCamera();
    }
    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			if (!Input.IsActionPressed("CamPan"))
				return;
			Vector3 prevrot = Rotation;
			Vector3 rot = new Vector3(Rotation.x - ((InputEventMouseMotion)@event).Relative.y * 0.0005f, Rotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.0005f, Rotation.z ) ;
			
			Rotation = rot;

			if (MyCamera.IsClipping())
				Rotation = prevrot;
		}
    }
	public override void _PhysicsProcess(float delta)
	{
		Vector3 prevrot = Rotation;
		Vector3 rot = Rotation;
		if (Input.IsActionPressed("ui_right"))
		{
			rot.y += 1f * 0.01f;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			rot.y -= 1f * 0.01f;
			
		}
        if (Input.IsActionPressed("ui_up"))
		{
			rot.x -= 1 * 0.01f;
		}	
		if (Input.IsActionPressed("ui_down"))
		{
			rot.x += 1 * 0.01f;
        }
		if (prevrot == rot)
			return;

		Rotation = rot;

		if (MyCamera.IsClipping())
			Rotation = prevrot;

	}
	
    
}

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
			Vector3 rot = new Vector3(Rotation.x - ((InputEventMouseMotion)@event).Relative.y * 0.001f, Rotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.001f, Rotation.z ) ;
			
			Rotation = rot;

			if (cam.GlobalTransform.origin.y < 1)
				Rotation = prevrot;
		}
    }
	
    
}

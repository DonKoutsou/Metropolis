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
			Vector3 rot = new Vector3(Rotation.x - ((InputEventMouseMotion)@event).Relative.y * 0.001f, Rotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.001f, Rotation.z );
			if (prevrot == rot)
				return;
			Rotation = rot;
			Vector3 clipdir;
			if (MyCamera.IsClipping(out clipdir))
				Rotation += new Vector3(Mathf.Deg2Rad(clipdir.y),Mathf.Deg2Rad(clipdir.x),0);
			if (cam.GlobalTranslation.y <= 0)
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
		Vector3 clipdir;
		if (MyCamera.IsClipping(out clipdir))
			Rotation += new Vector3(Mathf.Deg2Rad(clipdir.y),Mathf.Deg2Rad(clipdir.x),0);
		if (cam.GlobalTranslation.y <= 0)
			Rotation = prevrot;

	}
	
    
}

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
		InventoryUI inv = InventoryUI.GetInstance();
		if (inv.IsOpen)
			return;
		Vector3 prevrot = Rotation;
		Vector3 rot = Rotation;
		Vector2 mousepos = GetViewport().GetMousePosition();
		Vector2 screensize = GetViewport().Size;
		Vector2 amm = screensize/3;
		if (mousepos.x < amm.x)
		{
			rot.y += 0.00005f * (amm.x - mousepos.x);
		}
		if (mousepos.x > screensize.x - amm.x)
		{
			rot.y -= 0.00005f * (amm.x -(screensize.x - mousepos.x));
		}
        if (mousepos.y > screensize.y - amm.y)
		{
			if (Mathf.Rad2Deg(prevrot.x) > -20)
				rot.x -= 0.00005f * (amm.y -(screensize.y - mousepos.y));
		}	
		if (mousepos.y < amm.y)
		{
			if (Mathf.Rad2Deg(prevrot.x) < 90)
				rot.x += 0.00005f * (amm.y - mousepos.y);
        }
		if (Input.IsActionPressed("ui_right"))
		{
			rot.y += 0.01f;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			rot.y -= 0.01f;
		}
        if (Input.IsActionPressed("ui_up"))
		{
			if (Mathf.Rad2Deg(prevrot.x) > -20)
				rot.x -= 0.01f;
		}	
		if (Input.IsActionPressed("ui_down"))
		{
			if (Mathf.Rad2Deg(prevrot.x) < 90)
				rot.x += 0.01f;
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

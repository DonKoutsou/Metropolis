using Godot;
using System;

public class CameraPanPivot : Position3D
{
    Camera cam;
	CameraMovePivot MoveP;
	CameraZoomPivot zpivot;
	Vector3 offset;
	public Vector3 caminitpos;
	static CameraPanPivot instance;

	public static CameraPanPivot GetInstance()
	{
		return instance;
	}
    public override void _Ready()
	{
		cam = GetTree().Root.GetCamera();
		caminitpos = cam.Translation;
		MoveP = (CameraMovePivot)GetParent();
		zpivot = GetNode<CameraZoomPivot>("CameraZoomPivot");
		instance = this;
    }
    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			if (!Input.IsActionPressed("CamPan"))
				return;
			Vector3 prevrot = Rotation;
			Vector3 rot = new Vector3(Rotation.x - ((InputEventMouseMotion)@event).Relative.y * 0.001f, Rotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.001f, Rotation.z);
			if (prevrot == rot)
				return;
			Rotation = rot;
			Vector3 clipdir;
			if (MyCamera.IsClipping(out clipdir))
				Rotation += new Vector3(-0.05f, 0,0);
			if (cam.GlobalTranslation.y <= 0)
				Rotation = prevrot;
		}
    }
	public override void _Process(float delta)
	{
		InventoryUI inv = InventoryUI.GetInstance();
		if (inv.IsOpen || ActionMenu.IsSelecting())
			return;
		Vector3 prevrot = Rotation;
		Vector3 rot = Rotation;
		Vector2 mousepos = GetViewport().GetMousePosition();
		Vector2 screensize = GetViewport().Size;
		Vector2 amm = screensize/3;
		Vector2 pan = new Vector2();
		if (mousepos.x < amm.x)
		{
			float ammount = amm.x - mousepos.x;
			rot.y += 0.00005f * ammount;
			pan += new Vector2(-ammount * 0.05f, 0);
			//Pan(new Vector2(-ammount * 0.05f, 0));
		}
		if (mousepos.x > screensize.x - amm.x)
		{
			float ammount = amm.x -(screensize.x - mousepos.x);
			rot.y -= 0.00005f * ammount;
			//Pan(new Vector2(ammount * 0.05f, 0));
			pan += new Vector2(ammount * 0.05f, 0);
		}
        if (mousepos.y > screensize.y - amm.y)
		{
			if (Mathf.Rad2Deg(prevrot.x) > -20)
			{
				float ammount = amm.y -(screensize.y - mousepos.y);
				rot.x -= 0.00005f * ammount;
				//Pan(new Vector2(0, -ammount * 0.01f));
				pan += new Vector2(0, -ammount * 0.02f);
			}
		}
		if (mousepos.y < amm.y)
		{
			if (Mathf.Rad2Deg(prevrot.x) < 90)
			{
				float ammount = amm.y - mousepos.y;
				rot.x += 0.00005f * ammount;
				//Pan(new Vector2(0, ammount * 0.01f));
				pan += new Vector2(0, ammount * 0.02f);
			}
        }
		if (Input.IsActionPressed("ui_right"))
		{
			rot.y += 0.01f;
			Pan(new Vector2(1, 0));
			
		}
		if (Input.IsActionPressed("ui_left"))
		{
			rot.y -= 0.01f;
			Pan(new Vector2(-1, 0));
		}
        if (Input.IsActionPressed("ui_up"))
		{
			if (Mathf.Rad2Deg(prevrot.x) > -20)
			{
				rot.x -= 0.01f;
				Pan(new Vector2(0, -1));
			}
		}	
		if (Input.IsActionPressed("ui_down"))
		{
			if (Mathf.Rad2Deg(prevrot.x) < 90)
			{
				rot.x += 0.01f;
				Pan(new Vector2(0, 1));
			}
				
        }
		if (prevrot == rot)
			return;
		Pan(pan);
		Rotation = rot;
		//Vector3 clipdir;
		cam.Translation = caminitpos + offset;
		//if (MyCamera.IsClipping(out clipdir))
			//Rotation += new Vector3(-0.05f, 0,0);
		if (cam.GlobalTranslation.y <= 0)
			Rotation = prevrot;
	}
	private void Pan(Vector2 Pan)
	{
		Vector3 trans = Vector3.Zero;
		trans.x += Pan.x; trans.y += Pan.y;

        offset = new Vector3(Pan.x, Pan.y, 0);
	}
    
}

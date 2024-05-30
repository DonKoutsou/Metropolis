using Godot;
using System;

public class CameraPanPivot : Position3D
{
	//SpringArm
	SpringArm zpivot;
	CameraZoomPivot Zoom;
	Position3D PanXPivot;
	public Vector3 caminitpos;
	static CameraPanPivot instance;

	InventoryUI inv;
	public static CameraPanPivot GetInstance()
	{
		return instance;
	}
    public override void _Ready()
	{
		//store instane
		instance = this;

		zpivot = GetNode<SpringArm>("SpringArm");
		Zoom = zpivot.GetNode<CameraZoomPivot>("CameraZoomPivot");
		PanXPivot = zpivot.GetNode<CameraZoomPivot>("CameraZoomPivot").GetNode<Position3D>("CameraPanXPivot");

		//store initial translation to snap back if needed
		caminitpos = zpivot.Translation;
		
		
		inv = InventoryUI.GetInstance();
    }
    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			if (!Input.IsActionPressed("CamPan"))
				return;
			Vector3 prevrot = Rotation;
			Vector3 rot = new Vector3(Rotation.x - ((InputEventMouseMotion)@event).Relative.y * 0.001f, Rotation.y - ((InputEventMouseMotion)@event).Relative.x * 0.001f, Rotation.z);
			//if transform is same return
			if (prevrot == rot)
				return;

			Rotation = new Vector3(0, rot.y, 0);

		}
    }
	public override void _Process(float delta)
	{
		if (inv.IsOpen)
			return;
		Vector3 prevrot = new Vector3(PanXPivot.Rotation.x ,Rotation.y, 0);
		Vector3 rot = new Vector3(PanXPivot.Rotation.x ,Rotation.y, 0);
		Vector2 mousepos = GetViewport().GetMousePosition();
		Vector2 screensize = GetViewport().Size;
		Vector2 amm = screensize/3;
		Vector2 pan = new Vector2();
		if (mousepos.x < amm.x)
		{
			float ammount = amm.x - mousepos.x;
			rot.y += 0.00005f * ammount;
			pan += new Vector2(-ammount * 0.02f, 0);
			//Pan(new Vector2(-ammount * 0.05f, 0));
		}
		if (mousepos.x > screensize.x - amm.x)
		{
			float ammount = amm.x -(screensize.x - mousepos.x);
			rot.y -= 0.00005f * ammount;
			//Pan(new Vector2(ammount * 0.05f, 0));
			pan += new Vector2(ammount * 0.02f, 0);
		}
        if (mousepos.y > screensize.y - amm.y)
		{
			float ammount = amm.y -(screensize.y - mousepos.y);
			if (Mathf.Rad2Deg(prevrot.x) > -10)
			{
				
				rot.x -= 0.00003f * ammount;
				//Pan(new Vector2(0, -ammount * 0.01f));
				
			}
			pan += new Vector2(0, -ammount * 0.02f);
		}
		if (mousepos.y < amm.y)
		{
			float ammount = amm.y - mousepos.y;
			if (Mathf.Rad2Deg(prevrot.x) < 20)
			{
				
				rot.x += 0.00003f * ammount;
				//Pan(new Vector2(0, ammount * 0.01f));
				
			}
			pan += new Vector2(0, ammount * 0.05f);
        }
		// might put it back
		/*if (Input.IsActionPressed("ui_right"))
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
				
        }*/
		if (prevrot != rot)
		{
			Rotation = new Vector3(0, rot.y, 0);
			PanXPivot.Rotation = new Vector3(rot.x, 0, 0);
		}

		Pan(pan * Zoom.GetZoomNormalised());
		
	}
	private void Pan(Vector2 Pan)
	{
        Vector3 offset = new Vector3(Pan.x, Pan.y, 0);
		zpivot.Translation = caminitpos + offset;
	}
    
}

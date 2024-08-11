using Godot;
using System;

public class CameraPanPivot : Position3D
{
	//SpringArm
	SpringArm SpringArm;
	CameraZoomPivot ZoomPivot;
	Position3D PanXPivot;
	public Vector3 CameraInitialPosition;
	static CameraPanPivot instance;

	public static CameraPanPivot GetInstance()
	{
		return instance;
	}
    public override void _Ready()
	{
		//store instane
		instance = this;

		SpringArm = GetNode<SpringArm>("SpringArm");
		ZoomPivot = SpringArm.GetNode<CameraZoomPivot>("CameraZoomPivot");
		PanXPivot = ZoomPivot.GetNode<Position3D>("CameraPanXPivot");

		//store initial translation to snap back if needed
		CameraInitialPosition = SpringArm.Rotation;
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
	float YOffset = 0;

	float d = 0.01f;
	public override void _PhysicsProcess(float delta)
	{
		d -= delta;
		if (d > 0)
			return;
		d = 0.01f;
		Vector3 prevrot = new Vector3(PanXPivot.Rotation.x ,Rotation.y, 0);
		Vector3 rot = new Vector3(PanXPivot.Rotation.x ,Rotation.y, 0);
		
		//Vector2 pan = new Vector2();
		if (!PlayerUI.GetInstance().HasMenuOpen())
		{
			float mult = OS.WindowSize.x / DViewport.GetInstance().Size.x;
			Vector2 mousepos = DViewport.GetInstance().GetMousePosition() / mult;
			Vector2 screensize = DViewport.GetInstance().Size;
			Vector2 ammx = screensize/3;
			Vector2 ammy = screensize/6;
			if (mousepos.x < ammx.x)
			{
				float ammount = ammx.x - mousepos.x;
				rot.y += 0.00008f * ammount;
			}
			if (mousepos.x > screensize.x - ammx.x)
			{
				float ammount = ammx.x -(screensize.x - mousepos.x);
				rot.y -= 0.00008f * ammount;
			}
			//Down
			if (mousepos.y > screensize.y - ammy.y)
			{
				float ammount = ammy.y -(screensize.y - mousepos.y);
				//limmit for down
				if (Mathf.Rad2Deg(prevrot.x) > -20)
				{
					rot.x -= 0.00004f * ammount;
					YOffset += ammount * 0.05f;
				}
			}
			//UP
			if (mousepos.y < ammy.y)
			{
				float ammount = ammy.y - mousepos.y;
				//limmit for Up
				if (Mathf.Rad2Deg(prevrot.x) < 20)
				{
					rot.x += 0.00004f * ammount;
					YOffset -= ammount * 0.05f;
				}
			}
		}
		if (prevrot != rot)
		{
			Rotation = new Vector3(0, rot.y, 0);
			PanXPivot.Rotation = new Vector3(rot.x, 0, 0);
		}

		Vector3 sptrans = CameraInitialPosition;
		//sptrans.x -= Mathf.Deg2Rad(YOffset * ZoomPivot.GetZoomNormalised());
		sptrans.x -= Mathf.Deg2Rad(YOffset * ZoomPivot.GetZoomNormalised());
		SpringArm.Rotation = sptrans;
		
	}
    
}

using Godot;
using System;



public class CameraZoomPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public float MaxDist = 600;

	SpringArm arm;

	//Camera cam;
	//CameraPanPivot panp;
    public override void _Ready()
	{
		arm = (SpringArm)GetParent();
		//cam = GetNode<Camera>("Camera");
		//panp = (CameraPanPivot)GetParent();
    }
	public override void _Input(InputEvent @event)
	{
		if (MapGrid.GetInstance().IsMouseInMap())
			return;
		Vector3 prevpos = arm.Translation;
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (arm.Translation.y < MaxDist)
			{
				arm.SpringLength = arm.SpringLength * 1.1f;
				arm.Translation = new Vector3(prevpos.x, prevpos.y * 1.1f, prevpos.z);
			}
				
		}
		if (@event.IsActionPressed("ZoomIn"))
		{
			if (arm.Translation.y > 10)
			{
				arm.SpringLength = arm.SpringLength * 0.90f;
				arm.Translation = new Vector3(prevpos.x, prevpos.y * 0.90f, prevpos.z);
			}
				
		}
		//if (@event.IsActionPressed("FrameCamera"))
		//{
		//	Frame();
		//}
		//if (prevpos == Translation)
			//return;
		//Vector3 clipdir;
		//if (MyCamera.IsClipping(out clipdir) || GlobalTranslation.y <= 0)
			//Translation = prevpos;
		//panp.caminitpos = new Vector3(0, Translation.y, Translation.z);
	}
	public void Frame()
	{
		Translation = new Vector3(0, 25, 20);
	}
	
	
	
	
}

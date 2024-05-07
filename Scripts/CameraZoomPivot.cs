using Godot;
using System;



public class CameraZoomPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public float MaxDist = 600;

	SpringArm arm;

	Vector2 InitialTransforms;

	//Camera cam;
	CameraPanPivot panp;
    public override void _Ready()
	{
		arm = (SpringArm)GetParent();
		panp = (CameraPanPivot)GetParent().GetParent();
		InitialTransforms = new Vector2(arm.SpringLength, panp.Translation.y);
		//cam = GetNode<Camera>("Camera");
		
    }
	public override void _Input(InputEvent @event)
	{
		if (MapGrid.GetInstance().IsMouseInMap())
			return;
		Vector3 prevpos = panp.Translation;
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (panp.Translation.y < MaxDist)
			{
				PlayerCamera.GetInstance().Fov *= 1.02f;
				arm.SpringLength = arm.SpringLength * 1.1f;
				panp.Translation = new Vector3(prevpos.x, prevpos.y * 1.1f, prevpos.z);
			}
		}
		if (@event.IsActionPressed("ZoomIn"))
		{
			if (panp.Translation.y > 10)
			{
				PlayerCamera.GetInstance().Fov *= 0.98f;
				arm.SpringLength = arm.SpringLength * 0.90f;
				panp.Translation = new Vector3(prevpos.x, prevpos.y * 0.90f, prevpos.z);
			}
		}
		if (@event.IsActionPressed("FrameCamera"))
		{
			Frame();
		}
		//if (prevpos == Translation)
			//return;
		//Vector3 clipdir;
		//if (MyCamera.IsClipping(out clipdir) || GlobalTranslation.y <= 0)
			//Translation = prevpos;
		//panp.caminitpos = new Vector3(0, Translation.y, Translation.z);
	}
	public void Frame()
	{
		arm.SpringLength = InitialTransforms.x;
		panp.Translation = new Vector3(panp.Translation.x, InitialTransforms.y, panp.Translation.z);
		//arm.Translation = new Vector3(arm.Translation.x, InitialTransforms.y, arm.Translation.z);
	}
	
	
	
	
}

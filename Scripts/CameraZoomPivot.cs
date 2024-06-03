using Godot;
using System;



public class CameraZoomPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public float MaxDist = 600;
	[Export]
	public float MaxFov = 75;
	[Export]
	public float MinFov = 25;

	SpringArm arm;

	Vector2 InitialTransforms;

	//Camera cam;
	CameraPanPivot panp;
	
	static CameraZoomPivot instance;
    public override void _Ready()
	{
		arm = (SpringArm)GetParent();
		panp = (CameraPanPivot)GetParent().GetParent();
		InitialTransforms = new Vector2(arm.SpringLength, panp.Translation.y);
		instance = this;
		//cam = GetNode<Camera>("Camera");
		
    }
	public static CameraZoomPivot GetInstance()
	{
		return instance;
	}
	public float GetZoomNormalised()
	{
		return panp.Translation.y / MaxDist;
	}
	float ZoomStage = 1;
	public override void _Input(InputEvent @event)
	{
		if (MapGrid.GetInstance().IsMouseInMap())
			return;
		Vector3 prevpos = panp.Translation;
		if (@event.IsActionPressed("ZoomOut") && ZoomStage < 19)
		{
			ZoomStage += 1;
			
			PlayerCamera cam = PlayerCamera.GetInstance();
			float fovvalue = ZoomStage/ 20;
			cam.Fov = Mathf.Lerp(25, 75, fovvalue);
			float value = Mathf.Lerp(5, 220, fovvalue);
			arm.SpringLength = value;
			panp.Translation = new Vector3(prevpos.x, value, prevpos.z);
		}
		if (@event.IsActionPressed("ZoomIn") && ZoomStage > 0)
		{
			ZoomStage -= 1;

			PlayerCamera cam = PlayerCamera.GetInstance();

			float fovvalue = ZoomStage/ 20;
			cam.Fov = Mathf.Lerp(25, 75, fovvalue);
			float value = Mathf.Lerp(5, 220, fovvalue);
			arm.SpringLength = value;
			panp.Translation = new Vector3(prevpos.x, value, prevpos.z);
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

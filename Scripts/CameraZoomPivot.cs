using Godot;
using System;



public class CameraZoomPivot : Position3D
{
	[Export]
	public float MaxDist = 460;
	
	[Export]
	int zoomSteps = 39;
	SpringArm arm;

	//Vector2 InitialTransforms;

	//Camera cam;
	CameraPanPivot panp;
	
	static CameraZoomPivot instance;
    public override void _Ready()
	{
		arm = (SpringArm)GetParent();
		panp = (CameraPanPivot)GetParent().GetParent();
		//InitialTransforms = new Vector2(arm.SpringLength, panp.Translation.y);
		instance = this;
		//cam = GetNode<Camera>("Camera");
		SetPhysicsProcess(false);
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
		if (PlayerUI.HasMenuOpen())
			return;
		if (@event.IsActionPressed("ZoomOut") && ZoomStage < zoomSteps)
		{
			ZoomStage += 1;
			UpdatePos();
		}
		if (@event.IsActionPressed("ZoomIn") && ZoomStage > 1)
		{
			ZoomStage -= 1;
			UpdatePos();
		}
		if (@event.IsActionPressed("ToggleZoom"))
		{
			SetPhysicsProcess(true);
		}
		else if (@event.IsActionReleased("ToggleZoom"))
		{
			SetPhysicsProcess(false);
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
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
		if (Input.IsActionPressed("CameraUp") && ZoomStage > 1)
		{
			ZoomStage -= 1;
			UpdatePos();
		}
		if (Input.IsActionPressed("CameraDown") && ZoomStage < zoomSteps)
		{
			ZoomStage += 1;
			UpdatePos();
		}
    }
    public void UpdatePos()
	{
		//Vector3 prevpos = panp.Translation;
		//PlayerCamera cam = PlayerCamera.GetInstance();

		float fovvalue = ZoomStage/ (zoomSteps + 1);
		//cam.Fov = Mathf.Lerp(25, 65, fovvalue);
		float value = Mathf.Lerp(4, MaxDist, fovvalue);
		arm.SpringLength = value;
		//panp.Translation = new Vector3(prevpos.x, value / 4, prevpos.z);
	}
	public void Frame()
	{
		ZoomStage = 2;
		UpdatePos();
		//arm.SpringLength = InitialTransforms.x;
		//panp.Translation = new Vector3(panp.Translation.x, InitialTransforms.y, panp.Translation.z);
		//arm.Translation = new Vector3(arm.Translation.x, InitialTransforms.y, arm.Translation.z);
	}
	
	
	
	
}

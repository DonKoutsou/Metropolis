using Godot;
using System;

public class CameraMovePivot : Position3D
{
    CameraZoomPivot zpivot;

    Camera cam;

    static CameraMovePivot instance;

    public bool shouldfollowpl = true;

    Player pl;

	[Export]
	public float MaxDist = 150;

	public Vector3 offset = new Vector3();
    public override void _Ready()
    {
        base._Ready();
        zpivot = GetNode<CameraPanPivot>("CameraPanPivot").GetNode<SpringArm>("SpringArm").GetNode<CameraZoomPivot>("CameraZoomPivot");
        cam = GetTree().Root.GetCamera();
		pl = (Player)GetParent();
        instance = this;
    }
	
    //public override void _PhysicsProcess(float delta)
	//{
        /*if (shouldfollowpl)
        {
            Translation = Vector3.Zero;
        }
        Vector3 prev = zpivot.Translation;
		//Vector3 myprev = GlobalTranslation;
		Vector3 trans = zpivot.Translation;
		if (Input.IsActionPressed("Move_Right"))
		{
			trans.x += 1;
		}
		if (Input.IsActionPressed("Move_Left"))
		{
			trans.x -= 1;
		}

		if (Input.IsActionPressed("Move_Back"))
		{
			trans.z += 1f;
		}

		if (Input.IsActionPressed("Move_Forward"))
		{
			trans.z -= 1f;
        }
        if (Input.IsActionPressed("Move_Up"))
		{
			trans.y += 1f;
		}

		if (Input.IsActionPressed("Move_Down"))
		{
			trans.y -= 1f;
        }
		if (trans == prev)
			return;
        zpivot.Translation = trans;
		Vector3 clipdir;
        if (pl.GlobalTranslation.DistanceTo(zpivot.GlobalTranslation) < MaxDist && !MyCamera.IsClipping(out clipdir) && zpivot.GlobalTranslation.y > 0)
        {
            GlobalTranslation = zpivot.GlobalTranslation;
        }
		//Rotation = new Vector3(0.0f,0.0f,0.0f);
        zpivot.Translation = prev;*/
	//}
    static public CameraMovePivot GetInstance()
    {
        return instance;
    }
    /*public override void _Input(InputEvent @event)
	{
        if (@event.IsActionPressed("Move_Right") || @event.IsActionPressed("Move_Left") || @event.IsActionPressed("Move_Down") || @event.IsActionPressed("Move_Up"))
		{
			shouldfollowpl = false;
		}
		if (@event.IsActionPressed("FrameCamera"))
		{
			Frame();
		}
	}
	public void Frame()
	{
		shouldfollowpl = true;
	}*/
}

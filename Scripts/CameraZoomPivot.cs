using Godot;
using System;



public class CameraZoomPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public float MaxDist = 600;

	Camera cam;
	CameraPanPivot panp;
    public override void _Ready()
	{
		cam = GetNode<Camera>("Camera");
		panp = (CameraPanPivot)GetParent();
    }
	public override void _Input(InputEvent @event)
	{
		Vector3 prevpos = Translation;
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (Translation.y < MaxDist)
				Translation = new Vector3(prevpos.x, prevpos.y * 1.1f, prevpos.z * 1.1f);
		}
		if (@event.IsActionPressed("ZoomIn"))
		{
			if (Translation.y > 2)
				Translation = new Vector3(prevpos.x, prevpos.y * 0.90f, prevpos.z * 0.90f);
		}
		if (@event.IsActionPressed("FrameCamera"))
		{
			Frame();
		}
		if (prevpos == Translation)
			return;
		Vector3 clipdir;
		if (MyCamera.IsClipping(out clipdir) || GlobalTranslation.y <= 0)
			Translation = prevpos;
		//panp.caminitpos = new Vector3(0, Translation.y, Translation.z);
	}
	public void Frame()
	{
		Translation = new Vector3(0, 25, 20);
	}
	
	
	
	
}

using Godot;
using System;

public class CameraPivot : Position3D
{
    [Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).
    public override void _Process(float delta)
	{
        /*var spd = Speed;
        var direction = Vector3.Zero;
		if (Input.IsActionPressed("Move_Right"))
		{
			direction.x += 1;
		}

		if (Input.IsActionPressed("Move_Left"))
		{
			direction.x -= 1;
		}

		if (Input.IsActionPressed("Move_Down"))
		{
			direction.z += 1f;
		}

		if (Input.IsActionPressed("Move_Up"))
		{
            direction.z -= 1f;
		}
        Vector3 pos = GlobalTransform.origin;
        pos = pos + direction;
        GlobalTranslation = pos;*/
    }
}

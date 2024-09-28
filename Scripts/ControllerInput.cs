using Godot;
using System;

public class ControllerInput : Control
{
    static bool UsingController = false;

    public static bool IsUsingController()
    {
        return UsingController;
    }
    public override void _Ready()
    {
        
    }
    /*public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        Vector2 velocity = new Vector2(
        Input.GetActionStrength("Move_Right") - Input.GetActionStrength("Move_Left"),
        Input.GetActionStrength("Move_Down") - Input.GetActionStrength("Move_Up")
        ).LimitLength(2);

        velocity = new Vector2((float)Math.Round((double)velocity.x * 10, 3), (float)Math.Round((double)velocity.y * 10, 3));

        GetViewport().WarpMouse(GetGlobalMousePosition() + velocity);

    }*/
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
		{
			UsingController = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		if (@event is InputEventJoypadMotion)
		{
			UsingController = true;
            Input.MouseMode = Input.MouseModeEnum.Hidden;
		}
    }
}

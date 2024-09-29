using Godot;
using System;

public class ControllerInput : Control
{
    [Signal]
    public delegate void OnControllerSwitched(bool Toggle);
    static bool UsingController = false;

    public static bool IsUsingController()
    {
        return UsingController;
    }
    public static void ToggleController(bool Toggle)
    {
        UsingController = Toggle;
        if (Toggle)
        {
            Input.MouseMode = Input.MouseModeEnum.Hidden;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }
    public static ControllerInput GetInstance()
    {
        return Instance;
    }
    static ControllerInput Instance;
    public override void _Ready()
    {
        Instance = this;
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
        if (@event is InputEventMouseMotion && UsingController)
		{
            EmitSignal("OnControllerSwitched", false);
            UsingController = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		if (@event is InputEventJoypadMotion && !UsingController)
		{
            EmitSignal("OnControllerSwitched", true);
			UsingController = true;
            Input.MouseMode = Input.MouseModeEnum.Hidden;
		}
    }
    private void MouseActionConfirmed()
    {
        UsingController = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
    private void ControllerActionConfirmed()
    {
        UsingController = true;
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }
}

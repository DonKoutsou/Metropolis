using Godot;
using System;

public class Control_UI : Control
{
    Timer IdleTime;
    public override void _Ready()
    {
        IdleTime = new Timer(){
            WaitTime = 2,
            OneShot = true
        };
        AddChild(IdleTime);
        IdleTime.Connect("timeout", this, "TimerEnded");
        Visible = false;
        SetProcessInput(false);
    }
    public void PlayerToggle(Player pl)
    {
		bool toggle = pl != null;
        Visible = toggle;
		SetProcessInput(toggle);
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseMotion || Input.IsActionPressed("CamPan") || Input.IsActionPressed("ZoomOut") || Input.IsActionPressed("ZoomIn"))
		{
            return;
        }
        ToggleUI(true);
        IdleTime.Start();
    }
    private void TimerEnded()
    {
        ToggleUI();
    }
    public void ToggleUI(bool t = false)
    {
        int alpha = 0;
        if (t)
            alpha = 1;

        var tw = CreateTween();
        tw.TweenProperty(this, "modulate", new Color(1,1,1,alpha), 1);
    }
}

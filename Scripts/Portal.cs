using Godot;
using System;

public class Portal : Area
{
    [Export]
    bool Active = false;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        Toggle(DayNight.IsDay());
        DayNight.GetInstance().Connect("DayShift", this, "Toggle");
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        DayNight.GetInstance().Disconnect("DayShift", this, "Toggle");
    }
    public override void _Ready()
    {
        
    }
    private void BoatEntered(Node body)
    {
        if (!Active)
            return;
        Vehicle v = (Vehicle)body;
        v.Boost(2);
        GD.Print("Boat found!!");
    }
    public void Toggle(bool t)
    {
        Active = t;
        if (t)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("EmmisivePulse");
        }
        else
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
        }
        
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

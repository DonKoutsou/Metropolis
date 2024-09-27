using Godot;
using System;

public class Portal : Area
{
    [Export]
    bool Active = false;

    bool InView = false;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        Toggle(CustomEnviroment.IsDay());
        Sky.GetEnviroment().Connect("DayShift", this, "Toggle");
        
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        Sky.GetEnviroment().Disconnect("DayShift", this, "Toggle");
    }
    public override void _Ready()
    {
        
    }
    private void BoatEntered(Node body)
    {
        if (!Active)
            return;
        
        ActionTracker.OnActionDone("Portal");

        Vehicle v = (Vehicle)body;
        v.Boost(2);
    }
    public void Toggle(bool t)
    {
        Active = t;
        if (t)
        {
            Active = true;
            if (InView)
            {
                GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
            }
            GetNode<AnimationPlayer>("AnimationPlayer").Play("EmmisivePulse");
        }
        else
        {
            Active = false;
            GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
        }
        
    }
    private void VizOff(Camera cam)
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
        InView = false;
    }
    private void VizOn(Camera cam)
    {
        if (Active)
            GetNode<AnimationPlayer>("AnimationPlayer").Play("EmmisivePulse");
        InView = true;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

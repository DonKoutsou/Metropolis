using Godot;
using System;

public class VehicleBoostTrails : Spatial
{
    public void StartBoost()
    {
        GetNode<Spatial>("MotionTrail").Set("lifespan", 0.5f);
        GetNode<Spatial>("MotionTrail2").Set("lifespan", 0.5f);
        var tw1 = CreateTween();
        tw1.TweenProperty(GetNode<Spatial>("MotionTrail"), "lifespan", 0, 40);
        var tw2 = CreateTween();
        tw2.TweenProperty(GetNode<Spatial>("MotionTrail2"), "lifespan", 0, 40);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

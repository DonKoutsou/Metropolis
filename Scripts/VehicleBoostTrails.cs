using Godot;
using System;

public class VehicleBoostTrails : Spatial
{
    SceneTreeTween t1;
    SceneTreeTween t2;
    public void StartBoost()
    {
        if (t1 != null)
            t1.Kill();

        if (t2 != null)
            t2.Kill();

        GetNode<Spatial>("MotionTrail").Set("lifespan", 0.5f);
        GetNode<Spatial>("MotionTrail2").Set("lifespan", 0.5f);
        t1 = CreateTween();
        t1.TweenProperty(GetNode<Spatial>("MotionTrail"), "lifespan", 0, 40);
        t2 = CreateTween();
        t2.TweenProperty(GetNode<Spatial>("MotionTrail2"), "lifespan", 0, 40);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

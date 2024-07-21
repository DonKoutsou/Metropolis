using Godot;
using System;

public class VehicleBoostTrails : Spatial
{
    public void StartBoost()
    {
        GetNode<AnimationPlayer>("VehicleBoostAnimation").Play("StartTrail");
    }
    public override void _Ready()
    {
        GetNode<Spatial>("MotionTrail").Hide();
        GetNode<Spatial>("MotionTrail2").Hide();
    }
    private void BoostAnimationFinished(string Anim)
    {
        GetNode<AnimationPlayer>("VehicleBoostAnimation").Play("StopTrail");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

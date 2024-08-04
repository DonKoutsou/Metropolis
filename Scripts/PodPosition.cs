using Godot;
using System;

public class PodPosition : Position3D
{
    static PodPosition instance;
    public override void _Ready()
    {
        instance = this;
    }
    public static PodPosition GetInstance()
    {
        return instance;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

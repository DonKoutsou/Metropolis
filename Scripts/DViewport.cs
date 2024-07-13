using Godot;
using System;
using System.Dynamic;

public class DViewport : Viewport
{
    static DViewport instance;
    public override void _Ready()
    {
        instance = this;
    }
    public static DViewport GetInstance()
    {
        return instance;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

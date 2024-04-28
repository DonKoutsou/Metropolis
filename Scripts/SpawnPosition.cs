using Godot;
using System;

public class SpawnPosition : Position3D
{
    static SpawnPosition instance;
    public override void _Ready()
    {
        instance = this;
    }
    public static SpawnPosition GetInstance()
    {
        return instance;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

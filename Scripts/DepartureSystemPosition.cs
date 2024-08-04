using Godot;
using System;

public class DepartureSystemPosition : Position3D
{
    static DepartureSystemPosition instance;
    public override void _Ready()
    {
        instance = this;
    }
    public static DepartureSystemPosition GetInstance()
    {
        return instance;
    }
    [Export]
    PackedScene DepartureSystemScene = null;

    public void SpawnSystem()
    {
        AddChild(DepartureSystemScene.Instance());
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

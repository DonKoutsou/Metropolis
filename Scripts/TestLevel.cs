using Godot;
using System;

public class TestLevel : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //PlayerUI.OnPlayerSpawned(GetNode<Player>("Control/ViewportContainer/Viewport/Player"));

        UniversalLodManager.GetInstance().UpdateCamera(PlayerCamera.GetInstance());
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

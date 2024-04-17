using Godot;
using System;

public class MapUI : Control
{
    static bool IsMouseInMapBool = false;
    public static bool IsMouseInMap()
    {
        return IsMouseInMapBool;
    }
    private void OnMouseEntered()
    {
        IsMouseInMapBool = true;
    }
    private void OnMouseLeft()
    {
        IsMouseInMapBool = false;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

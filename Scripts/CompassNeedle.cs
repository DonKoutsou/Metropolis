using Godot;
using System;

public class CompassNeedle : Control
{
    CameraPanPivot camerapivot;
    public override void _Ready()
    {
        camerapivot = CameraPanPivot.GetInstance();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.

}

using Godot;
using System;
using System.Collections.Generic;

public partial class WindowCentering : Node
{
    public override void _EnterTree()
    {
        OS.WindowSize = new Vector2(1920, 1080);
        var actual_size = OS.GetRealWindowSize(); 
        var centered = new Vector2(OS.GetScreenSize().x / 2 - actual_size.x / 2, OS.GetScreenSize().y / 2 - actual_size.y / 2);
        OS.WindowPosition = centered;
    }
}
using Godot;
using System;
using System.Data;

public class Camera_FloatArea : Area
{
    static bool ishittinggroung = false;
    public static bool IsClipping()
    {
        
        return ishittinggroung;
    }
    
}

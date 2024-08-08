using Godot;
using System;
using System.Collections.Generic;

public class DoorPivot : Spatial
{
    [Export]
    List<NodePath> Doors = null;
    [Export]
    List<Vector3> Rotations = null;
    [Export]
    List<Vector3> Translations = null;
    public void Open()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            if (Rotations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "rotation_degrees", Rotations[i], 0.5f);
            }
            if (Translations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "translation", Translations[i], 0.5f);
            }

        }
    }
    public void Close()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            if (Rotations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "rotation_degrees", Vector3.Zero, 0.5f);
            }
            if (Translations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "translation", Vector3.Zero, 0.5f);
            }
        }
    }
}

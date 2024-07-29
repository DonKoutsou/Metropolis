using Godot;
using System;
using System.Linq;

[Tool]
public class CharacterSpawnLocations : Node
{
    [Export]
    Vector3[] Locations = new Vector3[0];
    [Export]
    Vector3[] Rotations = new Vector3[0];
    
    public bool HasChars()
    {
        return Locations != null && Locations.Count() > 0;
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Engine.EditorHint)
        {
            int dif = Locations.Count() - GetChildren().Count;
            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    Position3D p = new Position3D()
                    {
                        Name = "Position" + i.ToString()
                    };
                    p.Scale *= 20;
                    AddChild(p, true);
                    p.Owner = this;
                }
            }
            else if (dif < 0)
            {
                var children = GetChildren();
                for (int i = 0; i < -dif; i++)
                {
                    ((Node)children[i]).QueueFree();
                }
            }
            else if (Locations != null)
            {
                for (int i = 0; i < Locations.Count(); i++)
                {
                    ((Spatial)GetChild(i)).Translation = Locations[i];
                    if (i > Rotations.Count())
                    {
                        Rotations[i] = new Vector3();
                    }
                    ((Spatial)GetChild(i)).Rotation = Rotations[i];
                }
            }
        }
        
    }

}

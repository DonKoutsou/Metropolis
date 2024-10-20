using Godot;
using System;
using System.Linq;

[Tool]
public class VehicleSpawnLocation : Node
{
    [Export]
    public Vector3[] Locations = new Vector3[0];
    [Export]
    public Vector3[] Rotations = new Vector3[0];

    public bool HasVehicles()
    {
        return Locations != null && Locations.Count() > 0;
    }
    #if TOOLS
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
    #endif
}

using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class Port : Area
{
    [Export]
    List<Vector3> RestLocations = null;

    List <Position3D> Locations = new List<Position3D>();

    List <Spatial> Boats = new List<Spatial>();
    public override void _Ready()
    {
        
    }
    private void OnShipEntered(Node body)
    {
        Boats.Add((Spatial)body);
    }
    private void OnShipLeft(Node body)
    {
        Boats.Remove((Spatial)body);
    }
    public bool HasSpot()
    {
        bool hasspot = true;

        if (Boats.Count > 0)
        {
            List<Vector3> Rests = new List<Vector3>();
            foreach (Position3D loc in Locations)
            {
                Rests.Add(loc.GlobalTranslation);
            }
            
        }

        return hasspot;
    }
    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            if (Locations.Count > RestLocations.Count)
            {
                for (int i = 0; i <Locations.Count; i++)
                {
                    Position3D p = Locations[i];
                    p.QueueFree();
                }
                Locations.Clear();
            }
            for (int i = 0; i < RestLocations.Count; i++)
            {
                if (Locations.Count == i)
                {
                    Position3D p = new Position3D(){
                        Name = "Position" + i.ToString()
                    };
                    Locations.Add(p);
                    AddChild(p, true);
                    p.Owner = this;
                }
                Locations[i].Translation = RestLocations[i];

            }
        }
    }
}

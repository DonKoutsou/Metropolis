using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class Port : Area
{
    [Export]
    List<Vector3> RestLocations = null;

    [Export]
    Vector3 PortExtents = new Vector3(1,1,1);

    List <Position3D> Locations = new List<Position3D>();

    List <Spatial> Boats = new List<Spatial>();
    public override void _Ready()
    {
        if (!Engine.EditorHint)
        {
            for (int i = 0; i < RestLocations.Count; i++)
            {
                if (Locations.Count == i)
                {
                    Position3D p = new Position3D(){
                        Name = "Position" + i.ToString()
                    };
                    p.Scale *= 20;
                    Locations.Add(p);
                    AddChild(p, true);
                    //p.Owner = this;
                }
                Locations[i].GlobalTranslation = RestLocations[i];

            }
            ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents = PortExtents;
            GetNode<MeshInstance>("MeshInstance").QueueFree();

            Node par = GetParent();
            while (!(par is Island))
            {
                if (par == null)
                    return;
                par = par.GetParent();
            }
            Island ile = (Island)par;
            ile.RegisterChild(this);
        }
    }
    public void ClearPosition()
    {
        RestLocations.Clear();

    }
    public void AddPosition(Vector3 pos)
    {
        RestLocations.Add(pos);
    }
    private void OnShipEntered(Node body)
    {
        Boats.Add((Spatial)body);
    }
    private void OnShipLeft(Node body)
    {
        Boats.Remove((Spatial)body);
    }
    public bool IsInPort(Vehicle boat)
    {
        return Boats.Contains(boat);
    }
    public bool HasSpot(out Vector3 spot)
    {
        bool hasspot = true;
        spot = Vector3.Zero;
        List<Vector3> Rests = new List<Vector3>();
        foreach (Position3D loc in Locations)
        {
            Rests.Add(loc.GlobalTranslation);
        }
        
        if (Boats.Count > 0)
        {
            
            for (int i = 0; i <Boats.Count; i++)
            {
                
                for (int l = Rests.Count - 1; l > -1; l--)
                {
                    if (Rests[l].DistanceTo(Boats[i].GlobalTranslation) < 20)
                    {
                        Rests.RemoveAt(l);
                    }
                }
            }
            if (Rests.Count == 0)
                return false;
        }
        spot = Rests[0];
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
                    p.Scale *= 20;
                    Locations.Add(p);
                    AddChild(p, true);
                    p.Owner = this;
                }
                Locations[i].GlobalTranslation = RestLocations[i];

            }
            ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents = PortExtents;
            ((CubeMesh)GetNode<MeshInstance>("MeshInstance").Mesh).Size = PortExtents * 2;
        }
    }
}

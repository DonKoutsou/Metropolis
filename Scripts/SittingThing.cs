using Godot;
using System;
using System.Collections.Generic;

public class SittingThing : StaticBody
{
    List<Position3D> Seats = new List<Position3D>();
    List<bool> Occupied = new List<bool>();

    bool Inited = false;
    public override void _EnterTree()
    {
        base._EnterTree();
        if (Inited)
            return;
        RegisterSeats();
    }
    private void RegisterSeats()
    {
        foreach(Node Child in GetChildren())
        {
            if (Child is Position3D)
            {
                Seats.Add((Position3D)Child);
                Occupied.Add(false);
            }
        }
        Inited = true;
    }
    public void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable", toggle);
    }
    public override void _Ready()
    {
        
    }
    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
    }
    public void UpdateOccupation(Position3D seat, bool occupanc)
    {
        for (int i = 0; i < Seats.Count; i++)
        {
            if (Seats[i] == seat)
            {
                Occupied[i] = occupanc;
                break;
            }
        }
    }
    public bool HasEmptySeat()
    {
        bool EmptySeat = false;

        foreach (bool Occupancy in Occupied)
        {
            if (Occupancy == false)
            {
                EmptySeat = true;
                break;
            }    
        }
        return EmptySeat;
    }
    public Position3D GetSeat()
    {
        Position3D Seat = null;

        for (int i = 0; i < Seats.Count; i++)
        {
            if (Occupied[i] == false)
            {
                Seat = Seats[i];
                break;
            }    
        }
        return Seat;
    }

}

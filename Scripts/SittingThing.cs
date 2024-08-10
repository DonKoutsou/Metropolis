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
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
        if (!HasEmptySeat())
        {
            pl.GetTalkText().Talk("Δεν έχει χώρο.");
            return;
        }
        if (pl.HasVehicle())
        {
            if (!pl.GetVehicle().UnBoardVehicle(pl))
                return;
        }
        Position3D seat = GetSeat();
        pl.Sit(seat, this);
    }
    public string GetActionName(Player pl)
    {
        return "Κάτσε";
    }
    public bool ShowActionName(Player pl)
    {
        return true;
    }
    public string GetActionName2(Player pl)
    {
		return "Null.";
    }
    public string GetActionName3(Player pl)
    {
		return "Null.";
    }
    public bool ShowActionName2(Player pl)
    {
        return false;
    }
    public bool ShowActionName3(Player pl)
    {
        return false;
    }
    public string GetObjectDescription()
    {
        string desc;
        if (HasEmptySeat())
            desc = "Μπορώ να κάτσω.";
        else
            desc = "Δεν έχει χώρο.";
        return desc;
    }
    
    public override void _Ready()
    {
        
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

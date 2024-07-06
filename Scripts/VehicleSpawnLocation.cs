using Godot;
using System;
using System.Linq;

[Tool]
public class VehicleSpawnLocation : Node
{
    [Export]
    public PackedScene[] VehSpawns = new PackedScene[0];

    public bool HasVehicles()
    {
        return VehSpawns.Count() > 0;
    }

}

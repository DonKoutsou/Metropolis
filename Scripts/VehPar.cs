using Godot;
using System;

public class VehPar : Spatial
{
    [Export]
    bool PlayerVeh = false;
    public override void _Ready()
    {
        
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        GetNode<Vehicle>("VehicleBody").SetPlayerOwned(PlayerVeh);
    }
}

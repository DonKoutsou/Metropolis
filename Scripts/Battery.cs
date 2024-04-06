using Godot;
using System;

public class Battery : Item
{
    [Export]
    float Capacity = 100;

    float CurrentEnergy = 0;

    public override void _Ready()
    {
        base._Ready();
        CurrentEnergy = Capacity;
    }
    public void Recharge(float ammount)
    {
        CurrentEnergy += ammount;
    }
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
    }
    public float GetCapacity()
    {
        return Capacity;
    }

}

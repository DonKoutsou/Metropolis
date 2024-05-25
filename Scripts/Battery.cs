using Godot;
using System;

public class Battery : Item
{
    [Export]
    float Capacity = 100;

    [Export]
    float CurrentEnergy = 0;

    public override void _Ready()
    {
        base._Ready();
    }
    public void Recharge(float ammount)
    {
        CurrentEnergy += ammount;
        if (CurrentEnergy > Capacity)
        {
            CurrentEnergy = Capacity;
        }
    }
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
    }
    public float GetCapacity()
    {
        return Capacity;
    }
    public float GetCurrentCap()
    {
        return CurrentEnergy;
    }
    public void SetCurrentCap(float CurCap)
    {
        CurrentEnergy = CurCap;
    }
    public override string GetItemDesc()
    {
        return ItemDesc + " \n Capacity: " + CurrentEnergy + "/" + Capacity;
    }

}

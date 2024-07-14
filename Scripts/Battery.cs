using Godot;
using System;

public class Battery : Item
{
    [Export]
    float Capacity = 100;

    [Export]
    float CurrentEnergy = 0;

    [Export]
    float condition = 100;

    public void Recharge(float ammount)
    {
        CurrentEnergy += ammount;
        if (CurrentEnergy > Capacity)
        {
            CurrentEnergy = Capacity;
        }
    }
    public void Repair(float amm)
    {
        condition += amm;
    }
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount * (1 - condition / 100 + 1);
        condition -= ammount /10;
    }
    public float GetCapacity()
    {
        return Capacity;
    }
    public float GetCondition()
    {
        return condition;
    }
    public float GetCurrentCap()
    {
        return CurrentEnergy;
    }
    public void SetCurrentCap(float CurCap)
    {
        CurrentEnergy = CurCap;
    }
    public void SetCurrentCondition(float cond)
    {
        condition = cond;
    }
    public override string GetItemDesc()
    {
        return ItemDesc + " \n Capacity: " + (int)CurrentEnergy + "/" + Capacity + "\n Condition " + (int)condition + "/" + 100;
    }

}

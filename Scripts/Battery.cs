using Godot;
using System;

public class Battery : Item
{
    [Export]
    float Capacity = 100;

    [Export]
    float CurrentEnergy = 0;

    //[Export]
    //float condition = 100;

    public void Recharge(float ammount)
    {
        CurrentEnergy += ammount;
        if (CurrentEnergy > Capacity)
        {
            CurrentEnergy = Capacity;
        }
    }
    public override string GetInventoryItemName()
	{
		//return ItemName + CurrentEnergy.ToString() + condition;
        return ItemName + CurrentEnergy.ToString() ;
	}
   // public void Repair(float amm)
    //{
    //    condition += amm;
    //}
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
        //CurrentEnergy -= ammount * (1 - condition / 100 + 1);
        //condition = Mathf.Max(condition - ammount /10, 0);
    }
    public float GetCapacity()
    {
        return Capacity;
    }
    //public float GetCondition()
    //{
        //return condition;
    //}
    public float GetCurrentCap()
    {
        return CurrentEnergy;
    }
    public void SetCurrentCap(float CurCap)
    {
        CurrentEnergy = CurCap;
    }
    //public void SetCurrentCondition(float cond)
    //{
    //    condition = cond;
    //}
    public override string GetItemDesc()
    {
        return LocalisationHolder.GetString(ItemDesc) + " \n " + LocalisationHolder.GetString("Φόρτωση") + ": " + (int)CurrentEnergy + "/" + Capacity;
       // return ItemDesc + " \n Capacity: " + (int)CurrentEnergy + "/" + Capacity + "\n Condition " + (int)condition + "/" + 100;
    }
    public override void InputData(ItemInfo data)
	{
		base.InputData(data);

        float cap = (float)data.CustomData["CurrentEnergy"];
        SetCurrentCap(cap);

	}
    public override void GetCustomData(out string[] Keys, out object[] Values)
	{
		Keys = new string[1];
		Values = new object[1];
        
        Keys[0] = "CurrentEnergy";
		Values[0] = GetCurrentCap();
	}
}

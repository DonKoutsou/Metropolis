using Godot;
using System;

public class Toolbox : Item
{
    //[Export]
    //float SupplyCapacity = 100;

    //[Export]
    //float CurrentSupplies = 0;

    public override string GetItemDesc()
    {
        return ItemDesc;
        //return ItemDesc + " \n Supplies: " + (int)CurrentSupplies + "/" + SupplyCapacity;
    }
    /*public float GetCurrentSupplyAmmount()
    {
        return CurrentSupplies;
    }
    public void SetCurrentSupplies(float CurSup)
    {
        CurrentSupplies = CurSup;
    }
    public void ConsumeSupplies(float ammount)
    {
        CurrentSupplies = Math.Max(0, CurrentSupplies - ammount);
    }
    /*public void RepaiItem(Battery it)
    {
        float AmmountToRepair = 100 - it.GetCondition();
        float AmmountThatCaneBeRepaired = Math.Min(CurrentSupplies, AmmountToRepair);
        ConsumeSupplies(AmmountThatCaneBeRepaired);
        it.Repair(AmmountThatCaneBeRepaired);
    }
    public override string GetInventoryItemName()
	{
		return ItemName + CurrentSupplies.ToString();
	}*/
}

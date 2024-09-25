using Godot;
using System;

public class DrahmaStack : Item
{
    int Ammount = 3;

    public void SetAmmount(int amm)
    {
        Ammount = amm;
    }
    public override void InputData(ItemInfo data)
	{
		base.InputData(data);

        int q = (int)data.CustomData["Quantity"];
	    Ammount = q;
	}
    public override void GetCustomData(out string[] Keys, out object[] Values)
	{
		Keys = new string[1];
		Values = new object[1];
        
        Keys[0] = "Quantity";
		Values[0] = Ammount;
	}
    public Item[] DecomposeStack()
    {
        Item[] Dstack = new Item[Ammount];
        PackedScene d = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Drahma.tscn");
        for (int i = 0; i < Ammount; i++)
        {
            Dstack[i] = d.Instance<Item>();
        }
        return Dstack;
    }
    public override void OnItemPickedUp()
    {
        base.OnItemPickedUp();
        QueueFree();
    }
}

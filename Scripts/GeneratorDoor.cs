using Godot;
using System;
using System.Collections.Generic;

public class GeneratorDoor : StaticBody
{
    [Signal]
    public delegate void OnDoorOpened();
    [Export]
    ItemName ItemRequiredToOpen = ItemName.KEYCARD;
    [Export]
    bool ConsumeItem = false;
    bool Open = false;
    AnimationPlayer anim;
    public override void _Ready()
    {
        anim = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public string GetActionName(Player pl)
    {   
        string name = "Άνοιξε";
        if (Open)
        {
            name = "Κλείσε";
        }
        return name;
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
        return "Με ένα εκρηκτικό θα μπορούσα να το σπάσω";
    }
    public void ToggleDoor()
    {
        if (Open)
        {
            Open = false;
            anim.Play("Close");
        }
        else
        {
            Open = true;
            anim.Play("Open");
            EmitSignal("OnDoorOpened");
        }
        //pl.anim.PlayAnimation(anim);
        //pl.global
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
        if (pl.GetCharacterInventory().HasItemOfType(ItemRequiredToOpen))
        {
            ToggleDoor();
            if (ConsumeItem)
            {
                Inventory inv = pl.GetCharacterInventory();
                List<Item> its;
                ItemName[] types = {ItemRequiredToOpen};
                inv.GetItemsByType(out its, types);
                inv.DeleteItem(its[0]);
            }
        }
            
        else
            DialogueManager.GetInstance().ScheduleDialogue(pl, "Φένεταί σαν να χρειάζεται κάποιου είδους κλειδί για να ελέγξω την πύλη.");
            //pl.GetTalkText().Talk("Φένεταί σαν να χρειάζεται κάποιου είδους κλειδί για να ελέγξω την πύλη.");
    }

}

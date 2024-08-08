using Godot;
using System;

[Tool]
public class GeneratorDoor : StaticBody
{
    bool Open = false;
    AnimationPlayer anim;
    Spatial Switch;
    public override void _Ready()
    {
        anim = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
        Switch = GetParent().GetNode<Spatial>("Switch");
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
        if (pl.GetCharacterInventory().HasItemOfType(ItemName.KEYCARD))
            ToggleDoor();
        else
            pl.GetTalkText().Talk("Φένεταί σαν να χρειάζεται κάποιου είδους κλειδί για να ελέγξω την πύλη.");
    }

}

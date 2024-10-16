using Godot;
using System;
using System.Collections.Generic;
public class LightHouseSwitch : StaticBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
        {
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
        } 
        else
        {
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
        }
            
    }
    public void DoAction(Player pl)
	{
        if (pl.GetCharacterInventory().HasItemOfType(ItemName.BLOOD_VIAL))
        {
            WorldMap map = WorldMap.GetInstance();
            map.UnlockLightHouse(map.GetCurrentIleInfo());

            LightHouse L = GetParent<LightHouse>();
            //L.ToggeLightHouse(true);

            CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
            CameraAnimation.Connect("FadeOutFinished", L, "FixLightHouse");
            CameraAnimation.FadeInOut(1);
 
            Inventory inv = pl.GetCharacterInventory();
            List<Item> its;
            ItemName[] types = {ItemName.BLOOD_VIAL};
            inv.GetItemsByType(out its, types);
            inv.DeleteItem(its[0]);
        }
        else
            DialogueManager.GetInstance().ScheduleDialogue(pl, "Φένεταί σαν να χρειάζεται κάποιου είδους κλειδί για πάρει μπρός.");
            //pl.GetTalkText().Talk("Φένεταί σαν να χρειάζεται κάποιου είδους κλειδί για πάρει μπρός.");

        
        
    }
    public string GetActionName(Player pl)
    {
        return "Ενεργοποίηση φάρου";
    }
    public bool ShowActionName(Player pl)
    {
        WorldMap map = WorldMap.GetInstance();
        return !map.IsLightHouseUnlocked(map.GetCurrentIleInfo());
        //return true;
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
        return "Από εδώ βάζω μπρός τον φάρο.";
    }
}

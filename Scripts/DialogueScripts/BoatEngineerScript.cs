using Godot;
using System;
using System.Collections.Generic;

public class BoatEngineerScript : BaseDialogueScript
{
    int ToolboxesGiven = 0;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        List<string> Dialogues = new List<string>();
        
        switch(DialogueProg)
        {
            case 0:
            {
                Dialogues.Add("#Boatman1");
                Dialogues.Add("#Boatman2");
                DialogueProg ++;
                break;
            }
            case 1:
            {
                Dialogues.Add("#Boatman3");
                Dialogues.Add("#Boatman4");
                DialogueProg ++;
                break;
            }
            case 2:
            {
                Dialogues.Add("#Boatman5");
                Dialogues.Add("#Boatman6");
                Dialogues.Add("#Boatman7");
                DialogueProg ++;
                break;
            }
            case 3:
            {
                Dialogues.Add("#Boatman8");
                Dialogues.Add("#Boatman9");
                Dialogues.Add("#Boatman10");
                Dialogues.Add("#Boatman11");
                Dialogues.Add("#Boatman12");
                DialogueProg ++;
                break;
            }
            case 4:
            {
                Dialogues.Add("#Boatman13");
                Dialogues.Add("#Boatman14");
                Dialogues.Add("#Boatman15");
                DialogueProg ++;
                break;
            }
            case 5:
            {
                Dialogues.Add("#Boatman16");
                Dialogues.Add("#Boatman17");
                Dialogues.Add("#Boatman18");
                Dialogues.Add("#Boatman19");
                Dialogues.Add("#Boatman20");
                DialogueProg ++;
                break;
            }
            case 6:
            {
                Dialogues.Add("#Boatman21");
                Dialogues.Add("#Boatman22");
                Dialogues.Add("#Boatman23");
                Dialogues.Add("#Boatman24");
                Dialogues.Add("#Boatman25");
                Dialogues.Add("#Boatman26");
                DialogueProg ++;
                break;
            }
            case 7:
            {
                Dialogues.Add("#Boatman4");
                break;
            }
        }
        foreach (string diag in Dialogues)
            DialogueManager.GetInstance().ScheduleDialogue(Talker, diag);
    }
    public override bool ShouldShowExtraAction()
    {
        return DialogueProg == 7 && Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.TOOLBOX);
    }
    public override string GetExtraActionText()
    {
        return "GivToolB";
    }
    public override void Action1Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Tollboxes;
        ItemName[] types = {ItemName.TOOLBOX};
        inv.GetItemsByType(out Tollboxes, types);
        inv.DeleteItem(Tollboxes[0]);

        ToolboxesGiven ++;

        if (ToolboxesGiven == 3)
        {
            DialogueProg ++;
            DialogueManager.GetInstance().ScheduleDialogue(owner, "#Boatman27");
        }
        DialogueManager.GetInstance().ScheduleDialogue(owner, "#Boatman26");
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"ToolboxesGiven", ToolboxesGiven},
        };
        return savedata;
    }
}

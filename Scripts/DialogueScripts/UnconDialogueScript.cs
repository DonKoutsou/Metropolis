using Godot;
using System;
using System.Collections.Generic;

public class UnconDialogueScript : BaseDialogueScript
{
    bool GivenBattery = false;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        string text = string.Empty;
        
        switch(DialogueProg)
        {
            case 9:
            {
                text = "Ψάχνεις τους φάρους, κάτσε να σου σημειώσω τον κοντινότερο στον χάρτη.";
                DialogueProg ++;
                break;
            }
            case 1:
            {
                text = string.Format("Θα φύγω προς την Μητρόπολη, αν θημάμε καλά είναι κάπου προς τα {0}. Ελπίζω να σας δω εκέι.", WorldMap.GetInstance().GetExitDirection());
                DialogueProg ++;
                break;
            }
            case 2:
            {
                text = "Θα σε δω στην Μητρόπολη καΐκτση, καλό δρόμο.";
                CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
                CameraAnimation.Connect("FadeOutFinished", Talker, "DespawnChar");
                CameraAnimation.FadeInOut(3);
                break;
            }

        }
        Talker.GetTalkText().Talk(text);
    }
    public override bool ShouldShowExtraAction()
    {
        return !GivenBattery;
    }
    public override bool ShouldShowExtraAction2()
    {
        return false;
    }
    public override string GetExtraActionText()
    {
        return "Δώσε μπαταρία.";
    }
    public override string GetExtraActionText2()
    {
        return "Null";
    }
    public override string Action1Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
    
        List<Battery> bats;
        inv.GetBatteries(out bats);

        if (bats.Count == 0)
        {
            pl.GetTalkText().Talk("Δεν έχω μπαταρίες...");
            return "null";
        }

        //TalkingChar.RechargeCharacter(bats[0].GetCurrentCap());
        owner.Respawn();

        inv.DeleteItem(bats[0]);

        DialogueProg ++;

        GivenBattery = true;

        return "Νά'σε καλά καΐκτση, δεν ξέρω τι συνέβη. Τρελός ο πόνος της ατροφίας. Ευχαριστώ για την μπαταρία... Δεν ξέρω πώς να στο ανταποδώσω.";
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"GivenBattery", GivenBattery},
        };
        return savedata;
    }
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
        GivenBattery = (bool)Data["GivenBattery"];
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class UnconDialogueScript : BaseDialogueScript
{
    [Export]
	PackedScene[] PossibleRewards = null;
    bool GivenBattery = false;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        string text = string.Empty;
        string subscription = null;
        
        switch(DialogueProg)
        {
            case 0:
            {
                text = string.Format("IGoToMetropolis", WorldMap.GetInstance().GetExitDirection());
                DialogueProg ++;
                break;
            }
            case 1:
            {
                text = "SeeYouInM";
                subscription = "UnconCharDepart";
                DialogueProg ++;
                break;
            }

        }
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text, subscription);
    }
    public void UnconCharDepart(NPC Talker)
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Connect("FadeOutFinished", Talker, "DespawnChar");
        CameraAnimation.FadeInOut(3);
    }
    public override bool ShouldShowExtraAction()
    {
        return Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.BATTERY) && !GivenBattery;
    }
    public override bool ShouldShowExtraAction2()
    {
        return false;
    }
    public override string GetExtraActionText()
    {
        return "GiveBat";
    }
    public override string GetExtraActionText2()
    {
        return "Null";
    }
    public override void Action1Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
    
        List<Battery> bats;
        inv.GetBatteries(out bats);

        if (bats.Count == 0)
        {
            DialogueManager.GetInstance().ScheduleDialogue(pl, "NoBat");
            //pl.GetTalkText().Talk("Δεν έχω μπαταρίες...");
            return;
        }

        //TalkingChar.RechargeCharacter(bats[0].GetCurrentCap());
        owner.Respawn();

        inv.DeleteItem(bats[0]);

        GivenBattery = true;

        Item newItem = PossibleRewards[RandomContainer.Next(0, PossibleRewards.Count())].Instance<Item>();
        if (newItem is Book b)
		{
			int volume = BookVolumeHolder.GetRandomUnfoundVolume(b.GetSeries());
			b.SetVoluemeNumber(volume);
			BookVolumeHolder.OnVolumeFound(b.GetSeries(), volume);
		}
        inv.InsertItem(newItem);

        ActionTracker.OnActionDone("SaveUnconMachine");

        DialogueManager.GetInstance().ScheduleDialogue(owner, "Νά'σε καλά καΐκτση, δεν ξέρω τι συνέβη. Τρελός ο πόνος της ατροφίας. Ευχαριστώ για την μπαταρία... Δεν ξέρω πως να το ανταποδώσω, βρήκα αυτό στα ταξίδια μου, ελπίζω να σου φανεί χρήσημο.");
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
        if (Data.ContainsKey("DialogueProg"))
            DialogueProg = (int)Data["DialogueProg"];
        if (Data.ContainsKey("GivenBattery"))
            GivenBattery = (bool)Data["GivenBattery"];
    }
}
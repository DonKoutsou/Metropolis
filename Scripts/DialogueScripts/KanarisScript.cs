using Godot;
using System;
using System.Collections.Generic;

public class KanarisScript : BaseDialogueScript
{
    int MusicGiven = 0;
    bool GivenGuitar = false;
    int MusicSheetGiven = 0;
    int InfoStage = 0;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        List<string> Dialogues = new List<string>();

        if (Talker.IsPlayerInstrument())
        {
            Dialogues.Add("#KanarInter");
        }
        else
        {
            switch(DialogueProg)
            {
                case 0:
                {
                    Dialogues.Add("#Kanar1");
                    Dialogues.Add("#Kanar2");
                    Dialogues.Add("#Kanar3");
                    DialogueProg ++;
                    break;
                }
                case 1:
                {
                    Dialogues.Add("#Kanar4");
                    Dialogues.Add("#Kanar5");
                    DialogueProg ++;
                    break;
                }
                case 2:
                {
                    Dialogues.Add("#Kanar6");
                    Dialogues.Add("#Kanar7");
                    Dialogues.Add("#Kanar8");
                    DialogueProg ++;
                    break;
                }
                case 3:
                {
                    Dialogues.Add("#Kanar9");
                    Dialogues.Add("#Kanar10");
                    Dialogues.Add("#Kanar11");
                    Dialogues.Add("#Kanar12");
                    DialogueProg ++;
                    break;
                }
                case 4:
                {
                    Dialogues.Add("#Kanar13");
                    Dialogues.Add("#Kanar14");
                    Dialogues.Add("#Kanar15");
                    Dialogues.Add("#Kanar16");
                    Dialogues.Add("#Kanar17");
                    DialogueProg ++;
                    break;
                }
                case 5:
                {
                    Dialogues.Add("#Kanar17");
                    break;
                }
            }
        }
        foreach (string diag in Dialogues)
            DialogueManager.GetInstance().ScheduleDialogue(Talker, diag);
    }
    public override bool ShouldShowExtraAction()
    {
        bool showaction;

        if (!GivenGuitar)
        {
            showaction = Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.GUITAR);
        }
        else
        {
            showaction = Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.SHEET_MUSIC);
        }

        return DialogueProg == 5 && showaction;
    }
    public override bool ShouldShowExtraAction2()
    {
        ItemName[] types = {ItemName.VINYL, ItemName.CASSETTE};
        
        return DialogueProg == 5 && Player.GetInstance().GetCharacterInventory().HasAnyOfItems(types);
    }
    public override string GetExtraActionText()
    {
        string ActName;
        if (!GivenGuitar)
            ActName = "GivGuitar";
        else
            ActName = "GivMusSheet";
        return ActName;
    }
    public override string GetExtraActionText2()
    {
        return "GivMus";
    }
    public override void Action1Done(NPC owner, Player pl)
    {
        string returntext;
        if (!GivenGuitar)
        {
            Inventory inv = Player.GetInstance().GetCharacterInventory();
            ItemName[] types = {ItemName.GUITAR};
            List<Item> MusicSheets;
            inv.GetItemsByType(out MusicSheets, types);
            inv.DeleteItem(MusicSheets[0]);

            for (int i = 0; i < 2; i++)
            {
                Item newItem = GlobalItemCatalogue.GetInstance().GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
                inv.InsertItem(newItem);
            }

            GivenGuitar = true;
            returntext =  "#Kanar62";
        }
        else
        {
            Inventory inv = Player.GetInstance().GetCharacterInventory();
            ItemName[] types = {ItemName.SHEET_MUSIC};

            MusicSheetGiven ++;

            List<Item> MusicSheets;
            inv.GetItemsByType(out MusicSheets, types);
            inv.DeleteItem(MusicSheets[0]);

            Item newItem = GlobalItemCatalogue.GetInstance().GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
            inv.InsertItem(newItem);

            returntext =  "#Kanar63";
       
        }
        DialogueManager.GetInstance().ScheduleDialogue(owner, returntext);

        if (InfoStage == 2 || InfoStage == 5 ||InfoStage == 8 )
        {
            foreach (string Diag in GetLabInfo(InfoStage))
                DialogueManager.GetInstance().ScheduleDialogue(owner, Diag);
        }

        InfoStage ++;
    }
    public override void Action2Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        ItemName[] types = {ItemName.VINYL, ItemName.CASSETTE};

        MusicGiven ++;

        List<Item> Music;
        inv.GetItemsByType(out Music, types);
        inv.DeleteItem(Music[0]);

        Item newItem = GlobalItemCatalogue.GetInstance().GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
        inv.InsertItem(newItem);

        DialogueManager.GetInstance().ScheduleDialogue(owner, "#Kanar64");

        InfoStage ++;

        if (InfoStage == 2 || InfoStage == 5 ||InfoStage == 8 )
        {
            foreach (string Diag in GetLabInfo(InfoStage))
                DialogueManager.GetInstance().ScheduleDialogue(owner, Diag);
        }

        
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"MusicGiven", MusicGiven},
            {"MusicSheetGiven", MusicSheetGiven},
            {"GivenGuitar", GivenGuitar},
            {"InfoStage", InfoStage}
        };
        return savedata;
    }
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
        MusicGiven = (int)Data["MusicGiven"];
        MusicSheetGiven = (int)Data["MusicSheetGiven"];
        GivenGuitar = (bool)Data["GivenGuitar"];
        InfoStage = (int)Data["InfoStage"];
    }
    private List<string> GetLabInfo(int stage)
    {
        List<string> Dialogues = new List<string>();

        switch(stage)
        {
            case 0:
            {
                Dialogues.Add("#Kanar18");
                Dialogues.Add("#Kanar19");
                Dialogues.Add("#Kanar20");
                Dialogues.Add("#Kanar21");
                Dialogues.Add("#Kanar22");
                Dialogues.Add("#Kanar23");
                Dialogues.Add("#Kanar24");
                Dialogues.Add("#Kanar25");
                break;
            }
            case 1:
            {
                Dialogues.Add("#Kanar26");
                Dialogues.Add("#Kanar27");
                Dialogues.Add("#Kanar28");
                Dialogues.Add("#Kanar29");
                Dialogues.Add("#Kanar30");
                Dialogues.Add("#Kanar31");
                Dialogues.Add("#Kanar32");
                Dialogues.Add("#Kanar33");
                Dialogues.Add("#Kanar34");
                Dialogues.Add("#Kanar35");
                Dialogues.Add("#Kanar36");
                Dialogues.Add("#Kanar37");
                Dialogues.Add("#Kanar38");
                Dialogues.Add("#Kanar39");
                Dialogues.Add("#Kanar40");
                Dialogues.Add("#Kanar41");
                Dialogues.Add("#Kanar42");
                break;
            }
            case 2:
            {
                Dialogues.Add("#Kanar43");
                Dialogues.Add("#Kanar44");
                Dialogues.Add("#Kanar45");
                Dialogues.Add("#Kanar46");
                Dialogues.Add("#Kanar47");
                Dialogues.Add("#Kanar48");
                Dialogues.Add("#Kanar49");
                Dialogues.Add("#Kanar50");
                Dialogues.Add("#Kanar51");
                Dialogues.Add("#Kanar52");
                Dialogues.Add("#Kanar53");
                Dialogues.Add("#Kanar54");
                Dialogues.Add("#Kanar55");
                Dialogues.Add("#Kanar56");
                Dialogues.Add("#Kanar57");
                Dialogues.Add("#Kanar58");
                Dialogues.Add("#Kanar59");
                Dialogues.Add("#Kanar60");
                Dialogues.Add("#Kanar61");
                break;
            }
        }
        return Dialogues;
    }
}

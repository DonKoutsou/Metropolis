using Godot;
using System;
using System.Collections.Generic;

public class KanarisScript : BaseDialogueScript
{
    int MusicGiven = 0;
    int MusicSheetGiven = 0;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        string text = string.Empty;

        if (Talker.IsPlayerInstrument())
        {
            text = "Δώσε μου λίγο, τελειώνω.";
        }
        else
        {
            switch(DialogueProg)
            {
                case 0:
                {
                    text = "Άλλος ένας ταξιδιάρης που είδε φώς και μπήκε. Εσύ είσαι ο πρώτος που κουβαλάει ένα μωρό μαζί του. Τι σε φέρνει από εδώ καΐκτσή;";
                    DialogueProg ++;
                    break;
                }
                case 1:
                {
                    text = "Εγώ αράζω, δεν έχει μίνει και τίποτε άλλο να κάνει κανείς σε τούτο εδώ το μέρος. Έσω βρεί κάτι δίσκους προπολεμικούς και ακούω. Κάνει την ώρα να περνάει λίγο πιό ευχάριστα. Τι δε θα έδηνα για μια κιθάρα.";
                    DialogueProg ++;
                    break;
                }
                case 2:
                {
                    text = "Άμα βρείς καμία κιθάρα στα ταξίδια σου θα στην αντάλαζα για μερικά από αυτά τα εκρηκτηκά. Μπορεί να σου φανούν χρείσιμα κάπου, είναι μιά από τις γνώσεις που μου έμειναν απο το αφεντικό...";
                    break;
                }
                case 3:
                {
                    text = "Θα σου ανταλάξω ότι παρτιτούρες ή μουσική μου φέρεις με ερκηκτηκά. Θα σου φανούν χρήσημα εκέι έξω σίγουρα.";
                    break;
                }
            }
        }
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text);
    }
    public override bool ShouldShowExtraAction()
    {
        bool showaction = false;
        if (DialogueProg == 2)
        {
            showaction = Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.GUITAR);
        }
        if (DialogueProg == 3)
        {
            showaction = Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.SHEET_MUSIC);
        }
        return DialogueProg == 3 && Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.SHEET_MUSIC);
    }
    public override bool ShouldShowExtraAction2()
    {
        ItemName[] types = {ItemName.VINYL, ItemName.CASSETTE};
        
        return DialogueProg == 3 && Player.GetInstance().GetCharacterInventory().HasAnyOfItems(types);
    }
    public override string GetExtraActionText()
    {
        return "Δώσε παρτιτούρα.";
    }
    public override string GetExtraActionText2()
    {
        return "Δώσε μουσική.";
    }
    public override string Action1Done(NPC owner, Player pl)
    {
        string returntext = null;
        if (DialogueProg == 2)
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
            
            DialogueProg ++;

            returntext =  "Ποοοο είσαι τεράστιος, έχω να ρίξω κάτι πενιές. Ωρίστε 2 εκρηχτηκά. Μπορώ να φτιάξω περισσότερα. Άμα βρείς ταμπλατούρες για κιθάρα εκεί έξω φέρτες από εδώ και ανταλάζουμε πάλι.";
        }
        if (DialogueProg == 3)
        {
            Inventory inv = Player.GetInstance().GetCharacterInventory();
            ItemName[] types = {ItemName.SHEET_MUSIC};

            MusicSheetGiven ++;

            List<Item> MusicSheets;
            inv.GetItemsByType(out MusicSheets, types);
            inv.DeleteItem(MusicSheets[0]);

            Item newItem = GlobalItemCatalogue.GetInstance().GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
            inv.InsertItem(newItem);

            returntext =  "Σ'ωραίος... τσάκα άλλο ένα ακόμη εκρηχκτηκό.";
       
        }
        return returntext;
    }
    public override string Action2Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        ItemName[] types = {ItemName.VINYL, ItemName.CASSETTE};

        MusicGiven ++;

        List<Item> Music;
        inv.GetItemsByType(out Music, types);
        inv.DeleteItem(Music[0]);

        Item newItem = GlobalItemCatalogue.GetInstance().GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
        inv.InsertItem(newItem);


        return "Ωραίος, ";
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"MusicGiven", MusicGiven},
            {"MusicSheetGiven", MusicSheetGiven}
        };
        return savedata;
    }
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
        MusicGiven = (int)Data["MusicGiven"];
        MusicSheetGiven = (int)Data["MusicSheetGiven"];
    }
}

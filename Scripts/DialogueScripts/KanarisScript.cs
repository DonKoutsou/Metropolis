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
                    text = ".";
                    DialogueProg ++;
                    break;
                }
                case 1:
                {
                    text = "Αυτό που κουβαλάς είναι αυτό που νομίζω; Πηγένεις στην Μητρόπολη από'τι συμπερένω. Θα ήθελα να σε βοηθήσω στο ταξίδι σου αλλά δυστηχώς τα καΐκια που βλέπεις δεν λειτουργούν. Μου λείπουν τα εργαλεία και δεν μπορώ να τα επιδιορθώσω.";
                    DialogueProg ++;
                    break;
                }
                case 2:
                {
                    text = string.Format("Μόλις επισκευάσω τα καΐκια είσαι ελεύθερος να πάρεις όποιο θες. Τα πανία τους θα σε βοηθήσουν να φτάσεις στην Μητρόπολη πολύ πιο εύκολά, εφόσον ο άνεμος είναι με το μέρος σου.");
                    DialogueProg ++;
                    break;
                }
                case 3:
                {
                    text = "Ότι εργαλεία βρείς στα ταξίδια σου φέρτα εδώ και θα επισκευάσω τα καΐκια.";
                    break;
                }
                case 4:
                {
                    
                    break;
                }
            }
        }
        Talker.GetTalkText().Talk(text);
    }
    public override bool ShouldShowExtraAction()
    {
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

        Inventory inv = Player.GetInstance().GetCharacterInventory();
        ItemName[] types = {ItemName.SHEET_MUSIC};
        List<Item> MusicSheets;
        inv.GetItemsByType(out MusicSheets, types);
        inv.DeleteItem(MusicSheets[0]);

        Item newItem = GlobalItemCatalogue.GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
        inv.InsertItem(newItem);

        DialogueProg ++;

        return "Ενδιαφέρων, δεν περέμενα κάτι τέτοιο... είσαι σίγουρος οτι θες να το αποχοριστείς, είναι ένα αρκετά σπάνιο ανικείμενο στις ημέρες μας. Θα προσπαθήσω να το εκμετελευτό πλήρος, ευχαριστώ καΐκτση";
    }
    public override string Action2Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        ItemName[] types = {ItemName.VINYL, ItemName.CASSETTE};
        List<Item> Music;
        inv.GetItemsByType(out Music, types);
        inv.DeleteItem(Music[0]);

        Item newItem = GlobalItemCatalogue.GetItemByType(ItemName.EXPLOSIVE).Instance<Item>();
        inv.InsertItem(newItem);

        //GlobalItemCatalogue.GetItemByType

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

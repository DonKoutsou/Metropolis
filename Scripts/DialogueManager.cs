using Godot;
using System;
using System.Collections.Generic;
public class DialogueManager : Node
{
    static DialogueManager Instance;
    public override void _Ready()
    {
        Instance = this;
    }
    NPC TalkingChar;
	//static bool IsTalking = false;
    //public static bool IsPlayerTalking()
    //{
    //    return IsTalking;
    //}
    public static DialogueManager GetInstance()
    {
        return Instance;
    }
	public void StartDialogue(NPC character, string Dialogue = "TestTimeline")
	{
		Player pl = Player.GetInstance();
		if (pl.HasVehicle())
		{
			if (!pl.GetVehicle().UnBoardVehicle(pl))
				return;
		}
		Position3D talkpos = character.GetNode<Position3D>("TalkPosition");



		pl.UpdateLocationToMove(talkpos.GlobalTranslation);
		
		//DialogicSharp.SetVariable("MetropolisDirection", WorldMap.GetInstance().GetExitDirection());
		//DialogicSharp.SetVariable("GenericCharacter", character.GetCharacterName());
		Camera cam = pl.GetDialogueCamera();
		
		Spatial Diagcampivot = (Spatial)cam.GetParent();
		Diagcampivot.GlobalRotation = talkpos.GlobalRotation;
		Diagcampivot.GlobalTranslation = talkpos.GlobalTranslation;
		cam.LookAt(character.GetHeadGlobalPos(), Vector3.Up);
		
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeInDialogue");
		if (character.GetUnconState())
		{
			bool HasBat = pl.GetCharacterInventory().HasBatteries();
			//DialogicSharp.SetVariable("HasBatteries", HasBat.ToString().ToLower());
			//var dialogue = DialogicSharp.Start("UnConDialogue");
			//AddChild(dialogue);
			//dialogue.Connect("timeline_end", this, "EndUnconDialogue");
		}
		else
		{
			//var dialogue =  DialogicSharp.Start(Dialogue);

			//AddChild(dialogue);
			//dialogue.Connect("timeline_end", this, "EndDialogue");
		}
		TalkingChar = character;
		//IsTalking = true;
		//DialogueCam.Current = true;
	}
	/*public void EndUnconDialogue(Player pl, string timeline_name)
	{
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
		//string saved = DialogicSharp.GetVariable("SavedCharacter");
		if (saved == "true")
		{
            Inventory inv = pl.GetCharacterInventory();
			List<Battery> bats;
			inv.GetBatteries(out bats);

			//TalkingChar.RechargeCharacter(bats[0].GetCurrentCap());
			TalkingChar.Respawn();

			inv.DeleteItem(bats[0]);
		}
		TalkingChar.ResetLook();
		IsTalking = false;
	}*/
	public void EndDialogue(string timeline_name)
	{
		//string dialogueprog = DialogicSharp.GetVariable("DialogueProgressed");
		//if (dialogueprog == "true")
			//TalkingChar.Talked = true;
		//if (dialogueprog == "false")
			//TalkingChar.Talked = false;
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
		//IsTalking = false;
		TalkingChar.ResetLook();
	}
	public void GiveBoat()
	{
		PortWorker pw = (PortWorker)TalkingChar;
		Port p = pw.GetPort();
		p.GetBoatOfType(VehicleType.BASIC).SetPlayerOwned(true);
	}
}

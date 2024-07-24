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
    Character TalkingChar;
	static bool IsTalking = false;
    public static bool IsPlayerTalking()
    {
        return IsTalking;
    }
    public static DialogueManager GetInstance()
    {
        return Instance;
    }

	public void StartDialogue(Character character, string Dialogue = "TestTimeline")
	{
		Player pl = Player.GetInstance();
		if (pl.HasVehicle())
		{
			if (!pl.GetVehicle().UnBoardVehicle(pl))
				return;
		}
		Position3D talkpos = character.GetNode<Position3D>("TalkPosition");

		pl.UpdateLocationToMove(talkpos.GlobalTranslation);
		DialogicSharp.SetVariable("GenericCharacter", character.GetCharacterName());
		Camera cam = pl.GetDialogueCamera();
		
		Spatial Diagcampivot = (Spatial)cam.GetParent();
		Diagcampivot.GlobalRotation = talkpos.GlobalRotation;
		Diagcampivot.GlobalTranslation = talkpos.GlobalTranslation;
		cam.LookAt(character.GlobalTranslation, Vector3.Up);
		
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeInDialogue");
		if (character.GetUnconState())
		{
			bool HasBat = pl.GetCharacterInventory().HasBatteries();
			DialogicSharp.SetVariable("HasBatteries", HasBat.ToString().ToLower());
			var dialogue = DialogicSharp.Start("UnConDialogue");
			AddChild(dialogue);
			dialogue.Connect("timeline_end", this, "EndUnconDialogue");
		}
		else
		{
			var dialogue =  DialogicSharp.Start(Dialogue);

			AddChild(dialogue);
			dialogue.Connect("timeline_end", this, "EndDialogue");
		}
		TalkingChar = character;
		IsTalking = true;
		//DialogueCam.Current = true;
	}
	public void EndUnconDialogue(Player pl, string timeline_name)
	{
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
		string saved = DialogicSharp.GetVariable("SavedCharacter");
		if (saved == "true")
		{
            Inventory inv = pl.GetCharacterInventory();
			List<Battery> bats;
			inv.GetBatteries(out bats);

			//TalkingChar.RechargeCharacter(bats[0].GetCurrentCap());
			TalkingChar.Respawn();

			inv.DeleteItem(bats[0]);
		}
		IsTalking = false;
	}
	public void EndDialogue(string timeline_name)
	{
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeOutDialogue");
		IsTalking = false;
	}
	public void GiveBoat()
	{
		PortWorker pw = (PortWorker)TalkingChar;
		Port p = pw.GetPort();
		p.GetBoatOfType(VehicleType.BASIC).SetPlayerOwned(true);
	}
}

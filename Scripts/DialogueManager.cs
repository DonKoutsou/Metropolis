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
			if (!pl.currveh.UnBoardVehicle(pl))
				return;
		}
		Position3D talkpos = character.GetNode<Position3D>("TalkPosition");

		pl.loctomove = talkpos.GlobalTranslation;
		DialogicSharp.SetVariable("GenericCharacter", character.CharacterName);
		((Spatial)pl.DialogueCam.GetParent()).GlobalRotation = talkpos.GlobalRotation;
		CameraAnimationPlayer.GetInstance().PlayAnim("FadeInDialogue");
		if (character.CurrentEnergy == 0)
		{
			bool HasBat = pl.CharacterInventory.HasBatteries();
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
            Inventory inv = pl.CharacterInventory;
			List<Battery> bats;
			inv.GetBatteries(out bats);

			TalkingChar.RechargeCharacter(bats[0].GetCurrentCap());
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

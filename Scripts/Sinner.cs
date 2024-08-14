using Godot;
using System;

public class Sinner : NPC
{
	[Export]
	PackedScene NoteScene = null;
	public void OnDoorOpened()
	{
		anim.ToggleIdle();
		DialogueManager.GetInstance().ScheduleDialogue(this, "Ενδιαφέρον... Ευχαριστώ καΐκτσή.");
		//GetTalkText().Talk("Ενδιαφέρον... Ευχαριστώ καΐκτσή.");
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
		CameraAnimation.Connect("FadeOutFinished", this, "TeleportOut");
		CameraAnimation.FadeInOut(1);
	}
	public void TeleportOut()
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "TeleportOut");

		Position3D tppos = GetNode<Position3D>("TpPos");
		GlobalTranslation = tppos.GlobalTranslation;

		CameraAnimation.Connect("FadeOutFinished", this, "TeleportToWall");
		CameraAnimation.FadeInOut(5);

		DialogueManager.GetInstance().ScheduleDialogue(this, "Θα σε περιμένω εδώ...");
		//GetTalkText().Talk("Θα σε περιμένω εδώ...");

		Item note = NoteScene.Instance<Item>();

		SinnerPossition pos = SinnerPossition.GetInstance();

		Island wallile = pos.GetParent<Island>();

		IslandInfo info = WorldMap.GetInstance().GetIleInfo(wallile);

		note.ItemDesc = note.ItemDesc + info.Position.ToString();

		Player.GetInstance().GetCharacterInventory().InsertItem(note);
	}
	public void TeleportToWall()
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "TeleportToWall");


		Node par = GetParent();
		while (!(par is Island))
		{
            if (par == null)
				return;
			par = par.GetParent();
		}
		Island ile = (Island)par;
		ile.UnRegisterChild(this);

		ile.RemoveChild(this);

		SinnerPossition pos = SinnerPossition.GetInstance();

		Island wallile = pos.GetParent<Island>();

		wallile.AddChild(this);
		wallile.RegisterChild(this);

		Translation =  SinnerPossition.GetInstance().Translation;
		Rotation = SinnerPossition.GetInstance().Rotation;
		
		anim.ToggleSitting();
	}
	
}

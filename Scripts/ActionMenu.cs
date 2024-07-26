using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
////////////////////////////////////////////////////////////////////////////////////////
/*

 █████╗  ██████╗████████╗██╗ ██████╗ ███╗   ██╗    ███╗   ███╗███████╗███╗   ██╗██╗   ██╗
██╔══██╗██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║    ████╗ ████║██╔════╝████╗  ██║██║   ██║
███████║██║        ██║   ██║██║   ██║██╔██╗ ██║    ██╔████╔██║█████╗  ██╔██╗ ██║██║   ██║
██╔══██║██║        ██║   ██║██║   ██║██║╚██╗██║    ██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║   ██║
██║  ██║╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║    ██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╔╝
╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝ 
*/
////////////////////////////////////////////////////////////////////////////////////////                                                                                         
public class ActionMenu : Control
{
	Spatial SelectedObj;
	Player pl;
    bool selecting = false;
    Button PickButton;
	Button IntButton;
	Button IntButton2;

	[Export]
	int VisibleInteractableDistance = 100;
	bool PerformingAction = false;
	//0 is Pickupaction
	//1 is Int 1
	//2 is Int 2
	int ActionIndex = 0;

	[Export(PropertyHint.Layers3dPhysics)]
	public uint SelectLayer { get; set; }

	public override void _Ready()
	{
		SetPhysicsProcess(false);
		VBoxContainer cont = GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer");
        PickButton = cont.GetNode<Button>("PickUp_Button");
		IntButton = cont.GetNode<Button>("Interact_Button");
		IntButton2 = cont.GetNode<Button>("Interact_Button2");
		IntButton.Hide();
		PickButton.Hide();
		IntButton2.Hide();
		pl = (Player)GetParent();
	}
	public bool IsSelecting()
	{
		return SelectedObj != null;
	}
	public bool IsSelected(Spatial obj)
	{
		return SelectedObj == obj;
	}
	public void StartPerformingAction(int type)
	{
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		pl.UpdateLocationToMove(Acomp.GetActionPos(pl.GlobalTranslation));
		PerformingAction = true;
		ActionIndex = type;
	}
	private void On_PickUp_Button_Down()
	{
		if (pl.HasVehicle() && pl.GetVehicle() != SelectedObj)
		{
			pl.GetTalkText().Talk("Δεν μπορώ πάνω από την βάρκα");
		}
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		Vector3 actionpos = Acomp.GetActionPos(pl.GlobalTranslation);
		if (actionpos.DistanceTo(pl.GlobalTranslation) > Acomp.ActionDistance)
		{
			if (!PerformingAction)
			{
				StartPerformingAction(0);
			}
			return;
		}
		
		if (SelectedObj is Item it)
		{
			if (!pl.GetCharacterInventory().InsertItem(it))
			{
				pl.GetTalkText().Talk("Δέν έχω χώρο.");
			}
		}
		else if (SelectedObj is NPC chara)
		{
			DialogueManager.GetInstance().StartDialogue(chara, chara.GetDialogue());
			//TalkText.GetInst().Talk("Φίλος", (Character)SelectedObj);
		}
		else if (SelectedObj is Vehicle veh)
		{
			if (!veh.IsPlayerOwned())
			{
				pl.GetTalkText().Talk("Δεν είναι δικιά μου. Δεν μπορώ να την χρησιμοποιήσω.");
				return;
			}
			if (!pl.HasVehicle())
			{
				veh.BoardVehicle(pl);
				pl.SetVehicle(veh);
			}
			else
			{
				if (!veh.UnBoardVehicle(pl))
					return;
				pl.SetVehicle(null);
			}
		}
		else if (SelectedObj is Furniture furn)
		{
			Item foundit;
			furn.Search(out foundit);
			if (foundit != null)
			{
				pl.GetCharacterInventory().InsertItem(foundit);
				pl.GetTalkText().Talk(foundit.GetItemPickUpText());
			}
			else
				pl.GetTalkText().Talk("Τίποτα");
		}
		else if (SelectedObj is WindGenerator generator)
		{
			List<Item> batteries;
			pl.GetCharacterInventory().GetItemsByType(out batteries, ItemName.BATTERY);
			float availableenergy = generator.GetCurrentEnergy();
			float rechargeamm = 0;
			for (int i = batteries.Count - 1; i > -1; i--)
			{
				if (availableenergy <= 0)
					break;
				Battery bat = (Battery)batteries[i];
				float cap = bat.GetCapacity();
				float energy = bat.GetCurrentCap();
				if (energy < cap)
				{
					float reachargeammount = cap - energy;
					if (availableenergy > reachargeammount)
					{
						bat.Recharge(reachargeammount);
						availableenergy -= reachargeammount;
						rechargeamm += reachargeammount;
					}
					else
					{
						bat.Recharge(availableenergy);
						rechargeamm += availableenergy;
						availableenergy = 0;
					}
				}
			}
			/*float charrechargeamm = pl.GetCharacterBatteryCap() - pl.GetCurrentCharacterEnergy();
			if (charrechargeamm > availableenergy)
			{
				pl.RechargeCharacter(charrechargeamm);
				rechargeamm += charrechargeamm;
			}
			else
			{
				pl.RechargeCharacter(availableenergy);
				rechargeamm += availableenergy;
			}*/
			generator.ConsumeEnergy(rechargeamm);
			int time = (int)Math.Round(rechargeamm / 6);
			int days, hours, mins;
			DayNight.MinsToTime(time, out days,out hours, out mins);
			DayNight.ProgressTime(days, hours, mins);
			
		}
		else if (SelectedObj is FireplaceLight fire)
		{
			fire.ToggleFileplace();
		}
		else if (SelectedObj is SittingThing sit)
		{
			if (!sit.HasEmptySeat())
			{
				pl.GetTalkText().Talk("Δεν έχει χώρο.");
				return;
			}
			if (pl.HasVehicle())
			{
				if (!pl.GetVehicle().UnBoardVehicle(pl))
					return;
			}
			Position3D seat = sit.GetSeat();
			pl.Sit(seat, sit);
		}
		else if (SelectedObj is Ladder lad)
		{
			lad.TraverseLadder(pl);
		}
		else if (SelectedObj is GeneratorDoor gen)
		{
			gen.ToggleDoor();
		}
		else if (SelectedObj is JobBoardPanel jobp)
		{
			jobp.ToggleUI(true);
		}
		else if (SelectedObj is Breakable br)
		{
			List<Item> items;
			pl.GetCharacterInventory().GetItemsByType(out items, ItemName.EXPLOSIVE);
			pl.GetCharacterInventory().RemoveItem(items[0]);
			br.AtatchExplosive(pl, (Explosive)items[0]);
		}
		selecting = false;
		Stop();
	}
	private void On_Interact_Button2_Down()
	{
		if (SelectedObj is Vehicle)
		{
			Vehicle veh = (Vehicle)SelectedObj;
			veh.ToggleMachine(!veh.IsRunning());
		}
	}
	private void On_Interact_Button_Down()
	{
		if (SelectedObj is Item)
		{
			pl.GetTalkText().Talk(((Item)SelectedObj).GetItemName());
		}
		else if (SelectedObj is Character)
		{
			pl.GetTalkText().Talk("Φίλος");
		}
        
		else if (SelectedObj is Vehicle veh)
		{
			if (veh.IsPlayerOwned())
				pl.GetTalkText().Talk("Καΐκάρα μου!");
			else
				pl.GetTalkText().Talk("Καΐκι, δεν είμαι σίγουρος πιανού.");
		}
		else if (SelectedObj is WindGenerator)
		{
			pl.GetTalkText().Talk("Γεννήτρια");
		}
		else if (SelectedObj is Furniture)
		{
			Furniture furni = (Furniture)SelectedObj;
			if (furni.HasBeenSearched())
				pl.GetTalkText().Talk("Το έψαξα, είναι άδειο");
			else
				pl.GetTalkText().Talk(furni.FurnitureDescription);
		}
		else if (SelectedObj is SittingThing)
		{
			SittingThing sit = (SittingThing)SelectedObj;
			if (!sit.HasEmptySeat())
			{
				pl.GetTalkText().Talk("Δεν έχει χώρο.");
				return;
			}
			pl.GetTalkText().Talk("Μπορώ να κάτσω.");
		}
		else if (SelectedObj is Ladder)
		{
			pl.GetTalkText().Talk("Σκάλα");
		}
		else if (SelectedObj is JobBoardPanel)
		{
			pl.GetTalkText().Talk("Πίνας αγγελιών");
		}
		else if (SelectedObj is Breakable)
		{
			pl.GetTalkText().Talk("Με ένα εκρηκτικό θα μπορούσα να το σπάσω");
		}
	}
	public void Start(Spatial obj)
	{
		if (DialogueManager.IsPlayerTalking())
			return;
		if (selecting)
            return;
		
		if (obj is Item)
		{
			PickButton.Text = "Πάρε";
		}
		else if (obj is Character)
		{
			PickButton.Text = "Kουβέντα";
		}
		else if (obj is Vehicle)
		{
			if (pl.HasVehicle() && pl.GetVehicle() == obj)
				PickButton.Text = "Αποβιβάση";
			else
				PickButton.Text = "Επιβιβάση";
		}
		else if (obj is Furniture)
		{
			PickButton.Text = "Ψάξε";
		}
		else if (obj is WindGenerator)
		{
			PickButton.Text = "Φόρτιση";
		}
		else if (obj is SittingThing)
		{
			PickButton.Text = "Κάτσε";
		}
		else if (obj is FireplaceLight fp)
		{
			IntButton.Hide();
			if (fp.State)
				PickButton.Text = "Σβήσε.";
			else
				PickButton.Text = "Άναψε.";
			//PickButton.Hide();
		}
		else if (obj is Ladder || obj is GeneratorDoor)
		{
			//IntButton.Hide();
			//FireplaceLight fp = (FireplaceLight)SelectedObj;

			PickButton.Text = (string)obj.Call("GetActionName", pl.GlobalTranslation);
			
			//PickButton.Hide();
		}
		else if (obj is JobBoardPanel)
		{
			PickButton.Text = "Κοίτα";
		}
		else if (obj is Breakable)
		{
			if (!pl.GetCharacterInventory().HasItemOfType(ItemName.EXPLOSIVE))
				PickButton.Hide();
			else
				PickButton.Show();
			PickButton.Text = "Τοποθέτησε εκρηκτικό.";
		}
		DeselectCurrent();
		SelectedObj = obj;
		SelectedObj.Call("HighLightObject", true);
		PickButton.Show();
		IntButton.Show();
		Show();
		SetPhysicsProcess(true);
	}
	void DeselectCurrent()
	{
		if (SelectedObj != null)
			SelectedObj.Call("HighLightObject", false);

		SelectedObj = null;
	}
	public void Stop()
	{
		pl.GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").ResetHead();
		if (PerformingAction)
		{
			PerformingAction = false;
			ActionIndex = 0;
			pl.UpdateLocationToMove(pl.GlobalTranslation);
		}
        if (selecting)
            return;
		if (SelectedObj == null)
			return;
		DeselectCurrent();
		
		Hide();
		SetPhysicsProcess(false);
		//WarpMouse(GetViewport().Size/2);
		RectPosition = new Vector2 (0.0f, 0.0f);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		Vector3 actionpos = Acomp.GetActionPos(pl.GlobalTranslation);

		var screenpos = DViewport.GetInstance().GetCamera().UnprojectPosition(actionpos);

		pl.GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").HeadLookAt(actionpos);
		//Vector3 pos = SelectedObj.GlobalTransform.origin;

		//if (pl.GlobalTransform.origin.DistanceTo(pos) > 60 && selecting)
			//PickButton.Hide();
		//else
			//PickButton.Show();

		if (SelectedObj is Furniture furn)
		{
			if (furn.HasBeenSearched())
				PickButton.Hide();	
		}

		RectPosition = new Vector2 (screenpos.x, screenpos.y +50);

		if (screenpos < Vector2.Zero || screenpos > DViewport.GetInstance().Size)
			Stop();

		if (PerformingAction)
		{
			if (actionpos.DistanceTo(pl.GlobalTranslation) <= SelectedObj.GetNode<ActionComponent>("ActionComponent").ActionDistance)
			{
				pl.UpdateLocationToMove(pl.GlobalTranslation);
				
				if (ActionIndex == 0)
				{
					On_PickUp_Button_Down();
				}
				if (ActionIndex == 1)
				{
					On_Interact_Button_Down();
				}
				if (ActionIndex == 2)
				{
					On_Interact_Button2_Down();
				}
				PerformingAction = false;
			}
			//if (pl.loctomove != SelectedObj.GlobalTranslation)
			//{
			//	PerformingAction = false;
			//}
			//else
			//{
			//	pl.loctomove = SelectedObj.GlobalTranslation;
			//}
		}
		
		//GlobalTranslation =  new Vector3(itempos.x, itempos.y, itempos.z);
	}
    private void Selecting_Action()
    {
        selecting = true;
    }
    private void Stoped_Selecting_Action()
    {
        selecting = false;
    }
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Select"))
		{
			var spacestate = GetTree().Root.World.DirectSpaceState;
			float mult = OS.WindowSize.x / DViewport.GetInstance().Size.x;
			Vector2 mousepos = DViewport.GetInstance().GetMousePosition() / mult;
			Camera cam = DViewport.GetInstance().GetCamera();
			Vector3 rayor = cam.ProjectRayOrigin(mousepos);
			Vector3 rayend = rayor + cam.ProjectRayNormal(mousepos) * 10000;
			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, SelectLayer);
			//if ray finds nothiong return
			if (rayar.Count == 0)
			{
				Stop();
				return;
			}
			Spatial obj = (Spatial)rayar["collider"];
			
			ActionComponent Acomp = obj.GetNode<ActionComponent>("ActionComponent");
			Vector3 actionpos = Acomp.GetActionPos(pl.GlobalTranslation);

			if (actionpos.DistanceTo(pl.GlobalTranslation) > 100)
			{
				Stop();
				return;
			}
			Start(obj);
		}
		if (@event.IsActionPressed("ActionCheck"))
		{
			Vector3 plpos = pl.GlobalTranslation;
			var interactables = GetTree().GetNodesInGroup("Interactables");
			foreach (Spatial inter in interactables)
			{
				if (plpos.DistanceTo(inter.GlobalTranslation) < VisibleInteractableDistance)
					inter.Call("HighLightObject", true);
			}
		}
		else if (@event.IsActionReleased("ActionCheck"))
		{
			var interactables = GetTree().GetNodesInGroup("Interactables");
			foreach (Node inter in interactables)
			{
				if (inter != SelectedObj)
					inter.Call("HighLightObject", false);
			}
		}
	}
}






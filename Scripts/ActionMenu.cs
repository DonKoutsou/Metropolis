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

	bool PerformingAction = false;
	//0 is Pickupaction
	//1 is Int 1
	//2 is Int 2
	int ActionIndex = 0;

	[Export(PropertyHint.Layers3dPhysics)]
	public uint SelectLayer { get; set; }

	public override void _Ready()
	{
		pl = (Player)GetParent().GetParent();
		SetProcess(false);
		VBoxContainer cont = GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer");
        PickButton = cont.GetNode<Button>("PickUp_Button");
		IntButton = cont.GetNode<Button>("Interact_Button");
		IntButton2 = cont.GetNode<Button>("Interact_Button2");
		IntButton.Hide();
		PickButton.Hide();
		IntButton2.Hide();
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
		pl.loctomove = (Vector3)SelectedObj.Call("GetActionPos", pl.GlobalTranslation);;
		PerformingAction = true;
		ActionIndex = type;
	}
	private void On_PickUp_Button_Down()
	{
		if (pl.HasVecicle && pl.currveh != SelectedObj)
		{
			TalkText.GetInst().Talk("Δεν μπορώ πάνω από την βάρκα", pl);
		}
		Vector3 actionpos = (Vector3)SelectedObj.Call("GetActionPos", pl.GlobalTranslation);
		if (actionpos.DistanceTo(pl.GlobalTranslation) > SelectedObj.GetNode<ActionComponent>("ActionComponent").ActionDistance)
		{
			if (!PerformingAction)
			{
				StartPerformingAction(0);
			}
			return;
		}
		
		if (SelectedObj is Item)
		{
			Item it = (Item)SelectedObj;
			if (!pl.GetCharacterInventory().InsertItem(it))
			{
				TalkText.GetInst().Talk("Δέν έχω χώρο.", pl);
			}
			else
			{
				
			}
			
		}
		else if (SelectedObj is Character)
		{
			DialogueManager.GetInstance().StartDialogue(pl, (Character) SelectedObj);
			//TalkText.GetInst().Talk("Φίλος", (Character)SelectedObj);
		}
		else if (SelectedObj is Vehicle)
		{
			Vehicle veh = (Vehicle)SelectedObj;
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
		else if (SelectedObj is Furniture)
		{
			Furniture furn = (Furniture)SelectedObj;
			Item foundit;
			furn.Search(out foundit);
			if (foundit != null)
			{
				pl.CharacterInventory.InsertItem(foundit);
				TalkText.GetInst().Talk(foundit.GetItemPickUpText(), pl);
			}
			else
				TalkText.GetInst().Talk("Τίποτα", pl);
		}
		else if (SelectedObj is WindGenerator)
		{
			WindGenerator generator = (WindGenerator)SelectedObj;
			List<Item> batteries;
			pl.CharacterInventory.GetItemsByType(out batteries, ItemName.BATTERY);
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
			float charrechargeamm = pl.GetCharacterBatteryCap() - pl.GetCurrentCharacterEnergy();
			if (charrechargeamm > availableenergy)
			{
				pl.RechargeCharacter(charrechargeamm);
				rechargeamm += charrechargeamm;
			}
			else
			{
				pl.RechargeCharacter(availableenergy);
				rechargeamm += availableenergy;
			}
			generator.ConsumeEnergy(rechargeamm);
			int time = (int)Math.Round(rechargeamm / 6);
			int days, hours, mins;
			DayNight.MinsToTime(time, out days,out hours, out mins);
			DayNight.ProgressTime(days, hours, mins);
			
		}
		else if (SelectedObj is FireplaceLight)
		{
			((FireplaceLight)SelectedObj).ToggleFileplace();
		}
		else if (SelectedObj is SittingThing)
		{
			SittingThing sit = (SittingThing)SelectedObj;
			if (!sit.HasEmptySeat())
			{
				TalkText.GetInst().Talk("Δεν έχει χώρο.", pl);
				return;
			}
			if (pl.HasVecicle)
			{
				if (!pl.currveh.UnBoardVehicle(pl))
					return;
			}
			Position3D seat = sit.GetSeat();
			pl.Sit(seat, sit);
		}
		else if (SelectedObj is Ladder)
		{
			((Ladder)SelectedObj).TraverseLadder(pl);
		}
		else if (SelectedObj is GeneratorDoor)
		{
			((GeneratorDoor)SelectedObj).ToggleDoor();
		}
		selecting = false;
		Stop();
	}
	private void On_Interact_Button2_Down()
	{
		if (SelectedObj is Vehicle)
		{
			Vehicle veh = (Vehicle)SelectedObj;
			veh.ToggleMachine(!veh.Working);
		}
	}
	private void On_Interact_Button_Down()
	{
		if (SelectedObj is Item)
		{
			TalkText.GetInst().Talk(((Item)SelectedObj).GetItemName(), pl);
		}
		else if (SelectedObj is Character)
		{
			TalkText.GetInst().Talk("Φίλος", pl);
		}
        
		else if (SelectedObj is Vehicle)
		{
			TalkText.GetInst().Talk("Βάρκα", pl);
		}
		else if (SelectedObj is WindGenerator)
		{
			TalkText.GetInst().Talk("Γεννήτρια", pl);
		}
		else if (SelectedObj is Furniture)
		{
			Furniture furni = (Furniture)SelectedObj;
			if (furni.HasBeenSearched())
				TalkText.GetInst().Talk("Το έψαξα, είναι άδειο", pl);
			else
				TalkText.GetInst().Talk(furni.FurnitureDescription, pl);
		}
		else if (SelectedObj is SittingThing)
		{
			SittingThing sit = (SittingThing)SelectedObj;
			if (!sit.HasEmptySeat())
			{
				TalkText.GetInst().Talk("Δεν έχει χώρο.", pl);
				return;
			}
			TalkText.GetInst().Talk("Μπορώ να κάτσω.", pl);
		}
		else if (SelectedObj is Ladder)
		{
			TalkText.GetInst().Talk("Σκάλα", pl);
		}
	}
	public void Start(Spatial obj)
	{
		if (DialogueManager.IsPlayerTalking())
			return;
		if (selecting)
            return;
		DeselectCurrent();
		SelectedObj = obj;
		SelectedObj.Call("HighLightObject", true);
		PickButton.Show();
		IntButton.Show();
		if (SelectedObj is Item)
		{
			PickButton.Text = "Πάρε";
		}
		else if (SelectedObj is Character)
		{
			PickButton.Text = "Kουβέντα";
		}
		else if (SelectedObj is Vehicle)
		{
			if (pl.HasVecicle && pl.currveh == SelectedObj)
				PickButton.Text = "Αποβιβάση";
			else
				PickButton.Text = "Επιβιβάση";
		}
		else if (SelectedObj is Furniture)
		{
			PickButton.Text = "Ψάξε";
		}
		else if (SelectedObj is WindGenerator)
		{
			PickButton.Text = "Φόρτιση";
		}
		else if (SelectedObj is SittingThing)
		{
			PickButton.Text = "Κάτσε";
		}
		else if (SelectedObj is FireplaceLight)
		{
			IntButton.Hide();
			FireplaceLight fp = (FireplaceLight)SelectedObj;
			if (fp.State)
				PickButton.Text = "Σβήσε.";
			else
				PickButton.Text = "Άναψε.";
			//PickButton.Hide();
		}
		else if (SelectedObj is Ladder || SelectedObj is GeneratorDoor)
		{
			//IntButton.Hide();
			//FireplaceLight fp = (FireplaceLight)SelectedObj;

			PickButton.Text = (string)SelectedObj.Call("GetActionName", pl.GlobalTranslation);
			//PickButton.Hide();
		}
		Show();
		SetProcess(true);
	}
	void DeselectCurrent()
	{
		if (SelectedObj != null)
			SelectedObj.Call("HighLightObject", false);

		SelectedObj = null;
	}
	public void Stop()
	{
		if (PerformingAction)
		{
			PerformingAction = false;
			ActionIndex = 0;
			pl.loctomove = pl.GlobalTranslation;
		}
        if (selecting)
            return;
		if (SelectedObj == null)
			return;
		DeselectCurrent();
		
		Hide();
		SetProcess(false);
		//WarpMouse(GetViewport().Size/2);
		RectPosition = new Vector2 (0.0f, 0.0f);
	}
	
	public override void _Process(float delta)
	{
		Vector3 actionpos = (Vector3)SelectedObj.Call("GetActionPos", pl.GlobalTranslation);
		var screenpos = GetTree().Root.GetCamera().UnprojectPosition(actionpos);

		//Vector3 pos = SelectedObj.GlobalTransform.origin;

		//if (pl.GlobalTransform.origin.DistanceTo(pos) > 60 && selecting)
			//PickButton.Hide();
		//else
			//PickButton.Show();

		if (SelectedObj is Furniture)
		{
			Furniture furni = (Furniture)SelectedObj;
			if (furni.HasBeenSearched())
				PickButton.Hide();	
		}
		RectPosition = new Vector2 (screenpos.x, screenpos.y +50);

		if (screenpos < Vector2.Zero || screenpos > GetViewport().Size)
			Stop();

		if (PerformingAction)
		{
			if (actionpos.DistanceTo(pl.GlobalTranslation) <= SelectedObj.GetNode<ActionComponent>("ActionComponent").ActionDistance)
			{
				pl.loctomove = pl.GlobalTranslation;
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
			Vector2 mousepos = GetViewport().GetMousePosition();
			Camera cam = GetTree().Root.GetCamera();
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
			Vector3 actionpos = (Vector3)obj.Call("GetActionPos", pl.GlobalTranslation);
			if (actionpos.DistanceTo(pl.GlobalTranslation) > 100)
			{
				Stop();
				return;
			}
			Start(obj);
		}
		if (@event.IsActionPressed("ActionCheck"))
		{
			var interactables = GetTree().GetNodesInGroup("Interactables");
			foreach (Node inter in interactables)
			{
				inter.Call("HighLightObject", true);
			}
		}
		else
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






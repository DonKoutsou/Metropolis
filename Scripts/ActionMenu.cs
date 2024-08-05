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

	[Export]
	Material OutLineMat = null;
	
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
		VBoxContainer cont = GetNode<VBoxContainer>("VBoxContainer/PanelContainer/VBoxContainer");
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
		
		SelectedObj.Call("DoAction", pl);

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
		pl.GetTalkText().Talk((string)SelectedObj.Call("GetObjectDescription"));
	}
	public void Start(Spatial obj)
	{
		if (DialogueManager.IsPlayerTalking())
			return;
		if (selecting)
            return;
		

		if (obj is Vehicle v)
		{
			if (pl.HasVehicle() && pl.GetVehicle() == v)
				GetNode<VehicleHud>("VBoxContainer/VehicleUI").Visible = true;

		}
		else
			GetNode<VehicleHud>("VBoxContainer/VehicleUI").Visible = false;

		//PickButton.Show();
		IntButton.Show();

		PickButton.Text = (string)obj.Call("GetActionName", pl);
		PickButton.Visible = (bool)obj.Call("ShowActionName", pl);

		DeselectCurrent();
		SelectedObj = obj;
		SelectedObj.Call("HighLightObject", true, OutLineMat);
		
		Show();
		SetPhysicsProcess(true);
	}
	void DeselectCurrent()
	{
		if (SelectedObj != null)
			SelectedObj.Call("HighLightObject", false, OutLineMat);

		SelectedObj = null;
	}
	public void Stop()
	{
		if (SelectedObj == null)
			return;
		pl.GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").ResetHead();
		if (PerformingAction)
		{
			PerformingAction = false;
			ActionIndex = 0;
			pl.UpdateLocationToMove(pl.GlobalTranslation);
		}
		if (selecting)
		{
            return;
		}

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

		if (SelectedObj is Pod p)
		{
			if (p.IsOpen())
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
					inter.Call("HighLightObject", true, OutLineMat);
			}
		}
		else if (@event.IsActionReleased("ActionCheck"))
		{
			var interactables = GetTree().GetNodesInGroup("Interactables");
			foreach (Node inter in interactables)
			{
				if (inter != SelectedObj)
					inter.Call("HighLightObject", false, OutLineMat);
			}
		}
	}
}






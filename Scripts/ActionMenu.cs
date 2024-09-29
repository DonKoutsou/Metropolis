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
	Node SelectedObj;
	Player Play;
    bool selecting = false;
    Button PickButton;
	Button IntButton;
	Button IntButton2;
	Button IntButton3;
	//[Export]
	//int VisibleInteractableDistance = 100;

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
		IntButton3 = cont.GetNode<Button>("Interact_Button3");
		IntButton.Hide();
		PickButton.Hide();
		IntButton2.Hide();
		IntButton3.Hide();

		SetProcessInput(false);
	}
	List<Node> InteractableNodes = new List<Node>();
	int CurrentSelected = 0;
	public void ActionObjectInteraction(bool EnterBool, Node body)
	{
		body.Call("HighLightObject", EnterBool, OutLineMat);
		if (EnterBool)
			InteractableNodes.Add(body);
		else
		{
			InteractableNodes.Remove(body);
			Stop();
		}
	}
	public void ConnectPlayer(Player pl)
	{
		Play = pl;
		SetProcessInput(true);
		pl.Connect("InteractableObjectEntered", this, "ActionObjectInteraction");
	}
	public void DissconnectPlayer()
	{
		SetProcessInput(false);
	}
	public bool IsSelecting()
	{
		return SelectedObj != null;
	}
	public bool IsSelected(Node obj)
	{
		return SelectedObj == obj;
	}
	public void StartPerformingAction(int type)
	{
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		Play.UpdateLocationToMove(Acomp.GetActionPos(Play.GlobalTranslation));
		PerformingAction = true;
		ActionIndex = type;
	}
	private void On_PickUp_Button_Down()
	{
		if (Play.HasVehicle() && Play.GetVehicle() != SelectedObj)
		{
			DialogueManager.GetInstance().ForceDialogue(Play, "Δεν μπορώ πάνω από την βάρκα");
			//Play.GetTalkText().Talk("Δεν μπορώ πάνω από την βάρκα");
		}
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		Vector3 actionpos = Acomp.GetActionPos(Play.GlobalTranslation);
		if (actionpos.DistanceTo(Play.GlobalTranslation) > Acomp.ActionDistance)
		{
			if (!PerformingAction)
			{
				StartPerformingAction(0);
			}
			return;
		}
		
		SelectedObj.Call("DoAction", Play);

		selecting = false;
		Stop();
		CurrentSelected = 0;
	}
	private void On_Interact_Button2_Down()
	{
		if (Play.HasVehicle() && Play.GetVehicle() != SelectedObj)
		{
			DialogueManager.GetInstance().ForceDialogue(Play, "Δεν μπορώ πάνω από την βάρκα");
			//Play.GetTalkText().Talk("Δεν μπορώ πάνω από την βάρκα");
		}
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		Vector3 actionpos = Acomp.GetActionPos(Play.GlobalTranslation);
		if (actionpos.DistanceTo(Play.GlobalTranslation) > Acomp.ActionDistance)
		{
			StartPerformingAction(2);
			return;
		}
		
		SelectedObj.Call("DoAction2", Play);

		selecting = false;
		Stop();
		CurrentSelected = 0;
	}
	private void On_Interact_Button3_Down()
	{
		if (Play.HasVehicle() && Play.GetVehicle() != SelectedObj)
		{
			DialogueManager.GetInstance().ForceDialogue(Play, "Δεν μπορώ πάνω από την βάρκα");
			//Play.GetTalkText().Talk("Δεν μπορώ πάνω από την βάρκα");
		}
		ActionComponent Acomp = SelectedObj.GetNode<ActionComponent>("ActionComponent");
		Vector3 actionpos = Acomp.GetActionPos(Play.GlobalTranslation);
		if (actionpos.DistanceTo(Play.GlobalTranslation) > Acomp.ActionDistance)
		{
			if (!PerformingAction)
			{
				StartPerformingAction(3);
			}
			return;
		}
		
		SelectedObj.Call("DoAction3", Play);

		selecting = false;
		Stop();
		CurrentSelected = 0;
	}
	private void On_Interact_Button_Down()
	{
		DialogueManager.GetInstance().ForceDialogue(Play, (string)SelectedObj.Call("GetObjectDescription"));
		//Play.GetTalkText().Talk((string)SelectedObj.Call("GetObjectDescription"));
	}
	public void Start(Node obj)
	{
		if (Play.BeingTalkedTo)
			return;
		if (selecting)
            return;

		if (obj is Vehicle v)
		{
			if (Play.HasVehicle() && Play.GetVehicle() == v)
			{
				GetNode<VehicleHud>("VBoxContainer/VehicleUI").Visible = true;
				GetNode<Button>("VBoxContainer/VehicleUI/Panel/MarginContainer/HBoxContainer/Label/EngineToggle").FocusMode = FocusModeEnum.All;
			}
		}
		else
		{
			GetNode<VehicleHud>("VBoxContainer/VehicleUI").Visible = false;
			GetNode<Button>("VBoxContainer/VehicleUI/Panel/MarginContainer/HBoxContainer/Label/EngineToggle").FocusMode = FocusModeEnum.None;
		}

		//PickButton.Show();
		IntButton.Show();

		PickButton.Text = (string)obj.Call("GetActionName", Play);
		PickButton.Visible = (bool)obj.Call("ShowActionName", Play);

		IntButton2.Text = (string)obj.Call("GetActionName2", Play);
		IntButton2.Visible = (bool)obj.Call("ShowActionName2", Play);

		IntButton3.Text = (string)obj.Call("GetActionName3", Play);
		IntButton3.Visible = (bool)obj.Call("ShowActionName3", Play);

		DeselectCurrent();
		SelectedObj = obj;
		SelectedObj.Call("HighLightObject", true, OutLineMat);

		if (ControllerInput.IsUsingController())
		{
			IntButton.GrabFocus();
		}
		
		Show();
		SetPhysicsProcess(true);
	}
	void DeselectCurrent()
	{
		if (SelectedObj != null && !InteractableNodes.Contains(SelectedObj))
			SelectedObj.Call("HighLightObject", false, OutLineMat);

		SelectedObj = null;
	}
	public void Stop()
	{
		if (SelectedObj == null)
			return;

		Play.GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").ResetHead();
		if (PerformingAction)
		{
			PerformingAction = false;
			ActionIndex = 0;
			Play.UpdateLocationToMove(Play.GlobalTranslation);
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
		Vector3 actionpos = Acomp.GetActionPos(Play.GlobalTranslation);


		Vector2 s = GetViewportRect().Size;
		float mult =  s.x / DViewport.GetInstance().Size.x;
		var screenpos = DViewport.GetInstance().GetCamera().UnprojectPosition(actionpos);

		Play.GetNode<LoddedCharacter>("Pivot/Guy/Armature/Skeleton").HeadLookAt(actionpos);
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

		RectPosition = new Vector2 (screenpos.x, screenpos.y +50) * mult;

		if (screenpos < Vector2.Zero || screenpos > s)
		{
			CurrentSelected = 0;
			Stop();
		}

		if (PerformingAction)
		{
			Vector3 PlGlobalPos = Play.GlobalTranslation;
			if (actionpos.DistanceTo(PlGlobalPos) <= SelectedObj.GetNode<ActionComponent>("ActionComponent").ActionDistance)
			{
				Play.UpdateLocationToMove(PlGlobalPos);
				
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
				if (ActionIndex == 3)
				{
					On_Interact_Button3_Down();
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
	private void ActionLocationReached()
	{

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
			float mult = GetViewportRect().Size.x / DViewport.GetInstance().Size.x;
			Vector2 mousepos = GetViewport().GetMousePosition() / mult;
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

			Vector3 PlayPos = Play.GlobalTranslation;
			
			ActionComponent Acomp = obj.GetNode<ActionComponent>("ActionComponent");
			
			if (Acomp == null)
			{
				return;
			}
				
			Vector3 actionpos = Acomp.GetActionPos(PlayPos);

			if (actionpos.DistanceTo(PlayPos) > 100)
			{
				Stop();
				return;
			}
			Start(obj);
		}
		if (@event.IsActionPressed("NextInteractable"))
		{
			if (InteractableNodes.Count > CurrentSelected)
			{
				CurrentSelected ++;
				Start(InteractableNodes[CurrentSelected-1]);
			}
			else
			{
				CurrentSelected = 0;
				Stop();
			}
		}
		/*if (@event.IsActionPressed("ActionCheck"))
		{
			Vector3 plpos = Play.GlobalTranslation;
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
		}*/
	}
}






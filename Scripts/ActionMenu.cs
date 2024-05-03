using Godot;
using System;
using System.Collections.Generic;
public class ActionMenu : Control
{
	static Spatial SelectedObj;

	Player pl;

    static bool selecting = false;

    Button PickButton;
	Button IntButton;


	public override void _Ready()
	{
		pl = (Player)GetParent().GetParent();
		SetProcess(false);
		VBoxContainer cont = GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer");
        PickButton = cont.GetNode<Button>("PickUp_Button");
		IntButton = cont.GetNode<Button>("Interact_Button2");
	}

	public static bool IsSelecting()
	{
		return SelectedObj != null;
	}
	private void On_PickUp_Button_Down()
	{
		if (SelectedObj is Item)
		{
			Item it = (Item)SelectedObj;
			if (!pl.GetCharacterInventory().InsertItem(it))
			{
				TalkText.GetInst().Talk("Δέν έχω χώρο.", pl);
			}
			if (it.GetItemType() == (int)ItemName.ROPE)
			{
				MeshInstance rope = pl.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment2").GetNode<MeshInstance>("Rope");
				rope.Show();
			}
			selecting = false;
			Stop();
		}
		else if (SelectedObj is Character)
		{
			TalkText.GetInst().Talk("Φίλος", (Character)SelectedObj);
		}
		else if (SelectedObj is Vehicle)
		{
			Vehicle veh = (Vehicle)SelectedObj;
			if (!pl.HasVehicle())
			{
				veh.BoardVehicle(pl);
				pl.SetVehicle(veh);
				selecting = false;
				Stop();
			}
			else
			{
				veh.UnBoardVehicle(pl);
				pl.SetVehicle(null);
				selecting = false;
				Stop();
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
				TalkText.GetInst().Talk("Ένα " + foundit.GetItemName(), pl);
			}
			else
				TalkText.GetInst().Talk("Τίποτα", pl);
			selecting = false;
			Stop();
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
			int time = (int)Math.Round(rechargeamm / 10);
			int days, hours, mins;
			DayNight.MinsToTime(time, out days,out hours, out mins);
			DayNight.ProgressTime(days, hours, mins);
			selecting = false;
			Stop();
		}
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
        else if (SelectedObj is FireplaceLight)
		{
			((FireplaceLight)SelectedObj).ToggleFileplace();
			selecting = false;
			Stop();
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
	}
	public void Start(Spatial obj)
	{
		if (selecting)
            return;
		DeselectCurrent();
		SelectedObj = obj;
		if (SelectedObj is Item)
		{
			PickButton.Text = "Πάρε";
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		}
		else if (SelectedObj is Character)
		{
			PickButton.Text = "Kουβέντα";

			((ShaderMaterial)SelectedObj.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
			((ShaderMaterial)SelectedObj.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh003").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		}
		else if (SelectedObj is FireplaceLight)
		{
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		}
		else if (SelectedObj is Vehicle)
		{
			PickButton.Text = "Επιβιβάσου";
			//IntButton.Text = "Έναρξη";
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		}
		else if (SelectedObj is Furniture)
		{
			PickButton.Text = "Ψάξε";
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		}
		else if (SelectedObj is WindGenerator)
		{
			PickButton.Text = "Φόρτιση";
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance2").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		}

		Show();
		SetProcess(true);
	}
	void DeselectCurrent()
	{
		if (SelectedObj is Item)
		{
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		}
		else if (SelectedObj is Character)
		{
			((ShaderMaterial)SelectedObj.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			((ShaderMaterial)SelectedObj.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh003").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		}
		else if (SelectedObj is FireplaceLight)
		{
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		}
		else if (SelectedObj is Vehicle)
		{
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		}
		else if (SelectedObj is Furniture)
		{
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		}
		else if (SelectedObj is WindGenerator)
		{
			((ShaderMaterial)SelectedObj.GetNode<MeshInstance>("MeshInstance2").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		}
		SelectedObj = null;
		
	}
	public void Stop()
	{
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
		var screenpos = Vector2.Zero;
		if (SelectedObj is Item)
		{
			IntButton.Hide();
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedObj.GlobalTransform.origin);
			Vector3 pos = SelectedObj.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 10)
				PickButton.Hide();
			else
				PickButton.Show();
		}
		else if (SelectedObj is Character)
		{
			IntButton.Hide();
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedObj.GlobalTransform.origin);
			Vector3 pos = SelectedObj.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 10)
				PickButton.Hide();
			else
				PickButton.Show();
		}
		else if (SelectedObj is FireplaceLight)
		{
			IntButton.Hide();
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedObj.GlobalTransform.origin);
			PickButton.Hide();
		}
		else if (SelectedObj is Vehicle)
		{
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedObj.GlobalTransform.origin);
			Vector3 pos = SelectedObj.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 30)
			{
				PickButton.Hide();
				IntButton.Hide();
			}
			else
			{
				PickButton.Show();

				IntButton.Hide();
			}
				
		}
		else if (SelectedObj is Furniture)
		{
			IntButton.Hide();
			Furniture furni = (Furniture)SelectedObj;
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedObj.GlobalTransform.origin);
			Vector3 pos = SelectedObj.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) < 30 && !furni.HasBeenSearched())
				PickButton.Show();
			else
				PickButton.Hide();	
		}
		else if (SelectedObj is WindGenerator)
		{
			IntButton.Hide();
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedObj.GlobalTransform.origin);
			Vector3 pos = SelectedObj.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 60)
				PickButton.Hide();
			else
				PickButton.Show();
		}
		RectPosition = new Vector2 (screenpos.x, screenpos.y +50);

		if (screenpos < Vector2.Zero || screenpos > GetViewport().Size)
			Stop();
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

}






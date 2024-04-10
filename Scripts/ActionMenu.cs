using Godot;
using System;

public class ActionMenu : Control
{
	static Item SelectedItem;

	static Character SelectedChar;

	static FireplaceLight Fireplace;

	static Vehicle vehicle;

	Player pl;

    static bool selecting = false;

    Button PickButton;

	public static bool IsSelecting()
	{
		return SelectedItem != null || SelectedChar != null || Fireplace != null || vehicle != null;
	}
	private void On_PickUp_Button_Down()
	{
		if (SelectedItem != null)
		{
			Spatial house = (Spatial)SelectedItem.GetParent();
			if (house != null && house is House)
				((House)house).OnItemPicked();

			if (!pl.GetCharacterInventory().InsertItem(SelectedItem))
			{
				TalkText.GetInst().Talk("Δέν έχω χώρο.", pl);
			}
			if (SelectedItem.GetItemType() == (int)ItemName.ROPE)
			{
				MeshInstance rope = pl.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment2").GetNode<MeshInstance>("Rope");
				rope.Show();
			}
			DeselectCurrent();
			selecting = false;
			Stop();
		}
		if (SelectedChar != null)
		{
			TalkText.GetInst().Talk("Φίλος", SelectedChar);
		}
		if (vehicle != null)
		{
			if (!pl.HasVehicle())
			{
				vehicle.BoardVehicle(pl);
				pl.SetVehicle(vehicle);
				DeselectCurrent();
				selecting = false;
				Stop();
			}
			else
			{
				vehicle.UnBoardVehicle(pl);
				pl.SetVehicle(null);
				DeselectCurrent();
				selecting = false;
				Stop();
			}
		}
	}
	private void On_Interact_Button_Down()
	{
		if (SelectedItem != null)
		{
			TalkText.GetInst().Talk(SelectedItem.GetItemName(), pl);
		}
		if (SelectedChar != null)
		{
			TalkText.GetInst().Talk("Φίλος", pl);
		}
        if (Fireplace != null)
		{
			Fireplace.ToggleFileplace();
			DeselectCurrent();
			selecting = false;
			Stop();
		}
		if (vehicle != null)
		{
			TalkText.GetInst().Talk("Βάρκα", pl);
		}
	}
	
	public void Start(Item obj)
	{
        if (selecting)
            return;

		DeselectCurrent();

		SelectedItem = obj;
		PickButton.Text = "Πάρε";
		((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);

		Show();
		SetProcess(true);
	}
	public void Start(Character obj)
	{
		if (selecting)
            return;

		DeselectCurrent();
		
		SelectedChar = obj;
		PickButton.Text = "Kουβέντα";

		((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh003").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		
		Show();
		SetProcess(true);
	}
	public void Start(FireplaceLight obj)
	{
		if (selecting)
            return;

		DeselectCurrent();
			
		Fireplace = obj;
		((ShaderMaterial)Fireplace.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		
		Show();
		SetProcess(true);
	}
	public void Start(Vehicle veh)
	{
		if (selecting)
            return;

		DeselectCurrent();
			
		vehicle = veh;
		PickButton.Text = "Επιβιβάσου";
		((ShaderMaterial)vehicle.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		
		Show();
		SetProcess(true);
	}
	void DeselectCurrent()
	{
		if (SelectedItem != null)
		{
			((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedItem = null;
		}
		if (SelectedChar != null)
		{
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("Mesh003").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedChar = null;
		}
		if (Fireplace != null)
		{
			((ShaderMaterial)Fireplace.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			Fireplace = null;
		}
		if (vehicle != null)
		{
			((ShaderMaterial)vehicle.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			vehicle = null;
		}
	}
	public void Stop()
	{
        if (selecting)
            return;

		DeselectCurrent();
		
		Hide();
		SetProcess(false);
		RectPosition = new Vector2 (0.0f, 0.0f);
	}
	public override void _Ready()
	{
		pl = (Player)GetParent().GetParent();
		SetProcess(false);
        PickButton = GetNode<Button>("PickUp_Button");
	}
	public override void _Process(float delta)
	{
		var screenpos = Vector2.Zero;
		if (SelectedItem != null)
		{
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedItem.GlobalTransform.origin);
			Vector3 pos = SelectedItem.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 10)
				PickButton.Hide();
			else
				PickButton.Show();
		}
		else if(SelectedChar != null)
		{
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedChar.GlobalTransform.origin);
			Vector3 pos = SelectedChar.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 10)
			{
				PickButton.Hide();
			}
			else
			{
				PickButton.Show();
			}
		}
		else if (Fireplace != null)
		{
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(Fireplace.GlobalTransform.origin);
			PickButton.Hide();
		}
		else if (vehicle != null)
		{
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(vehicle.GlobalTransform.origin);
			Vector3 pos = vehicle.GlobalTransform.origin;
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 30)
			{
				PickButton.Hide();
			}
			else
			{
				PickButton.Show();
			}
				
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






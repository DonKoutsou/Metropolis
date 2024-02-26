using Godot;
using System;

public class ActionMenu : Control
{

	Item SelectedItem;

	Character SelectedChar;

	Player pl;

    bool selecting = false;

    Button PickButton;

	private void On_PickUp_Button_Down()
	{
		if (SelectedItem == null)
			return;
		pl.GetCharacterInventory().InsertItem(SelectedItem);
        selecting = false;
		Stop();
	}
	private void On_Interact_Button_Down()
	{
		if (SelectedItem != null)
		{
			TalkText.GetInst().Talk(SelectedItem.GetItemName());
		}
		if (SelectedChar != null)
		{
			TalkText.GetInst().Talk("Φίλος");
		}
        
	}
	
	public void Start(Item obj)
	{
        if (selecting)
            return;
		//GetNode<Button>("PickUp_Button").focus;
		if (SelectedItem != null)
		{
			((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedItem = null;
		}
			
		if (SelectedChar != null)
		{
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface36").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface14001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedChar = null;
		}
		SelectedItem = obj;
		((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		Show();
		SetProcess(true);
	}
	public void Start(Character obj)
	{
		if (selecting)
            return;
		//GetNode<Button>("PickUp_Button").focus;
		if (SelectedItem != null)
		{
			((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedItem = null;
		}
			
		if (SelectedChar != null)
		{
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface36").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface14001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedChar = null;
		}
			

		SelectedChar = obj;
		((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface36").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface14001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);

		Show();
		SetProcess(true);
	}
	public void Stop()
	{
        if (selecting)
            return;
		if (SelectedItem != null)
		{
			((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedItem = null;
		}
		if (SelectedChar != null)
		{
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface36").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			((ShaderMaterial)SelectedChar.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<MeshInstance>("polySurface14001").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
			SelectedChar = null;
		}
		
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
			if (pl.GlobalTransform.origin.DistanceTo(pos) > 5)
				PickButton.Hide();
			else
				PickButton.Show();
		}
		else if(SelectedChar != null)
		{
			screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedChar.GlobalTransform.origin);
			PickButton.Hide();
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






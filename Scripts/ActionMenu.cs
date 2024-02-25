using Godot;
using System;

public class ActionMenu : Control
{

	Item SelectedItem;

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
		if (SelectedItem == null)
			return;
        TalkText.GetInst().Talk(SelectedItem.GetItemName());
	}
	
	public void Start(Item obj)
	{
        if (selecting)
            return;
		//GetNode<Button>("PickUp_Button").focus;
		if (SelectedItem != null)
			((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		SelectedItem = obj;
		((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", true);
		Show();
		SetProcess(true);
	}
	public void Stop()
	{
        if (selecting)
            return;
		if (SelectedItem != null)
			((ShaderMaterial)SelectedItem.GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", false);
		SelectedItem = null;
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
		var screenpos = GetTree().Root.GetCamera().UnprojectPosition(SelectedItem.GlobalTransform.origin);
		RectPosition = new Vector2 (screenpos.x, screenpos.y +50);
        if (pl.GlobalTransform.origin.DistanceTo(SelectedItem.GlobalTransform.origin) > 5)
            PickButton.Hide();
        else
            PickButton.Show();
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






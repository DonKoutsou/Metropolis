using Godot;
using System;

public class LightHouse : House
{
	public bool Enabled = false;
	public override void _Ready()
	{
		base._Ready();
		ToggeLightHouse(Enabled);
	}
    public override void Entered(Node body)
	{
		//((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		//((SpatialMaterial)GetNode<MeshInstance>("Walls").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		GetNode<Occluder>("Occluder").Visible = false;
		GetNode<Occluder>("Occluder2").Visible = false;

		GetNode<Spatial>("Furnitures").Show();
		GetNode<Spatial>("Decorations").Show();

		AnimationPlayer Anim = GetNode<AnimationPlayer>("AnimationPlayer");
		Anim.Play("Open");
		
		return;

	}
	protected override void On_Door_Animation_Finished(string anim)
	{
		if (anim == "Close")
		{
			//((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
			//((SpatialMaterial)GetNode<MeshInstance>("Walls").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
			GetNode<Occluder>("Occluder").Visible = true;
			GetNode<Occluder>("Occluder2").Visible = true;
			GetNode<Spatial>("Furnitures").Hide();
			GetNode<Spatial>("Decorations").Hide();
		}
	}
	public override void Left(Node body)
	{
		AnimationPlayer Anim = GetNode<AnimationPlayer>("AnimationPlayer");
		Anim.Play("Close");
		return;
	}
	public void ToggeLightHouse(bool t)
	{
		Spatial LightHouseParts = GetNode<Spatial>("LightHouse_Particles");

		foreach (Particles parts in LightHouseParts.GetChildren())
		{
			parts.Emitting = t;
		}	
	}
	public void FixLightHouse()
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "FixLightHouse");

		AnimationPlayer anim = GetNode<AnimationPlayer>("LightHouse_UnlockedAnimation/AnimationPlayer");
		anim.Play("EnabledAnim");

		ToggeLightHouse(true);
	}
	private void EnableAnimationFinished(string anim)
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Connect("FadeOutFinished", this, "FadeFin");
		CameraAnimation.FadeInOut(1);
	}
	private void FadeFin()
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "FadeFin");
		PlayerCamera.GetInstance().Current = true;
	}
}

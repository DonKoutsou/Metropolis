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

		base.Entered(body);

	}
	
	public override void Left(Node body)
	{
		GetNode<Occluder>("Occluder").Visible = true;
		GetNode<Occluder>("Occluder2").Visible = true;
		base.Left(body);
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

		GetNode<Camera>("LightHouse_UnlockedAnimation/Camera").Current = true;

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
		WorldMap map = WorldMap.GetInstance();
        map.UnlockLightHouse(map.GetCurrentIleInfo());
		
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "FadeFin");
		
		PlayerCamera.GetInstance().Current = true;
	}
}

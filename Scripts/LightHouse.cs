using Godot;
using System;

public class LightHouse : House
{
	public override void _Ready()
	{
		base._Ready();

		foreach(Particles p in GetNode<Spatial>("LightHouse_Particles").GetChildren())
		{
			p.Emitting = false;
		}
	}
    public override void Entered(Node body)
	{
		if (!IsInsideTree())
            return;
		((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		((SpatialMaterial)GetNode<MeshInstance>("Walls").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		GetNode<Occluder>("Occluder").Visible = false;
		GetNode<Occluder>("Occluder2").Visible = false;
		
		return;

	}
	public override void Left(Node body)
	{
		if (!IsInsideTree())
            return;
		((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		((SpatialMaterial)GetNode<MeshInstance>("Walls").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		GetNode<Occluder>("Occluder").Visible = true;
		GetNode<Occluder>("Occluder2").Visible = true;
		
		return;

	}
	public void EnableLightHouse()
	{
		foreach(Particles p in GetNode<Spatial>("LightHouse_Particles").GetChildren())
		{
			p.Emitting = true;
		}
	}
}

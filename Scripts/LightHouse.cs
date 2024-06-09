using Godot;
using System;

public class LightHouse : House
{
    public override void Entered(Node body)
	{
		((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		GetNode<Occluder>("Occluder").Visible = false;
		GetNode<Occluder>("Occluder2").Visible = false;

		
		return;

	}
	public override void Left(Node body)
	{
		((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		GetNode<Occluder>("Occluder").Visible = true;
		GetNode<Occluder>("Occluder2").Visible = true;
		
		return;

	}
}

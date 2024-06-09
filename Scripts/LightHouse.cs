using Godot;
using System;

public class LightHouse : House
{
	public override void _Ready()
	{
		Node parent = GetParent();
		
		while (!(parent is Island))
		{
			if (parent == null)
				return;
			parent = parent.GetParent();
		}
		Island ile = (Island)parent;
		ile.RegisterChild(this);
	}
    public override void Entered(Node body)
	{
		((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		((SpatialMaterial)GetNode<MeshInstance>("Walls").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		GetNode<Occluder>("Occluder").Visible = false;
		GetNode<Occluder>("Occluder2").Visible = false;
		
		return;

	}
	public override void Left(Node body)
	{
		((SpatialMaterial)GetNode<MeshInstance>("LightHouseBody").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		((SpatialMaterial)GetNode<MeshInstance>("Walls").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		GetNode<Occluder>("Occluder").Visible = true;
		GetNode<Occluder>("Occluder2").Visible = true;
		
		return;

	}
}

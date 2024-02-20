using Godot;
using System;

public class HouseDoor : Door
{
	bool m_Knocked = false;

	//House ParentHouse;

	StaticBody HouseExterior;

	public override void _Ready()
	{
		HouseExterior = GetParent().GetNode<StaticBody>("HouseExterior");
		//ParentHouse = (House)GetParent();
	}
	public bool GetKnocked()
	{
		return m_Knocked;
	}
	public override void Touch(object body)
	{
		Vector3 forw = GlobalTransform.basis.z;
		Vector3 toOther = GetNode<CollisionShape>("CollisionShape").GlobalTransform.origin - ((Spatial)body).GlobalTransform.origin;
		var thing = forw.Dot(toOther);
		if (thing < 0)
		{
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetSurfaceMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetSurfaceMaterial(2)).ParamsCullMode = SpatialMaterial.CullMode.Front;
		}
		else
		{
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetSurfaceMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
			((SpatialMaterial)HouseExterior.GetNode<MeshInstance>("MeshInstance").GetSurfaceMaterial(2)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
		}
	}
	public bool Knock()
	{
		//m_Knocked = true;
		//GetNode<AudioStreamPlayer2D>("DoorKnockSound").Play();
		//EmitSignal(nameof(OnKnocked), this);
		//return !ParentHouse.GetIsEmpty();
		return false;
	}
	[Signal]
	public delegate void OnKnocked(HouseDoor door);
}

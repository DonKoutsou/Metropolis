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
		Vector3 toOther = GlobalTransform.origin - ((Spatial)body).GlobalTransform.origin;
		var thing = forw.Dot(toOther);
		if (thing < 0)
		{
            HouseExterior.Hide();
        }
        else
        {
            HouseExterior.Show();
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

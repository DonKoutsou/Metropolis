using Godot;
using System;

public class HouseDoor : Door
{
    bool m_Knocked;

    House ParentHouse;

    public override void _Ready()
	{
        ParentHouse = (House)GetParent();
	}
    public bool GetKnocked()
    {
        return m_Knocked;
    }
    public override void Touch(object body)
	{
		
	}
    public bool Knock()
    {
        m_Knocked = true;
        GetNode<AudioStreamPlayer2D>("DoorKnockSound").Play();
        EmitSignal(nameof(OnKnocked), this);
        //return !ParentHouse.GetIsEmpty();
        return false;
    }
    [Signal]
    public delegate void OnKnocked(HouseDoor door);
}

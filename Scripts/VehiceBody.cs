using Godot;
using System;

public class Vehicebody : VehicleBody
{
    Position3D SteeringWheel;
    public override void _Ready()
    {
        base._Ready();
        SteeringWheel = GetNode<Position3D>("SteeringWheel");
    }
    public float GetSteer(Vector3 loc)
    {
        SteeringWheel.LookAt(loc, Vector3.Up);
        float steer = SteeringWheel.Rotation.y;
        if (steer > 0)
        {
            steer = Math.Min(30, steer);
        }
        if (steer < 0)
        {
            steer = Math.Max(-30, steer);
        }
        return steer;
    }
    public void BoardVehicle(Character cha)
    {
        //Rotation = new Vector3(Rotation.x,Rotation.y,0.0f);
        //cha.GlobalTranslation = GlobalTranslation;
        cha.GetParent().RemoveChild(cha);

        Spatial guy = cha.GetNode<Spatial>("Pivot");
        
        
        AddChild(cha);
        //cha.Translation = new Vector3 (0,0,0);
        cha.OnVehicleBoard();
        cha.Transform = GetNode<Position3D>("Position3D").Transform;
        guy.Rotation = new Vector3(0,0,0);
        
        //Translation = new Vector3(0.0f,1.0f,0.0f);
    }
     public void UnBoardVehicle(Character cha)
    {
        cha.GetParent().RemoveChild(cha);

        
        MyWorld.GetInstance().AddChild(cha);
        cha.OnVehicleUnBoard();
        cha.GlobalTranslation = GlobalTranslation;
        cha.Rotation = new Vector3(0,0,0);
    }
}

using Godot;
using System;
using System.Collections.Generic;

public class Vehicle : RigidBody
{
    [Export]
    float speed = 15000;
    [Export]
    float turnspeed = 2000;
    [Export]
    int HoverForce = 500;
    Position3D SteeringWheel;

    List<RayCast> Rays = new List<RayCast>();

    public Vector3 loctomove;

    bool Working = false;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        for (int i = 0; i < 4; i ++)
        {
            RayCast ray = Rays[i];
            ray.ForceRaycastUpdate();
            if (ray.IsColliding())
            {
                var collisionpoint = ray.GetCollisionPoint();
                var dist = collisionpoint.DistanceTo(ray.GlobalTransform.origin);
                float distmulti = dist / - ray.CastTo.y;
                AddForce(Vector3.Up * (1 - distmulti)* HoverForce * delta, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
        }
        if (!Working)
            return;

        float distance = loctomove.DistanceTo(GlobalTransform.origin);
        if (distance < 1)
        {
            return;
        }
        Vector3 force = GlobalTransform.basis.z;
        AddCentralForce(force * (speed * (Math.Min(100, distance)/ 100)) * delta);
        float steer = GetSteer(loctomove);
        if (steer > 0)
        {
            AddTorque(-GlobalTransform.basis.y * turnspeed * delta);
        }
        if (steer < 0)
        {
            AddTorque(GlobalTransform.basis.y * turnspeed * delta);
        }
    }
    public override void _Ready()
    {
        base._Ready();
        SteeringWheel = GetNode<Position3D>("SteeringWheel");
        
        Spatial parent = (Spatial)GetParent();
        Rays.Insert(0, parent.GetNode<RayCast>("RayFL"));
        Rays.Insert(1, parent.GetNode<RayCast>("RayFR"));
        Rays.Insert(2, parent.GetNode<RayCast>("RayBL"));
        Rays.Insert(3, parent.GetNode<RayCast>("RayBR"));
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
        
        GetParent().AddChild(cha);
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = cha.GetPath();
        //cha.Translation = new Vector3 (0,0,0);
        cha.OnVehicleBoard();
        cha.Transform = CharTrasn.Transform;
        guy.Rotation = new Vector3(0,0,0);
        Working = true;
        
        //Translation = new Vector3(0.0f,1.0f,0.0f);
    }
     public void UnBoardVehicle(Character cha)
    {
        cha.GetParent().RemoveChild(cha);

        
        MyWorld.GetInstance().AddChild(cha);
        cha.OnVehicleUnBoard();
        cha.GlobalTranslation = GlobalTranslation;
        cha.Rotation = new Vector3(0,0,0);
        Working = false;
    }
}

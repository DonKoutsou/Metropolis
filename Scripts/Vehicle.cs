using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Vehicle : RigidBody
{
    [Export]
    float speed = 15000;
    [Export]
    float turnspeed = 2000;
    [Export]
    int HoverForce = 500;

    [Export]
    Curve forcecurve = null;

    [Export]
    Curve Hoverforcecurve = null;
    Position3D SteeringWheel;

    List<RayCast> Rays = new List<RayCast>();

    RayCast frontray;

    public Vector3 loctomove;

    bool Working = false;

    List<Character> passengers = new List<Character>();

    float latsspeed;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        bool strong = false;
        float hoverforce2 = Hoverforcecurve.Interpolate(latsspeed /700000) * HoverForce;
        float forcemulti = 1;
        frontray.ForceRaycastUpdate();
        if (frontray.IsColliding())
            forcemulti = 2;
        bool past = false;
        for (int i = 0; i < Rays.Count; i ++)
        {
            RayCast ray = Rays[i];
            ray.ForceRaycastUpdate();
            
            if (Math.Abs(GlobalTransform.origin.y - ray.GlobalTransform.origin.y) > 3)
                past = true;
            
            
            if (ray.IsColliding())
            {
                var collisionpoint = ray.GetCollisionPoint();
                var dist = collisionpoint.DistanceTo(ray.GlobalTransform.origin);
                float distmulti = dist / - ray.CastTo.y;
                float multi = forcecurve.Interpolate(distmulti);
                if (dist < 4)
                {
                    strong = true;
                    multi *= 4;
                }
                
                Vector3 f = Vector3.Zero;
                //if (past)
                    //continue;
                //if (!past)
                    f= (Vector3.Up * hoverforce2 * delta) * multi;
                //else
                    //f= (Vector3.Up * HoverForce * delta) * -multi;
                AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
            else
            {
                Vector3 f = Vector3.Zero;
                //if (past)
                    //continue;
                f = (Vector3.Up * hoverforce2 * delta) * -8;
                //else
                   // f= (Vector3.Up * HoverForce * delta) * 8;

                AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
        }
        if (!Working)
            return;

        float distance = loctomove.DistanceTo(GlobalTransform.origin);
        if (distance < 10)
        {
            latsspeed = 0;
            //AddTorque(Vector3.Zero);
            return;
        }
        Vector3 force = Vector3.Zero;
        if (strong)
            force = -GlobalTransform.basis.z;
        else
            force = GlobalTransform.basis.z;
        force.y = 0;
        latsspeed = speed * (Math.Min(500, distance - 10)/ 500);
        AddCentralForce(force * latsspeed * delta);
        
        float rotx = Mathf.Rad2Deg(Rotation.x);
        if (rotx > 25)
        {
            Vector3 torq = -GlobalTransform.basis.x * turnspeed * delta;
            AddTorque(torq);
        }
        if (rotx < -25)
        {
            Vector3 torq = GlobalTransform.basis.x * turnspeed * delta;
            AddTorque(torq);
        }
        

        if (rotx > 45)
            Capsize();
        if (rotx < -45)
            Capsize();
        float rotz = Mathf.Rad2Deg(Rotation.z);
        if (rotz > 45)
            Capsize();
        if (rotz < -45)
            Capsize();
        if (rotz > 25)
        {
            Vector3 torq = -GlobalTransform.basis.z * turnspeed * delta;
            AddTorque(torq);
        }
        if (rotz < -25)
        {
            Vector3 torq = GlobalTransform.basis.z * turnspeed * delta;
            AddTorque(torq);
        }
        float steer = GetSteer(loctomove);
        if (steer > 0)
        {
            Vector3 torq = -GlobalTransform.basis.y * turnspeed * delta;
            torq.z = 1000;
            AddTorque(torq);
        }
        if (steer < 0)
        {
            Vector3 torq = GlobalTransform.basis.y * turnspeed * delta;
            torq.z = -1000;
            AddTorque(torq);
            //AddTorque(GlobalTransform.basis.y * turnspeed * delta);
        }
    }
    public override void _Ready()
    {
        base._Ready();
        SteeringWheel = GetNode<Position3D>("SteeringWheel");
        
        Spatial parent = (Spatial)GetParent();
        frontray = parent.GetNode<RayCast>("RayF");
        Rays.Insert(0, parent.GetNode<RayCast>("RayFL"));
        Rays.Insert(1, parent.GetNode<RayCast>("RayFR"));
        //Rays.Insert(2, parent.GetNode<RayCast>("RayF"));
        Rays.Insert(2, parent.GetNode<RayCast>("RayBL"));
        Rays.Insert(3, parent.GetNode<RayCast>("RayBR"));
        //Rays.Insert(5, parent.GetNode<RayCast>("RayB"));
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
    public void Jump()
    {
        for (int i = 0; i < Rays.Count; i ++)
        {
            if (Rays[i].IsColliding())
                AddForce(Vector3.Up * (HoverForce / 4), Rays[i].GlobalTransform.origin - GlobalTransform.origin);
        }
    }
    public void BoardVehicle(Character cha)
    {
        bool isthing = GetParent().GetParent() is MyWorld;
        if (!isthing)
        {
            Transform prevpos = ((Spatial)GetParent()).GlobalTransform;
            GetParent().GetParent().RemoveChild(GetParent());
            MyWorld.GetInstance().AddChild(GetParent());
            ((Spatial)GetParent()).GlobalTransform = prevpos;
        }
        
        Rotation = new Vector3(0,Rotation.y,0);
        //cha.GlobalTranslation = GlobalTranslation;
        cha.GetParent().RemoveChild(cha);

        Spatial guy = cha.GetNode<Spatial>("Pivot");
        guy.Rotation = new Vector3(0,0,0);
        cha.Rotation =  new Vector3(0,0,0);
        cha.SetCollisionMaskBit(4, false);
        GetParent().AddChild(cha);
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn.RemotePath = cha.GetPath();
        CharTrasn2.RemotePath = guy.GetPath();
        //cha.Translation = new Vector3 (0,0,0);
        cha.OnVehicleBoard();
        cha.Transform = CharTrasn.Transform;
        
        Working = true;
        passengers.Insert(passengers.Count, cha);
        //Translation = new Vector3(0.0f,1.0f,0.0f);
    }
    private void Capsize()
    {
        if (passengers.Count == 0)
            return;
        Character chartothrowout = passengers[0];
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        chartothrowout.GetParent().RemoveChild(chartothrowout);
        chartothrowout.SetCollisionMaskBit(4, true);
        MyWorld.GetInstance().AddChild(chartothrowout);
        chartothrowout.OnVehicleUnBoard();
        chartothrowout.GlobalTranslation = GlobalTranslation;
        chartothrowout.Rotation = new Vector3(0,0,0);
        Working = false;
        chartothrowout.SetVehicle(null);
    }
     public void UnBoardVehicle(Character cha)
    {
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        cha.GetParent().RemoveChild(cha);
        cha.SetCollisionMaskBit(4, true);
        MyWorld.GetInstance().AddChild(cha);
        cha.OnVehicleUnBoard();
        cha.GlobalTranslation = GlobalTranslation;
        cha.Rotation = new Vector3(0,0,0);
        Working = false;
    }
}

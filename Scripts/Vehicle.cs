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
    int JumpForce = 50;

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

    List<Particles> ExaustParticles = new List<Particles>();

    AnimationPlayer Anim;
    bool wingsdeployed = false;
    WindDetector WindD;
    bool WindOnWings = false;
    List<ShaderMaterial> WingMaterials = new List<ShaderMaterial>();
    public void ToggleWings(bool toggle)
    {
        if (Anim.IsPlaying())
            return;
        if (toggle)
        {
            Anim.Play("WingsOut");
            wingsdeployed = true;
        }
        else
        {
            Anim.Play("Wings");
            wingsdeployed = false;
            EnableWindOnWings(false);
        }
    }
    private void EnableWindOnWings(bool toggle)
    {
        if (!toggle)
        {
            WingMaterials[0].SetShaderParam("strength", 0);
            WingMaterials[1].SetShaderParam("strength", 0);
            WingMaterials[2].SetShaderParam("strength", 0);
            WingMaterials[3].SetShaderParam("strength", 0);
            WindOnWings = false;
        }
        else
        {
            WingMaterials[0].SetShaderParam("strength", 10);
            WingMaterials[1].SetShaderParam("strength", 10);
            WingMaterials[2].SetShaderParam("strength", 10);
            WingMaterials[3].SetShaderParam("strength", 10);
            WindOnWings = true;
        }
        
    }
    public bool IsBoatFacingWind()
    {
        return WindD.BoatFacingWind;
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        bool strong = false;
        float hoverforce2 = Hoverforcecurve.Interpolate(latsspeed /(speed * 4)) * HoverForce;
        float forcemulti = 1;
        frontray.ForceRaycastUpdate();
        if (frontray.IsColliding())
        {
            forcemulti = 4;
            //var collisionpoint = frontray.GetCollisionPoint();
            //var dist = collisionpoint.DistanceTo(frontray.GlobalTransform.origin);
            //if (dist > 1 && latsspeed > speed * 2);
           // {
            //    Capsize();
           //}
        }
            

        for (int i = 0; i < Rays.Count; i ++)
        {
            RayCast ray = Rays[i];
            ray.ForceRaycastUpdate();
            
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

                f= (Vector3.Up * hoverforce2 * delta) * multi;

                AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
            else
            {
                Vector3 f = Vector3.Zero;

                f = (Vector3.Up * hoverforce2 * delta) * -8;

                AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
        }
        if (!Working)
            return;

        float distance = loctomove.DistanceTo(GlobalTransform.origin);
        if (distance < 5)
        {
            latsspeed = 0;
            return;
        }

        Vector3 force = Vector3.Zero;
        if (strong)
            force = -GlobalTransform.basis.z;
        else
            force = GlobalTransform.basis.z;
        force.y = 0;
        float fmulti = 1;
        if (!IsBoatFacingWind() && wingsdeployed)
        {
            fmulti = 0.04f *DayNight.GetWindStr();
            if (!WindOnWings)
                EnableWindOnWings(true);
        }
        else if (IsBoatFacingWind())
        {
            if (WindOnWings)
                EnableWindOnWings(false);
        }
        latsspeed = (speed * fmulti) * (Math.Min(500, distance - 5)/ 500);
        AddCentralForce(force * latsspeed * delta);
        

        //keeping balance and casizing if to rotated in x 
        float rotx = Mathf.Rad2Deg(Rotation.x);
        if (rotx > 0)
        {
            float rotmulti = rotx / 45;
            Vector3 torq = -GlobalTransform.basis.x * turnspeed * delta;
            AddTorque(torq * rotmulti);
        }
        else if (rotx < 0)
        {
            float rotmulti = rotx / -45;
            Vector3 torq = GlobalTransform.basis.x * turnspeed * delta;
            AddTorque(torq * rotmulti);
        }
        if (rotx > 45 || rotx < -45)
            Capsize();

        //keeping balance and casizing if to rotated in z
        float rotz = Mathf.Rad2Deg(Rotation.z);

        if (rotz > 0)
        {
            float rotmulti = rotz / 45;
            Vector3 torq = -GlobalTransform.basis.z * turnspeed * delta;
            AddTorque(torq * rotmulti);
            if (rotz > 45)
                Capsize();
        }
        else if (rotz < 0)
        {
            float rotmulti = rotz / -45;
            Vector3 torq = GlobalTransform.basis.z * turnspeed * delta;
            AddTorque(torq * rotmulti);
            if (rotz < -45)
                Capsize();
        }

        //Steering
        SteeringWheel.LookAt(loctomove, Vector3.Up);
        float steer = Mathf.Rad2Deg(SteeringWheel.Rotation.y);

        if (steer > 0)
        {
            Vector3 torq;
            if (steer < 175)
            {
                torq = -GlobalTransform.basis.y * turnspeed * delta;
                torq.x *= -1;
                torq.z = 1000;
            }
            else
            {
                float st = 180 - steer;
                torq = (-GlobalTransform.basis.y * turnspeed * delta) * (st / 180);
                torq.x *= -1;
                torq.z = 1000  * (st / 180);
            }
            AddTorque(torq);
        }
        else if (steer < 0)
        {
            
            Vector3 torq;
            if (steer > -175)
            {
                torq = (GlobalTransform.basis.y * turnspeed * delta);
                torq.z = -1000;
            }
            else
            {
                float st = 180 - -steer;
                torq = (GlobalTransform.basis.y * turnspeed * delta)  * (st / 180);
                torq.z = -1000  * (st / 180);
            }
            AddTorque(torq);
        }
    }
    private void ToggleMachine(bool toggle)
    {
        if (toggle)
        {
            ExaustParticles[0].Emitting = true;
            ExaustParticles[1].Emitting = true;
        }
        else
        {
            ExaustParticles[0].Emitting = false;
            ExaustParticles[1].Emitting = false;
        }
    }
    public override void _Ready()
    {
        base._Ready();
        WindD = GetNode<WindDetector>("WindDetector");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        SteeringWheel = GetNode<Position3D>("SteeringWheel");
        ExaustParticles.Insert(0, GetNode<Particles>("ExaustParticlesL"));
        ExaustParticles.Insert(1, GetNode<Particles>("ExaustParticlesR"));
        ExaustParticles[0].Emitting = false;
        ExaustParticles[1].Emitting = false;
        Spatial parent = (Spatial)GetParent();
        frontray = parent.GetNode<RayCast>("RayF");
        Rays.Insert(0, parent.GetNode<RayCast>("RayFL"));
        Rays.Insert(1, parent.GetNode<RayCast>("RayFR"));
        //Rays.Insert(2, parent.GetNode<RayCast>("RayF"));
        Rays.Insert(2, parent.GetNode<RayCast>("RayBL"));
        Rays.Insert(3, parent.GetNode<RayCast>("RayBR"));
        ((Spatial)GetParent()).GlobalRotation = Vector3.Zero;
        //Rays.Insert(5, parent.GetNode<RayCast>("RayB"));
        

        WingMaterials.Insert(0, (ShaderMaterial)GetNode<MeshInstance>("MeshInstance2").GetActiveMaterial(1));
        WingMaterials.Insert(1, (ShaderMaterial)GetNode<MeshInstance>("MeshInstance5").GetActiveMaterial(1));
        WingMaterials.Insert(2, (ShaderMaterial)GetNode<MeshInstance>("MeshInstance3").GetActiveMaterial(1));
        WingMaterials.Insert(3, (ShaderMaterial)GetNode<MeshInstance>("MeshInstance4").GetActiveMaterial(1));
        ToggleWings(false);
        EnableWindOnWings(false);
    }
    public void Jump()
    {
        for (int i = 0; i < Rays.Count; i ++)
        {
            if (Rays[i].IsColliding())
                AddForce(Vector3.Up * (JumpForce), Rays[i].GlobalTransform.origin - GlobalTransform.origin);
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
        Vector3 prevrot = cha.GlobalRotation;

        cha.GetParent().RemoveChild(cha);

        Spatial guy = cha.GetNode<Spatial>("Pivot");

        cha.SetCollisionMaskBit(4, false);
        GetParent().AddChild(cha);
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn.RemotePath = cha.GetPath();
        CharTrasn2.RemotePath = guy.GetPath();

        cha.OnVehicleBoard();

        cha.GlobalRotation = prevrot;
        Working = true;
        passengers.Insert(passengers.Count, cha);
        ToggleMachine(true);
    }
    private void Capsize()
    {
        if (passengers.Count == 0)
            return;
        Character chartothrowout = passengers[0];
        Vector3 prevrot = chartothrowout.GlobalRotation;
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
        chartothrowout.GlobalRotation = prevrot;
        chartothrowout.SetVehicle(null);
        ToggleMachine(false);
    }
     public void UnBoardVehicle(Character cha)
    {
        Vector3 prevrot = cha.GlobalRotation;
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        cha.GetParent().RemoveChild(cha);
        cha.SetCollisionMaskBit(4, true);
        MyWorld.GetInstance().AddChild(cha);
        cha.OnVehicleUnBoard();
        cha.GlobalTranslation = GlobalTranslation;
        cha.GlobalRotation = prevrot;
        cha.Rotation = new Vector3(0,0,0);
        Working = false;
        ToggleMachine(false);
    }
    public override void _Input(InputEvent @event)
	{
        if (!Working)
            return;
		if (@event.IsActionPressed("Run"))
		{

            if (wingsdeployed)
                ToggleWings(false);
            else
                ToggleWings(true);
        }
    }
}

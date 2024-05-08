using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
/*
██╗   ██╗███████╗██╗  ██╗██╗ ██████╗██╗     ███████╗
██║   ██║██╔════╝██║  ██║██║██╔════╝██║     ██╔════╝
██║   ██║█████╗  ███████║██║██║     ██║     █████╗  
╚██╗ ██╔╝██╔══╝  ██╔══██║██║██║     ██║     ██╔══╝  
 ╚████╔╝ ███████╗██║  ██║██║╚██████╗███████╗███████╗
  ╚═══╝  ╚══════╝╚═╝  ╚═╝╚═╝ ╚═════╝╚══════╝╚══════╝
*/                                                                                   

public class Vehicle : RigidBody
{
    ///////////////////////////////////Exports///////////////////////////////////
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
    [Export]
    Mesh LeftWingMesh = null;
    [Export]
    Mesh LeftDestWingMesh = null;
    [Export]
    Mesh RightWingMesh = null;
    [Export]
    Mesh RightDestWingMesh = null;
    ///////////////////////////////////////////////////////////////////////////
    
    //Engine is on
    public bool Working = false;

    //Used to calculate steering
    Position3D SteeringWheel;

    ///////////Hover Rays////////////////
    List<RayCast> Rays = new List<RayCast>();
    RayCast frontray;
    /////////////////////////////////////
    public Vector3 loctomove;

    List<Character> passengers = new List<Character>();

    float latsspeed = 0.0f;

    List<Particles> ExaustParticles = new List<Particles>();

     WindDetector WindD;
    
   

    ////////////Damage///////////////
    VehicleDamageManager DamageMan;
    SpotLight FrontLight;
    /////////////////////////////////////
    ////////////Wings////////////////
    bool WindOnWings = false;
    List<ShaderMaterial> WingMaterials = new List<ShaderMaterial>();
    bool wingsdeployed = false;
    AnimationPlayer Anim;
    /////////////////////////////////////
    
    public override void _Ready()
    {
        base._Ready();
        WindD = GetNode<WindDetector>("WindDetector");
        FrontLight = GetNode<MeshInstance>("MeshInstance2").GetNode<SpotLight>("SpotLight");
        FrontLight.LightEnergy = 0;
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
        Rays.Insert(2, parent.GetNode<RayCast>("RayBL"));
        Rays.Insert(3, parent.GetNode<RayCast>("RayBR"));
        ((Spatial)GetParent()).GlobalRotation = Vector3.Zero;

        DamageMan = GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        for (int i = 0; i < 4; i++)
        {
            MeshInstance wing = GetNodeOrNull<MeshInstance>("WingMesh" + i);
            if (wing == null)
                break;
            WingMaterials.Insert(i, (ShaderMaterial)wing.GetActiveMaterial(1));
        }

        ToggleWings(false);
        EnableWindOnWings(false);
        //SetProcessInput(false);
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        //ulong ms = OS.GetSystemTimeMsecs();
        Hover(delta);
        
        

        float distance = loctomove.DistanceTo(GlobalTransform.origin);

        float fmulti = 1;
        Vector3 force = Vector3.Zero;
        if (Working)
        {
            force = GlobalTransform.basis.z;
            force.y = 0;
        }
        Vector3 wingforce = Vector3.Zero;
        if (!IsBoatFacingWind() && wingsdeployed)
        {
            Vector3 f = GlobalTransform.basis.z;
            f.y = 0;
            float workingwingmulti = GetWorkingSailMulti();
            fmulti += 0.01f *DayNight.GetWindStr();
            //f *= workingwingmulti;
            wingforce += f * fmulti * speed * workingwingmulti;
            if (!WindOnWings)
                EnableWindOnWings(true);
        }
        else if (IsBoatFacingWind())
        {
            if (WindOnWings)
                EnableWindOnWings(false);
        }
        latsspeed = speed * (Math.Min(500, distance)/ 500);
        //engine
        AddCentralForce(force * latsspeed * delta);
        //sails
        AddCentralForce(wingforce * delta);
        
        if (!Working)
        {
            loctomove = GlobalTransform.origin;
            return;
        }
        Steer(delta);
       // ulong msaf = OS.GetSystemTimeMsecs();
		//GD.Print("Vehicle processing took " + (msaf - ms).ToString() + " ms");
    }

    public bool ToggleWings(bool toggle)
    {
        if (Anim.IsPlaying())
            return false;
        
        if (toggle)
        {
            Anim.Play("WingsOut");
        }
        else
        {
            Anim.Play("Wings");
            EnableWindOnWings(false);
        }
        wingsdeployed = toggle;
        DamageMan.ToggleWingCollision(toggle);
        return true;
    }
    public bool HasDeployedWings()
    {
        return wingsdeployed;
    }
    public void OnWingDamaged(int index)
    {
        if (GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh == LeftWingMesh)
            GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh = LeftDestWingMesh;
        else if (GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh == RightWingMesh)
            GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh = RightDestWingMesh;
    }
    public int GetWingCount()
    {
        return WingMaterials.Count;
    }
    public void OnLightDamaged()
    {
        FrontLight.LightEnergy = 0;
    }
    private void EnableWindOnWings(bool toggle)
    {
        if (!toggle)
        {
            for (int i = 0; i < WingMaterials.Count; i++)
            {
                if (DamageMan.IsWingWorking(i))
                    WingMaterials[i].SetShaderParam("strength", 0);
            }
            WindOnWings = false;
        }
        else
        {
            float amm = DayNight.GetWindStr() / 10;
            for (int i = 0; i < WingMaterials.Count; i++)
            {
                if (DamageMan.IsWingWorking(i))
                    WingMaterials[i].SetShaderParam("strength", amm);
            }
            WindOnWings = true;
        }
        
    }
    public bool IsBoatFacingWind()
    {
        return WindD.BoatFacingWind;
    }
    
    // return true if hovering up and false if down
    public void Hover(float delta)
    {
        float Force = Hoverforcecurve.Interpolate(latsspeed /(speed)) * HoverForce;
        float forcemulti = 1;
        if (Working)
        {
            frontray.ForceRaycastUpdate();
            if (frontray.IsColliding())
            {
                forcemulti = 4;
            }
        }
        for (int i = 0; i < Rays.Count; i ++)
        {
            
            RayCast ray = Rays[i];
            ray.ForceRaycastUpdate();
            
            if (ray.IsColliding())
            {
                var collisionpoint = ray.GetCollisionPoint();
                var collisionobj = ray.GetCollider();
                var dist = collisionpoint.DistanceTo(ray.GlobalTransform.origin);
                float distmulti = dist / - ray.CastTo.y;

                Particles part = ray.GetNode<Particles>("Particles");

                if (dist < 20)
                {
                    part.Emitting = true;
                    float particleoffset = dist;
                    if (((StaticBody)collisionobj).Name == "SeaBed")
                        particleoffset -= 4;
                    part.Translation = new Vector3(part.Translation.x, - particleoffset, part.Translation.z);
                }
                else
                {
                    part.Emitting = false;
                }

                float multi = forcecurve.Interpolate(distmulti);
                if (dist < 4)
                {
                    multi *= 4;
                }
                
                Vector3 f = Vector3.Zero;

                f= Vector3.Up * Force * delta * multi;

                AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
            else
            {
                ray.GetNode<Particles>("Particles").Emitting = false;
                Vector3 f = Vector3.Zero;

                f = Vector3.Up * Force * delta * -8;

                AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
        }
        float rotx = Mathf.Rad2Deg(Rotation.x);
        if (rotx > 5 || rotx < -5)
        {
            float sign = Mathf.Sign(rotx);
            float rotmulti = rotx / (45 * sign);
            Vector3 torq = GlobalTransform.basis.x * - sign * turnspeed * delta;
            AddTorque(torq * rotmulti * 0.2f);
        }

        //keeping balance and casizing if to rotated in z
        float rotz = Mathf.Rad2Deg(Rotation.z);

        if (rotz > 5 || rotz < -5)
        {
            float sign = Mathf.Sign(rotz);
            float rotmulti = rotz / (45 * sign);
            Vector3 torq = GlobalTransform.basis.z * -sign * turnspeed * delta;
            AddTorque(torq * rotmulti * 0.2f);
        }

        if (rotx > 100 || rotx < -100 || rotz < -100 || rotz > 100)
            Capsize();
    }
    public void Steer(float delta)
    {
        SteeringWheel.LookAt(loctomove, Vector3.Up);
        float steer = Mathf.Rad2Deg(SteeringWheel.Rotation.y);

        int mutli = - Math.Sign(steer);

        Vector3 torq;

        float eq = 1.0f;

        Vector3 basis = GlobalTransform.basis.y * mutli;

        if (steer > 175)
        {
            float st = 180 - steer;
            eq  *=  st / 180;
        }
        if (steer < -175)
        {
            float st = 180 - -steer;
            eq  *=  st / 180;
        }

        torq = basis * turnspeed * delta * eq;
        torq.x *= -1;
        torq.z = 1000 * mutli * eq;
        AddTorque(torq);
    }
    private float GetWorkingSailMulti()
    {
        int wingcount = WingMaterials.Count;
        float count = 0;
        for (int i = 0; i < wingcount; i++)
        {
            if (DamageMan.IsWingWorking(i))
                count += 1;
        }
        return count * 0.25f;
    }
    public  void ToggleMachine(bool toggle)
    {
        if (toggle)
        {
            loctomove = GlobalTransform.origin;
            ExaustParticles[0].Emitting = true;
            ExaustParticles[1].Emitting = true;
            Working = true;
        }
        else
        {
            loctomove = GlobalTransform.origin;
            ExaustParticles[0].Emitting = false;
            ExaustParticles[1].Emitting = false;
            Working = false;
        }
    }
    public  void ToggleLights(bool toggle)
    {
        if (DamageMan.GetLightCondition() == false)
            return;
        if (toggle)
        {
            FrontLight.LightEnergy = 1;
        }
        else
        {
            FrontLight.LightEnergy = 0;
        }
    }
    public bool LightCondition()
    {
        return FrontLight.LightEnergy == 1;
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
        //SetProcessInput(true);
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

        cha.OnVehicleBoard(this);

        cha.GlobalRotation = prevrot;
        
        passengers.Insert(passengers.Count, cha);
        //ToggleMachine(true);
    }
    private void Capsize()
    {
        if (passengers.Count == 0)
            return;
        //SetProcessInput(false);
        
        Character chartothrowout = passengers[0];
        passengers.Clear();
        Vector3 prevrot = chartothrowout.GlobalRotation;
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        chartothrowout.GetParent().RemoveChild(chartothrowout);
        chartothrowout.SetCollisionMaskBit(4, true);
        MyWorld.GetInstance().AddChild(chartothrowout);
        chartothrowout.GlobalTranslation = GlobalTranslation;
        chartothrowout.Rotation = new Vector3(0,0,0);
        chartothrowout.GlobalRotation = prevrot;
        chartothrowout.SetVehicle(null);
        ToggleMachine(false);
        ToggleLights(false);
        chartothrowout.OnVehicleUnBoard(this);
    }
     public void UnBoardVehicle(Character cha)
    {
        passengers.Clear();
        //SetProcessInput(false);
        Vector3 prevrot = cha.GlobalRotation;
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        cha.GetParent().RemoveChild(cha);
        cha.SetCollisionMaskBit(4, true);
        MyWorld.GetInstance().AddChild(cha);
        cha.GlobalTranslation = GlobalTranslation;
        cha.GlobalRotation = prevrot;
        cha.Rotation = new Vector3(0,0,0);
        ToggleMachine(false);
        ToggleLights(false);
        cha.OnVehicleUnBoard(this);
    }
    ///////Action Menu////////
    public void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", toggle);
    }
    //////Data Saving////////
    public void InputData(VehicleInfo data)
	{
        if (data.removed)
        {
            GetParent().QueueFree();
            return;
        }
		GlobalTranslation = data.loc;
        GlobalRotation = data.rot;
        GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager").InputData(data.DamageInfo);
	}
    public void ReparentVehicle(Island ile)
    {
        Vector3 orig = GlobalTranslation;
        Vector3 origrot = GlobalRotation;

        Player pl = (Player)passengers[0];
        Vector3 prevrot = pl.GlobalRotation;

        Spatial vehpar = (Spatial)GetParent();
        Spatial par = (Spatial)vehpar.GetParent();
        par.RemoveChild(vehpar);
        ile.AddChild(vehpar);
        
        GlobalTranslation = orig;
        GlobalRotation = origrot;

        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn.RemotePath = pl.GetPath();
        CharTrasn2.RemotePath = pl.GetNode<Spatial>("Pivot").GetPath();

        pl.GlobalRotation = prevrot;
    }
}
public class VehicleInfo
{
    public string VehName;
    public Vector3 loc;
    public Vector3 rot;
    public VehicleDamageInfo DamageInfo;
    public bool removed = false;
    public string scenedata;
    public void UpdateInfo(Vehicle veh)
    {
        loc = veh.GlobalTranslation;
        rot = veh.GlobalRotation;
        VehicleDamageManager Damageman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        DamageInfo = new VehicleDamageInfo();
        DamageInfo.UpdateInfo(veh);
    }
    public void SetInfo(Vehicle veh)
    {
        VehName = veh.GetParent().Name;
        loc = veh.GlobalTranslation;
        rot = veh.GlobalRotation;
        scenedata = veh.GetParent().Filename;
        VehicleDamageManager Damageman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        DamageInfo = new VehicleDamageInfo();
        List<int> destw;
        Damageman.GetDestroyedWings(out destw);
        DamageInfo.SetInfo(veh);
    }
}

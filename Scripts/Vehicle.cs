using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public bool SpawnBroken = false;
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
    //RayCast frontray;
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
        ExaustParticles.Insert(2, GetNode<Particles>("ExaustParticlesLSteer"));
        ExaustParticles.Insert(3, GetNode<Particles>("ExaustParticlesRSteer"));
        ExaustParticles[0].Emitting = false;
        ExaustParticles[1].Emitting = false;
        ExaustParticles[2].Emitting = false;
        ExaustParticles[3].Emitting = false;
        Spatial parent = (Spatial)GetParent();
        //frontray = parent.GetNode<RayCast>("RayF");
        Rays.Insert(0, parent.GetNode<RayCast>("RayFL"));
        Rays.Insert(1, parent.GetNode<RayCast>("RayFR"));
        Rays.Insert(2, parent.GetNode<RayCast>("RayBL"));
        Rays.Insert(3, parent.GetNode<RayCast>("RayBR"));
        //((Spatial)GetParent()).GlobalRotation = Vector3.Zero;

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

        Node par = GetParent();
		while (!(par is Island))
		{
            if (par == null)
				return;
			par = par.GetParent();
		}
		Island ile = (Island)par;
		ile.RegisterChild(this);

        thr.Start(this, "Balance", 0.01);
        
        //SetProcessInput(false);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        float engineforce = latsspeed / 500000;
        ((ParticlesMaterial)ExaustParticles[0].ProcessMaterial).Scale = engineforce * 5;
        ((ParticlesMaterial)ExaustParticles[0].ProcessMaterial).InitialVelocity = engineforce * 60 + 5;
        ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = engineforce * 15;
        ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").PitchScale = engineforce * 0.1f + 0.2f;
        ((ParticlesMaterial)ExaustParticles[1].ProcessMaterial).Scale = engineforce * 5;
        ((ParticlesMaterial)ExaustParticles[1].ProcessMaterial).InitialVelocity = engineforce * 60 + 5;
        ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = engineforce * 15;
        ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").PitchScale = engineforce * 0.1f + 0.2f;
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        //ulong ms = OS.GetSystemTimeMsecs();

        if (!thr.IsAlive())
        {
            //if (thr.IsActive())
            thr.WaitToFinish();
            thr = new Thread();
            thr.Start(this, "Balance", delta);
        }

        Hover(delta);

        
        float distance = loctomove.DistanceTo(GlobalTransform.origin);

        float fmulti = 1;
        Vector3 force = Vector3.Zero;
        
        force = GlobalTransform.basis.z;
        force.y = 0;
        
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
        

        //sails
        AddCentralForce(wingforce * delta);

        if (!Working)
        {   
            return;
        }

        //engine
        AddCentralForce(force * latsspeed * delta);
        

        

        if (!SteerThr.IsAlive())
        {
            //if (SteerThr.IsActive())
            SteerThr.WaitToFinish();
            SteerThr = new Thread();
            SteerThr.Start(this, "Steer", delta);
        }
        
        //Steer(delta);
        AddTorque(steert);
        steert = Vector3.Zero;
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
                //if (DamageMan.IsWingWorking(i))
                WingMaterials[i].SetShaderParam("strength", 0);
            }
        }
        else
        {
            float amm = DayNight.GetWindStr() / 10;
            for (int i = 0; i < WingMaterials.Count; i++)
            {
                //if (DamageMan.IsWingWorking(i))
                WingMaterials[i].SetShaderParam("strength", amm);
            }
        }
        WindOnWings = toggle;
    }
    public bool IsBoatFacingWind()
    {
        return WindD.BoatFacingWind;
    }
    Thread thr = new Thread();
    Thread SteerThr;
    // return true if hovering up and false if down
    public void Hover(float delta)
    {
        float Force = Hoverforcecurve.Interpolate(latsspeed /(speed)) * HoverForce;
        //float forcemulti = 1;
        /*if (Working)
        {
            //frontray.ForceRaycastUpdate();
            if (frontray.IsColliding())
            {
                forcemulti = 4;
            }
        }*/
        Vector3 f ;
        for (int i = 0; i < Rays.Count; i ++)
        {
            
            RayCast ray = Rays[i];
            //ray.ForceRaycastUpdate();
            Particles part = ray.GetNode<Particles>("Particles");
            if (ray.IsColliding())
            {
                var collisionpoint = ray.GetCollisionPoint();
                var collisionobj = ray.GetCollider();
                var dist = collisionpoint.DistanceTo(ray.GlobalTransform.origin);
                float distmulti = dist / - ray.CastTo.y;

                
                //Particles Flame = ray.GetNode<Particles>("EngineParticles");

                if (dist < 20 && Working)
                {
                    //Flame.Emitting = true;
                    float particleoffset = dist;
                    if (((Node)collisionobj).Name == "SeaBed")
                        particleoffset -= 4;
                    part.Translation = new Vector3(part.Translation.x, - particleoffset, part.Translation.z);
                }

                part.Emitting = dist >= 20;
                float multi = forcecurve.Interpolate(distmulti);
                if (dist < 4)
                {
                    multi *= 4;
                }
                //((ParticlesMaterial)Flame.ProcessMaterial).Scale = multi;
                //((ParticlesMaterial)Flame.ProcessMaterial).InitialVelocity = multi * 10 + 5;

                f= Vector3.Up * Force * delta * multi;

                //AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
                
            }
            else
            {
                part.Emitting = false;
                //ray.GetNode<Particles>("EngineParticles").Emitting = false;
                f = Vector3.Up * 8000 * delta * -8;

                //AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
            AddForce(f, ray.GlobalTransform.origin - GlobalTransform.origin);
        }
        //Balance(delta);
        //if (thr != null)
            //thr.WaitToFinish();
        AddTorque(torquebalance);
        torquebalance = Vector3.Zero;
        
        //keeping balance and casizing if to rotated in z
        
    }
    Vector3 torquebalance = Vector3.Zero;
    Vector3 steert = Vector3.Zero;
    private void Balance(float delta)
    {
        float rotx = Mathf.Rad2Deg(Rotation.x);
        if (rotx > 5 || rotx < -5)
        {
            float sign = Mathf.Sign(rotx);
            float rotmulti = rotx / (45 * sign);
            Vector3 torq = GlobalTransform.basis.x * - sign * turnspeed * delta;
            torquebalance = torq * rotmulti * 0.2f;
            if (rotx > 100 || rotx < -100)
                CallDeferred("Capsize");
        }

        
        float rotz = Mathf.Rad2Deg(Rotation.z);

        if (rotz > 5 || rotz < -5)
        {
            float sign = Mathf.Sign(rotz);
            float rotmulti = rotz / (45 * sign);
            Vector3 torq = GlobalTransform.basis.z * -sign * turnspeed * delta;
            torquebalance = torq * rotmulti * 0.2f;
            if (rotz < -100 || rotz > 100)
                CallDeferred("Capsize");
        }
    }
    public void Steer(float delta)
    {
        //if (SteeringWheel.GlobalTranslation.DistanceTo(loctomove) < 0.1f)
            //return;
            
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

        float steeramm = eq * mutli;
        if (steeramm > 0)
        {
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).Scale = steeramm * 5;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).InitialVelocity = steeramm* 60 + 5;
            ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = steeramm * 15;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).InitialVelocity = 5;
            ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
        }
        else if (steeramm < 0)
        {
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).Scale = Mathf.Abs(steeramm) * 5;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).InitialVelocity = Mathf.Abs(steeramm) * 60 + 5;
            ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = Mathf.Abs(steeramm) * 15;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).InitialVelocity = 5;
            ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
        }
        else
        {
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).InitialVelocity = 5;
            ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).InitialVelocity = 5;
            ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
        }

        torq = basis * turnspeed * delta * eq;
        torq.x *= -1;
        torq.z = 1000 * mutli * eq;
        //AddTorque(torq);
        steert = torq;

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
        if (SteerThr == null)
        {
            SteerThr = new Thread();
            SteerThr.Start(this, "Steer", 0.01);
        }
        loctomove = GlobalTransform.origin;
        ExaustParticles[0].Emitting = toggle;
        ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        ExaustParticles[1].Emitting = toggle;
        ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        ExaustParticles[2].Emitting = toggle;
        ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        ExaustParticles[3].Emitting = toggle;
        ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        Working = toggle;
        SetProcess(toggle);
    }
    public bool IsRunning()
    {
        return Working;
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
            //GetParent().GetParent().RemoveChild(GetParent());
            //MyWorld.GetInstance().AddChild(GetParent());
            ((Spatial)GetParent()).GlobalTransform = prevpos;
        }
        
        Rotation = new Vector3(0,Rotation.y,0);
        Vector3 prevrot = cha.GlobalRotation;

        cha.GetParent().RemoveChild(cha);

        Spatial guy = cha.GetNode<Spatial>("Pivot");

        cha.Anims().ToggleSitting();

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
        chartothrowout.Anims().ToggleIdle();
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
     public bool UnBoardVehicle(Character cha)
    {
        
        //SetProcessInput(false);
        Vector3 prevrot = cha.GlobalRotation;
        
        Vector3 postoput;
        if (!CheckForGround(out postoput))
        {
            TalkText.GetInst().Talk("Πρέπει να πάω πιό κοντά στην στεριά.", cha);
            return false;
        }
        passengers.Clear();
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        cha.GetParent().RemoveChild(cha);
        cha.SetCollisionMaskBit(4, true);
        MyWorld.GetInstance().AddChild(cha);
        cha.GlobalTranslation = postoput;
        cha.GlobalRotation = prevrot;
        cha.Rotation = new Vector3(0,0,0);
        cha.Anims().ToggleIdle();
        ToggleMachine(false);
        ToggleLights(false);
        cha.OnVehicleUnBoard(this);
        return true;
    }
    private bool CheckForGround(out Vector3 GroundPos)
    {
        bool HasFloor = false;
        GroundPos = Vector3.Zero;
        Spatial groundChecks = GetNode<Spatial>("GroundChecks");
        foreach(RayCast cast in groundChecks.GetChildren())
        {
            cast.ForceRaycastUpdate();
            if (!cast.IsColliding())
                continue;
            bool ItsSea = ((CollisionObject)cast.GetCollider()).GetCollisionLayerBit(8);
			if (ItsSea)
			{
				continue;
			}
            HasFloor = true;
            GroundPos = cast.GetCollisionPoint();
            break;
        }
        return HasFloor;
    }
    ///////Action Menu////////
    public void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable", toggle);
    }
    //////Data Saving////////
    public void InputData(VehicleInfo data)
	{
        //if (data.removed)
        //{
        //    GetParent().QueueFree();
        //    return;
        //}
		GlobalTranslation = data.loc;
        GlobalRotation = data.rot;
        GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager").InputData(data.DamageInfo);
	}
    public void ReparentVehicle(Island from, Island ile)
    {
        Vector3 orig = GlobalTranslation;
        Vector3 origrot = GlobalRotation;

        Player pl = (Player)passengers[0];
        Vector3 prevrot = pl.GlobalRotation;

        Spatial vehpar = (Spatial)GetParent();
        Spatial par = (Spatial)vehpar.GetParent();
        
        from.UnRegisterChild(this);
        par.RemoveChild(vehpar);
        ile.AddChild(vehpar);
        ile.RegisterChild(this);
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
    //public bool removed = false;
    public string scenedata;
    public void UpdateInfo(Vehicle veh)
    {
        loc = veh.GlobalTranslation;
        rot = veh.GlobalRotation;
        //VehicleDamageManager Damageman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
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
    public Dictionary<string, object>GetPackedData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        GDScript SaveGD = GD.Load<GDScript>("res://Scripts/VehicleDamageSaveInfo.gd");
		Resource damageinfo = (Resource)SaveGD.New();
        damageinfo.Call("_SetData", DamageInfo.GetPackedData());
        data.Add("Name", VehName);
        data.Add("Location", loc);
        data.Add("Rotation", rot);
        data.Add("DamageInfo", damageinfo);
        //data.Add("Removed", removed);
        data.Add("SceneData", scenedata);
        return data;
    }
    public void UnPackData(Godot.Object data)
    {
        VehName = (string)data.Get("Name");
        loc = (Vector3)data.Get("Location");
        rot = (Vector3)data.Get("Rotation");
        //removed = (bool)data.Get("Removed");
        scenedata = (string)data.Get("SceneData");

        Resource DamageData = (Resource)data.Get("DamageInfo");

        VehicleDamageInfo info = new VehicleDamageInfo();
        info.UnPackData(DamageData);
        DamageInfo = info;
    }
}

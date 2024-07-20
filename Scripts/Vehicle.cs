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

public enum VehicleType
{
    BASIC,
    ADVANCED,
    MASTERWORK
}
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
    //[Export]
    //Mesh LeftWingMesh = null;
    //[Export]
    //Mesh LeftDestWingMesh = null;
    //[Export]
    //Mesh RightWingMesh = null;
    //[Export]
    //Mesh RightDestWingMesh = null;
    [Export]
    VehicleType VType = VehicleType.BASIC;
    ///////////////////////////////////////////////////////////////////////////
    
    //Engine is on
    public bool Working = false;

    //Used to calculate steering
    Spatial SteeringWheel;

    ///////////Hover Rays////////////////
    List<Spatial> Rays = new List<Spatial>();
    //RayCast frontray;
    /////////////////////////////////////
    public Vector3 loctomove;

    List<Character> passengers = new List<Character>();

    float latsspeed = 0.0f;

    List<Particles> ExaustParticles = new List<Particles>();

    WindDetector WindD;

    public float GetLastSpeed()
    {
        return latsspeed / speed;
    }
    public void SetPlayerOwned(bool toggle)
    {
        PlayerOwned = toggle;
        if (toggle)
            AddToGroup("PlayerBoat");
        else
        {
            if (IsInGroup("PlayerBoat"))
                RemoveFromGroup("PlayerBoat");
        }
            
    }
    public bool IsPlayerOwned()
    {
        return PlayerOwned;
    }
    [Export]
    bool PlayerOwned = false;

    ////////////Damage///////////////
   // VehicleDamageManager DamageMan;
    SpotLight FrontLight;
    /////////////////////////////////////
    ////////////Wings////////////////
    bool WindOnWings = false;
    List<ShaderMaterial> WingMaterials = new List<ShaderMaterial>();
    //bool wingsdeployed = false;
    AnimationPlayer Anim;
    /////////////////////////////////////
    //public VehicleDamageManager GetDamageManager()
    //{
    //    return DamageMan;
    //}
    public override void _Ready()
    {
        base._Ready();
        WindD = GetNode<WindDetector>("WindDetector");
        FrontLight = GetNode<MeshInstance>("LightMesh").GetNode<SpotLight>("SpotLight");
        FrontLight.LightEnergy = 0;
        //Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        SteeringWheel = GetNode<Spatial>("SteeringWheel");
        ExaustParticles.Insert(0, GetNode<Particles>("ExaustParticlesL"));
        ExaustParticles.Insert(1, GetNode<Particles>("ExaustParticlesR"));
        ExaustParticles.Insert(2, GetNode<Particles>("ExaustParticlesLSteer"));
        ExaustParticles.Insert(3, GetNode<Particles>("ExaustParticlesRSteer"));
        ExaustParticles[0].Emitting = false;
        ExaustParticles[1].Emitting = false;
        ExaustParticles[2].Emitting = false;
        ExaustParticles[3].Emitting = false;
        //Spatial parent = (Spatial)GetParent();
        ZeroPos();
        //frontray = parent.GetNode<RayCast>("RayF");
        Rays.Insert(0, GetNode<Spatial>("RemoteTransform"));
        Rays.Insert(1, GetNode<Spatial>("RemoteTransform2"));
        Rays.Insert(2, GetNode<Spatial>("RemoteTransform3"));
        Rays.Insert(3, GetNode<Spatial>("RemoteTransform4"));
        //((Spatial)GetParent()).GlobalRotation = Vector3.Zero;

        //DamageMan = GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        for (int i = 0; i < 4; i++)
        {
            MeshInstance wing = GetNodeOrNull<MeshInstance>("WingMesh" + i);
            if (wing == null)
                break;
            WingMaterials.Insert(i, (ShaderMaterial)wing.GetActiveMaterial(0));
        }

        //ToggleWings(false);
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
    public bool HasWings()
    {
        return WingMaterials.Count > 0;
    }
    public VehicleType GetVehicleType()
    {
        return VType;
    }
    public void ZeroPos()
    {
        Spatial parent = (Spatial)GetParent();
        Vector3 prevpos = parent.Translation;
        if (prevpos == Vector3.Zero)
            return;
        parent.Translation = Vector3.Zero;
        
        Translation += prevpos;
        Rotation = parent.Rotation;
        parent.Rotation = Vector3.Zero;
    }
    
    public override void _Process(float delta)
    {
        base._Process(delta);

        float engineforce = latsspeed / speed;
        ((ParticlesMaterial)ExaustParticles[0].ProcessMaterial).Scale = engineforce * 5;
        ((ParticlesMaterial)ExaustParticles[0].ProcessMaterial).InitialVelocity = engineforce * 60 + 5;
        //ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = engineforce * 15;
        ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").PitchScale = engineforce + 0.6f;
        ((ParticlesMaterial)ExaustParticles[1].ProcessMaterial).Scale = engineforce * 5;
        ((ParticlesMaterial)ExaustParticles[1].ProcessMaterial).InitialVelocity = engineforce * 60 + 5;
        //ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = engineforce * 15;
        ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").PitchScale = engineforce + 0.6f;
    }
    float SpeedBuildup = 0;
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        //ulong ms = OS.GetSystemTimeMsecs();
        if (!PlayerOwned)
            return;

        if (!thr.IsActive())
        {
            //if (thr.IsActive())
            thr = new Thread();
            thr.Start(this, "Balance", delta);
        }
        

        Hover(delta);

        
        float distance = loctomove.DistanceTo(GlobalTranslation);
        float distmulti = Math.Min(500, distance)/ 500;
        SpeedBuildup = Mathf.Max(Mathf.Min(SpeedBuildup + (distmulti * 2 - 1) / 700, 1), 0 );
        float fmulti = 1;

        Vector3 force = GlobalTransform.basis.z;
        force.y = 0;
        
        Vector3 wingforce = Vector3.Zero;
        if (!IsBoatFacingWind() && HasWings())
        {
            Vector3 f = GlobalTransform.basis.z;
            f.y = 0;
            //float workingwingmulti = GetWorkingSailMulti();
            fmulti += DayNight.GetWindStr();
            //f *= workingwingmulti;
            //wingforce += f * fmulti * speed * workingwingmulti;

            wingforce += f * fmulti * speed;
            if (!WindOnWings)
                EnableWindOnWings(true);
        }
        else if (IsBoatFacingWind())
        {
            if (WindOnWings)
                EnableWindOnWings(false);
        }
        latsspeed = speed * (distmulti * SpeedBuildup);
        

        //sails
        AddCentralForce(wingforce * delta);

        if (!Working)
        {
            loctomove = GlobalTranslation;   
            return;
        }

        //engine
        AddCentralForce(force * latsspeed * delta);
        

        
        if (passengers.Count == 0)
        {
            return;
        }
        if (!SteerThr.IsActive())
        {
            //if (SteerThr.IsActive())
            
            SteerThr = new Thread();
            SteerThr.Start(this, "Steer", delta);
        }
        
        //Steer(delta);
        AddTorque(steert);
        steert = Vector3.Zero;
       // ulong msaf = OS.GetSystemTimeMsecs();
		//GD.Print("Vehicle processing took " + (msaf - ms).ToString() + " ms");
    }
    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
    }
    //public bool ToggleWings(bool toggle)
    //{
    //    if (Anim.IsPlaying())
    //        return false;
        
    //    if (toggle)
    //    {
    //        Anim.Play("WingsOut");
    //    }
    //    else
    //    {
    //        Anim.Play("Wings");
    //        EnableWindOnWings(false);
    //    }
    //    wingsdeployed = toggle;
     //   DamageMan.ToggleWingCollision(toggle);
    //    return true;
    //}
    //public bool HasDeployedWings()
    //{
    //    return wingsdeployed;
    //}
    //public void OnWingDamaged(int index)
    //{
    //    if (GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh == LeftWingMesh)
    //        GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh = LeftDestWingMesh;
    //    else if (GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh == RightWingMesh)
    //        GetNode<MeshInstance>("WingMesh" + index.ToString()).Mesh = RightDestWingMesh;
    //}
    //public int GetWingCount()
    //{
    //    return WingMaterials.Count;
    //}
    //public void OnLightDamaged()
    //{
    //    FrontLight.LightEnergy = 0;
    //}
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
    //public override void _ExitTree()
    //{
        //base._ExitTree();
        //if (thr != null && thr.IsActive())
        //{
        //    thr.WaitToFinish();
        //}
        //if (SteerThr != null && SteerThr.IsActive())
        //{
        //    SteerThr.WaitToFinish();
        //}
    //}
    
    public void Hover(float delta)
    {
        float Force = Hoverforcecurve.Interpolate(latsspeed /speed) * HoverForce;

        Vector3 f ;
        for (int i = 0; i < Rays.Count; i ++)
        {
            RemoteTransform rem = (RemoteTransform)Rays[i];
           
            RayCast ray = rem.GetNode<RayCast>(rem.RemotePath);

            Spatial EnginePivot = ray.GetNode<Spatial>("EnginePivot");
            //ray.ForceRaycastUpdate();
            Particles part = EnginePivot.GetNode<Particles>("Particles");
            Particles partd = EnginePivot.GetNode<Particles>("ParticlesDirt");
            Vector3 engrot = new Vector3(Mathf.Deg2Rad(Mathf.Lerp(0, 45, latsspeed /speed)), Rotation.y, 0);
            if (ray.IsColliding())
            {
                var collisionpoint = ray.GetCollisionPoint();
                var collisionobj = ray.GetCollider();
                var dist = collisionpoint.DistanceTo(ray.GlobalTranslation);
                float distmulti = dist / - ray.CastTo.y;

                
                //Particles Flame = ray.GetNode<Particles>("EngineParticles");

                if (dist <= 35 && Working)
                {
                    //Flame.Emitting = true;
                    float particleoffset = dist + engrot.x / 10;
                    
                    //particleoffset -= 4;
                    part.Translation = new Vector3(part.Translation.x, - particleoffset, part.Translation.z);
                    partd.Translation = new Vector3(partd.Translation.x, - particleoffset, partd.Translation.z);
                }
                if (((Node)collisionobj).Name == "SeaBed")
                {
                    part.Emitting = Working;
                    partd.Emitting = false;
                }
                else
                {
                    part.Emitting = false;
                    partd.Emitting = Working;
                }
                float multi = forcecurve.Interpolate(distmulti);
                //if (dist < 4)
                //{
                //    multi *= 4;
                //}
                //((ParticlesMaterial)Flame.ProcessMaterial).Scale = multi;
                //((ParticlesMaterial)Flame.ProcessMaterial).InitialVelocity = multi * 10 + 5;

                f= Vector3.Up * Force * delta * multi;
                //f= Vector3.Up * Force * delta 
                //AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
                
            }
            else
            {
                part.Emitting = false;
                partd.Emitting = false;
                //ray.GetNode<Particles>("EngineParticles").Emitting = false;
                f = Vector3.Up * 8000 * delta * -8;

                //AddForce(f * forcemulti, ray.GlobalTransform.origin - GlobalTransform.origin);
            }
            EnginePivot.Rotation = engrot;
            AddForce(f, ray.GlobalTranslation - GlobalTranslation);
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
            torquebalance += torq * rotmulti;
            if (rotx > 100 || rotx < -100)
                CallDeferred("Capsize");
        }

        
        float rotz = Mathf.Rad2Deg(Rotation.z);

        if (rotz > 5 || rotz < -5)
        {
            float sign = Mathf.Sign(rotz);
            float rotmulti = rotz / (45 * sign);
            Vector3 torq = GlobalTransform.basis.z * -sign * turnspeed * delta;
            torquebalance += torq * rotmulti;
            if (rotz < -100 || rotz > 100)
                CallDeferred("Capsize");
        }
        thr.CallDeferred("wait_to_finish");
    }
    public void Steer(float delta)
    {
        //if (SteeringWheel.GlobalTranslation.DistanceTo(loctomove) < 0.1f)
            //return;
        
        if (loctomove.DistanceTo(SteeringWheel.GlobalTranslation) > 0.01f)
        {
            Vector3 prevpos = SteeringWheel.Rotation;
            SteeringWheel.LookAt(loctomove, Vector3.Up);
            SteeringWheel.Rotation = new Vector3(prevpos.x, SteeringWheel.Rotation.y, prevpos.z);
        }
        //Vector3 dir = globalrota
        float steer = Mathf.Rad2Deg(SteeringWheel.Rotation.y);
        //Spatial SteerPad = GetNode<Spatial>("SteerPad");
        //SteerPad.Rotation = new Vector3(SteerPad.Rotation.x, steer, SteerPad.Rotation.z);

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
            //ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = steeramm * 15;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).InitialVelocity = 5;
            //ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
        }
        else if (steeramm < 0)
        {
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).Scale = Mathf.Abs(steeramm) * 5;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).InitialVelocity = Mathf.Abs(steeramm) * 60 + 5;
            //ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = Mathf.Abs(steeramm) * 15;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).InitialVelocity = 5;
            //ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
        }
        else
        {
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[3].ProcessMaterial).InitialVelocity = 5;
           //ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).Scale = 0;
            ((ParticlesMaterial)ExaustParticles[2].ProcessMaterial).InitialVelocity = 5;
            //ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").UnitDb = 0;
        }

        torq = basis * turnspeed * delta * eq;
        torq.x *= -1;
        //torq.z = 1000 * mutli * eq;
        //AddTorque(torq);
        steert = torq;
        SteerThr.CallDeferred("wait_to_finish");
    }
    //private float GetWorkingSailMulti()
    //{
    //    int wingcount = WingMaterials.Count;
    //    float count = 0;
    //    for (int i = 0; i < wingcount; i++)
    //    {
     //       if (DamageMan.IsWingWorking(i))
     //           count += 1;
     //   }
     //  return count * 0.25f;
    //}
    public bool ToggleMachine(bool toggle)
    {
        if (toggle)
        {
            if (!HasAlive())
                return false;
        }
        else
        {
            if (LightCondition())
                ToggleLights(false);
        }
        if (toggle && SteerThr == null)
        {
            SteerThr = new Thread();
            SteerThr.Start(this, "Steer", 0.01);
        }
        CallDeferred("ToggleEngineVFX", toggle);
        loctomove = GlobalTranslation;
        
        Working = toggle;
        SetProcess(toggle);
        return true;
    }
    public bool HasAlive()
    {
        bool hasalive = false;
        foreach (Character pas in passengers)
        {
            if (pas.IsAlive())
            {
                hasalive = true;
            }
        }
        return hasalive;
    }
    public void ToggleEngineVFX(bool toggle)
    {
        ExaustParticles[0].Emitting = toggle;
        ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        ExaustParticles[1].Emitting = toggle;
        ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        ExaustParticles[2].Emitting = toggle;
        ExaustParticles[2].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
        ExaustParticles[3].Emitting = toggle;
        ExaustParticles[3].GetNode<AudioStreamPlayer3D>("EngineSound").Playing = toggle;
    }
    public bool IsRunning()
    {
        return Working;
    }
    public bool ToggleLights(bool toggle)
    {
        if (toggle)
        {
            if (!HasAlive())
                return false;
        }
        //if (DamageMan.GetLightCondition() == false)
            //return false;
        if (toggle)
        {
            FrontLight.LightEnergy = 10;
        }
        else
        {
            FrontLight.LightEnergy = 0;
        }
        return true;
    }
    public bool LightCondition()
    {
        return FrontLight.LightEnergy == 1;
    }
    
    public void Jump()
    {
        for (int i = 0; i < Rays.Count; i ++)
        {
            RemoteTransform rem = (RemoteTransform)Rays[i];
            RayCast ray = rem.GetNode<RayCast>(rem.RemotePath);
            if (ray.IsColliding())
                AddForce(Vector3.Up * (JumpForce), Rays[i].GlobalTransform.origin - GlobalTransform.origin);
        }
    }
    
    public void BoardVehicle(Character cha)
    {
        ContactMonitor = true;
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
    public void Capsize()
    {
        if (passengers.Count == 0)
            return;
        //SetProcessInput(false);
        ContactMonitor = false;
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
        VehicleHud hud = VehicleHud.GetInstance();
        
        ToggleMachine(false);
        hud.GetButton("Engine").SetPressedNoSignal(false);
        ToggleLights(false);
        hud.GetButton("Lights").SetPressedNoSignal(false);
        chartothrowout.OnVehicleUnBoard(this);
    }
     public bool UnBoardVehicle(Character cha)
    {
        
        //SetProcessInput(false);
        Vector3 prevrot = cha.GlobalRotation;
        
        Vector3 postoput;
        if (!CheckForGround(out postoput))
        {
            cha.GetTalkText().Talk("Πρέπει να πάω πιό κοντά στην στεριά.");
            return false;
        }
        ContactMonitor = false;
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
        
        VehicleHud hud = VehicleHud.GetInstance();
        ToggleMachine(false);
        hud.GetButton("Engine").SetPressedNoSignal(false);
        ToggleLights(false);
        hud.GetButton("Lights").SetPressedNoSignal(false);

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
        if (!PlayerOwned)
            return;
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable", toggle);
    }
    //////Data Saving////////
    public void InputData(VehicleInfo data)
	{
        //if (data.removed)
        //{
        //    GetParent().QueueFree();
        //    return;
        //}
        ZeroPos();
		Translation = data.loc;
        Rotation = data.rot;
        SetPlayerOwned(data.PlayerOwned);
        //GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager").InputData(data.DamageInfo);
	}
    public void ReparentVehicle(Island ile)
    {
        Vector3 orig = GlobalTranslation;
        Vector3 origrot = GlobalRotation;

        Player pl = (Player)passengers[0];
        Vector3 prevrot = pl.GlobalRotation;

        Spatial vehpar = (Spatial)GetParent();
        Island par = (Island)vehpar.GetParent();

        if (par == ile)
            return;
        
        par.UnRegisterChild(this);
        par.RemoveChild(vehpar);
        ile.AddChild(vehpar, true);
        ile.RegisterChild(this);
        vehpar.Translation = Vector3.Zero;
        vehpar.Rotation = Vector3.Zero;
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
    //public VehicleDamageInfo DamageInfo;
    //public bool removed = false;
    public string scenedata;
    public bool PlayerOwned;
    public void UpdateInfo(Vehicle veh)
    {
        veh.ZeroPos();
        //Spatial par = (Spatial)veh.GetParent();

        loc = veh.Translation;
        rot = veh.Rotation;
        PlayerOwned = veh.IsPlayerOwned();
        //VehicleDamageManager Damageman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        //DamageInfo = new VehicleDamageInfo();
        //DamageInfo.UpdateInfo(veh);
    }
    public void SetInfo(Vehicle veh)
    {
        veh.ZeroPos();
        VehName = veh.GetParent().Name;

        loc = veh.Translation;
        rot = veh.Rotation;
           
        scenedata = veh.GetParent().Filename;
        //VehicleDamageManager Damageman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        //DamageInfo = new VehicleDamageInfo();
        PlayerOwned = veh.IsPlayerOwned();
        //List<int> destw;
        //Damageman.GetWingStates(out destw);
        //DamageInfo.SetInfo(veh);
    }
    public Dictionary<string, object>GetPackedData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"Name", VehName},
            {"Location", loc},
            {"Rotation", rot},
            {"PlayerOwned", PlayerOwned},
            {"SceneData", scenedata},
        };
        //GDScript SaveGD = GD.Load<GDScript>("res://Scripts/VehicleDamageSaveInfo.gd");
		//Resource damageinfo = (Resource)SaveGD.New();
        //damageinfo.Call("_SetData", DamageInfo.GetPackedData());

        //data.Add("DamageInfo", damageinfo);
        return data;
    }
    public void UnPackData(Godot.Object data)
    {
        VehName = (string)data.Get("Name");
        loc = (Vector3)data.Get("Location");
        rot = (Vector3)data.Get("Rotation");
        PlayerOwned = (bool)data.Get("PlayerOwned");
        scenedata = (string)data.Get("SceneData");

       // Resource DamageData = (Resource)data.Get("DamageInfo");

        //VehicleDamageInfo info = new VehicleDamageInfo();
        //info.UnPackData(DamageData);
        //DamageInfo = info;
    }
}

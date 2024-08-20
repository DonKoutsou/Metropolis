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
    //[Export]
    //bool SpawnBroken = false;
    [Export]
    Curve forcecurve = null;
    [Export]
    Curve Hoverforcecurve = null;
    [Export]
    Curve AccelerationCurve = null;
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
    bool Working = false;

    //Used to calculate steering
    Spatial SteeringWheel;

    ///////////Hover Rays////////////////
    List<Spatial> Rays = new List<Spatial>();
    //RayCast frontray;
    /////////////////////////////////////
    Vector3 loctomove;

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
    public void UpdateMoveLoc(Vector3 NewLoc)
    {
        loctomove = NewLoc;
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
    //AnimationPlayer Anim;
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
        ExaustParticles.Insert(0, GetNode<Particles>("Position3D/MeshInstance3/ExaustParticlesL"));
        ExaustParticles.Insert(1, GetNode<Particles>("Position3D/MeshInstance3/ExaustParticlesR"));
        //ExaustParticles.Insert(2, GetNode<Particles>("ExaustParticlesLSteer"));
       // ExaustParticles.Insert(3, GetNode<Particles>("ExaustParticlesRSteer"));
        ExaustParticles[0].Emitting = false;
        ExaustParticles[1].Emitting = false;
        //ExaustParticles[2].Emitting = false;
        //ExaustParticles[3].Emitting = false;
        //Spatial parent = (Spatial)GetParent();
        ZeroPos();
        //frontray = parent.GetNode<RayCast>("RayF");
        Rays.Insert(0, GetNode<Spatial>("RemoteTransform"));
        Rays.Insert(1, GetNode<Spatial>("RemoteTransform2"));
        Rays.Insert(2, GetNode<Spatial>("RemoteTransform3"));
        Rays.Insert(3, GetNode<Spatial>("RemoteTransform4"));

        BoostTimer = new Timer()
        {
            WaitTime = 40,
            OneShot = true
        };
        GetNode<VehicleBoostTrails>("VehicleBoostTrails").AddChild(BoostTimer);
        BoostTimer.Connect("timeout", this, "BoostEnded");
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
    public void DespawnVeh()
    {
        

        GetParent().QueueFree();
        Node par = GetParent();
		while (!(par is Island))
		{
            if (par == null)
				return;
			par = par.GetParent();
		}
		Island ile = (Island)par;
		ile.UnRegisterChild(this);
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
    
    float d = 0.01f;
    public override void _Process(float delta)
    {
        base._Process(delta);

        d -= delta;
        if (d > 0)
            return;
        d = 0.01f;

        float engineforce = latsspeed / speed;
        ((ParticlesMaterial)ExaustParticles[0].ProcessMaterial).Scale = engineforce * 5;
        ((ParticlesMaterial)ExaustParticles[0].ProcessMaterial).InitialVelocity = engineforce * 60 + 5;

        ExaustParticles[0].GetNode<AudioStreamPlayer3D>("EngineSound").PitchScale = engineforce + 0.6f;

        ((ParticlesMaterial)ExaustParticles[1].ProcessMaterial).Scale = engineforce * 2.5f;
        ((ParticlesMaterial)ExaustParticles[1].ProcessMaterial).InitialVelocity = engineforce * 60 + 5 * 2.5f;

        ExaustParticles[1].GetNode<AudioStreamPlayer3D>("EngineSound").PitchScale = engineforce + 0.6f;

        MeshInstance str = GetNode<MeshInstance>("Position3D/MeshInstance3");
        Vector3 strrot = str.Rotation;

        strrot.y = Mathf.Clamp( (Mathf.Pi * Math.Sign(SteeringWheel.Rotation.y)) - SteeringWheel.Rotation.y , -1, 1);
        //str.Rotation = strrot;
        var tw = CreateTween();
        tw.TweenProperty(str, "rotation", strrot, 0.25f);
    }
    public void Boost(int Ammount)
    {
        SpeedBuildup = 1;
        LinearVelocity *= Ammount;

        //EnergyBuff = Ammount * (speed * 1200);

        HasBoost = true;
        
        GetNode<VehicleBoostTrails>("VehicleBoostTrails").StartBoost();

        BoostTimer.Start();
    }
    private void BoostEnded()
    {
        HasBoost = false;
    }
    Timer BoostTimer;
    bool HasBoost = false;
    public float GetRPM()
    {
        if (HasBoost)
            return 0;
        return SpeedBuildup;
    }
    float SpeedBuildup = 0;
    
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (!PlayerOwned)
            return;

        if (!thr.IsActive())
        {
            thr = new Thread();
            thr.Start(this, "Balance", delta);
        }
        Hover(delta);
        
        float distance = loctomove.DistanceTo(GlobalTranslation);
        float distmulti = Math.Min(500, distance)/ 500;
        //range from -0.001 to 0.001
        float accelMulti = (distmulti * 2 - 1) / 1000;
        if (accelMulti > 0)
            SpeedBuildup = Mathf.Clamp(SpeedBuildup + (accelMulti * AccelerationCurve.Interpolate(SpeedBuildup)), 0, 1);
        else
            SpeedBuildup = Mathf.Clamp(SpeedBuildup + (accelMulti * 8), 0, 1);
        float fmulti = 1;

        Vector3 force = GlobalTransform.basis.z;
        force.y = 0;
        
        //sails
        if (HasWings())
        {
            Vector3 wingforce = Vector3.Zero;
            if (!IsBoatFacingWind())
            {
                Vector3 f = GlobalTransform.basis.z;
                f.y = 0;

                fmulti += DayNight.GetWindStr();

                wingforce += f * fmulti * speed;
                if (!WindOnWings)
                    EnableWindOnWings(true);
            }
            else if (IsBoatFacingWind())
            {
                if (WindOnWings)
                    EnableWindOnWings(false);
            }
            AddCentralForce(wingforce * (delta * 2));
        }
        
        latsspeed = speed * (distmulti * SpeedBuildup);

        if (!Working)
        {
            loctomove = GlobalTranslation;   
            return;
        }

        //EnergyBuff = Mathf.Max(0, EnergyBuff - latsspeed);
        
        //engine
        AddCentralForce(force * latsspeed * (delta * 2));

        if (!SteerThr.IsActive())
        {
            SteerThr = new Thread();
            SteerThr.Start(this, "Steer", delta);
        }

        AddTorque(steert);
        steert = Vector3.Zero;
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
    
    public void Hover(float delta)
    {
        float Force = Hoverforcecurve.Interpolate(latsspeed /speed) * HoverForce;

        Vector3 f ;
        for (int i = 0; i < Rays.Count; i ++)
        {
            RemoteTransform rem = (RemoteTransform)Rays[i];
           
            RayCast ray = rem.GetNode<RayCast>(rem.RemotePath);

            Vector3 grot = ray.GlobalRotation;
            ray.GlobalRotation = new Vector3(grot.x, rem.GlobalRotation.y, grot.z);

            Spatial EnginePivot = ray.GetNode<Spatial>("EnginePivot");
            ray.ForceRaycastUpdate();
            Particles part = ray.GetNode<Particles>("Particles");
            Particles partd = ray.GetNode<Particles>("ParticlesDirt");
            Particles partHover = EnginePivot.GetNode<Particles>("HoverEngineParticles");
            AudioStreamPlayer3D sound = ray.GetNode<AudioStreamPlayer3D>("HoverSound");
            Vector3 engrot = new Vector3(Mathf.Deg2Rad(Mathf.Lerp(0, 45, latsspeed /speed)), 0, 0);
            if (ray.IsColliding())
            {
                var collisionpoint = ray.GetCollisionPoint();
                var collisionobj = ray.GetCollider();
                var dist = collisionpoint.DistanceTo(ray.GlobalTranslation);
                float distmulti = dist / - ray.CastTo.y;

                Particles parttotranslate;
                if (((Node)collisionobj).Name == "SeaBed")
                {
                    part.Emitting = Working;
                    partd.Emitting = false;

                    parttotranslate = part;
                }
                else
                {
                    part.Emitting = false;
                    partd.Emitting = Working;

                    parttotranslate = partd;
                }
                if (dist <= 35)
                {
                    float particleoffset = dist;

                    parttotranslate.Translation = new Vector3(parttotranslate.Translation.x, - particleoffset, parttotranslate.Translation.z);
                }
                
                float multi = forcecurve.Interpolate(distmulti);

                f= Vector3.Up * Force * delta * multi;
                partHover.Emitting = Working;
                if (sound.Playing != Working)
                    sound.Playing = Working;
                
                sound.PitchScale = Mathf.Lerp(0.5f, 1.5f, latsspeed /speed);
            }
            else
            {
                part.Emitting = false;
                if (!sound.Playing)
                    sound.Playing = false;
                partd.Emitting = false;
                partHover.Emitting = false;
                f = Vector3.Up * 8000 * delta * -8;
            }
            EnginePivot.Rotation = engrot;
            if (Working)
                AddForce(f, ray.GlobalTranslation - GlobalTranslation);
        }
        if (Working)
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
        if (loctomove.DistanceTo(SteeringWheel.GlobalTranslation) > 0.01f)
        {
            Vector3 prevpos = SteeringWheel.Rotation;
            SteeringWheel.LookAt(loctomove, Vector3.Up);
            SteeringWheel.Rotation = new Vector3(prevpos.x, SteeringWheel.Rotation.y, prevpos.z);
            
        }

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
        //AddTorque(torq);
        steert = torq;
        SteerThr.CallDeferred("wait_to_finish");
    }
    
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
        ToggleEngineVFX(toggle);
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
        cha.GettingInVehicle = true;
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
        
        //Rotation = new Vector3(0,Rotation.y,0);
        Vector3 prevrot = cha.GlobalRotation;

        cha.GetParent().RemoveChild(cha);

        Spatial guy = cha.GetNode<Spatial>("Pivot");
        Spatial CamPiv = cha.GetNode<Spatial>("CameraMovePivot");
        cha.Anims().ToggleSitting();

        cha.SetCollisionMaskBit(4, false);
        GetParent().AddChild(cha);
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        RemoteTransform CameraRemot = GetNode<RemoteTransform>("CharacterCameraRemoteTransform");
        CharTrasn.RemotePath = cha.GetPath();
        CharTrasn2.RemotePath = guy.GetPath();
        CameraRemot.RemotePath = CamPiv.GetPath();
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

        chartothrowout.GettingInVehicle = true;

        chartothrowout.Anims().ToggleIdle();
        passengers.Clear();
        Vector3 prevrot = chartothrowout.GlobalRotation;
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        RemoteTransform CamTrans = GetNode<RemoteTransform>("CharacterCameraRemoteTransform");
        CamTrans.RemotePath = this.GetPath();

        chartothrowout.GetNode<Spatial>("CameraMovePivot").Translation = Vector3.Zero;
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
        cha.GettingInVehicle = true;
        Vector3 prevrot = cha.GlobalRotation;
        
        Vector3 postoput;
        if (!CheckForGround(out postoput))
        {
            DialogueManager.GetInstance().ForceDialogue(cha, "Πρέπει να πάω πιό κοντά στην στεριά.");
            //cha.GetTalkText().Talk("Πρέπει να πάω πιό κοντά στην στεριά.");
            return false;
        }
        ContactMonitor = false;
        passengers.Clear();
        RemoteTransform CharTrasn = GetNode<RemoteTransform>("CharacterRemoteTransform");
        CharTrasn.RemotePath = this.GetPath();
        RemoteTransform CharTrasn2 = GetNode<RemoteTransform>("CharacterRemoteTransform2");
        CharTrasn2.RemotePath = this.GetPath();
        RemoteTransform CamTrans = GetNode<RemoteTransform>("CharacterCameraRemoteTransform");
        CamTrans.RemotePath = this.GetPath();

        cha.GetNode<Spatial>("CameraMovePivot").Translation = Vector3.Zero;
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
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (!PlayerOwned)
            return;

        if (toggle)
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
		if (!IsPlayerOwned())
        {
            DialogueManager.GetInstance().ForceDialogue(pl, "Δεν είναι δικιά μου. Δεν μπορώ να την χρησιμοποιήσω.");
            //pl.GetTalkText().Talk("Δεν είναι δικιά μου. Δεν μπορώ να την χρησιμοποιήσω.");
            return;
        }
        if (!pl.HasVehicle())
        {
            BoardVehicle(pl);
            pl.SetVehicle(this);
        }
        else
        {
            if (!UnBoardVehicle(pl))
                return;
            pl.SetVehicle(null);
        }
	}
    public string GetActionName(Player pl)
    {
        string actiontex;
        if (pl.HasVehicle() && pl.GetVehicle() == this)
        {
            actiontex = "Αποβιβάση";
        } 
        else
            actiontex = "Επιβιβάση";

        return actiontex;
    }
    public string GetActionName2(Player pl)
    {
		return "Null.";
    }
    public string GetActionName3(Player pl)
    {
		return "Null.";
    }
    public bool ShowActionName2(Player pl)
    {
        return false;
    }
    public bool ShowActionName3(Player pl)
    {
        return false;
    }
    public bool ShowActionName(Player pl)
    {
        return true;
    }
    public string GetObjectDescription()
    {
        string desc;
        if (IsPlayerOwned())
            desc = "Καΐκάρα μου!";
        else
            desc = "Καΐκι, δεν είμαι σίγουρος πιανού.";
        return desc;
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





////////////////////old stuff
/////public bool ToggleWings(bool toggle)
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
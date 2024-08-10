using Godot;
using System;
using System.Collections.Generic;
public class WindGenerator : StaticBody
{
    [Export]
    private float EnergyCapacity = 100;
    [Export]
    private float CurrentEnergy;
    [Export]
    private float EnergyPerWindStreangth = 0.1f;
    AnimationPlayer anim;
    AnimationPlayer anim2;
    AnimationPlayer anim3;
    static Random rand = new Random(69420);

    [Export]
    bool Auto = false;
    [Export]
    bool HasInternals = false;

    public float GetCurrentEnergy()
    {
        return CurrentEnergy;
    }
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
    }

    int scale;
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        anim2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        anim3 = GetNode<AnimationPlayer>("AnimationPlayer3");
        anim.CurrentAnimation = "Blade_Rot";
        anim2.CurrentAnimation = "Blade_Rot";
        if (Auto)
        {
            SetProcess(false);
            anim.Play();
            anim2.Play();
            return;
        }
            
        anim3.CurrentAnimation = "LookDir";
        anim.Advance(rand.Next(0, 5000) / 1000);
        //anim.PlaybackSpeed = rand.Next(2500, 3000) / 1000;
        anim3.Stop();
        anim2.Advance(rand.Next(0, 5000) / 1000);
        //anim2.PlaybackSpeed = rand.Next(2500, 3000) / 1000;
        //Spatial rotorpivot = GetNode<Spatial>("Rotor_Pivot");
        //rotorpivot.LookAt(new Vector3(rotorpivot.Translation.x, rotorpivot.Translation.y, rotorpivot.Translation.z + 1), Vector3.Up);
        scale = Math.Max((int)(Scale.x * GetNode<MeshInstance>("MeshInstance").Scale.x), 1);
        Node parent = GetParent();
		while (!(parent is Island))
		{
            if (parent == null)
            {
                SetProcess(false);
                return;
            }
				
			parent = parent.GetParent();
		}
		Island ile = (Island)parent;
		ile.RegisterChild(this);

        if (HasInternals)
        {
            GetNode<Spatial>("GenInternals").Hide();
            GetNode<Spatial>("GenInternals2").Hide();
        }
        //WindGenThread = new Thread();
        //WindGenThread.Start(this, "UpdateGenerator", GlobalRotation);
    }
    //Thread WindGenThread;
    //Mutex mut = new Mutex();
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;

        UpdateGenerator();
        /*if (!WindGenThread.IsAlive())
        {
            WindGenThread.WaitToFinish();
            WindGenThread = new Thread();
            WindGenThread.Start(this, "UpdateGenerator", GlobalRotation);
                
        }*/
    }
    public void UpdateGenerator()
    {
        //mut.Lock();
        float winddir = DayNight.GetWindDirection();
        float windstr = DayNight.GetWindStr();
        //mut.Unlock();
        //Spatial rotorpivot = GetNode<Spatial>("Rotor_Pivot");
        float rot = winddir;
        //mut.Lock();
        Vector3 originalRotation = GlobalRotation;
        //mut.Unlock();
        rot += Mathf.Rad2Deg(originalRotation.y) + 180;
        if (rot > 360)
            rot = rot - 360;
        anim3.Seek(rot / 36, true);
        //rotorpivot.GlobalRotation = new Vector3(0.0f, rot, 0.0f);
        float animspeed = windstr * 0.03f;
        float energy = EnergyPerWindStreangth * (windstr/100);
 
        anim.PlaybackSpeed = Mathf.Max(animspeed / scale, 0);
        anim2.PlaybackSpeed = Mathf.Max(animspeed / scale, 0);
        
        if (CurrentEnergy + energy < EnergyCapacity)
        {
            //mut.Lock();
            CurrentEnergy += energy;
            //mut.Unlock();
        }
    }
    private void _on_Generator_visibility_changed()
    {
        if (Visible)
        {
            anim.Play();
            anim2.Play();
            SetProcess(true);
        }
        
        else
        {
            anim.Stop();
            anim2.Stop();
            SetProcess(false);
        }
    }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (HasInternals)
            return;

        if (toggle)
            GetNode<MeshInstance>("MeshInstance2").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("MeshInstance2").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
        List<Item> batteries;
        pl.GetCharacterInventory().GetItemsByType(out batteries, ItemName.BATTERY);
        float availableenergy = GetCurrentEnergy();
        if (availableenergy <= 1)
        {
            pl.GetTalkText().Talk("Έχει ξεμίνει από ενέργεια...");
            return;
        }
        float rechargeamm = 0;
        for (int i = batteries.Count - 1; i > -1; i--)
        {
            if (availableenergy <= 0)
                break;
            Battery bat = (Battery)batteries[i];
            float cap = bat.GetCapacity();
            float energy = bat.GetCurrentCap();
            if (energy < cap)
            {
                float reachargeammount = cap - energy;
                if (availableenergy > reachargeammount)
                {
                    bat.Recharge(reachargeammount);
                    availableenergy -= reachargeammount;
                    rechargeamm += reachargeammount;
                }
                else
                {
                    bat.Recharge(availableenergy);
                    rechargeamm += availableenergy;
                    availableenergy = 0;
                }
            }
        }
        float charrechargeamm = pl.GetCharacterBatteryCap() - pl.GetCurrentCharacterEnergy();
        if (charrechargeamm > availableenergy)
        {
            pl.RechargeCharacter(charrechargeamm);
            rechargeamm += charrechargeamm;
        }
        else
        {
            pl.RechargeCharacter(availableenergy);
            rechargeamm += availableenergy;
        }
        ConsumeEnergy(rechargeamm);
        int time = (int)Math.Round(rechargeamm / 6);
        int days, hours, mins;
        DayNight.MinsToTime(time, out days,out hours, out mins);
        DayNight.ProgressTime(days, hours, mins);
    }
    public string GetActionName(Player pl)
    {
        return "Φόρτιση";
    }
    public bool ShowActionName(Player pl)
    {
        return true;
    }
    public string GetObjectDescription()
    {
        return "Γεννήτρια";
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
    public void SetData(WindGeneratorInfo info)
	{
        int curday;
        DayNight.GetDay(out curday);
        int curHours, curMins;
        DayNight.GetTime(out curHours, out curMins);
        int hours = curHours - info.Despawnhour;
        int days = curday - info.DespawnDay;
        while (days > 0)
        {
            days -= 1;
            hours += 24;
        }
		CurrentEnergy = Math.Min(info.CurrentEnergy + hours, EnergyCapacity);
	}
    /*private void CharacterEntered(Node body)
    {
        if (!IsInsideTree())
            return;
        ((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Front;
        SetCollisionLayerBit(2, false);
    }
    private void CharacterLeft(Node body)
    {
        if (!IsInsideTree())
            return;
        ((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).ParamsCullMode = SpatialMaterial.CullMode.Disabled;
        SetCollisionLayerBit(2, true);
    }*/
    private void CharacterEntered(Node body)
    {
        GetNode<Spatial>("GenInternals").Show();
        GetNode<Spatial>("GenInternals2").Show();

        AudioServer.SetBusEffectEnabled(0,0, true);
    }
    private void CharacterLeft(Node body)
    {
        GetNode<Spatial>("GenInternals").Hide();
        GetNode<Spatial>("GenInternals2").Hide();

        AudioServer.SetBusEffectEnabled(0,0, false);
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        if (!HasInternals)
            return;
        GetNode<Spatial>("GenInternals").GetNode<AnimationPlayer>("AnimationPlayer").Play("Gen");
        GetNode<Spatial>("GenInternals2").GetNode<AnimationPlayer>("AnimationPlayer").Play("Gen");
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        if (!HasInternals)
            return;
        GetNode<Spatial>("GenInternals").GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetNode<Spatial>("GenInternals2").GetNode<AnimationPlayer>("AnimationPlayer").Stop();
    }
}

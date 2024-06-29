using Godot;
using System;
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

    public float GetCurrentEnergy()
    {
        return CurrentEnergy;
    }
    public void ConsumeEnergy(float ammount)
    {
        CurrentEnergy -= ammount;
    }
    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
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
    public void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance2").GetActiveMaterial(0).NextPass).SetShaderParam("enable", toggle);
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
    private void CharacterEntered(Node body)
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
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GlobalJobManager : Node
{
    static GlobalJobManager Instance;
    List<Job> Jobs = new List<Job>();
    int JobAmmount = 0;

    List<Job> AssignedJobs = new List<Job>();

    public void OnJobAssigned(Job j)
    {
        AssignedJobs.Add(j);
        InventoryUI.GetInstance().ConfigureJob(j);
    }
    public void OnJobCanceled(Job j)
    {
        AssignedJobs.Remove(j);
    }
    public bool HasJobAssigned()
    {
        return AssignedJobs.Count > 0;
    }
    public override void _Ready()
    {
        Instance = this;
    }
    public static GlobalJobManager GetInstance()
    {
        return Instance;
    }
    public void OnNewDay()
    {
        Jobs.Clear();
        JobAmmount = RandomContainer.Next(4, 9);
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (JobAmmount > Jobs.Count)
        {
            int dif = RandomContainer.Next(0,3);
            DeliverJob d = CreateDeliver((Difficulty)dif);
            foreach (Job j in Jobs)
            {
                if (j.GetLocation() == d.GetLocation())
                {
                    return;
                }
            }
            Jobs.Add(d);
        }
        if (Jobs.Count == JobAmmount)
        {
            SetProcess(false);
        }
    }
    public void GetJobs(out Job[] jobs)
    {
        jobs = new Job[Jobs.Count];
        for(int i = 0; i < Jobs.Count; i++)
        {
            jobs[i] = Jobs[i];
        }
    }
    private DeliverJob CreateDeliver(Difficulty dif)
    {
        WorldMap map = WorldMap.GetInstance();
        DeliverJob j = null;
        if (dif == Difficulty.Easy)
        {
            IslandInfo ile = map.GetRandomLightHouse(1, 10);
            j = new DeliverJob(20, ile.SpecialName, ile.Position);
        }
        else if (dif == Difficulty.Medium)
        {
            IslandInfo ile = map.GetRandomLightHouse(11, 25);
            j = new DeliverJob(50, ile.SpecialName, ile.Position);
        }
        else if (dif == Difficulty.Hard)
        {
            IslandInfo ile = map.GetRandomLightHouse(25, 41);
            j = new DeliverJob(100, ile.SpecialName, ile.Position);
        }
        return j;
    }
    private void CreateEscort(Difficulty dif)
    {
        WorldMap map = WorldMap.GetInstance();
        if (dif == Difficulty.Easy)
        {
            IslandInfo ile = map.GetRandomLightHouse(1, 10);
            EscortJob j = new EscortJob(20, "Quifsa", ile.Position);
        }
        else if (dif == Difficulty.Medium)
        {
            IslandInfo ile = map.GetRandomLightHouse(11, 25);
            EscortJob j = new EscortJob(40, "Quifsa", ile.Position);
        }
        else if (dif == Difficulty.Hard)
        {
            IslandInfo ile = map.GetRandomLightHouse(25, 41);
            EscortJob j = new EscortJob(60, "Quifsa", ile.Position);
        }
    }
    private void CreateRescue(Difficulty dif)
    {
        WorldMap map = WorldMap.GetInstance();
        if (dif == Difficulty.Easy)
        {
            IslandInfo ile = map.GetRandomIle(1, 10);
        }
        else if (dif == Difficulty.Medium)
        {
            IslandInfo ile = map.GetRandomIle(11, 25);
        }
        else if (dif == Difficulty.Hard)
        {
            IslandInfo ile = map.GetRandomIle(25, 41);
        }
    }

}
public class Job
{
    protected PackedScene Reward = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Drahma.tscn");
    protected int RewardAmmount = 10;
    protected NPC JobOwner = null;
    protected Vector2 Location;
    protected string owner;
    public PackedScene GetReward()
    {
        return Reward;
    }
    public int GetRewardAmmount()
    {
        return RewardAmmount;
    }
    public virtual string GetJobName()
    {
        return "Δουλειά";
    }
   // public NPC GetJobOwner()
    //{
    //    return JobOwner;
    //}
    public Job(int amm, string OwnerName, Vector2 loc)
    {
        RewardAmmount = amm;
        owner = OwnerName;
        Location = loc;
    }
    public string GetOwnerName()
    {
        return owner;
    }
    public Vector2 GetLocation()
    {
        return Location;
    }
}
public class RescueJob : Job
{
    public RescueJob(int amm, string OwnerName, Vector2 loc) : base(amm, OwnerName, loc)
    {
        RewardAmmount = amm;
        owner = OwnerName;
        Location = loc;
    }
    public override string GetJobName()
    {
        return "Διάσωση";
    }
}
public class DeliverJob : Job
{
    PackedScene ObjectToDeliver = ResourceLoader.Load<PackedScene>("res://Scenes/Vehicles/BoatCargo.tscn");

    public DeliverJob(int amm, string OwnerName, Vector2 loc) : base(amm, OwnerName, loc)
    {
        RewardAmmount = amm;
        owner = OwnerName;
        Location = loc;
    }
    public override string GetJobName()
    {
        return "Μεταφορά";
    }
    
}
public class EscortJob : Job
{
    public EscortJob(int amm, string OwnerName, Vector2 loc) : base(amm, OwnerName, loc)
    {
        RewardAmmount = amm;
        owner = OwnerName;
    }
    public override string GetJobName()
    {
        return "Συνοδεία";
    }
}

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
        PlayerUI.GetInstance().GetUI(PlayerUIType.INVENTORY).Call("ConfigureJob", j);
    }
    public void OnJobCanceled(Job j)
    {
        AssignedJobs.Remove(j);
    }
    public bool HasJobAssigned()
    {
        return AssignedJobs.Count > 0;
    }
    public List<Job> GetJobs()
    {
        return Jobs;
    }
    public List<Job> GetAssignedJobs()
    {
        return AssignedJobs;
    }
    public void LoadDeliverJobs(Vector2[] DJobs, int activeDJob)
    {
        for (int i = 0; i < DJobs.Count(); i++)
        {
            Vector2 pos = DJobs[i];
            
            float dist = Math.Max(pos.x, pos.y);

            int reward = 20;
            if (dist > 24)
                reward = 100;
            else if (dist > 10)
                reward = 50;

            IslandInfo ile = WorldMap.GetInstance().GetIle(pos);
            
            DeliverJob j = new DeliverJob(reward, ile.SpecialName, ile.Position);

            Jobs.Add(j);
            if (i == activeDJob)
            {
                AssignedJobs.Add(j);
                PlayerUI.GetInstance().GetUI(PlayerUIType.INVENTORY).Call("ConfigureJob", j);
                if (j is DeliverJob del)    
                {
                    var b = GetTree().GetNodesInGroup("PlayerBoat");
                    
                    Spatial cargo = del.GetDeliveryObj().Instance<Spatial>();
                    ((Vehicle)b[0]).AddChild(cargo);
                    cargo.Translation = Vector3.Zero;
                    cargo.Rotation = Vector3.Zero;
                }
            }
                
        }
    }
    public void SetJobAmm(int ammount)
    {
        JobAmmount = ammount;
    }
    private DeliverJob CreateDeliver(Difficulty dif)
    {
        WorldMap map = WorldMap.GetInstance();
        DeliverJob j = null;
        int reward = 0;
        IslandInfo ile = null;
        if (dif == Difficulty.Easy)
        {
            ile = map.GetRandomLightHouse(1, 10);
            reward = 20;
        }
        else if (dif == Difficulty.Medium)
        {
            ile = map.GetRandomLightHouse(11, 25);
            reward = 50;
        }
        else if (dif == Difficulty.Hard)
        {
            ile = map.GetRandomLightHouse(25, 41);
            reward = 100;
        }
        if (ile == null)
            return null;
        j = new DeliverJob(reward, ile.SpecialName, ile.Position);
        return j;
    }
    public bool HasJobOnIsland(Vector2 pos, out List<Job> Jobs)
    {
        Jobs = new List<Job>();
        foreach (Job j in AssignedJobs)
        {
            if (j.GetLocation() == pos)
            {
                Jobs.Add(j);
            }
        }
        return Jobs.Count > 0;
    }
    //return reward ammount
    public int OnJobFinished(Job j)
    {
        Player pl = Player.GetInstance();

        Inventory inv = pl.GetCharacterInventory();

        PackedScene rew = ResourceLoader.Load<PackedScene>(j.GetReward());

        int rewamm = j.GetRewardAmmount();

        for (int i = 0; i < rewamm; i++)
        {
            inv.InsertItem(rew.Instance<Item>());
        }
        AssignedJobs.Remove(j);
        Jobs.Remove(j);

        return rewamm;
    }
    public override void _Ready()
    {
        Instance = this;
        SetProcess(false);
    }
    public static GlobalJobManager GetInstance()
    {
        return Instance;
    }
    public void OnNewDay()
    {
        if (AssignedJobs.Count == 0)
            Jobs.Clear();
        else
        {
            foreach (Job j in Jobs)
            {
                if (AssignedJobs.Contains(j))
                    continue;
                Jobs.Remove(j);
            }
        }
        
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
            if (d == null)
                return;
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
    protected string Reward = "res://Scenes/Items/Drahma.tscn";
    protected int RewardAmmount = 10;
    protected Vector2 Location;
    protected string owner;
    public string GetReward()
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
    readonly PackedScene ObjectToDeliver = ResourceLoader.Load<PackedScene>("res://Scenes/Vehicles/BoatCargo.tscn");

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
    public PackedScene GetDeliveryObj()
    {
        return ObjectToDeliver;
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
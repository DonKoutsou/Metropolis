using Godot;
using System;

public class GlobalJobManager : Node
{
    static GlobalJobManager Instance;
    public override void _Ready()
    {
        Instance = this;
    }
    public static GlobalJobManager GetInstance()
    {
        return Instance;
    }
    private void CreateDeliver(Difficulty dif)
    {
        WorldMap map = WorldMap.GetInstance();
        if (dif == Difficulty.Easy)
        {
            IslandInfo ile = map.GetRandomLightHouse(1, 10);
            DeliverJob j = new DeliverJob(20, "Quifsa", ile.Position);
        }
        else if (dif == Difficulty.Medium)
        {
            IslandInfo ile = map.GetRandomLightHouse(11, 25);
            DeliverJob j = new DeliverJob(40, "Quifsa", ile.Position);
        }
        else if (dif == Difficulty.Hard)
        {
            IslandInfo ile = map.GetRandomLightHouse(25, 41);
            DeliverJob j = new DeliverJob(60, "Quifsa", ile.Position);
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
    protected PackedScene Reward = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Drahma.tscn");
    protected int RewardAmmount = 10;
    protected NPC JobOwner = null;
    
    protected string owner;
    public PackedScene GetReward()
    {
        return Reward;
    }
    public int GetRewardAmmount()
    {
        return RewardAmmount;
    }
   // public NPC GetJobOwner()
    //{
    //    return JobOwner;
    //}
    public Job(int amm, string OwnerName)
    {
        RewardAmmount = amm;
        owner = OwnerName;
    }
}
public class RescueJob : Job
{
    Vector2 Location;
    public RescueJob(int amm, string OwnerName, Vector2 loc) : base(amm, OwnerName)
    {
        RewardAmmount = amm;
        owner = OwnerName;
        Location = loc;
    }
}
public class DeliverJob : Job
{
    PackedScene ObjectToDeliver = ResourceLoader.Load<PackedScene>("res://Scenes/Vehicles/BoatCargo.tscn");
    Vector2 DeliverDestination;

    public DeliverJob(int amm, string OwnerName, Vector2 dest) : base(amm, OwnerName)
    {
        RewardAmmount = amm;
        owner = OwnerName;
        DeliverDestination = dest;
    }

}
public class EscortJob : Job
{
    Vector2 EscortLoc;
    public EscortJob(int amm, string OwnerName, Vector2 Loc) : base(amm, OwnerName)
    {
        RewardAmmount = amm;
        owner = OwnerName;
        EscortLoc = Loc;
    }
}

using Godot;
using System;

public class JobBoard : Control
{
    public void GenerateJobs()
    {

    }
    public void CreateJob(JobType type, Difficulty dif)
    {
        
    }
    private void CreateRescue(Difficulty dif)
    {
        
    }
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
public enum JobType
{
    Rescue,
    Deliver,
    Escort,
}
public enum Difficulty
{
    Easy,
    Medium,
    Hard,
}
public class Job
{
    public PackedScene Reward = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Drahma.tscn");
    public int RewardAmmount = 10;
    public NPC JobOwner = null;

    public PackedScene GetReward()
    {
        return Reward;
    }
    public int GetRewardAmmount()
    {
        return RewardAmmount;
    }
    public NPC GetJobOwner()
    {
        return JobOwner;
    }
    public Job(PackedScene Rewards, int amm, NPC owner)
    {
        Reward = Rewards;
        RewardAmmount = amm;
        JobOwner = owner;
    }
}
public class RescueJob : Job
{
    Vector2 Location;
    public RescueJob(PackedScene Rewards, int amm, NPC owner, Vector2 loc) : base(Rewards, amm, owner)
    {
        Reward = Rewards;
        RewardAmmount = amm;
        JobOwner = owner;
        Location = loc;
    }
}
public class DeliverJob : Job
{
    PackedScene ObjectToDeliver = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Drahma.tscn");
    Vector2 DeliverDestination;

    public DeliverJob(PackedScene Rewards, int amm, NPC owner, PackedScene ObjTodel, Vector2 dest) : base(Rewards, amm, owner)
    {
        Reward = Rewards;
        RewardAmmount = amm;
        JobOwner = owner;
        ObjectToDeliver = ObjTodel;
        DeliverDestination = dest;
    }

}
public class EscortJob : Job
{
    Vector2 EscortLoc;
    public EscortJob(PackedScene Rewards, int amm, NPC owner, Vector2 Loc) : base(Rewards, amm, owner)
    {
        Reward = Rewards;
        RewardAmmount = amm;
        JobOwner = owner;
        EscortLoc = Loc;
    }
}
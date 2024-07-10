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

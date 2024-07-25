using Godot;
using System;

public class RandomContainer : Node
{
    static Random RandomInstance;
    static int RandomTimes;

    public static void OnGameStart(int seed)
    {
        RandomInstance = new Random(seed);
    }
    public static int Next(int from, int to)
    {
        RandomTimes ++;
        return RandomInstance.Next(from, to);
    }
    public static int GetState()
    {
        return RandomTimes;
    }
    public static void LoadState(int times, int seed)
    {
        RandomTimes = times;

        RandomInstance = new Random(seed);
		for (int i = 0; i < RandomTimes; i++)
		{
			RandomInstance.NextDouble();
		}
    }
    public RandomContainer()
    {
        RandomInstance = new Random();
    }
    public override void _Ready()
    {
        base._Ready();
        //
    }
    /*public static double NextDouble(double from, double to)
    {
        RandomTimes++;
        return RandomInstance.NextDouble(from, to);
    }*/
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

}

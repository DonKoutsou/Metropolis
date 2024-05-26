using Godot;
using System;

public class LoadingScreen : Control
{
    [Export]
    int WaitTime;

    static int Wtime;
    static LoadingScreen Instance;
    public override void _Ready()
    {
        base._Ready();
        Hide();
        Instance = this;
        SetProcess(false);
        GetNode<ProgressBar>("ProgressBar").MaxValue = WaitTime;
        Wtime = WaitTime;
    }
    public static int GetWaitTime()
    {
        return Wtime;
    }
    public static LoadingScreen GetInstance()
    {
        return Instance;
    }
    public void Start()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
        Show();
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        WorldMap map = WorldMap.GetInstance();
        if (map == null)
            return;

        GetNode<ProgressBar>("ProgressBar").Value = map.currentile;

        if (map.currentile > WaitTime)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
            SetProcess(false);
        }
    }
}

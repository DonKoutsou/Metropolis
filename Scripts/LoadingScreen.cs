using Godot;
using System;

public class LoadingScreen : Control
{
    static LoadingScreen Instance;
    public override void _Ready()
    {
        base._Ready();
        Hide();
        Instance = this;
        SetProcess(false);
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

        GetNode<ProgressBar>("ProgressBar").Value = map.currentile / 3;

        if (map.currentile > 300)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
            SetProcess(false);
        }
    }
}

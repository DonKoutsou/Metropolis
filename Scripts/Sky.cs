using Godot;
using System;

public class Sky : Spatial
{
    static WorldParticleManager SkyParticleManager;
    static CustomEnviroment enviroment;
    public override void _Ready()
    {
        SkyParticleManager = GetNode<WorldParticleManager>("WorldParticleManager");
        enviroment = GetNode<CustomEnviroment>("DayNightController");

        SunMoonPivot sunmoonpiv = SkyParticleManager.GetNode<SunMoonPivot>("SunMoonPivot");
		enviroment.SunMoonMeshPivot = sunmoonpiv;
    }
    public static void OnPlayerSpawned(Player pl)
    {
        pl.GetNode<RemoteTransform>("WorldParticleRemoteTransform").RemotePath = SkyParticleManager.GetPath();
    }
    public static void OnGameStart()
    {
        enviroment.SetProcess(true);
        WorldSoundManager.PlaySound("Rain");
        WorldSoundManager.PlaySound("Enviroment");
        SkyParticleManager.GetNode<Rain>("WindAllignedParticles/Rain").SetProcess(true);
    }
    public static CustomEnviroment GetEnviroment()
    {
        return enviroment;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

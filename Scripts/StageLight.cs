using Godot;
using System;

public class StageLight : MeshInstance
{
    public void StagePassed()
    {
        GetNode<AnimationPlayer>("StageLightAnim").Play("StagePass");
        GetNode<AudioStreamPlayer>("LightSound").PitchScale = 1.1f;
        GetNode<AudioStreamPlayer>("LightSound").Play();
    }
    public void StageSemiPass()
    {
        GetNode<AnimationPlayer>("StageLightAnim").Play("StageSemiPass");
        GetNode<AudioStreamPlayer>("LightSound").PitchScale = 1;
        GetNode<AudioStreamPlayer>("LightSound").Play();
    }
    public void Reset()
    {
        GetNode<AnimationPlayer>("StageLightAnim").Play("RESET");
    }
}

using Godot;
using System;

public class StageLight : MeshInstance
{
    public void StagePassed()
    {
        GetNode<AnimationPlayer>("StageLightAnim").Play("StagePass");
    }
    public void StageSemiPass()
    {
        GetNode<AnimationPlayer>("StageLightAnim").Play("StageSemiPass");
    }
    public void Reset()
    {
        GetNode<AnimationPlayer>("StageLightAnim").Play("RESET");
    }
}

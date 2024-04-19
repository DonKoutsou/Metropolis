using Godot;
using System;

public class SeaManager : Node
{
    [Export]
    float minvalue = -1;
    [Export]
    float MaxValue = 2;

    float currentvalue = 0;

    static SceneTree tree; 
    public override void _Ready()
    {
        base._Ready();
        tree = GetTree();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public static void SyncSeas()
    {
        var seas = tree.GetNodesInGroup("Sea");
        foreach (Node sea in seas)
        {
            sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").Stop(true);
            sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").Play("Wave");
        }
    }
}

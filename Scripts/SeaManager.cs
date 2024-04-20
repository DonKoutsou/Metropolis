using Godot;
using System;

public class SeaManager : Node
{
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
        float animstage = 0;
        foreach (Node sea in seas)
        {
            if (animstage == 0)
            {
                animstage = sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimationPosition;
            }
            sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").Seek(animstage);
            sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").Play();
        }
    }
}

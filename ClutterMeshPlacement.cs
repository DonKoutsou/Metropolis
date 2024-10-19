using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class ClutterMeshPlacement : Spatial
{
    #if DEBUG
    [Export]
    bool TurnMeshesToMulti = false;
    [Export]
    bool TurnMultiToMeshes = false;

    public override void _Process(float delta)
    {      
        if (Engine.EditorHint)
        {
            if (TurnMultiToMeshes)
                TurnMultiToMeshesFunc();
            if (TurnMeshesToMulti)
                TurnMeshesToMultiFunc();
        }
    }
    private void TurnMultiToMeshesFunc()
    {
        foreach (CustomMultiMesh bod in GetNode<Spatial>("MultiMeshes").GetChildren())
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>(bod.FileThing);
            for(int i = 0; i < bod.Multimesh.InstanceCount; i++)
            {
                Spatial spawned = scene.Instance<Spatial>();
                
                AddChild(spawned);
                if (GetParent() is Island)
                    spawned.Owner = GetParent();
                else
                    spawned.Owner = this;

                spawned.Transform = bod.Multimesh.GetInstanceTransform(i);
            }
            bod.QueueFree();
        }
        foreach (StaticBody bod in GetNode<Spatial>("Collisions").GetChildren())
        {
            bod.QueueFree();
        }
        TurnMultiToMeshes = false;
    }
    private void TurnMeshesToMultiFunc()
    {
        if (GetChildCount() == 2)
        {
            TurnMeshesToMulti = false;
            return;
        }
            
        var MultiChildren = GetNode<Spatial>("MultiMeshes").GetChildren();

        List<string> SceneList = new List<string>();
        for (int i = 0; i < GetChildCount(); i ++)
        {
            if (GetChild(i) is StaticBody bod)
            {
                if (!SceneList.Contains(bod.Filename))
                    SceneList.Add(bod.Filename);
            }
        }
        foreach (StaticBody bod in GetNode<Spatial>("Collisions").GetChildren())
        {
            bod.QueueFree();
        }
        
        foreach (string M in SceneList)
        {
            List<StaticBody> Bodies = new List<StaticBody>();
            for (int i = 0; i < GetChildCount(); i ++)
            {
                if (GetChild(i) is StaticBody n)
                {
                    if (n.Filename == M)
                        Bodies.Add(n);
                }
            }
            CustomMultiMesh Multi = null;
            foreach (CustomMultiMesh mmesh in MultiChildren)
            {
                if (mmesh.FileThing == M)
                {
                    Multi = mmesh;
                    break;
                }
            }
            if (Multi == null)
            {
                Multi = ResourceLoader.Load<PackedScene>("res://Scenes/Systems/CustomMultiMesh.tscn").Instance<CustomMultiMesh>();
                Multi.Multimesh = new MultiMesh(){
                    Mesh = Bodies[0].GetNode<MeshInstance>("MeshInstance").Mesh,
                    TransformFormat = MultiMesh.TransformFormatEnum.Transform3d,
                    InstanceCount = Bodies.Count
                };           
                Multi.FileThing = M;
                Multi.MaterialOverride = Bodies[0].GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0);
            }
            else
            {
                Multi.Multimesh.InstanceCount = Bodies.Count;
            }

            GetNode<Spatial>("MultiMeshes").AddChild(Multi);
            Multi.Owner = GetParent();;

            StaticBody body = new StaticBody(){
                CollisionMask = Bodies[0].CollisionMask,
                CollisionLayer = Bodies[0].CollisionLayer
            };
            GetNode<Spatial>("Collisions").AddChild(body);
            body.Owner = GetParent();;

            

            //Shape ColShape = Bodies[0].GetNode<CollisionShape>("CollisionShape").Shape;
            for (int i = 0; i < Bodies.Count; i++)
            {
                Dictionary<Shape, Transform> Shapes = new Dictionary<Shape, Transform>();
                for (int v = 0; v < Bodies[i].GetChildCount(); v++)
                {
                    if (Bodies[i].GetChild(v) is CollisionShape col)
                    {
                        Shapes.Add(col.Shape, col.GlobalTransform);
                    }
                }
                foreach (KeyValuePair<Shape, Transform> ShapePair in Shapes)
                {
                    CollisionShape shap = new CollisionShape(){
                    Shape = ShapePair.Key
                    };
                    body.AddChild(shap);
                    shap.Owner = GetParent();;
                    shap.Transform = ShapePair.Value;
                }
                Transform meshtrans = Bodies[i].GetNode<MeshInstance>("MeshInstance").GlobalTransform;

                Multi.Multimesh.SetInstanceTransform(i, meshtrans);
            }
        }
        for (int i = 0; i < GetChildCount(); i ++)
        {
            if (GetChild(i) is StaticBody bod)
            {
                bod.QueueFree();
            }
        }
        TurnMeshesToMulti = false;
    }
    #endif
}

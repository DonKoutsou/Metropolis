using System.Collections.Generic;
using Godot;

// This sample changes all node names.
// Called right after the scene is imported and gets the root node.
#if TOOLS
[Tool]
public partial class ExportMeshFromGLB : EditorScenePostImport
{
    public override Object PostImport(Object scene)
    {
        GD.Print(scene);

        string name = GetSourceFile().GetFile();
        string path = GetSourceFile();
        path = path.Substr(0, path.Length - name.Length);
        name = name.Substr(0, name.Length - 4);

        GD.Print(name);
        GD.Print(path);

        List<CollisionShape> collisions = FindCollisionShapes((Node)scene);
        List<MeshInstance> Meshes = FindMeshes((Node)scene);

        for (int i = 0; i < collisions.Count; i++)
        {
            ResourceSaver.Save(path + name + "_Col_" + i.ToString() + ".tres", collisions[i].Shape);
        }
        for (int i = 0; i < Meshes.Count; i++)
        {
            if (Meshes[i].Name.Substr(0, 4) == "-col")
                continue;
            ResourceSaver.Save(path + name + "_Mesh_" + i.ToString() + ".tres", Meshes[i].Mesh);
        }
        // Change all node names to "modified_[oldnodename]"
        //Iterate(scene);
        return scene; // Remember to return the imported scene
    }
    public List<CollisionShape> FindCollisionShapes(Node rootNode)
    {
        List<CollisionShape> collisionShapes = new List<CollisionShape>();
        TraverseTree(rootNode, collisionShapes);
        return collisionShapes;
    }
    public List<MeshInstance> FindMeshes(Node rootNode)
    {
        List<MeshInstance> Meshes = new List<MeshInstance>();
        TraverseTreeMesh(rootNode, Meshes);
        return Meshes;
    }
    private void TraverseTree(Node node, List<CollisionShape> collisionShapes)
    {
        // Check if the current node is a CollisionShape and add it to the list if it is
        if (node is CollisionShape collisionShape)
        {
            collisionShapes.Add(collisionShape);
        }

        // Iterate over each child of the current node
        foreach (Node child in node.GetChildren())
        {
            // Recursively call TraverseTree on each child node
            TraverseTree(child, collisionShapes);
        }
    }
    private void TraverseTreeMesh(Node node, List<MeshInstance> Meshes)
    {
        // Check if the current node is a CollisionShape and add it to the list if it is
        if (node is MeshInstance Meshi)
        {
            Meshes.Add(Meshi);
        }

        // Iterate over each child of the current node
        foreach (Node child in node.GetChildren())
        {
            // Recursively call TraverseTree on each child node
            TraverseTreeMesh(child, Meshes);
        }
    }
}
#endif
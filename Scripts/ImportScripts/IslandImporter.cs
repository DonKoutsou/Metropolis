using System;
using System.Collections.Generic;
using System.IO;
using Godot;

// This sample changes all node names.
// Called right after the scene is imported and gets the root node.
[Tool]
public partial class IslandImporter : EditorScenePostImport
{
    public override Godot.Object PostImport(Godot.Object scene)
    {
        Godot.File file = new Godot.File();
        file.Open(GetSourceFile(), Godot.File.ModeFlags.Read);
        string content = file.GetAsText();
        JSONParseResult json = JSON.Parse(content);
        Dictionary<string, Godot.Collections.Dictionary> CustomProps = FindExtras((Godot.Collections.Dictionary)json.Result);

        Spatial Islandscene = ResourceLoader.Load<PackedScene>("res://Scenes/Islands/IslandBase.tscn").Instance<Spatial>();


        string name = GetSourceFile().GetFile();
        string path = GetSourceFile();
        path = path.Substr(0, path.Length - name.Length);
        name = name.Substr(0, name.Length - 5);


        List<MeshInstance> Meshes = FindMeshes((Node)scene);

        for (int i = 0; i < Meshes.Count; i++)
        {
            if (Meshes[i].Name.Contains("Island"))
            {
                MeshInstance Duplicate = (MeshInstance)Meshes[i].Duplicate();
                Duplicate.Mesh.SurfaceSetMaterial(0, ResourceLoader.Load<Material>("res://Assets/TerainTex/SandSimplePlanar.tres"));
                StaticBody b = (StaticBody)Meshes[i].GetChild(0).Duplicate();
                CollisionShape c = (CollisionShape)Meshes[i].GetChild(0).GetChild(0).Duplicate();
                Islandscene.AddChild(Duplicate);
                Duplicate.Owner = Islandscene;
                Duplicate.AddChild(b);
                b.Owner = Islandscene;
                b.AddChild(c);
                c.Owner = Islandscene;

                b.SetCollisionLayerBit(0, false);
                b.SetCollisionMaskBit(0, false);
                b.SetCollisionLayerBit(2, true);
            }
            else
            {
                if (CustomProps.ContainsKey(Meshes[i].Name))
                {
                    Godot.Collections.Dictionary props = CustomProps[Meshes[i].Name];

                    Spatial w = ResourceLoader.Load<PackedScene>((string)props["SceneFile"]).Instance<Spatial>();
                    Islandscene.AddChild(w);
                    w.Owner = Islandscene;
                    w.Transform = Meshes[i].Transform;
                    
                    foreach (var keyValue in props)
                    {
                        System.Collections.DictionaryEntry en = (System.Collections.DictionaryEntry)keyValue;
                        if (en.Key.ToString() == "SceneFile")
                            continue;

                        w.Set(en.Key.ToString(), en.Value);
                    }
                    
                }
            }
        }
        Islandscene.GetNode<SoundMap>("SoundMap").RedoMapping = true;
        PackedScene sc = new PackedScene();
        sc.Pack(Islandscene);
        ResourceSaver.Save(path + name + ".tscn", sc);
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
    private void Thing()
    {
        string source = GetSourceFile();
        if (source.Contains(".glb"))
        {
            bool useHidden = (bool)ProjectSettings.GetSetting("application/config/use_hidden_project_data_directory");
            string dataFolder = useHidden ? ".godot" : "godot";

            string importedFile = "res://" + dataFolder + "/imported/" + 
                                  source.GetFile().Replace(".glb", "") + "-" + 
                                  source.MD5Text() + ".gltf";
            //source = importedFile;
            GD.Print(source);
            Godot.File file = new Godot.File();
            file.Open(source, Godot.File.ModeFlags.Read);
            GD.Print(file);
            string content = file.GetAsText();
            GD.Print(content);
            JSONParseResult json = JSON.Parse(content);
 
            FindExtras((Godot.Collections.Dictionary)json.Result);
        }
    }
    private Dictionary<string, Godot.Collections.Dictionary> FindExtras(Godot.Collections.Dictionary json)
    {
        Dictionary<string, Godot.Collections.Dictionary> ExtraList = new Dictionary<string, Godot.Collections.Dictionary>();
        if (json.Contains("nodes"))
        {
            foreach (var node in (Godot.Collections.Array)json["nodes"])
            {
                var nodeDict = (Godot.Collections.Dictionary)node;
                if (nodeDict.Contains("mesh"))
                {   
                    Single meshIndex = (Single)nodeDict["mesh"];
                    Godot.Collections.Array arr = (Godot.Collections.Array)json["meshes"];
                    var mesh = (Godot.Collections.Dictionary)arr[(int)meshIndex];

                    if (mesh.Contains("extras"))
                    {
                        Godot.Collections.Dictionary extr = (Godot.Collections.Dictionary)mesh["extras"];
                        if (extr.Count > 0)
                        {
                            var Exl = extr.Duplicate();
                            ExtraList.Add((string)nodeDict["name"], Exl);
                        }
                    }
                }
                //if (nodeDict.Contains("extras"))
                //{
                 //   GD.Print(nodeDict["extras"]);
                //}
            }
        }
        return ExtraList;
    }
}
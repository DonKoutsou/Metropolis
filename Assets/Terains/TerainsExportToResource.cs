using Godot;
using System;

[Tool]
public class TerainsExportToResource : Spatial
{
    [Export]
    bool Unlock = false;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!Unlock)
        {
            return;
        }
        GD.Print("Starting Loop");
        foreach (MeshInstance instance in GetChildren())
        {
            Mesh Tosave = instance.Mesh;
            string name = instance.Name;

            string loc = "res://Assets/Terains/";

            if (name.Substr(name.Length - 4, 4) == "_LOD")
            {
                loc += name.Substr(0, name.Length - 4) + "/";
            }
            else
            {
                loc += name + "/";
            }

            ResourceSaver.Save(loc + name + ".tres", Tosave);
            GD.Print("Saved :" + loc + name + ".tres", Tosave);
        }
        Unlock = false;
    }
}

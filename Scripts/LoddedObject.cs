using Godot;
using System;

[Tool]
public class LoddedObject : MeshInstance
{   
    [Export]
    Mesh LOD0 = null;

    [Export]
    Mesh LOD1 = null;
    public override void _EnterTree()
    {
        AddToGroup("LODDEDOBJ");
    }
    public void SwitchLod(bool LOD)
    {
        switch (LOD)
        {
            case false:
            {
                Mesh = LOD0;
                break;
            }
            case true:
            {
                Mesh = LOD1;
                break;
            }
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

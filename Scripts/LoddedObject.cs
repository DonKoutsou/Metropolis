using Godot;
using System;

public class LoddedObject : MeshInstance
{   
    [Export]
    public Mesh LOD0 = null;
    [Export]
    public Mesh LOD1 = null;
    [Export]
    public Mesh LOD2 = null;
    [Export]
    public bool HideIfNoLod = false;

    public float abbLeangth = 0;

    public override void _Ready()
    {
        base._Ready();
        abbLeangth = GetTransformedAabb().GetLongestAxisSize();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        AddToGroup("LODDEDOBJ");
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        RemoveFromGroup("LODDEDOBJ");
    }
    public bool HasLod(int lod)
    {
        bool has = false;
        switch (lod)
        {
            case 0:
            {
                has = LOD0 != null;
                break;
            }
            case 1:
            {
                has = LOD1 != null;
                break;
            }
            case 2:
            {
                has = LOD2 != null;
                break;
            }
        }
        return has;
    }
    public void SwitchLod(int LOD)
    {
        switch (LOD)
        {
            case 0:
            {
                Mesh = LOD0;
                Show();
                break;
            }
            case 1:
            {
                if (LOD1 == null)
                {
                    if (HideIfNoLod)
                    {
                        Hide();
                    }
                }    
                else
                {
                    Mesh = LOD1;
                    Show();
                }
                    
                break;
            }
            case 2:
            {
                if (LOD2 == null)
                {
                    if (HideIfNoLod)
                    {
                        Hide();
                    }
                }     
                else
                {
                    Mesh = LOD2;
                    Show();
                }
                    
                break;
            }
        }
    }

}

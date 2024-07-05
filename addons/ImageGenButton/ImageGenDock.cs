
#if TOOLS
using Godot;
using System;
using System.Linq;

[Tool]
public class ImageGenDock : EditorPlugin
{
    Control buttondock;
    Control ImageGen;
    Control PortConfig;

    public override void _EnterTree()
    {
        buttondock = (Control)GD.Load<PackedScene>("res://addons/ImageGenButton/ButtonDock.tscn").Instance(); 
        ImageGen = buttondock.GetNode<VBoxContainer>("VBoxContainer").GetNode<Control>("Dock2");
        PortConfig = buttondock.GetNode<VBoxContainer>("VBoxContainer").GetNode<Control>("Dock");
        AddControlToDock(DockSlot.LeftUl, buttondock);
        ImageGen.Connect("OnButtonClicked", this, "Clicked");
        PortConfig.Connect("OnButtonClicked", this, "PortClicked");
    }
    public void Clicked()
    {
        EditorInterface interf = GetEditorInterface();
        IslandImageHolder holder = (IslandImageHolder)interf.GetSelection().GetSelectedNodes()[0];
        //Tabs scenetabs = (Tabs)interf.GetBaseControl().GetNode("@@49");
        //Node editornode = interf.GetBaseControl().GetNode("../..");
        
        if (holder == null)
        {
            GD.Print("No Island Holder Selected. Aborting." + holder.GetClass().ToString());
            return;
        }
        holder.ClearImages();
        string[] islandfiles = holder.GetIslandLocs();
        Random r = new Random();
        for (int i = 0; i < islandfiles.Count(); i++)
        {
            interf.OpenSceneFromPath(islandfiles[i]);
            Island ile = (Island)interf.GetEditedSceneRoot();
            Image im = ile.GenerateImage(r);
            ile.ImageID = holder.AddImage(im);
            interf.SaveScene();
            //editornode.Call("SceneTabClosed", scenetabs.CurrentTab);
        }
        interf.OpenSceneFromPath("res://Scenes/World/WorldRoot.tscn");
        
    }
    public void PortClicked()
    {   
        EditorInterface interf = GetEditorInterface();
        var holder = interf.GetSelection().GetSelectedNodes();

        Port p = null;

        foreach (Node thing in holder)
        {
            if (thing is Port)
            {
                p = (Port)thing;
                holder.Remove(thing);
                break;
            }
        }
        if (p == null)
        {
            GD.PushError("No Ports Selected");
            return;
        }
        p.ClearPosition();
        foreach (Node porthting in holder)
        {
            Position3D pos = porthting.GetNode<Position3D>("Position3D");
            p.AddPosition(pos.GlobalTranslation);
        }
    }
    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        // Remove the dock.
        RemoveControlFromDocks(ImageGen);
        // Erase the control from the memory.
        buttondock.Free();
    }

}
#endif

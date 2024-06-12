
#if TOOLS
using Godot;
using System;
using System.Linq;

[Tool]
public class ImageGenDock : EditorPlugin
{
    Control ImageGen;

    public override void _EnterTree()
    {
        ImageGen = (Control)GD.Load<PackedScene>("addons/ImageGenButton/ImageGenDock.tscn").Instance();
        AddControlToDock(DockSlot.LeftUl, ImageGen);
        ImageGen.Connect("OnButtonClicked", this, "Clicked");
    }
    public void Clicked()
    {
        EditorInterface interf = GetEditorInterface();
        IslandImageHolder holder = (IslandImageHolder)interf.GetSelection().GetSelectedNodes()[0];
        //Tabs scenetabs = (Tabs)interf.GetBaseControl().GetNode("@@49");
        //Node editornode = interf.GetBaseControl().GetNode("../..");
        holder.ClearImages();
        if (holder == null)
        {
            GD.Print("No Island Holder Selected. Aborting." + holder.GetClass().ToString());
            return;
        }
        string[] islandfiles = holder.GetIslandLocs();
        for (int i = 0; i < islandfiles.Count(); i++)
        {
            interf.OpenSceneFromPath(islandfiles[i]);
            Island ile = (Island)interf.GetEditedSceneRoot();
            Image im = ile.GenerateImage();
            ile.ImageID = holder.AddImage(im);
            interf.SaveScene();
            //editornode.Call("SceneTabClosed", scenetabs.CurrentTab);
        }
        
    }
    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        // Remove the dock.
        RemoveControlFromDocks(ImageGen);
        // Erase the control from the memory.
        ImageGen.Free();
    }

}
#endif

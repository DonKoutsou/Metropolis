
#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class ImageGenDock : EditorPlugin
{
    Control buttondock;
    Control ImageGen;
    Control PortConfig;
    Control LocImport;
    public override void _EnterTree()
    {
        buttondock = (Control)GD.Load<PackedScene>("res://addons/ImageGenButton/ButtonDock.tscn").Instance();
        ImageGen = buttondock.GetNode<VBoxContainer>("VBoxContainer").GetNode<Control>("Dock2");
        PortConfig = buttondock.GetNode<VBoxContainer>("VBoxContainer").GetNode<Control>("Dock");
        LocImport = buttondock.GetNode<VBoxContainer>("VBoxContainer").GetNode<Control>("Dock3");
        AddControlToDock(DockSlot.LeftUl, buttondock);
        ImageGen.Connect("OnButtonClicked", this, "Clicked");
        PortConfig.Connect("OnButtonClicked", this, "PortClicked");
        LocImport.Connect("OnButtonClicked", this, "DialogueImportClicked");
    }
    //List<string> IslandFiles = null;
    //Random GenerationRandom = null;
    //int GenerationIndex = 0;
    IslandImageHolder holder = null;
    public void Clicked()
    {
        EditorInterface interf = GetEditorInterface();
        holder = (IslandImageHolder)interf.GetSelection().GetSelectedNodes()[0];
        //Tabs scenetabs = (Tabs)interf.GetBaseControl().GetNode("@@49");
        //Node editornode = interf.GetBaseControl().GetNode("../..");
        if (holder.Images.Count < holder.Islands.Count())
        {
            int times = holder.Islands.Count() - holder.Images.Count;
            for (int i = 0; i < times; i++)
            {
                holder.Images.Add(null);
            }
        }
        if (holder == null)
        {
            GD.Print("No Island Holder Selected. Aborting." + holder.GetClass().ToString());
            return;
        }
        //IslandFiles = new List<string>();
        //GenerationRandom = new Random();
        //GenerationIndex = 0;

        //holder.ClearImages();
        string[] islandfiles = holder.GetIslandLocs();
        
        //for (int i = 0; i < islandfiles.Count(); i++)
        //{
        //    IslandFiles.Add(islandfiles[i]);
            //editornode.Call("SceneTabClosed", scenetabs.CurrentTab);
        //}
        interf.OpenSceneFromPath(islandfiles[holder.ImageToGenerate]);
        
        Island ile = (Island)interf.GetEditedSceneRoot();
        ile.GenerateImage(this);
        //interf.OpenSceneFromPath("res://Scenes/World/WorldRoot.tscn");
        
    }
    void GenerateNext()
    {
        string[] islandfiles = holder.GetIslandLocs();
        EditorInterface interf = GetEditorInterface();
        interf.OpenSceneFromPath(islandfiles[holder.ImageToGenerate]);
        Island ile = (Island)interf.GetEditedSceneRoot();
        ile.GenerateImage(this);
    }
    public void OnImageFinished(Image im, Island ile)
    {
        ile.ImageID = holder.AddImage(holder.ImageToGenerate, im);
        EditorInterface interf = GetEditorInterface();
        interf.SaveScene();
        //GenerationIndex ++;
        
        if (holder.GenerateAll)
        {
            if (holder.Images.Count - 1 == holder.ImageToGenerate)
                return;
            holder.ImageToGenerate ++;
            GenerateNext();
        }
        else
        {
            interf.OpenSceneFromPath("res://Scenes/World/MyWorld.tscn");
        }
        //interf.OpenSceneFromPath(IslandFiles[GenerationIndex]);
        //Island NewIlde = (Island)interf.GetEditedSceneRoot();
        //NewIlde.GenerateImage(GenerationRandom, this);
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
    public void DialogueImportClicked()
    {
        EditorInterface interf = GetEditorInterface();
        GD.Print("Getting Localisation Holder");
        LocalisationHolder holder = (LocalisationHolder)interf.GetSelection().GetSelectedNodes()[0];
        holder.ImportLocalisation();
    }
    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        // Remove the dock.
        RemoveControlFromDocks(ImageGen);
        // Erase the control from the memory.
        buttondock.QueueFree();
    }

}
#endif

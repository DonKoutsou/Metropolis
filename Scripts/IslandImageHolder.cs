using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class IslandImageHolder : Node
{
    [Export]
    public List<Image> Images = null;
    [Export]
    public string[] Islands = null;
    static IslandImageHolder instance;      
    public override void _Ready()
    {
        if (!Engine.EditorHint)
            instance = this;
    }
    public void ClearImages()
    {
        Images = new List<Image>();
    }
    public int AddImage(Image im)
    {
        Images.Add(im);
        return Images.Count - 1;
    }
    static public IslandImageHolder GetInstance()
    {
        return instance;
    }
    public string[] GetIslandLocs()
    {
        return Islands;
    }
}

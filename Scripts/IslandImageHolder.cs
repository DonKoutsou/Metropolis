using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class IslandImageHolder : Node
{
    [Export]
    public int ImageToGenerate = 0;
    [Export]
    public bool GenerateAll = false;
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
    public int AddImage(int index, Image im)
    {
        Images[index] = im;
        return index;
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

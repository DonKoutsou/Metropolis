using Godot;
using System;

public class MapTile : Control
{
    [Signal]
    public delegate void OnHovered(string Name);
    public int type;
    public string IslandName = string.Empty;
    public override void _Ready()
    {
        base._Ready();
        Modulate = new Color(1,1,1,0);
    }
    
    private void OnHover()
    {
        if (GetNode<TextureRect>("TextureRect").Visible)
            EmitSignal("OnHovered",IslandName);
        else
            EmitSignal("OnHovered",string.Empty);

    }
}

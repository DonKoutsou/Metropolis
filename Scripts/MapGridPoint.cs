using Godot;
using System;

public class MapGridPoint : Control
{
    public override void _Ready()
    {
        RichTextLabel text = GetNode<Panel>("Panel").GetNode<RichTextLabel>("GridNum");
        string name = Name.Remove(0, 1);
        text.BbcodeText = "[center]" + name;
    }
}

using Godot;
using System;

public class ItemPreviewViewport : Viewport
{
    static ItemPreviewViewport instance;
    public override void _Ready()
    {
        instance = this;
    }
    public static ItemPreviewViewport GetInstance()
    {
        return instance;
    }
}

using Godot;
using System;

public class LoadingScreen : CanvasLayer
{
    public void Dissable()
    {
        Hide();
    }
    public void EnableTime()
    {
        Show();
        GetNode<Timer>("Timer").Start();
    }
}

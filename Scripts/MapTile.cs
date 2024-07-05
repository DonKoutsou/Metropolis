using Godot;
using System;

public class MapTile : Control
{
    public int type;
    public override void _Ready()
    {
        base._Ready();
        Modulate = new Color(1,1,1,0);
        //GetNode<TextureRect>("TextureRect").GetNode<Panel>("SignPanel").Visible = false;
        Random r = new Random();
        for (int i = 0; i < 4; i++)
        {
            TextureRect t = GetNode<TextureRect>("TextureRect" + i);
            int th = r.Next(0,2);
            if (th == 0)
            {
                t.QueueFree();
            }   
        }
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;

public class WorldMap : TileMap
{
    [Export]
    public string[] scenestospawn;

    [Export]
    public string Entrytospawn;

    [Export]
    public string Exittospawn;

    //[Export]
    //bool ScrableDoors = false;

    [Export]
    public bool HideBasedOnState = false;

    
    public override void _Ready()
    {
        CallDeferred("EnableIslands");
    }

    void EnableIslands()
    {
        CellSize = new Vector2(2000, 2000);
        int leng = scenestospawn.Length;
        
        var cells = GetUsedCellsById(0);
        cells.Shuffle();
        Random random = new Random();
        foreach (Vector2 cellArray in cells)
        {
            int start2 = random.Next(0, scenestospawn.Length);
            var scene = GD.Load<PackedScene>(scenestospawn[start2]);
            Island Ile = (Island)scene.Instance();
            Vector2 postoput = MapToWorld(cellArray);
            postoput += CellSize / 2;
            Vector3 pos = new Vector3();
            pos.x = postoput.x;
            pos.z = postoput.y;
            Ile.GlobalTranslation = pos;
            ((MyWorld)GetParent()).RegisterIle(Ile);
            
        }
        
        var Exitcells = GetUsedCellsById(3);
        var Exitscene = GD.Load<PackedScene>(Exittospawn);
        foreach (Vector2 cellArray in Exitcells)
        {
            Island Ile = (Island)Exitscene.Instance();
            Vector2 postoput = MapToWorld(cellArray);
            postoput += CellSize / 2;
            Vector3 pos = new Vector3();
            pos.x = postoput.x;
            pos.z = postoput.y;
            Ile.GlobalTranslation = pos;
            GetParent().AddChild(Ile);
            
        }
        var Entrycells = GetUsedCellsById(1);
        var Entryscene = GD.Load<PackedScene>(Entrytospawn);
        foreach (Vector2 cellArray in Entrycells)
        {
            Island Ile = (Island)Entryscene.Instance();
            Vector2 postoput = MapToWorld(cellArray);
            postoput += CellSize / 2;
            Vector3 pos = new Vector3();
            pos.x = postoput.x;
            pos.z = postoput.y;
            Ile.GlobalTranslation = pos;
            GetParent().AddChild(Ile);
            var pls = GetTree().GetNodesInGroup("player");
            ((Player)pls[0]).GlobalTranslation = pos;
        }
       // GetTree().CallGroup("Islands", "Init");

        //if (ScrableDoors)
            //GetTree().CallGroup("Islands", "ScrambleDoorDestinations");
        
    }
    
}


using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;

public class WorldMap : TileMap
{
    [Export]
    public string[] scenestospawn;

    List <PackedScene> loadedscenes = new List<PackedScene>();

    [Export]
    public PackedScene Entrytospawn;

    [Export]
    public PackedScene Exittospawn;

    [Export]
    public PackedScene Pit_City;

    [Export]
    public PackedScene Slab;

    [Export]
    public PackedScene Volcano;

    [Export]
    public PackedScene Sea;

    //[Export]
    //bool ScrableDoors = false;

    [Export]
    public bool HideBasedOnState = false;

    Dictionary<Vector2, Island> IslandMap = new Dictionary<Vector2, Island>();

    public override void _Ready()
    {
        Hide();
        //CallDeferred("EnableIslands");
        CellSize = new Vector2(2000, 2000);
        ArrangeCellsBasedOnDistance();
        for (int i = 0; i < scenestospawn.Count(); i++)
        {
            var scene = GD.Load<PackedScene>(scenestospawn[i]);
            loadedscenes.Insert(i, scene);
        }
        //SetProcess(true);
        //CallDeferred("SetProcess", true);
    }
    List <Vector2> OrderedCells = new List<Vector2>();
    void ArrangeCellsBasedOnDistance()
    {
        var cells = GetUsedCellsById(1);
        foreach (Vector2 cellArray in cells)
        {
            float ind = Math.Abs(cellArray.x) + Math.Abs(cellArray.y);
            if (OrderedCells.Count == 0)
            {
                OrderedCells.Insert(0, cellArray);
                continue;
            }
            Vector2 closest = OrderedCells[0];
            float dif = Math.Abs(Math.Abs(closest.x) + Math.Abs(closest.y) - ind);
            for (int i = OrderedCells.Count - 1; i > -1; i--)
            {
                float newdif = Math.Abs(Math.Abs(OrderedCells[i].x) + Math.Abs(OrderedCells[i].y) - ind);
                if (dif > newdif)
                {
                    closest = OrderedCells[i];
                    dif = newdif;
                }
            }
            if (Math.Abs(closest.x) + Math.Abs(closest.y) < Math.Abs(cellArray.x) + Math.Abs(cellArray.y))
            {
                OrderedCells.Insert(OrderedCells.IndexOf(closest) + 1, cellArray);
                continue;
            }
            else
            {
                OrderedCells.Insert(OrderedCells.IndexOf(closest), cellArray);
                continue;
            }
        }
    }
    bool finishedspawning = false;
    public override void _Process(float delta)
	{
        delt -= delta;
        if (delt < 0)
        {
            delt = 0.1f;
            if (!finishedspawning)
                EnableIsland(currentile, currenttiletype);

            Vector2 plpos = new Vector2(pl.GlobalTransform.origin.x, pl.GlobalTransform.origin.z);
            if (plpos.DistanceTo(ClosestTile) > 1000)
            {
                Island ilefr = null;
                IslandMap.TryGetValue(ClosestTile, out ilefr);
                Island ileto = null;
                Vector2 newclos = FindClosest(plpos);
                IslandMap.TryGetValue(FindClosest(plpos), out ileto);
                ClosestTile = newclos;
                MyWorld.IleTransition(ilefr, ileto);
            }
        }
    }
    Vector2 FindClosest(Vector2 pos)
    {
        float dist = 999999999;
        Vector2 closest = Vector2.Zero;
        foreach(KeyValuePair<Vector2, Island> entry in IslandMap)
        {
            Vector2 ilepos = entry.Key;
            float Itdist = ilepos.DistanceTo(pos);
            if (dist > Itdist)
            {
                closest = ilepos;
                dist = Itdist;
            }
        }
        return closest;
    }
    int currentile;
    int currenttiletype;
    float delt = 0.1f;

    Vector2 entycell;

    List <Vector2> spawned = new List<Vector2>();

    List <float> rots = new List<float>{0f, 90f, 180f, -90f};

    Random random = new Random();

    Player pl;

    Vector2 ClosestTile;
    void EnableIsland(int curtile, int curtiletype)
    {
        if (curtiletype == 0)
        {
            Island entry = null;
            GD.Print("Starting to generate initial map.");
            var Entrycells = GetUsedCellsById(0);
            foreach (Vector2 cellArray in Entrycells)
            {
                entry = (Island)Entrytospawn.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                entycell = cellArray;
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                entry.loctospawnat = pos;
                //pos.y = 500;
                int index = random.Next(rots.Count);
                entry.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(entry);
                var pls = GetTree().GetNodesInGroup("player");
                pl = ((Player)pls[0]);
                pl.Teleport(entry.GetNode<Position3D>("SpawnPosition").GlobalTranslation);
                IslandMap.Add(postoput ,entry);
                //iles.Insert(iles.Count, entry);
                spawned.Insert(spawned.Count, cellArray);
                ClosestTile = postoput;
            }
            var cells = GetUsedCellsById(1);
            foreach (Vector2 cellArray in cells)
            {
                if (cellArray.DistanceTo(entycell) > 3)
                    continue;
                Random random = new Random();
                int start2 = random.Next(0, loadedscenes.Count);
                var scene = loadedscenes[start2];
                Island Ile = (Island)scene.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                Ile.loctospawnat = pos;
                int index = random.Next(rots.Count);
                Ile.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(Ile);
                IslandMap.Add(postoput ,Ile);
                //iles.Insert(iles.Count, Ile);
                spawned.Insert(spawned.Count, cellArray);
            }
            var pitcells = GetUsedCellsById(4);
            foreach (Vector2 cellArray in pitcells)
            {
                var scene = Pit_City;
                Island Ile = (Island)scene.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                Ile.loctospawnat = pos;
                int index = random.Next(rots.Count);
                Ile.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(Ile);
                IslandMap.Add(postoput ,Ile);
               // iles.Insert(iles.Count, Ile);
                spawned.Insert(spawned.Count, cellArray);
            }
            var SlabChunks = GetUsedCellsById(5);
            foreach (Vector2 cellArray in SlabChunks)
            {
                var scene = Slab;
                Island Ile = (Island)scene.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                Ile.loctospawnat = pos;
                int index = random.Next(rots.Count);
                Ile.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(Ile);
                IslandMap.Add(postoput ,Ile);
                //iles.Insert(iles.Count, Ile);
                spawned.Insert(spawned.Count, cellArray);
            }
            var VolcanoChunks = GetUsedCellsById(6);
            foreach (Vector2 cellArray in VolcanoChunks)
            {
                var scene = Volcano;
                Island Ile = (Island)scene.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                Ile.loctospawnat = pos;
                int index = random.Next(rots.Count);
                Ile.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(Ile);
                IslandMap.Add(postoput ,Ile);
                //iles.Insert(iles.Count, Ile);
                spawned.Insert(spawned.Count, cellArray);
            }

            MyWorld.ToggleIsland(entry, true, true);

            GD.Print("-------------- Initial map generation Finished ---------------");
            currenttiletype += 1;
        }
        if (curtiletype == 1)
        {
            float dist = (OrderedCells[curtile]).DistanceTo(entycell);
            if (dist < 3 || spawned.Contains(OrderedCells[curtile]))
            {
                currentile += 1;
                if (currentile >= OrderedCells.Count)
                    currenttiletype += 2;
                return;
            }
            GD.Print("Generating island " + (OrderedCells[curtile]).ToString());     
            Random random = new Random();
            int start2 = random.Next(0, scenestospawn.Length);
            var scene = GD.Load<PackedScene>(scenestospawn[start2]);
            Island Ile = (Island)scene.Instance();
            Vector2 postoput = MapToWorld(OrderedCells[curtile]);
            postoput += CellSize / 2;
            Vector3 pos = new Vector3();
            pos.x = postoput.x;
            pos.z = postoput.y;
            Ile.loctospawnat = pos;
            int index = random.Next(rots.Count);
            Ile.rotationtospawnwith = rots[index];
            ((MyWorld)GetParent()).RegisterIle(Ile);
            IslandMap.Add(postoput ,Ile);
            //iles.Insert(iles.Count, Ile);
            currentile += 1;
            if (currentile >= OrderedCells.Count)
                currenttiletype += 2;
            spawned.Insert(spawned.Count, OrderedCells[curtile]);
            GD.Print("-------------- Finished generating island ---------------");
        }
        if (curtiletype == 3)
        {
            GD.Print("Generating Exits");
            var Exitcells = GetUsedCellsById(3);
            foreach (Vector2 cellArray in Exitcells)
            {
                Island Ile = (Island)Exittospawn.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                Ile.loctospawnat = pos;
                int index = random.Next(rots.Count);
                Ile.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(Ile);
                IslandMap.Add(postoput ,Ile);
                //iles.Insert(iles.Count, Ile);
            }
            currenttiletype += 4;
            GD.Print("-------------- Finished generating exit ---------------");
        }
        if (curtiletype == 7)
        {
            GD.Print("Generating Seas");
            var Seas = GetUsedCellsById(7);
            foreach (Vector2 cellArray in Seas)
            {
                Island Ile = (Island)Sea.Instance();
                Vector2 postoput = MapToWorld(cellArray);
                postoput += CellSize / 2;
                Vector3 pos = new Vector3();
                pos.x = postoput.x;
                pos.z = postoput.y;
                Ile.loctospawnat = pos;
                //int index = random.Next(rots.Count);
                //Ile.rotationtospawnwith = rots[index];
                ((MyWorld)GetParent()).RegisterIle(Ile);
                IslandMap.Add(postoput ,Ile);
                //iles.Insert(iles.Count, Ile);
            }
            currenttiletype += 1;
            GD.Print("-------------- Finished generating exit ---------------");
        }
        if (curtiletype == 8)
        {
            GD.Print("Toggling final islands");
            finishedspawning = true;
            
            //SetProcess(false);
        }
    }
	
}


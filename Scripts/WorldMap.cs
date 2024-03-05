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

    [Export]
    public string[] Eventscenestospawn;

    List <PackedScene> loadedscenes = new List<PackedScene>();

    [Export]
    public PackedScene Entrytospawn;

    [Export]
    public PackedScene Exittospawn;

    [Export]
    public PackedScene Sea;

    [Export]
    public bool HideBasedOnState = false;

    [Export]
    public bool RandomRotation = true;

    [Export]
    public int seed = 69420;

    int currentile;
    int currenttiletype;
    float delt = 0.1f;

    Vector2 entycell;

    List <Vector2> spawned = new List<Vector2>();

    List <float> rots = new List<float>{0f, 90f, 180f, -90f};

    List <int> RandomisedEntryID = null;

    List <Vector2> OrderedCells = new List<Vector2>();

    Random random;

    Player pl;

    Vector2 CurrentTile;

    static Dictionary<Vector2, Island> IslandMap = new Dictionary<Vector2, Island>();

    bool finishedspawning = false;

    public override void _Ready()
    {
        Hide();

        int seed = Settings.GetGameSettings().Seed;

        random = new Random(seed);
            
        CellSize = new Vector2(8000, 8000);

        ArrangeCellsBasedOnDistance();
        for (int i = 0; i < scenestospawn.Count(); i++)
        {
            var scene = GD.Load<PackedScene>(scenestospawn[i]);
            loadedscenes.Insert(i, scene);
        }
        var pls = GetTree().GetNodesInGroup("player");
        pl = (Player)pls[0];
        //EnableInitialIsland();
    }
    public override void _Process(float delta)
	{
        delt -= delta;
        if (delt < 0)
        {
            delt = 0.1f;
            
            if (!finishedspawning)
                EnableIsland2(currentile);

            Vector2 plpos = new Vector2(pl.GlobalTransform.origin.x, pl.GlobalTransform.origin.z);
            if (plpos.DistanceTo(CurrentTile) > CellSize.x/2)
            {
                Island ilefr = null;
                IslandMap.TryGetValue(CurrentTile, out ilefr);
                
                CurrentTile = FindClosest(plpos);

                Island ileto = null;
                IslandMap.TryGetValue(CurrentTile, out ileto);

                if (ilefr == ileto)
                    return;

                MyWorld.IleTransition(ilefr, ileto);
            }
        }
    }
    void ArrangeCellsBasedOnDistance()
    {
        //arange all the cells that will get random tile by distance to entry and put in OrderedCells
        var cells = GetUsedCells();
        OrderedCells.Insert(0, (Vector2)cells[0]);
        for (int x = 1; x < cells.Count; x++)
        {
            Vector2 cellArray = (Vector2)cells[x];
            float ind = Math.Abs(cellArray.x) + Math.Abs(cellArray.y);

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
        //produce indexes of tiles events will be placed on
        RandomisedEntryID = new List<int>();
        for (int i = 0; i < Eventscenestospawn.Count(); i++)
        {
            int SpawnIndex = random.Next(0, OrderedCells.Count);
            RandomisedEntryID.Insert(i, SpawnIndex);
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

    public static void GetClosestIles(Island Ile, out List<Island> closeIles, int dist = 2)
    {
        closeIles = new List<Island>();
        Vector3 transform = Ile.GlobalTransform.origin;
        int ammount = 0;
        for (int i = 0; i < dist; i ++)
            ammount += ammount + 8;

        int offset = 8000 * dist;
        float row = transform.x - offset;
        float collumn = transform.z - offset;
        for (int i = 0; i < ammount + 1; i++)
        {
            Island ile;
            IslandMap.TryGetValue(new Vector2(row, collumn), out ile);
            if (ile != null)
                closeIles.Insert(closeIles.Count, ile);
            row += 8000;
            if (row > transform.x + offset)
            {
                row = transform.x - offset;
                collumn += 8000;
                if (collumn > transform.z + offset)
                {
                    break;
                }
            }
        }
        //foreach(KeyValuePair<Vector2, Island> entry in IslandMap)
        //{
        //    if (entry.Value == Ile)
        //        continue;
        //    Vector2 pos = new Vector2 (Ile.GlobalTransform.origin.x, Ile.GlobalTransform.origin.z);
        //    if (pos.DistanceTo(entry.Key) < 10000)
        //    {
        //        closeIles.Insert(closeIles.Count, entry.Value);
        //    }
        //}
    }
    PackedScene GetSceneToSpawn()
    {
        PackedScene scene = null;
        //0 entry
        if (currenttiletype == 0)
        {
            // if we have no maps spawned it means entry needs to spawn
            if (IslandMap.Count == 0)
            {
                scene = Entrytospawn;
            }
            // else get a random tile
            else
            {
                int start2 = random.Next(0, loadedscenes.Count);
                scene =  loadedscenes[start2];
            }
        }
        //1 random or event
        else if (currenttiletype == 1)
        {
            
            if (RandomisedEntryID.Contains(currentile))
            {
                scene = GD.Load<PackedScene>(Eventscenestospawn[RandomisedEntryID.IndexOf(currentile)]);
            }
            else
            {
                int start2 = random.Next(0, loadedscenes.Count);
                scene = loadedscenes[start2];
            }
        }
        //2 exit
        else if (currenttiletype == 2)
        {
            scene =  Exittospawn;
        }
        //3 sea
        else if (currenttiletype == 3)
        {
            scene =  Sea;
        }
        return scene;
    }
    PackedScene GetSceneToSpawn2(int type)
    {
        PackedScene scene = null;
        //0 entry
        if (type == 0)
        {
            // if we have no maps spawned it means entry needs to spawn
            //if (IslandMap.Count == 0)
            //{
                scene = Entrytospawn;
            //}
            // else get a random tile
            //else
            //{
           //     int start2 = random.Next(0, loadedscenes.Count);
           //     scene =  loadedscenes[start2];
           // }
        }
        //1 random or event
        else if (type == 1)
        {
            
            if (RandomisedEntryID.Contains(currentile))
            {
                scene = GD.Load<PackedScene>(Eventscenestospawn[RandomisedEntryID.IndexOf(currentile)]);
            }
            else
            {
                int start2 = random.Next(0, loadedscenes.Count);
                scene = loadedscenes[start2];
            }
        }
        //2 exit
        else if (type == 2)
        {
            scene =  Exittospawn;
        }
        //3 sea
        else if (type == 3)
        {
            scene =  Sea;
        }
        return scene;
    }
    void EnableInitialIsland()
    {
        Island entry = null;
        //GD.Print("Starting to generate initial map.");
        var Entrycells = GetUsedCellsById(0);
        foreach (Vector2 cellArray in Entrycells)
        {
            entry = (Island)GetSceneToSpawn().Instance();
            SpawnIsland(entry, cellArray, true);
                
            entycell = cellArray;
                
            
            
        }
        MyWorld.ToggleIsland(entry, true, true);
    }
    void EnableIsland2(int curtile)
    {
        if (spawned.Contains(OrderedCells[curtile]))
        {
            currentile += 1;
            if (currentile >= OrderedCells.Count)
                currenttiletype += 1;
            return;
        }
        int id = GetCell((int)OrderedCells[curtile].x, (int)OrderedCells[curtile].y);

        Island Ile = (Island)GetSceneToSpawn2(id).Instance();
        SpawnIsland(Ile, OrderedCells[curtile], true);
        
        currentile += 1;

        if (id == 0)
        {
            pl.Teleport(Ile.GetNode<Position3D>("SpawnPosition").GlobalTranslation);
            spawned.Insert(spawned.Count, OrderedCells[curtile]);
            CurrentTile = MapToWorld(OrderedCells[curtile]) + CellSize / 2;
            MyWorld.ToggleIsland(Ile, true, true);
        }


        if (currentile >= OrderedCells.Count)
        {
            finishedspawning = true;
            spawned.Clear();
        }
            
    }
    void EnableIsland(int curtile, int curtiletype)
    {
        if (curtiletype == 0)
        {
            ///Initial Map///
            Island entry = null;
            //GD.Print("Starting to generate initial map.");
            var Entrycells = GetUsedCellsById(curtiletype);
            foreach (Vector2 cellArray in Entrycells)
            {
                entry = (Island)GetSceneToSpawn().Instance();
                SpawnIsland(entry, cellArray, true);
                
                entycell = cellArray;
                
                var pls = GetTree().GetNodesInGroup("player");
                pl = (Player)pls[0];
                pl.Teleport(entry.GetNode<Position3D>("SpawnPosition").GlobalTranslation);
                spawned.Insert(spawned.Count, cellArray);
                CurrentTile = MapToWorld(cellArray) + CellSize / 2;
            }
            var cells = GetUsedCellsById(1);
            int ViewDistance = Settings.GetGameSettings().ViewDistance;
            foreach (Vector2 cellArray in cells)
            {
                if (cellArray.DistanceTo(entycell) > ViewDistance + 2)
                    continue;

                Island Ile = (Island)GetSceneToSpawn().Instance();
                SpawnIsland(Ile, cellArray, true);
                spawned.Insert(spawned.Count, cellArray);
            }
            MyWorld.ToggleIsland(entry, true, true);

            //GD.Print("-------------- Initial map generation Finished ---------------");
            currenttiletype += 1;
        }
        if (curtiletype == 1)
        {
            if (spawned.Contains(OrderedCells[curtile]))
            {
                currentile += 1;
                if (currentile >= OrderedCells.Count)
                    currenttiletype += 1;
                return;
            }
            //GD.Print("Generating island " + (OrderedCells[curtile]).ToString());  

            Island Ile = (Island)GetSceneToSpawn().Instance();
            SpawnIsland(Ile, OrderedCells[curtile], true);
            
            currentile += 1;

            if (currentile >= OrderedCells.Count)
            {
                spawned.Clear();
                currenttiletype += 1;
            }
                

            //GD.Print("-------------- Finished generating island ---------------");
        }
        if (curtiletype == 2)
        {
            //GD.Print("Generating Exits");
            var Exitcells = GetUsedCellsById(curtiletype);
            foreach (Vector2 cellArray in Exitcells)
            {
                Island Ile = (Island)Exittospawn.Instance();
                SpawnIsland(Ile, cellArray, true);
            }
            currenttiletype += 1;
            //GD.Print("-------------- Finished generating exit ---------------");
        }
        if (curtiletype == 3)
        {
            GD.Print("Generating Seas");
            var Seas = GetUsedCellsById(curtiletype);
            foreach (Vector2 cellArray in Seas)
            {
                Island Ile = (Island)GetSceneToSpawn().Instance();
                SpawnIsland(Ile, cellArray, false);
            }
            currenttiletype += 1;
            //GD.Print("-------------- Finished generating exit ---------------");
        }
        if (curtiletype == 4)
        {
            //GD.Print("Toggling final islands");
            finishedspawning = true;
        }
    }
	void SpawnIsland(Island Ile , Vector2 cell, bool rotate)
    {
        Vector2 postoput = MapToWorld(cell);
        postoput += CellSize / 2;
        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        pos.x = postoput.x;
        pos.z = postoput.y;
        Ile.loctospawnat = pos;
        if (rotate)
        {
            int index = random.Next(rots.Count);
            Ile.rotationtospawnwith = rots[index];
        }
       
        ((MyWorld)GetParent()).RegisterIle(Ile);
        IslandMap.Add(postoput ,Ile);
    }
}



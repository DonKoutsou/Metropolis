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
    int CellSizeOverride = 8000;

    [Export]
    public int seed = 69420;

    int currentile;

    float delt = 0.1f;

    List <float> rots = new List<float>{0f, 90f, 180f, -90f};

    List <int> RandomisedEntryID = null;

    List <Vector2> OrderedCells = new List<Vector2>();

    Random random;

    Player pl;

    Vector2 CurrentTile;

    static Dictionary<Vector2, Island> IslandMap = new Dictionary<Vector2, Island>();

    bool finishedspawning = false;

    Island entry;

    static WorldMap Instance;


    public static WorldMap GetInstance()
    {
        return Instance;
    }
    public override void _Ready()
    {
        Instance = this;
        Hide();

        int seed = Settings.GetGameSettings().Seed;

        random = new Random(seed);
            
        CellSize = new Vector2(CellSizeOverride, CellSizeOverride);

        ArrangeCellsBasedOnDistance();
        for (int i = 0; i < scenestospawn.Count(); i++)
        {
            var scene = GD.Load<PackedScene>(scenestospawn[i]);
            loadedscenes.Insert(i, scene);
        }
        var pls = GetTree().GetNodesInGroup("player");
        pl = (Player)pls[0];
    }
    public override void _Process(float delta)
	{
        delt -= delta;
        if (delt < 0)
        {
            delt = 0.1f;
            ulong ms = OS.GetSystemTimeMsecs();
            if (!finishedspawning)
                    EnableIsland(currentile);

            Vector2 plpos = new Vector2(pl.GlobalTransform.origin.x, pl.GlobalTransform.origin.z);
            if (plpos.DistanceTo(CurrentTile) > CellSize.x/2)
            {
                Island ilefr = null;
                IslandMap.TryGetValue(CurrentTile, out ilefr);

                if (ilefr != null)
                {
                    CurrentTile = FindClosest(plpos);

                    Island ileto = null;
                    IslandMap.TryGetValue(CurrentTile, out ileto);

                    if (ilefr != ileto)
                    {
                        MyWorld.IleTransition(ilefr, ileto);
                    }
                }
            }
            ulong msaf = OS.GetSystemTimeMsecs();
		    GD.Print("World map processing took " + (msaf - ms).ToString() + " ms");
        }
        
    }
    void ArrangeCellsBasedOnDistance()
    {
        //arange all the cells by distance from center of world put in OrderedCells array
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
        //produce indexes of where tiles events will be placed on
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

    public void GetClosestIles(Island Ile, out List<Island> closeIles, int dist = 2)
    {
        closeIles = new List<Island>();
        Vector3 transform = Ile.GlobalTransform.origin;
        int ammount = 0;
        for (int i = 0; i < dist; i ++)
            ammount += ammount + 8;

        int offset = CellSizeOverride * dist;
        float row = transform.x - offset;
        float collumn = transform.z - offset;
        for (int i = 0; i < ammount + 1; i++)
        {
            Island ile;
            IslandMap.TryGetValue(new Vector2(row, collumn), out ile);
            if (ile != null)
                closeIles.Insert(closeIles.Count, ile);
            row += CellSizeOverride;
            if (row > transform.x + offset)
            {
                row = transform.x - offset;
                collumn += CellSizeOverride;
                if (collumn > transform.z + offset)
                    break;

            }
        }
    }
    PackedScene GetSceneToSpawn(int type)
    {
        PackedScene scene = null;
        //0 entry
        if (type == 0)
            scene = Entrytospawn;

        //1 random or event
        else if (type == 1)
        {
            if (RandomisedEntryID.Contains(currentile))
                scene = GD.Load<PackedScene>(Eventscenestospawn[RandomisedEntryID.IndexOf(currentile)]);

            else
                scene = loadedscenes[random.Next(0, loadedscenes.Count)];
        }
        //2 exit
        else if (type == 2)
            scene =  Exittospawn;

        //3 sea
        else if (type == 3)
            scene =  Sea;

        return scene;
    }
    void EnableIsland(int curtile)
    {

        int id = GetCell((int)OrderedCells[curtile].x, (int)OrderedCells[curtile].y);

        Island Ile = (Island)GetSceneToSpawn(id).Instance();
        SpawnIsland(Ile, OrderedCells[curtile], true);
        
        currentile += 1;

        if (id == 0)
        {
            entry = Ile;
        }
        if (currentile == 20)
        {
            pl.Teleport(entry.GetNode<Position3D>("SpawnPosition").GlobalTranslation);
            WorldClipRaycast.EnableWorldClipRaycast();
            GetTree().Root.GetCamera().Fov = Settings.GetGameSettings().FOVOverride;
            CurrentTile = new Vector2 (entry.GlobalTransform.origin.x, entry.GlobalTransform.origin.z);
            MyWorld.ToggleIsland(entry, true, true);
        }

        if (currentile >= OrderedCells.Count)
            finishedspawning = true;
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



using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class Port : Area
{
    public bool Visited = false;
    [Export]
    List<Vector3> RestLocations = null;

    [Export]
    Vector3 PortExtents = new Vector3(1,1,1);

    List <Position3D> Locations = new List<Position3D>();

    List <Vehicle> Boats = new List<Vehicle>();

    PortWorker Worker;
    public void RegisterWorker(PortWorker worker)
    {
        Worker = worker;
    }
    public override void _Ready()
    {
        if (!Engine.EditorHint)
        {
            for (int i = 0; i < RestLocations.Count; i++)
            {
                if (Locations.Count == i)
                {
                    Position3D p = new Position3D(){
                        Name = "Position" + i.ToString()
                    };
                    p.Scale *= 20;
                    Locations.Add(p);
                    AddChild(p, true);
                    //p.Owner = this;
                }
                Locations[i].GlobalTranslation = RestLocations[i];

            }
            ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents = PortExtents;
            GetNode<MeshInstance>("MeshInstance").QueueFree();

            Node par = GetParent();
            while (!(par is Island))
            {
                if (par == null)
                    return;
                par = par.GetParent();
            }
            Island ile = (Island)par;
            ile.RegisterChild(this);
        }
    }
    public Vehicle GetBoatOfType(VehicleType t)
    {
        Vehicle b = null;
        for (int i = 0; i <Boats.Count; i++)
        {
            if (Boats[i].GetVehicleType() == t)
            {
                b = Boats[i];
                break;
            }
        }
        return b;
    }
    public void ClearPosition()
    {
        RestLocations.Clear();

    }
    public void AddPosition(Vector3 pos)
    {
        RestLocations.Add(pos);
    }
    Vehicle incomingBoat;
    List<Job> IncomingBoatJobs = null;
    private void OnShipEntered(Node body)
    {
        if (body is Vehicle v)
        {
            incomingBoat = v;
            Boats.Add(v);

            if (v.IsPlayerOwned())
            {
                if (!Visited)
                {
                    Island ile = (Island)GetParent();
                    Vector2 pos = WorldMap.GetInstance().GlobalToMap(ile.GlobalTranslation);
                    MapGrid.GetInstance().SetPortVissible(pos, new Vector2(Translation.x, Translation.z));
                    Visited = true;
                }
                
                /*GlobalJobManager jobm = GlobalJobManager.GetInstance();
                if (jobm.HasJobAssigned())
                {
                    Island ile = (Island)GetParent();
                    Vector2 pos = WorldMap.GetInstance().GlobalToMap(ile.GlobalTranslation);
                    if (jobm.HasJobOnIsland(pos, out IncomingBoatJobs))
                    {
                        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
                        CameraAnimation.Connect("FadeOutFinished", this, "CompleteJobs");
                        CameraAnimation.FadeInOut(2);
                    }
                }*/
                Worker.GetTalkText().Talk("Καλωσόρισες καϊκτσή μαγκίτη πρώτε");
            }
        }
        if (body is Player pl)
        {
            if (!Visited)
            {
                Island ile = (Island)GetParent();
                Vector2 pos = WorldMap.GetInstance().GlobalToMap(ile.GlobalTranslation);
                MapGrid.GetInstance().SetPortVissible(pos, new Vector2(Translation.x, Translation.z));
                Visited = true;
            }
        }
       
    }
    void CompleteJobs()
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "CompleteJobs");
        incomingBoat.Capsize();
        Player.GetInstance().Teleport(Worker.GetNode<Position3D>("TalkPosition").GlobalTranslation);
        Vector3 spot;
        if (HasSpot(out spot))
        {
            incomingBoat.GlobalTranslation = spot;
        }
        
        GlobalJobManager jobm = GlobalJobManager.GetInstance();
        
        
        int rew = 0;
        
        foreach (Job j in IncomingBoatJobs)
        {
            rew += jobm.OnJobFinished(j);
            if (j is DeliverJob)
            {
                incomingBoat.GetNode<Spatial>("BoatCargo").QueueFree();
            }
        }
        DialogicSharp.SetVariable("RewardAmmount", rew.ToString());
        DialogueManager.GetInstance().StartDialogue(Worker, "RewardDialogue");
        incomingBoat = null;
    }
    private void OnShipLeft(Node body)
    {
        if (body is Vehicle v)
        {
            Boats.Remove(v);
            if (v.IsPlayerOwned())
            {
                Worker.GetTalkText().Talk("Καλό δρόμο καϊκτσή, καλή επιστροφή");
            }
        }
        
    }
    public bool IsInPort(Vehicle boat)
    {
        return Boats.Contains(boat);
    }
    public bool PlayerHasBoatInPort()
    {
        for (int i = 0; i <Boats.Count; i++)
        {
            if (Boats[i].IsPlayerOwned())
            {
                return true;
            }
        }
        return false;
    }
    public Vehicle GetPlayerBoat()
    {
        Vehicle plBoat = null;
        for (int i = 0; i <Boats.Count; i++)
        {
            if (Boats[i].IsPlayerOwned())
            {
                plBoat = Boats[i];
                break;
            }
        }
        return plBoat;
    }
    public bool HasSpot(out Vector3 spot)
    {
        bool hasspot = true;
        spot = Vector3.Zero;
        List<Vector3> Rests = new List<Vector3>();
        foreach (Position3D loc in Locations)
        {
            Rests.Add(loc.GlobalTranslation);
        }
        
        if (Boats.Count > 0)
        {
            
            for (int i = 0; i <Boats.Count; i++)
            {
                
                for (int l = Rests.Count - 1; l > -1; l--)
                {
                    if (Rests[l].DistanceTo(Boats[i].GlobalTranslation) < 20)
                    {
                        Rests.RemoveAt(l);
                    }
                }
            }
            if (Rests.Count == 0)
                return false;
        }
        spot = Rests[0];
        return hasspot;
    }
    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            if (Locations.Count > RestLocations.Count)
            {
                for (int i = 0; i <Locations.Count; i++)
                {
                    Position3D p = Locations[i];
                    p.QueueFree();
                }
                Locations.Clear();
            }
            for (int i = 0; i < RestLocations.Count; i++)
            {
                if (Locations.Count == i)
                {
                    Position3D p = new Position3D(){
                        Name = "Position" + i.ToString()
                    };
                    p.Scale *= 20;
                    Locations.Add(p);
                    AddChild(p, true);
                    p.Owner = this;
                }
                Locations[i].GlobalTranslation = RestLocations[i];

            }
            ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents = PortExtents;
            ((CubeMesh)GetNode<MeshInstance>("MeshInstance").Mesh).Size = PortExtents * 2;
        }
    }
}
public class PortInfo
{
	public Vector2 Location;
	public bool Visited;

	public void SetInfo(Vector2 Loc, bool Vis)
	{
		Location = Loc;
		Visited = Vis;
	}
	public void UpdateInfo(Port p)
	{
		Visited = p.Visited;
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>(){
			{"Location", Location},
			{"Visited", Visited},
		};
		
		return data;
	}
	public void UnPackData(Resource data)
    {
        Location = (Vector2)data.Get("Location");
		Visited = (bool)data.Get("Visited");
	}
}
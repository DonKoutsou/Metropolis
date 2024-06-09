using Godot;
using System;
using System.Collections.Generic;

public class MyWorld : Spatial
{
	[Export]
	Dictionary<int, PackedScene> GlobalItemListConfiguration = new Dictionary<int, PackedScene>();

	[Export]
	PackedScene PlayerScene = null;

	[Signal]
    public delegate void PlayerSpawnedEventHandler(Player Pl);

	static Dictionary<int, PackedScene> GlobalItemList = new Dictionary<int, PackedScene>();
	
	static List<IslandInfo> Orderedilestodissable = new List<IslandInfo>();
	
	static List<IslandInfo> Orderedilestoenable = new List<IslandInfo>();

	List<IslandInfo> ActiveIles = new List<IslandInfo>();
	
	public List<IslandInfo> GetActiveIles()
	{
		List<IslandInfo> Active = new List<IslandInfo>();
		foreach (IslandInfo info in ActiveIles)
		{
			Active.Add(info);
		}
		return Active;
	}
	static MyWorld Instance;
	public static MyWorld GetInstance()
	{
		return Instance;
	}
	public bool LoadSave = false;
	public override void _Ready()
	{
		base._Ready();
		//pl = GetNode<Player>("Player");
		foreach (KeyValuePair<int, PackedScene> pair in GlobalItemListConfiguration)
		{
			PackedScene text = pair.Value;
			int key = pair.Key;
			GlobalItemList.Add(key, text);
		}
		
		Instance = this;
		GetNode<WorldMap>("WorldMap").Init(LoadSave);
	}

	float d = 0.2f;
	bool EnableDissableBool = false;
	public override void _Process(float delta)
	{
		d -= delta;
		if (d <= 0)
		{
			d = 0.2f;
			if (!EnableDissableBool)
			{
				EnableDissableBool = true;
				if (Orderedilestodissable.Count > 0)
				{
					IslandInfo ile = Orderedilestodissable[0];
					Orderedilestodissable.Remove(ile);
					//if (ile.IsIslandSpawned())
					//{
					ToggleIsland(ile, false, false);
				}
			}
			else if (EnableDissableBool)
			{
				EnableDissableBool = false;
				if (Orderedilestoenable.Count > 0)
				{
					IslandInfo ile = Orderedilestoenable[0];
					Orderedilestoenable.Remove(ile);
					//if (!ile.IsIslandSpawned())
					//{
					ToggleIsland(ile, true, false);
				}
			}
		}
	}
	public static PackedScene GetItemByType(ItemName name)
	{
		PackedScene path;
		GlobalItemList.TryGetValue((int)name, out path);
		return path;
	}
	public Player SpawnPlayer(Vector3 pos)
	{
		Player pl = (Player)PlayerScene.Instance();
		AddChild(pl);
		pl.Teleport(pos);
		WorldMap.GetInstance().pl = pl;
		EmitSignal("PlayerSpawnedEventHandler", pl);
		VehicleHud.GetInstance().ConnectToPlayer(pl);
		CameraAnimationPlayer.GetInstance().FadeIn(6);
		return pl;
	}
	public void OnPlayerKilled()	
	{
		StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();
		start.GameOver();
		SaveLoadManager.GetInstance().ClearSaves();
	}
	
	public static void ArrangeIlesBasedOnDistance(List<IslandInfo> ilestodissable, List<IslandInfo> ilestoenable)
    {
		if (ilestoenable.Count > 0)
		{
			foreach (IslandInfo IleArray in ilestoenable)
			{
				Vector2 ilepos = IleArray.Position;
				if (Orderedilestoenable.Contains(IleArray))
					continue;
				float ind = Math.Abs(ilepos.x) + Math.Abs(ilepos.y);
				if (Orderedilestoenable.Count == 0)
				{
					Orderedilestoenable.Insert(0, IleArray);
					continue;
				}
				IslandInfo closest = Orderedilestoenable[0];
				Vector2 closestpos = closest.Position;
				float dif = Math.Abs(Math.Abs(closestpos.x) + Math.Abs(closestpos.y) - ind);
				for (int i = Orderedilestoenable.Count - 1; i > -1; i--)
				{
					Vector2 Orderedpos = Orderedilestoenable[i].Position;
					float newdif = Math.Abs(Math.Abs(Orderedpos.x) + Math.Abs(Orderedpos.y) - ind);
					if (dif > newdif)
					{
						closest = Orderedilestoenable[i];
						dif = newdif;
					}
				}

				if (Math.Abs(closestpos.x) + Math.Abs(closestpos.y) < Math.Abs(ilepos.x) + Math.Abs(ilepos.y))
				{
					Orderedilestoenable.Insert(Orderedilestoenable.IndexOf(closest) + 1, IleArray);
					continue;
				}
				else
				{
					Orderedilestoenable.Insert(Orderedilestoenable.IndexOf(closest), IleArray);
					continue;
				}
			}
		}

		if (ilestodissable.Count > 0)
		{
			foreach (IslandInfo IleArray in ilestodissable)
			{
				if (Orderedilestodissable.Contains(IleArray))
					continue;

				Vector2 ilepos = IleArray.Position;
				float ind = Math.Abs(ilepos.x) + Math.Abs(ilepos.y);
				if (Orderedilestodissable.Count == 0)
				{
					Orderedilestodissable.Insert(0, IleArray);
					continue;
				}
				IslandInfo closest = Orderedilestodissable[0];
				Vector2 closestpos = closest.Position;
				float dif = Math.Abs(Math.Abs(closestpos.x) + Math.Abs(closestpos.y) - ind);
				for (int i = Orderedilestodissable.Count - 1; i > -1; i--)
				{
					Vector2 Orderedpos = Orderedilestoenable[i].Position;
					float newdif = Math.Abs(Math.Abs(Orderedpos.x) + Math.Abs(Orderedpos.y) - ind);
					if (dif > newdif)
					{
						closest = Orderedilestodissable[i];
						dif = newdif;
					}
				}
				if (Math.Abs(closestpos.x) + Math.Abs(closestpos.y) < Math.Abs(ilepos.x) + Math.Abs(ilepos.y))
				{
					Orderedilestodissable.Insert(Orderedilestodissable.IndexOf(closest) + 1, IleArray);
					continue;
				}
				else
				{
					Orderedilestodissable.Insert(Orderedilestodissable.IndexOf(closest), IleArray);
					continue;
				}
			}
		}
    }
	
	public static void IleTransition(IslandInfo from, IslandInfo to)
	{
		//GD.Print("Transitioning from : " + from.Name + " to " + to.Name);
		WorldMap map = WorldMap.GetInstance();
		map.SyncSeas();
		List<IslandInfo> ilestodissable = new List<IslandInfo>();
		List<IslandInfo> ilestoenable = new List<IslandInfo>();
		
		int ViewDistance = Settings.GetGameSettings().ViewDistance;

		List<IslandInfo> closestfrom;
		map.GetClosestIles(from ,out closestfrom, ViewDistance);

		List<IslandInfo> closestto;
		map.GetClosestIles(to,out closestto, ViewDistance);

		for (int i = 0; i < closestfrom.Count; i ++)
		{
			if (closestfrom[i] == to)
				continue;
			if (closestto.Contains(closestfrom[i]))
				continue;
			if (Orderedilestodissable.Contains(closestfrom[i]))
				continue;
			if (!ilestodissable.Contains(closestfrom[i]))
			{
				ilestodissable.Insert(ilestodissable.Count, closestfrom[i]);

				if (Orderedilestoenable.Contains(closestfrom[i]))
					Orderedilestoenable.Remove(closestfrom[i]);
			}

		}
		for (int i = 0; i < closestto.Count; i ++)
		{

			if (!ilestoenable.Contains(closestto[i]) )
				ilestoenable.Insert(ilestoenable.Count, closestto[i]);
			if (ilestodissable.Contains(closestto[i]))
				ilestodissable.Remove(closestto[i]);
			if (Orderedilestodissable.Contains(closestto[i]))
				Orderedilestodissable.Remove(closestto[i]);
		}
		ArrangeIlesBasedOnDistance(ilestodissable, ilestoenable);

		//GD.Print("-----------Transition Finished------------");
	}

	public void ToggleIsland(IslandInfo ileinfo,bool toggle, bool affectneigh)
	{
		if (toggle && !ileinfo.IsIslandSpawned())
		{
			ActiveIles.Add(ileinfo);
			WorldMap.GetInstance().ReSpawnIsland(ileinfo).InputData(ileinfo);
		}
		else if (!toggle && ileinfo.IsIslandSpawned())
		{
			ActiveIles.Remove(ileinfo);
			Island ile = ileinfo.Island;
			ileinfo.UpdateInfo(ile);
			if (ileinfo.KeepInstance == true)
			{
				RemoveChild(ile);
				ile.Visible = false;
			}
			else
				ile.QueueFree();
		}
		else if (toggle && ileinfo.KeepInstance == true && !ActiveIles.Contains(ileinfo))
		{
			ActiveIles.Add(ileinfo);
			Island i = ileinfo.Island;
			AddChild(i);
			i.Visible = true;
			i.InputData(ileinfo);
		}
		if (affectneigh)
		{
			List<IslandInfo> closestto;
			int ViewDistance = Settings.GetGameSettings().ViewDistance;
			WorldMap.GetInstance().GetClosestIles(ileinfo, out closestto, ViewDistance);

			for (int i = 0; i < closestto.Count; i ++)
				ToggleIsland(closestto[i], toggle, false);
		}
	}
	static private void ToggleChildrenCollision(Node node, bool toggle, bool recursive)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is CollisionShape)
			{
				((CollisionShape)child).Disabled = toggle;
			}
			if (recursive)
			{
				ToggleChildrenCollision(child, toggle, recursive);
			}
		}
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause"))
		{
			Pause();
		}
	}
	public void Pause()
	{
		StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();

		bool paused = GetTree().Paused;
		
		start.Pause(!paused);

		GetTree().Paused = !paused;
	}
};

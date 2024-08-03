using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MyWorld : Spatial
{
	[Export]
	PackedScene PlayerScene = null;

	[Signal]
    public delegate void PlayerSpawnedEventHandler(Player Pl);
	
	static List<IslandInfo> Orderedilestodissable = new List<IslandInfo>();
	
	static List<IslandInfo> Orderedilestoenable = new List<IslandInfo>();

	static List<IslandInfo> ActiveIles = new List<IslandInfo>();
	
    public override void _EnterTree()
    {
        base._EnterTree();
    }
	public override void _ExitTree()
    {
        base._ExitTree();
		Orderedilestodissable.Clear();
		Orderedilestoenable.Clear();
		ActiveIles.Clear();
    }
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
	
	public Player SpawnPlayer(Vector3 pos, Vector3 rot)
	{
		Player pl = (Player)PlayerScene.Instance();
		AddChild(pl);
		pl.Teleport(pos);
		pl.GlobalRotation = rot;
		//WorldMap.GetInstance().pl = pl;
		EmitSignal("PlayerSpawnedEventHandler", pl);
		MapGrid.GetInstance().ConnectPlayer(pl);
		VehicleHud.GetInstance().ConnectToPlayer(pl);
		CameraAnimationPlayer.GetInstance().FadeIn(6);
		return pl;
	}
	public void OnPlayerKilled(string reason = null)	
	{
		//try to find characters on islands around that can rescue you
		/*WorldMap map = WorldMap.GetInstance();
		IslandInfo currentile = map.GetCurrentIleInfo();
		Island ile = currentile.Island;
		Character rescuer;
		if (ile.HasCharacters())
		{
			if (TryRescue(ile, Player.GetInstance().GlobalTranslation, out rescuer))
			{
				Rescue(rescuer, ile);
				return;
			}
		}
		List<IslandInfo> CloseIles;
		map.GetClosestIles(currentile,  out CloseIles, 2);
		foreach (IslandInfo island in CloseIles)
		{
			Island iletocheck = island.Island;
			if (TryRescue(iletocheck, Player.GetInstance().GlobalTranslation, out rescuer))
			{
				Rescue(rescuer, iletocheck);
				return;
			}
		}*/

		StartingScreen start = ((WorldRoot)GetParent().GetParent().GetParent().GetParent()).GetStartingScreen();
		start.GameOver(reason);
		SaveLoadManager.GetInstance().ClearSaves();
	}
	bool TryRescue(Island ile, Vector3 pos, out Character rescuer)
	{
		rescuer = null;
		List<NPC> chars;
		ile.GetCharacters(out chars);
		foreach (Character cha in chars)
		{
			if (!cha.GetUnconState())
			{
				if (rescuer != null && rescuer.GlobalTranslation.DistanceTo(pos) < cha.GlobalTranslation.DistanceTo(pos))
					continue;
				rescuer = cha;
			}
		}
		if (rescuer != null)
			return true;
		return false;
	}
	void Rescue(NPC rescuer, Island ile)
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Connect("FadeOutFinished", this, "FinishRescue");
        CameraAnimation.FadeInOut(3);
		Rescuer = rescuer;
		RescueIle = ile;
	}
	NPC Rescuer;
	Island RescueIle;
	void FinishRescue()
	{
		CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "FinishRescue");
		Player pl = Player.GetInstance();
		if (pl.HasVehicle())
		{
			Vehicle v = pl.GetVehicle();
			if (RescueIle.HasPort())
			{
				v.ReparentVehicle(RescueIle);
				Port p = RescueIle.GetPort(0);
				Vector3 spot;
				if (p.HasSpot(out spot))
				{
					v.GlobalTranslation = spot;
				}
			}
			pl.GetVehicle().Capsize();
		}
		//pl.RechargeCharacter(100);
		pl.Respawn();
		pl.Teleport(Rescuer.GetNode<Position3D>("TalkPosition").GlobalTranslation);

		DialogueManager.GetInstance().StartDialogue(Rescuer, "RescueDialogue");

		Rescuer = null;
		RescueIle = null;
	}
	public void AttemptDeathSave()
	{

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
	
	public static void IleTransition(IslandInfo to)
	{
		//GD.Print("Transitioning from : " + from.Name + " to " + to.Name);
		WorldMap map = WorldMap.GetInstance();
		//map.SyncSeas();
		List<IslandInfo> ilestodissable = new List<IslandInfo>();
		List<IslandInfo> ilestoenable = new List<IslandInfo>();

		//Vector2 pos = WorldMap.GetInstance().GlobalToMap();
		
		//to.Visited = true;
		MapGrid.GetInstance().SetIslandVisited(to);
		DayNight.GetInstance().UpdatePlayerDistance(Math.Max(Math.Abs(to.Position.x), Math.Abs(to.Position.y)) / 20);
		
		int ViewDistance = Settings.GetGameSettings().ViewDistance;

		//List<IslandInfo> closestfrom;
		//map.GetClosestIles(from ,out closestfrom, ViewDistance);

		List<IslandInfo> closestto;
		map.GetClosestIles(to,out closestto, ViewDistance);

		for (int i = 0; i < ActiveIles.Count; i ++)
		{
			if (ActiveIles[i] == to)
				continue;
			if (closestto.Contains(ActiveIles[i]))
				continue;
			if (Orderedilestodissable.Contains(ActiveIles[i]))
				continue;
			if (!ilestodissable.Contains(ActiveIles[i]))
			{
				ilestodissable.Insert(ilestodissable.Count, ActiveIles[i]);

				if (Orderedilestoenable.Contains(ActiveIles[i]))
					Orderedilestoenable.Remove(ActiveIles[i]);
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
		if (toggle)
		{
			if (ileinfo.KeepInstance == true && !ActiveIles.Contains(ileinfo))
			{
				ActiveIles.Add(ileinfo);
				Island i = ileinfo.Island;
				AddChild(i);
				i.Visible = true;
				i.InputData(ileinfo);
			}
			else if (!ileinfo.IsIslandSpawned())
			{
				ActiveIles.Add(ileinfo);
				WorldMap.GetInstance().ReSpawnIsland(ileinfo).InputData(ileinfo);
			}
			
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


		if (affectneigh)
		{
			List<IslandInfo> closestto;
			int ViewDistance = Settings.GetGameSettings().ViewDistance;
			WorldMap.GetInstance().GetClosestIles(ileinfo, out closestto, ViewDistance);

			for (int i = 0; i < closestto.Count; i ++)
				ToggleIsland(closestto[i], toggle, false);
		}
		#if DEBUG
		MapGrid.GetInstance().OnIslandToggled(ileinfo.Position, toggle);
		#endif
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
		StartingScreen start = WorldRoot.GetInstance().GetStartingScreen();

		bool paused = GetTree().Paused;
		
		start.Pause(!paused);

		GetTree().Paused = !paused;
	}
};

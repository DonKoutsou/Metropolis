using Godot;
using System;
using System.Collections.Generic;

public class MyWorld : Spatial
{
	Player pl;

	bool EnableDissableBool = false;


	static List<Island> Orderedilestodissable = new List<Island>();
	static List<Island> Orderedilestoenable = new List<Island>();

	public override void _Ready()
	{
		base._Ready();
		pl = GetNode<Player>("Player");
		Instance = this;
	}
	static MyWorld Instance;
	public static MyWorld GetInstance()
	{
		return Instance;
	}
	public void OnPlayerKilled()	
	{
		StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();
		start.GameOver();
	}
	public Player GetPlayer()
	{
		return pl;
	}
	public void RegisterIle(Island ile)
	{
		AddChild(ile);
		ToggleIsland(ile, false);
	}
	float d = 0.2f;
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
					for (int i = Orderedilestodissable.Count - 1; i >= 0; i--)
					{
						Island ile = Orderedilestodissable[i];
						Orderedilestodissable.Remove(ile);
						if (ile.m_enabled)
						{
							ToggleIsland(ile, false);
							break;
						}	
					}
				}
			}
			else if (EnableDissableBool)
			{
				EnableDissableBool = false;
				if (Orderedilestoenable.Count > 0)
				{
					var iles = Orderedilestoenable;
					for (int i = 0; i < iles.Count; i++)
					{
						Island ile = Orderedilestoenable[i];
						Orderedilestoenable.Remove(ile);
						if (!ile.m_enabled)
						{
							ToggleIsland(ile, true);
							break;
						}	
					}
				}
			}
		}
	}
	public static void ArrangeIlesBasedOnDistance(List<Island> ilestodissable, List<Island> ilestoenable)
    {
		if (ilestoenable.Count > 0)
		{
			foreach (Island IleArray in ilestoenable)
			{
				if (Orderedilestoenable.Contains(IleArray))
					continue;
				float ind = Math.Abs(IleArray.GlobalTransform.origin.x) + Math.Abs(IleArray.GlobalTransform.origin.y);
				if (Orderedilestoenable.Count == 0)
				{
					Orderedilestoenable.Insert(0, IleArray);
					continue;
				}
				Island closest = Orderedilestoenable[0];
				float dif = Math.Abs(Math.Abs(closest.GlobalTransform.origin.x) + Math.Abs(closest.GlobalTransform.origin.y) - ind);
				for (int i = Orderedilestoenable.Count - 1; i > -1; i--)
				{
					float newdif = Math.Abs(Math.Abs(Orderedilestoenable[i].GlobalTransform.origin.x) + Math.Abs(Orderedilestoenable[i].GlobalTransform.origin.y) - ind);
					if (dif > newdif)
					{
						closest = Orderedilestoenable[i];
						dif = newdif;
					}
				}
				if (Math.Abs(closest.GlobalTransform.origin.x) + Math.Abs(closest.GlobalTransform.origin.y) < Math.Abs(IleArray.GlobalTransform.origin.x) + Math.Abs(IleArray.GlobalTransform.origin.y))
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
			foreach (Island IleArray in ilestodissable)
			{
				if (Orderedilestodissable.Contains(IleArray))
					continue;
				float ind = Math.Abs(IleArray.GlobalTransform.origin.x) + Math.Abs(IleArray.GlobalTransform.origin.y);
				if (Orderedilestodissable.Count == 0)
				{
					Orderedilestodissable.Insert(0, IleArray);
					continue;
				}
				Island closest = Orderedilestodissable[0];
				float dif = Math.Abs(Math.Abs(closest.GlobalTransform.origin.x) + Math.Abs(closest.GlobalTransform.origin.y) - ind);
				for (int i = Orderedilestodissable.Count - 1; i > -1; i--)
				{
					float newdif = Math.Abs(Math.Abs(Orderedilestodissable[i].GlobalTransform.origin.x) + Math.Abs(Orderedilestodissable[i].GlobalTransform.origin.y) - ind);
					if (dif > newdif)
					{
						closest = Orderedilestodissable[i];
						dif = newdif;
					}
				}
				if (Math.Abs(closest.GlobalTransform.origin.x) + Math.Abs(closest.GlobalTransform.origin.y) < Math.Abs(IleArray.GlobalTransform.origin.x) + Math.Abs(IleArray.GlobalTransform.origin.y))
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
	
	public static void IleTransition(Island from, Island to)
	{
		//GD.Print("Transitioning from : " + from.Name + " to " + to.Name);

		List<Island> ilestodissable = new List<Island>();
		List<Island> ilestoenable = new List<Island>();
		int ViewDistance = Settings.GetGameSettings().ViewDistance;
		List<Island> closestfrom;
		WorldMap.GetInstance().GetClosestIles(from ,out closestfrom, ViewDistance);

		List<Island> closestto;
		WorldMap.GetInstance().GetClosestIles(to,out closestto, ViewDistance);

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

	public static void ToggleIsland(Island ile, bool Toggle, bool affectneigh = false)
	{
		if (Toggle)
		{
			ile.EnableIsland();
			ToggleChildrenCollision(ile, false, true);
		}
		else
		{
			ile.DeactivateIsland();
			ToggleChildrenCollision(ile, true, true);
		}
			
			
		if (affectneigh)
		{
			List<Island> closestto;
			int ViewDistance = Settings.GetGameSettings().ViewDistance;
			WorldMap.GetInstance().GetClosestIles(ile, out closestto, ViewDistance);

			for (int i = 0; i < closestto.Count; i ++)
				ToggleIsland(closestto[i], Toggle);
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
			StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();
			if (Settings.GetGameSettings().Visible)
			{
				start.CloseSettings();
				return;
			}
			
			bool paused = GetTree().Paused;
		
			start.Pause(!paused);

			GetTree().Paused = !paused;
		}
	}
};

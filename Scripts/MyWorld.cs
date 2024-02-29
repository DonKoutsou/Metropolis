using Godot;
using System;
using System.Collections.Generic;



public class MyWorld : Spatial
{
	Player pl;

	float d = 0.2f;

	public int Seed;

	public override void _Ready()
	{
		
		base._Ready();
		pl = GetNode<Player>("Player");
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
		//ile.Hide();
		AddChild(ile);
		ToggleIsland(ile, false);
		//GD.Print("Spawned Island on locations: " + ile.loctospawnat);
	}
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
	public static void ArrangeIlesBasedOnDistance()
    {
        var cells = ilestoenable;
		if (cells.Count > 0)
		{
			foreach (Island IleArray in cells)
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
		ilestoenable.Clear();
		cells.Clear();
		cells = ilestodissable;
		if (cells.Count > 0)
		{
			foreach (Island IleArray in cells)
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
		ilestodissable.Clear();
    }
	static List<Island> ilestodissable = new List<Island>();
	static List<Island> ilestoenable = new List<Island>();

	static List<Island> Orderedilestodissable = new List<Island>();
	static List<Island> Orderedilestoenable = new List<Island>();
	public static void IleTransition(Island from, Island to)
	{
		GD.Print("Transitioning from : " + from.Name + " to " + to.Name);
		List<Island> closestfrom;
		WorldMap.GetClosestIles(from ,out closestfrom);
		//from.GetClosestIles(out closestfrom);

		List<Island> closestto;
		WorldMap.GetClosestIles(to,out closestto);
		ilestodissable.Clear();
		ilestoenable.Clear();
		//to.GetClosestIles(out closestto);
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
				//ToggleIsland(closestfrom[i], false);
			}
		}
		for (int i = 0; i < closestto.Count; i ++)
		{
			//ToggleIsland(closestto[i], true);
			if (!ilestoenable.Contains(closestto[i]) )
				ilestoenable.Insert(ilestoenable.Count, closestto[i]);
			if (ilestodissable.Contains(closestto[i]))
				ilestodissable.Remove(closestto[i]);
			if (Orderedilestodissable.Contains(closestto[i]))
				Orderedilestodissable.Remove(closestto[i]);
		}
		ArrangeIlesBasedOnDistance();
		GD.Print("-----------Transition Finished------------");
	}

	public static void ToggleIsland(Island ile, bool Toggle, bool affectneigh = false)
	{
		if (Toggle)
		{
			ile.EnableIsland();
		}
		else
		{
			ile.DeactivateIsland();
		}
		if (affectneigh)
		{
			List<Island> closestto;
			WorldMap.GetClosestIles(ile, out closestto);
			//ile.GetClosestIles(out closestto);
			for (int i = 0; i < closestto.Count; i ++)
			{
				ToggleIsland(closestto[i], Toggle);
			}
		}
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause"))
		{
			StartingScreen start = ((MainWorld)GetParent()).GetStartingScreen();
			if (!GetTree().Paused)
			{
				GetTree().Paused = true;
				start.Pause(true);
			}
			else
			{
				GetTree().Paused = false;
				start.Pause(false);
			}
				
		}
	}
};

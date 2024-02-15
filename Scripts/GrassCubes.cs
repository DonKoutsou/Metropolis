using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GrassCubes : Spatial
{
	Spatial[] grass;
	Player pl;
	public override void _Ready()
	{
		grass = new Spatial[25];
		GetChildren().CopyTo(grass, 0);
	}
	public override void _Process(float delta)
	{
		if (pl == null)
		{
			var pls = GetTree().GetNodesInGroup("player");
			pl = (Player)pls[0];
		}
		Vector3 plpos = pl.GlobalTransform.origin;
		for (int i = 0; i < grass.Count(); i++)
		{
			Vector3 grasspos = grass[i].GlobalTransform.origin;
			if (grasspos.DistanceTo(plpos) > 1000)
			{
				if (grass[i].Visible)
					grass[i].Hide();
				continue;
			}
			else
			{
				if (!grass[i].Visible)
					grass[i].Show();
				grass[i].Call("Update", plpos);
			}
		}
	}
	public void ToggleGrass(bool toggle)
	{
		if (toggle)
		{
			Show();
			SetProcess(true);
		}
		else
		{
			Hide();
			SetProcess(false);
		}
		
	}
}

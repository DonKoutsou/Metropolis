using Godot;
using System;

public class Door : Wall
{
	Island IslandAccess;

	Door m_closestdoor;

	Position3D m_spawnpos;

	public void SetIslandToAccess(Island ile)
	{
		if (ile == null)
			return;
		IslandAccess = ile;
		Island parent = (Island)GetParent();

		Vector2 parislandloc = new Vector2(parent.Transform.origin.x, parent.Transform.origin.z) ;
		Vector2 islandloc = new Vector2(IslandAccess.Transform.origin.x, IslandAccess.Transform.origin.z) ;
		//down
		if (islandloc.y > parislandloc.y)
		{
			m_closestdoor = IslandAccess.a_doors[2];
		}
			//up
		if (islandloc.y < parislandloc.y)
		{
			m_closestdoor = IslandAccess.a_doors[1];
		}
			//right
		if (islandloc.x > parislandloc.x)
		{
			m_closestdoor = IslandAccess.a_doors[0];
		}
			//left
		if (islandloc.x < parislandloc.x)
		{
			m_closestdoor = IslandAccess.a_doors[3];
		}
	}
	public void GetIslandAcces(out Island ile)
	{
		ile = IslandAccess;	
	}
	Position3D GetSpawnPos()
	{
		return m_spawnpos;
	}
		
	public override void _Ready()
	{
		m_spawnpos = GetNode<Position3D>("SpawnPos");
	}


	 public override void _Process(float delta)
	{

	}
	 public override void Touch(object body)
	{
		if (body is Mob)
			return;

		if (IslandAccess == null)
			return;

		Character player = (Character)body;

		if (player == null)
			return;

		if (player is Player)
		{
			((Island)GetParent()).DeactivateIsland();
			IslandAccess.EnableIsland();
		}
		else
		{
			player.UpdateMap();
		}

		Vector3 mypos = player.Transform.origin;
		Vector3 ispos = m_closestdoor.GetSpawnPos().GlobalTransform.origin;
		player.GlobalTranslation = ispos;
		//sl.UpdateMap();

		
		
		GD.Print("Teleported from " + mypos + " to " + ispos);
	}

}







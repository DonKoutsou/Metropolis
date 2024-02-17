using Godot;
using System;

public class Door : Wall
{
	Island IslandAccess;


	Position3D m_spawnpos;

	public void SetIslandToAccess(Island ile)
	{
		if (ile == null)
			return;

		IslandAccess = ile;
		Island parent = (Island)GetParent();

		Vector2 parislandloc = new Vector2(parent.Transform.origin.x, parent.Transform.origin.z) ;
		Vector2 islandloc = new Vector2(IslandAccess.Transform.origin.x, IslandAccess.Transform.origin.z) ;

		Island checkdis;
		ile.GetClosestDoor(GlobalTransform.origin).GetIslandAcces(out checkdis);
		if (checkdis == null)
		{
			ile.GetClosestDoor(GlobalTransform.origin).Toggle(true);
			ile.GetClosestDoor(GlobalTransform.origin).SetIslandToAccess(parent);
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
		Toggle(false);
	}
	 public override void Touch(object body)
	{
		Vector3 forw = GlobalTransform.basis.z;
		Vector3 toOther = GlobalTransform.origin - ((Spatial)body).GlobalTransform.origin;
		var thing = forw.Dot(toOther);
		if (thing < 0)
			return;
		
		if (body is Mob)
			return;

		if (IslandAccess == null)
			return;

		Character player = (Character)body;

		if (player == null)
			return;

		if (player is Player)
		{
			MyWorld.IleTransition(IslandAccess, (Island)GetParent());
			//player.UpdateMap();
		}
		else
		{
			player.UpdateMap();
		}

		//Vector3 mypos = player.Transform.origin;
		//Vector3 ispos = m_closestdoor.GetSpawnPos().GlobalTransform.origin;
		//player.GlobalTranslation = ispos;
		//sl.UpdateMap();

		
		
		//GD.Print("Teleported from " + mypos + " to " + ispos);
	}
	public void Toggle(bool toggle)
	{
		if (toggle)
		{
			//GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
			Material blueprintShader = ResourceLoader.Load<Material>("res://Scenes/BlueMat.tres");
			GetNode<MeshInstance>("MeshInstance").MaterialOverride = blueprintShader;
		}
		else
		{
			//GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",true);
			Material blueprintShader = ResourceLoader.Load<Material>("res://Scenes/RedMat.tres");
			GetNode<MeshInstance>("MeshInstance").MaterialOverride = blueprintShader;		
		}
	}
	public void ToggleCollisions(bool toggle)
	{
		if (toggle)
			GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
		else
			GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",true);
	}

}







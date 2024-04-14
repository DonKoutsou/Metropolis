using Godot;
using System;

public class Door : Wall
{
	Island IslandAccess;

	public void SetIslandToAccess(Island ile)
	{
		if (ile == null)
			return;

		IslandAccess = ile;
		Island parent = (Island)GetParent();

		Vector2 parislandloc = new Vector2(parent.Transform.origin.x, parent.Transform.origin.z) ;
		Vector2 islandloc = new Vector2(IslandAccess.Transform.origin.x, IslandAccess.Transform.origin.z) ;

	}
	public void GetIslandAcces(out Island ile)
	{
		ile = IslandAccess;	
	}
		
	public override void _Ready()
	{
		Toggle(false);
	}
	 public override bool Touch(object body)
	{
		Vector3 forw = GlobalTransform.basis.z;
		Vector3 toOther = GetNode<CollisionShape>("CollisionShape").GlobalTransform.origin - ((Spatial)body).GlobalTransform.origin;
		var thing = forw.Dot(toOther);
		if (thing > 0)
			return false;
		
		if (body is Mob)
			return false;

		if (IslandAccess == null)
			return false;

		Character player = (Character)body;

		if (player == null)
			return false;

		if (player is Player)
		{
			//MyWorld.IleTransition((Island)GetParent(), IslandAccess);
			//player.UpdateMap();
		}
		return true;

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







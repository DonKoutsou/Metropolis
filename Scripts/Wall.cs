using Godot;
using System;
using System.Drawing.Text;

public class Wall : Island
{
	[Export]
	bool IsEventWall = false;
	public override void _Ready()
	{
		Node b = GetNodeOrNull("Bounds");
		if (b != null)
			b.QueueFree();
		#if DEBUG
		if (Engine.EditorHint)
			return;
		#endif
		Translation = SpawnGlobalLocation;
		
		if (IsEventWall)
			Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(GetEventWallRotation()));

		StaticBody sea = GetNode<StaticBody>("SeaBed");
		sea.GlobalRotation = Vector3.Zero;
	}
	private float GetEventWallRotation()
	{
		float rot = 0;
		Vector2 gtrans = WorldMap.GetInstance().WorldToMap(new Vector2(GlobalTranslation.x, GlobalTranslation.z));
		if (gtrans.x > gtrans.y)
		{
			//right
			if (gtrans.x == 20)
			{
				rot = -90;
			}
			//up
			else if (gtrans.y == -20)
			{
				rot = 0;
			}
		}
		if (gtrans.x < gtrans.y)
		{
			//down
			if (gtrans.y == 20)
			{
				rot = 180;
			}
			//left
			else if (gtrans.x == -20)
			{
				rot = 90;
			}
		}
		return rot;
	}
}




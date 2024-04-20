using Godot;
using System;
using System.Collections.Generic;

public class VehicleDamageManager : Spatial
{
	List<CollisionShape> WingColliders = new List<CollisionShape>();
    
    List<int> DestroyedWings = new List<int>();
	CollisionShape HullCollision;

	Vehicle veh;

	private void On_Ship_Part_Collided(RID body_rid, object body, int body_shape_index, int local_shape_index)
    {
        CollisionShape obj = (CollisionShape)veh.ShapeOwnerGetOwner((uint)local_shape_index);
        bool DamagedWing = false;
        for (int i = 0; i < WingColliders.Count; i++)
        {
            if (obj != WingColliders[i])
                continue;
            DamagedWing = true;
            veh.OnWingDamaged(i);
            DestroyedWings.Insert(DestroyedWings.Count, i);
            WingColliders[i].SetDeferred("disabled", true);
            GD.Print("Wings Collided");
        }
        if (!DamagedWing)
        {
            GD.Print("Body Collided");
        }
    }
    public void ToggleWingCollision(bool Toggle, int index = -1)
    {
        for (int i = 0; i < WingColliders.Count; i++)
        {
            if (DestroyedWings.Contains(i))
                continue;
            WingColliders[i].SetDeferred("disabled", !Toggle);
        }
    }
    public bool IsWingWorking(int index)
    {
        return !DestroyedWings.Contains(index);
    }
	public override void _Ready()
	{
		veh = GetParent().GetNode<Vehicle>("VehicleBody");
		WingColliders.Insert(0, veh.GetNode<CollisionShape>("WingCollider0"));
		WingColliders.Insert(1, veh.GetNode<CollisionShape>("WingCollider1"));
		WingColliders.Insert(2, veh.GetNode<CollisionShape>("WingCollider2"));
		WingColliders.Insert(3, veh.GetNode<CollisionShape>("WingCollider3"));
		HullCollision = veh.GetNode<CollisionShape>("CollisionShape");
	}

}




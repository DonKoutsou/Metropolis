using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class VehicleDamageManager : Spatial
{

	List<CollisionShape> WingColliders = new List<CollisionShape>();
    
    List<int> DestroyedWings = new List<int>();
	CollisionShape HullCollision;
    CollisionShape LightCollision;
    bool LightCondition = true;
	Vehicle veh;

    public void GetDestroyedWings(out List<int>Destroyed)
    {
        Destroyed = new List<int>();
        for (int i = 0; i < DestroyedWings.Count; i++)
        {
            Destroyed.Add(DestroyedWings[i]);
        }
    }
	private void On_Ship_Part_Collided(RID body_rid, object body, int body_shape_index, int local_shape_index)
    {
        CollisionShape obj = (CollisionShape)veh.ShapeOwnerGetOwner((uint)local_shape_index);
        for (int i = 0; i < WingColliders.Count; i++)
        {
            if (obj != WingColliders[i])
                continue;
            veh.OnWingDamaged(i);
            DestroyedWings.Insert(DestroyedWings.Count, i);
            //WingColliders[i].SetDeferred("disabled", true);
            GD.Print("Wings Collided");
            return;
        }
        if (obj == HullCollision)
        {
            GD.Print("Body Collided");
        }
        else if (obj == LightCollision)
        {
            GD.Print("Lights Collided");
            LightCondition = false;
            veh.OnLightDamaged();
            LightCollision.SetDeferred("Disabled", true);
        }
    }
    public bool GetLightCondition()
    {
        return LightCondition;
    }
    public void ToggleWingCollision(bool Toggle, int index = -1)
    {
        for (int i = 0; i < WingColliders.Count; i++)
        {
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
        for (int i = 0; i < 4; i++)
        {
            CollisionShape wing = veh.GetNodeOrNull<CollisionShape>("WingCollider" + i);
            if (wing == null)
                break;
            WingColliders.Insert(i, wing);
        }

		HullCollision = veh.GetNode<CollisionShape>("CollisionShape");
        LightCollision = veh.GetNode<CollisionShape>("CollisionShape2");
	}
    public void InputData(VehicleDamageInfo data)
	{
        LightCondition = data.LightCondition;
        if (!LightCondition)
            veh.OnLightDamaged();
        foreach (int wings in data.DestroyedWings)
        {
            veh.OnWingDamaged(wings);
            DestroyedWings.Add(wings);
        }
	}

}
public class VehicleDamageInfo
{
    public List<int> DestroyedWings = new List<int>();
    public bool LightCondition = true;
    public void UpdateInfo(Vehicle veh)
    {
        VehicleDamageManager damman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        damman.GetDestroyedWings(out DestroyedWings);
        LightCondition = damman.GetLightCondition();
    }
    public void SetInfo(Vehicle veh)
    {
        VehicleDamageManager damman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        damman.GetDestroyedWings(out DestroyedWings);
        LightCondition = damman.GetLightCondition();
    }
    public Dictionary<string, object>GetPackedData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        int[] wings = new int[DestroyedWings.Count];
        for (int i = 0; i < DestroyedWings.Count; i++)
            wings[i] = DestroyedWings[i];
        data.Add("DestroyedWings", wings);
        data.Add("LightCondition", LightCondition);
        return data;
    }
    public void UnPackData(Godot.Object data)
    {
        int[] wings = (int[])data.Get("DestroyedWings");
        for (int i = 0; i < wings.Count(); i++)
        {
            DestroyedWings[i] = wings[i];
        }
		LightCondition = (bool)data.Get("LightCondition");
		
    }
}



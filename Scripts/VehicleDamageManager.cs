using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class VehicleDamageManager : Spatial
{
    /*
	List<CollisionShape> WingColliders = new List<CollisionShape>();
    List<CollisionShape> EngineColliders = new List<CollisionShape>();
    List<int> WingStates = new List<int>();
    //List<int> DestroyedEngines = new List<int>();
    List<int> EngineStates = new List<int>();
	CollisionShape HullCollision;
    int HullState = 100;
    CollisionShape LightCollision;
    bool LightCondition = true;
	Vehicle veh;

    public void GetWingStates(out List<int>_WingStates)
    {
        _WingStates = new List<int>();
        for (int i = 0; i < WingStates.Count; i++)
        {
            _WingStates.Add(WingStates[i]);
        }
    }
    public void GetEngineStates(out List<int>_EngineStates)
    {
        _EngineStates = new List<int>();
        for (int i = 0; i < EngineStates.Count; i++)
        {
            _EngineStates.Add(EngineStates[i]);
        }
    }
    public int GetEngineState(int index)
    {
        return EngineStates[index];
    }
    public int GetHullState()
    {
        return HullState;
    }
    public void RepairEngine(int index, int amm)
    {
        EngineStates[index] = Math.Min(100, EngineStates[index] + amm);
    }
	private void On_Ship_Part_Collided(RID body_rid, object body, int body_shape_index, int local_shape_index)
    {
        float speed = veh.GetLastSpeed();
        CollisionShape obj = (CollisionShape)veh.ShapeOwnerGetOwner((uint)local_shape_index);
        for (int i = 0; i < WingColliders.Count; i++)
        {
            if (obj != WingColliders[i])
                continue;
            veh.OnWingDamaged(i);
            WingStates[i] = Math.Max(0, WingStates[i] - (int)(speed * 10));
            return;
        }
        for (int i = 0; i < EngineColliders.Count; i++)
        {
            if (obj != EngineColliders[i])
                continue;
            //veh.OnWingDamaged(i);
            EngineStates[i] = Math.Max(0, EngineStates[i] - (int)(speed * 10));
            ((Particles)obj.GetChild(0)).Emitting = true;
            //WingColliders[i].SetDeferred("disabled", true);
            return;
        }
        if (obj == HullCollision)
        {
            HullState = Math.Max(0, HullState - (int)(speed * 50));
        }
        else if (obj == LightCollision)
        {
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
        return WingStates[index] > 0;
    }
    public bool IsEngineWorking(int index)
    {
        return EngineStates[index] > 0;
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
            WingStates.Add(100);
        }
        for (int i = 0; i < 4; i++)
        {
            CollisionShape engine = veh.GetNodeOrNull<CollisionShape>("EngineCollider" + i);
            if (engine == null)
                break;
            EngineColliders.Insert(i, engine);
            EngineStates.Add(100);
        }
        
		HullCollision = veh.GetNode<CollisionShape>("CollisionShape");
        LightCollision = veh.GetNode<CollisionShape>("CollisionShape2");
	}
    public void InputData(VehicleDamageInfo data)
	{
        LightCondition = data.LightCondition;
        HullState = data.HullState;
        if (!LightCondition)
            veh.OnLightDamaged();

        var wings = data.WingStates;
        for (int i = 0; i < wings.Count; i++)
        {
            //veh.OnWingDamaged(wings);
            WingStates[i] = wings[i];
        }
        var engines = data.EngineStates;
        for (int i = 0; i < engines.Count; i++)
        {
            EngineStates[i] = engines[i];
        }
	}*/

}
public class VehicleDamageInfo
{
    //public List<int> WingStates = new List<int>();
    public List<int> EngineStates = new List<int>();
    public int HullState;
    public bool LightCondition = true;
    /*
    public void UpdateInfo(Vehicle veh)
    {
        VehicleDamageManager damman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        //damman.GetWingStates(out WingStates);
        damman.GetEngineStates(out EngineStates);
        LightCondition = damman.GetLightCondition();
        HullState = damman.GetHullState();
    }
    public void SetInfo(Vehicle veh)
    {
        VehicleDamageManager damman = veh.GetParent().GetNode<VehicleDamageManager>("VehicleDamageManager");
        //damman.GetWingStates(out WingStates);
        damman.GetEngineStates(out EngineStates);
        LightCondition = damman.GetLightCondition();
        HullState = damman.GetHullState();
    }
    public Dictionary<string, object>GetPackedData()
    {
        

        int[] wings = new int[WingStates.Count];
        for (int i = 0; i < WingStates.Count; i++)
            wings[i] = WingStates[i];

        int[] enginestates = new int[EngineStates.Count];
        for (int i = 0; i < EngineStates.Count; i++)
            enginestates[i] = EngineStates[i];

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            //{"WingStates", wings},
            {"EngineStates", enginestates},
            {"LightCondition", LightCondition},
            {"HullState", HullState}
        };
        return data;
    }
    public void UnPackData(Godot.Object data)
    {
        int[] wings = (int[])data.Get("WingStates");
        for (int i = 0; i < wings.Count(); i++)
        {
            WingStates.Add(wings[i]);
        }
        int[] engines = (int[])data.Get("EngineStates");
        for (int i = 0; i < engines.Count(); i++)
        {
            EngineStates.Add(engines[i]);
        }
		LightCondition = (bool)data.Get("LightCondition");
		HullState = (int)data.Get("HullState");
    }*/
}



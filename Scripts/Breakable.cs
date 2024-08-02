using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Breakable : StaticBody
{
    [Export(PropertyHint.Layers3dPhysics)]
    public uint BreakAbleLayer { get; set; }

    Explosive AtatchedEx;
    public override void _Ready()
    {
        base._Ready();

        Node parent = GetParent();
		while (!(parent is Island))
		{
            if (parent == null)
            {
                SetProcess(false);
                return;
            }
				
			parent = parent.GetParent();
		}
		Island ile = (Island)parent;
		ile.RegisterChild(this);

        SetProcess(false);
        
    }
    
    public void AtatchExplosive(Player pl, Explosive ex)
    {

        var spacestate = GetWorld().DirectSpaceState;
        Vector3 rayor = pl.GlobalTranslation;
        rayor.y += 4;
        Vector3 rayend = GlobalTranslation;

        var rayar = spacestate.IntersectRay(rayor, rayend, null, BreakAbleLayer);
        
        if (rayar.Count == 0)
            return;

        ex.GetParent().RemoveChild(ex);
        AddChild(ex);

        Vector3 AttatchPos = (Vector3)rayar["position"];

        ex.GlobalTranslation = AttatchPos;
        ex.LookAt(rayor, Vector3.Up);
        ex.StartExplosive();
        ex.Connect("OnExploded", this, "AtatchedExplosiveGoneBoom");

        AtatchedEx = ex;
    }
    public void AtatchedExplosiveGoneBoom()
    {
        AtatchedEx.GetNode<MeshInstance>("MeshInstance").Hide();    
        GetNode<Spatial>("RockParent").Hide();
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (!AtatchedEx.GetNode<Particles>("Explosion/Particles2").Emitting)
        {
            RemoveBreakable();
        }
    }
    public void RemoveBreakable()
    {
        Node parent = GetParent();
        while (!(parent is Island))
        {
            if (parent == null)
            {
                SetProcess(false);
                return;
            }
                
            parent = parent.GetParent();
        }
        Island ile = (Island)parent;

        ile.UnRegisterChild(this);
        QueueFree();
    }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
            GetNode<MeshInstance>("RockParent/MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("RockParent/MeshInstance").MaterialOverlay = null;
    }
}
public class BreakableInfo
{
	public string BreakableName;
	public bool Destroyed;
	public void UpdateInfo(bool Dest)
	{
		Destroyed = Dest;
	}
	public void SetInfo(string name, bool Dest)
	{
		BreakableName = name;
		Destroyed = Dest;
	}
	public Dictionary<string, object>GetPackedData()
	{
		Dictionary<string, object> data = new Dictionary<string, object>()
		{
			{"Name", BreakableName},
			{"Destroyed", Destroyed}
		};
		return data;
	}
    public void UnPackData(Resource data)
    {
        BreakableName = (string)data.Get("Name");
		Destroyed = (bool)data.Get("Destroyed");
    }
}

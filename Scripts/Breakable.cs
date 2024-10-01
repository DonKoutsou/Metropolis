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

        SetProcess(false);


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
        QueueFree();
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
        
    }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
            GetNode<MeshInstance>("RockParent/MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("RockParent/MeshInstance").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
        List<Item> items;
        ItemName[] types = {ItemName.EXPLOSIVE};
        pl.GetCharacterInventory().GetItemsByType(out items, types);
        pl.GetCharacterInventory().RemoveItem(items[0], false);
        AtatchExplosive(pl, (Explosive)items[0]);
    }
    public string GetActionName(Player pl)
    {
		return "Τοποθέτησε εκρηκτικό.";
    }
    public string GetActionName2(Player pl)
    {
		return "Null.";
    }
    public string GetActionName3(Player pl)
    {
		return "Null.";
    }
    public bool ShowActionName2(Player pl)
    {
        return false;
    }
    public bool ShowActionName3(Player pl)
    {
        return false;
    }

    public bool ShowActionName(Player pl)
    {
        return pl.GetCharacterInventory().HasItemOfType(ItemName.EXPLOSIVE);
    }
    public string GetObjectDescription()
    {
        return "Με ένα εκρηκτικό θα μπορούσα να το σπάσω";
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

using Godot;
using System;


[Tool]
public class PaintCan : Item
{
    [Export]
    Color CanColor = new Color(1,1,1);

    [Export]
    public bool RandomiseColor = false;
    public override void _Ready()
    {
        if(Engine.EditorHint)
            return;
        base._Ready();
        if (RandomiseColor)
        {
            CanColor = LimbRandomColorProvider.GetRandomColor();
        }
        ((GradientTexture)((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, CanColor);
    }
    public void SetColor(Color col)
    {
        CanColor = col;
    }
    public Color GetColor()
    {
        return CanColor;
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(!Engine.EditorHint)
            return;
        
        ((GradientTexture)((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, CanColor);
    }
    public override string GetInventoryItemName()
	{
		return LimbRandomColorProvider.TranslateColor(CanColor) + ItemName;
	}
    public override string GetItemDesc()
    {
        return "Χρώμα :" + LimbRandomColorProvider.TranslateColor(CanColor) + "\n" + LocalisationHolder.GetString(ItemDesc);
    }

    public override void InputData(ItemInfo data)
	{
		base.InputData(data);

        Color cap = (Color)data.CustomData["CanColor"];
	    SetColor(cap);
	}
    public override void GetCustomData(out string[] Keys, out object[] Values)
	{
		Keys = new string[1];
		Values = new object[1];
        
        Keys[0] = "CanColor";
		Values[0] = GetColor();
	}
}

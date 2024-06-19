using Godot;
using System;

[Tool]
public class GeneratorDoor : StaticBody
{
    bool Open = false;
    AnimationPlayer anim;
    Spatial Switch;
    public override void _Ready()
    {
        anim = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
        Switch = GetParent().GetNode<Spatial>("Switch");
    }
    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        Vector3 pos = Switch.GlobalTranslation;
        pos.y -= Switch.Translation.y;
        return Switch.GlobalTranslation;
    }
    public string GetActionName(Vector3 PlayerPos)
    {   
        string name = "Άνοιξε";
        if (Open)
        {
            name = "Κλείσε";
        }
        return name;
    }
    public void ToggleDoor()
    {
        if (Open)
        {
            Open = false;
            anim.Play("Close");
        }
        else
        {
            Open = true;
            anim.Play("Open");
        }

        //pl.anim.PlayAnimation(anim);
        //pl.global
    }
    public void HighLightObject(bool toggle)
    {
		((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable",  toggle);
    }
}

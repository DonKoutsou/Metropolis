using Godot;
using System;
using System.Drawing;

public class TalkText : Label3D
{
    [Export]
    Curve TextSizePerDistance = null;
    //[Export]
    //Curve ElevationPerDistance = null;
    static TalkText instance;
    public void Talk(string diag)
    {
        Text = diag;
        GetNode<Timer>("UpdateTimer").Start();
        Show();
        SetProcess(true);
    }
    public void TurnOff()
    {
        SetProcess(false);
        Hide();
    }
    public override void _Process(float delta)
    {
       /* var pl = GetTree().GetNodesInGroup("player");
        if (pl.Count == 0)
            return;
        var plpos = ((Spatial)pl[0]).GlobalTransform.origin;
        var screenpos = GetTree().Root.GetCamera().UnprojectPosition(plpos);
        var campos = GetTree().Root.GetCamera().GlobalTransform.origin;
        float dist = plpos.DistanceTo(campos);
        var minval = (dist-0)/(300-0);
        var sz = (int)Math.Round(TextSizePerDistance.Interpolate(minval));
        //RectPosition =  new Vector2(screenpos.x - 1300, screenpos.y - ElevationPerDistance.Interpolate(minval) * 2);
        ((DynamicFont)Font).Size = sz;*/
    }
    public static TalkText GetInst()
    {
        return instance;
    }
    public override void _Ready()
    {
        base._Ready();
        SetProcess(false);
        Hide();
        instance = this;
    }

}

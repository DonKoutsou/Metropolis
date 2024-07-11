using Godot;
using System;

public class JobBoardPanel : StaticBody
{
    JobBoard boardUI;

    public override void _Ready()
    {
        base._Ready();
        boardUI = GetNode<JobBoard>("JobBoardUI");
        boardUI.Hide();
    }
    public void ToggleUI(bool toggle)
    {
        boardUI.UpdateJobs();
        boardUI.Visible = toggle;
    }
    public void HighLightObject(bool toggle)
    {
        ShaderMaterial Mat = (ShaderMaterial)GetNode<MeshInstance>("MeshInstance3").MaterialOverlay;
        if (Mat != null)
        {
            Mat.SetShaderParam("enable",  toggle);
        }
    }
    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        return GlobalTranslation;
    }

}

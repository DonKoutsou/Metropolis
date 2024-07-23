using Godot;
using System;

public class JobBoardPanel : StaticBody
{
    [Export]
    NodePath Port = null;
    public void ToggleUI(bool toggle)
    {
       // JobBoard boardUI = (JobBoard)PlayerUI.GetInstance().GetUI(PlayerUIType.JOBBOARD);
        //boardUI.ToggleUI(toggle);
       //boardUI.SetPort(GetNode<Port>(Port));
    }
    public void HighLightObject(bool toggle)
    {
        ShaderMaterial Mat = (ShaderMaterial)GetNode<MeshInstance>("MeshInstance3").MaterialOverlay;
        if (Mat != null)
        {
            Mat.SetShaderParam("enable",  toggle);
        }
    }

}

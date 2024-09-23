using Godot;
using System;

public class MemoryButton : MeshInstance
{
    [Export]
    int ButtonNum = 0;
    [Signal]
    public delegate void OnPressed(MemoryButton button);
    public bool Enabled = false;

    private void ButtonHovered(Node camera, InputEvent ev, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@ev.IsActionPressed("Select"))
		{
            if (!Enabled)
                return;
			EmitSignal("OnPressed", this);
            Flash();
		}
    }
    public void Flash()
    {
        if (GetNode<AnimationPlayer>("ButtonAnims").IsPlaying())
            GetNode<AnimationPlayer>("ButtonAnims").Stop();
        GetNode<AnimationPlayer>("ButtonAnims").Play("Flash");
    }
    public void FlashStatic()
    {
        if (GetNode<AnimationPlayer>("ButtonAnims").IsPlaying())
            GetNode<AnimationPlayer>("ButtonAnims").Stop(true);
        GetNode<AnimationPlayer>("ButtonAnims").Play("FlashStatic");
    }
    public int GetButtonNumber()
    {
        return Name.Substr(7, 1).ToInt();
    }
}

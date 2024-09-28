using Godot;
using System;

public class MemoryButton : Spatial
{
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
    private void AnimFin(string anim)
    {
        if (anim == "Flash")
        {
            FlashStatic();
        }
    }
    public void FlashStatic()
    {
        if (GetNode<AnimationPlayer>("ButtonAnims").IsPlaying())
            GetNode<AnimationPlayer>("ButtonAnims").Stop(true);
        GetNode<AnimationPlayer>("ButtonAnims").Play("FlashStatic");
    }
    public void Reset()
    {
        GetNode<AnimationPlayer>("ButtonAnims").Play("RESET");
    }
    public int GetButtonNumber()
    {
        return Name.Substr(7, 1).ToInt();
    }
}

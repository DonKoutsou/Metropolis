using Godot;
using System;

public class ScreenEffects : Control
{
    [Export]
    NodePath AnimationPlayer = null;
    public override void _Ready()
    {
        Visible = false;
    }
    public void PlayerToggle(Player pl)
    {
        Visible = pl != null;
    }
    public void PlayEffect(ScreenEffectTypes type)
    {
        if (type == ScreenEffectTypes.DAMAGE)
            GetNode<AnimationPlayer>(AnimationPlayer).Play("Damage");
    }

}
public enum ScreenEffectTypes
{
    DAMAGE,

}
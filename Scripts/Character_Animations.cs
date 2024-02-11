using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



public class Character_Animations : AnimationPlayer
{
    AnimationTree animtree;
    public override void _Ready()
    {
        base._Ready();
        animtree = GetNode<AnimationTree>("AnimationTree");
    }
    public void PlayAnimation(E_Animations AnimationName)
    {
        switch (AnimationName)
        {
            case E_Animations.Idle:
            {
                animtree.Set("parameters/Idle_Walk_Blend/blend_amount", 0f);
                break;
            }
            case E_Animations.Walk:
            {
                animtree.Set("parameters/Idle_Walk_Blend/blend_amount", 1f);
                break;
            }
            case E_Animations.Jump:
            {
                animtree.Set("parameters/JumpShot/active", true);
                break;
            }
        }
        
        /*string anima = Enum.GetName(AnimationName.GetType(), AnimationName);
        string[] que =  GetQueue();
        if (que.Count() > 0)
        {
            if (que.Contains(anima))
            {
                return;
            }
        }
        
        Queue(anima);*/
    }
    public void ForceAnimation(E_Animations AnimationName)
    {
        string anima = Enum.GetName(AnimationName.GetType(), AnimationName);
        CurrentAnimation = anima;
    }
    
    
}
public enum E_Animations
{
    Idle,
    Walk,
    Jump,
}
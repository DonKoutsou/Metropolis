using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



public class Character_Animations : AnimationPlayer
{
    AnimationTree animtree;
    Spatial parent;

    Particles walkpart;
    float currot;
    public override void _Ready()
    {
        base._Ready();
        animtree = GetNode<AnimationTree>("AnimationTree");
        walkpart = GetParent().GetNode<Particles>("Particles");
        walkpart.Emitting = false;
        parent = (Spatial)GetParent();
    }
    public void PlayAnimation(E_Animations AnimationName)
    {
        switch (AnimationName)
        {
            case E_Animations.Idle:
            {
                animtree.Set("parameters/Idle_Walk_Blend/blend_amount", 0f);
                Vector3 rot = new Vector3(0, 0, 0);
                parent.Rotation = rot;
                walkpart.Emitting = false;
                
                break;
            }
            case E_Animations.Walk:
            {
                animtree.Set("parameters/Idle_Walk_Blend/blend_amount", 1f);
                animtree.Set("parameters/Walk_Speed/scale", 2f);
                if (currot > 0)
                    currot -= 3f;
                Vector3 rot = new Vector3(Mathf.Deg2Rad(currot), 0, 0);
                parent.Rotation = rot;
                walkpart.Emitting = true;
                if (walkpart.Amount != 4)
                    walkpart.Amount = 4;
                break;
            }
            case E_Animations.Jump:
            {
                animtree.Set("parameters/JumpShot/active", true);
                walkpart.Emitting = false;
                break;
            }
            case E_Animations.Run:
            {
                if (currot < 10)
                    currot += 1f;
                Vector3 rot = new Vector3(Mathf.Deg2Rad(currot), 0, 0);
                parent.Rotation = rot;
                animtree.Set("parameters/Idle_Walk_Blend/blend_amount", 1f);
                animtree.Set("parameters/Walk_Speed/scale", 4f);
                walkpart.Emitting = true;
                if (walkpart.Amount != 8)
                    walkpart.Amount = 8;
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
    Run,
    Jump,
}
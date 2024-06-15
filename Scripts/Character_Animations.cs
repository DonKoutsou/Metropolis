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
    AnimationNodeStateMachinePlayback stateMachine;

    CharacterSoundManager Sounds;
    //AnimationPlayer AnimPlayer;
    public override void _Ready()
    {
        base._Ready();
        animtree = GetNode<AnimationTree>("AnimationTree");
        stateMachine = (AnimationNodeStateMachinePlayback)animtree.Get("parameters/playback");
        animtree.Active = true;

        walkpart = GetParent().GetNode<Particles>("Particles");
        walkpart.Emitting = false;
        parent = (Spatial)GetParent();
        Sounds = parent.GetNode<CharacterSoundManager>("CharacterSoundManager");
    }
    public bool IsStanding()
    {
        return stateMachine.GetCurrentNode() == "Standing";
    }
    public bool IsClimbing()
    {
        return CurrentAnimation == "ClimbUp" || CurrentAnimation == "ClimbDown";
    }
    public void ToggleInstrument(bool toggle)
    {
        int amm = 1;
        if (!toggle)
            amm = 0;
        animtree.Set("parameters/Walking/IntrumentBlend/blend_amount", amm);
        animtree.Set("parameters/Sitting/IntrumentBlend/blend_amount", amm);
    }
    public void ToggleIdle()
    {
        animtree.Set("parameters/conditions/Dead", false);
        animtree.Set("parameters/conditions/Sitting", false);
        animtree.Set("parameters/conditions/Idle", true);
        
    }
    public void ToggleDeath()
    {
        
        animtree.Set("parameters/conditions/Sitting", false);
        animtree.Set("parameters/conditions/Idle", false);
        animtree.Set("parameters/conditions/Dead", true);
        
    }
    public void ToggleSitting(bool chair = false)
    {
        animtree.Set("parameters/conditions/Idle", false);
        animtree.Set("parameters/conditions/Dead", false);
        animtree.Set("parameters/conditions/Sitting", true);
        Sounds.ToggleSound(false, "Walk");
        if (chair)
            animtree.Set("parameters/Sitting/ChairBlend/blend_amount", 1);
        else
            animtree.Set("parameters/Sitting/ChairBlend/blend_amount", 0);
    }
    public void PlayAnimation(E_Animations AnimationName)
    {
        switch (AnimationName)
        {
            case E_Animations.Idle:
            {
                animtree.Set("parameters/Walking/Idle_Walk_Blend/blend_amount", 0f);
                animtree.Set("parameters/Walking/Idle_Run_Blend/blend_amount", 0f);
                Vector3 rot = new Vector3(0, 0, 0);
                parent.Rotation = rot;
                walkpart.Emitting = false;
                Sounds.ToggleSound(false, "Walk");
                break;
            }
            case E_Animations.Walk:
            {
                animtree.Set("parameters/Walking/Idle_Walk_Blend/blend_amount", 1f);
                animtree.Set("parameters/Walking/Idle_Run_Blend/blend_amount", 0f);
                //animtree.Set("parameters/Walk_Speed/scale", 2f);
                if (currot > 0)
                    currot -= 3f;
                Vector3 rot = new Vector3(Mathf.Deg2Rad(currot), 0, 0);
                parent.Rotation = rot;
                walkpart.Emitting = true;
                if (walkpart.Amount != 4)
                    walkpart.Amount = 4;
                Sounds.ToggleSound(true, "Walk", 0.5f, 3);

                break;
            }
            case E_Animations.Jump:
            {
                animtree.Set("parameters/Walking/JumpShot/active", true);
                walkpart.Emitting = false;
                Sounds.ToggleSound(false, "Walk");
                break;
            }
            case E_Animations.Run:
            {
                if (currot < 10)
                    currot += 1f;
                Vector3 rot = new Vector3(Mathf.Deg2Rad(currot), 0, 0);
                parent.Rotation = rot;
                animtree.Set("parameters/Walking/Idle_Walk_Blend/blend_amount", 0f);
                animtree.Set("parameters/Walking/Idle_Run_Blend/blend_amount", 1f);

                //animtree.Set("parameters/Walk_Speed/scale", 4f);
                walkpart.Emitting = true;
                if (walkpart.Amount != 8)
                    walkpart.Amount = 8;
                Sounds.ToggleSound(true, "Walk", 1, 5);
                break;
            }
            case E_Animations.ClimbUp:
            {
                animtree.Set("parameters/Walking/ClimbUp/active", true);
                walkpart.Emitting = false;
                Sounds.ToggleSound(false, "Walk");
                break;
            }
            case E_Animations.ClimbDown:
            {
                animtree.Set("parameters/Walking/ClimbDown/active", true);
                walkpart.Emitting = false;
                Sounds.ToggleSound(false, "Walk");
                break;
            }
            /*case E_Animations.Death:
            {
                animtree.Set("parameters/SittingBlend/blend_amount", 0f);
                animtree.Set("parameters/DeadBlend/blend_amount", 1f);
                break;
            }*/
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
    ClimbUp,
    ClimbDown
}
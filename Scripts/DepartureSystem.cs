using Godot;
using System;

public class DepartureSystem : Spatial
{
    static DepartureSystem instance;
    public override void _Ready()
    {
        base._Ready();
        instance = this;
    }
    public static DepartureSystem GetInstance()
    {
        return instance;
    }
    public void OnChoiceMade(bool BabyTaken)
    {
        if (BabyTaken)
        {

            GetNode<DialogueTrigger>("WithoutMakingChoise/DialogueTrigger2").Enabled = false;
            GetNode<DialogueTrigger>("WithoutMakingChoise/DialogueTrigger3").Enabled = false;
            GetNode<DialogueTrigger>("WithoutMakingChoise/DialogueTrigger4").Enabled = false;
            GetNode<DialogueTrigger>("AfterLeavingBaby/DialogueTrigger2").Enabled = false;
            GetNode<DialogueTrigger>("AfterLeavingBaby/DialogueTrigger3").Enabled = false;
            GetNode<DialogueTrigger>("AfterLeavingBaby/DialogueTrigger4").Enabled = false;
            GetNode<GameOverTrigger>("GameOverTrigger").Enabled = false;
            
            QueueFree();
        }
        else
        {
            GetNode<CollisionShape>("WithoutMakingChoise/DialogueTrigger2/Area/CollisionShape").Disabled = true;
            GetNode<CollisionShape>("WithoutMakingChoise/DialogueTrigger3/Area/CollisionShape").Disabled = true;
            GetNode<CollisionShape>("WithoutMakingChoise/DialogueTrigger4/Area/CollisionShape").Disabled = true;

            GetNode<CollisionShape>("AfterLeavingBaby/DialogueTrigger2/Area/CollisionShape").Disabled = false;
            GetNode<CollisionShape>("AfterLeavingBaby/DialogueTrigger3/Area/CollisionShape").Disabled = false;
            GetNode<CollisionShape>("AfterLeavingBaby/DialogueTrigger4/Area/CollisionShape").Disabled = false;
        }
    }
}

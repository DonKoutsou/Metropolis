using Godot;
using System;

public class Vehicle : RigidBody
{
    public void BoardVehicle(Character cha)
    {
        Rotation = new Vector3(Rotation.x,Rotation.y,0.0f);
        cha.GlobalTranslation = GlobalTranslation;
        GetParent().RemoveChild(this);
        Spatial guy = cha.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy");
        guy.Translation = new Vector3(0.0f, 1.0f, 0.0f);
        //Translation = new Vector3(0.0f, -1.0f, 0.0f);
        Sleeping = true;
        
        cha.GetNode<Spatial>("Pivot").AddChild(this);

        Rotation = new Vector3(0.0f,0.0f,0.0f);

        Translation = new Vector3(0.0f,1.0f,0.0f);
    }
     public void UnBoardVehicle(Character cha)
    {
        //cha.GlobalTransform = GlobalTransform;
        cha.GetNode<Spatial>("Pivot").RemoveChild(this);
        Spatial guy = cha.GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy");
        guy.Translation = new Vector3(0.0f, 0.0f, 0.0f);
        //Translation = new Vector3(0.0f, -1.0f, 0.0f);
        //CollisionLayer = 0;
        
        cha.GetParent().AddChild(this);
        
        
        GlobalTranslation = new Vector3(cha.GlobalTranslation.x + 4, cha.GlobalTranslation.y+4, cha.GlobalTranslation.z + 4);
        Sleeping = false;
    }
}

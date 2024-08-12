using Godot;
using System;
using System.Collections.Generic;

public class DoorPivot : Spatial
{
    [Export]
    float OpenCloseTime = 0.5f;
    [Export]
    List<NodePath> Doors = null;
    [Export]
    List<Vector3> Rotations = null;
    [Export]
    List<Vector3> Translations = null;
    [Export]
    AudioStream OpenSound = null;
    [Export]
    AudioStream CloseSound = null;

    AudioStreamPlayer3D AudioPlayer;

    public override void _Ready()
    {
        base._Ready();
        AudioPlayer = new AudioStreamPlayer3D(){
            MaxDistance = 4096,
            UnitSize = 3,
        };
        AddChild(AudioPlayer);
    }
    public void Open()
    {

        for (int i = 0; i < Doors.Count; i++)
        {
            if (Rotations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "rotation_degrees", Rotations[i], OpenCloseTime);
            }
            if (Translations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "translation", Translations[i], OpenCloseTime);
            }
        }
        AudioPlayer.Stream = OpenSound;
        AudioPlayer.Play();
    }
    public void Close()
    {
        
        for (int i = 0; i < Doors.Count; i++)
        {
            if (Rotations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "rotation_degrees", Vector3.Zero, OpenCloseTime);
            }
            if (Translations.Count > i)
            {
                var tw = CreateTween();
                tw.TweenProperty(GetNode(Doors[i]), "translation", Vector3.Zero, OpenCloseTime);
            }
        }
        AudioPlayer.Stream = CloseSound;
        AudioPlayer.Play();
    }
}

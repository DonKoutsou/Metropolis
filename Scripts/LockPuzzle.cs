using Godot;
using System;
using System.Collections.Generic;

public class LockPuzzle : BasePuzzle
{
    Spatial LockPick;
    Spatial LockRotatingBody;
    int PuzzleSolution;
    bool MovingPick = false;
    float allowed;
    Dictionary<string, AudioStreamPlayer> Sounds = new Dictionary<string, AudioStreamPlayer>();

    public override void _Ready()
    {
        LockPick = GetNode<Spatial>("LockRotatingBody/LockPick");
        LockRotatingBody = GetNode<Spatial>("LockRotatingBody");

        Sounds.Add("StopLockSound", GetNode<AudioStreamPlayer>("StopLockSound"));
        Sounds.Add("OpenLockSound", GetNode<AudioStreamPlayer>("OpenLockSound"));
        Sounds.Add("SolvedSound", GetNode<AudioStreamPlayer>("SolvedSound"));

        ProduceSolution();
    }
    private void ProduceSolution()
    {
        Random r = new Random();

        PuzzleSolution = r.Next(-91, 91);
    }
    private float GetAllowedRotation()
    {
        float lockpickrot = Mathf.Rad2Deg(LockPick.Rotation.y);
        float dif = Mathf.Abs(lockpickrot - PuzzleSolution);
        if (dif < 5)
            return 90;

        float alowed = 90 - Mathf.Min(90, dif);

        return alowed;
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionPressed("Select"))
        {
            MovingPick = true;
            allowed = GetAllowedRotation();
            return;
        }
        else
        {
            MovingPick = false;
            Sounds["StopLockSound"].Stop();
        }
        if (@event is InputEventMouseMotion)
        {
            Vector2 pos = new Vector2(((InputEventMouseMotion)@event).Relative.x, ((InputEventMouseMotion)@event).Relative.y);

            Vector3 rot = LockPick.Rotation;

            rot.y = Mathf.Clamp(rot.y - pos.x / 80, Mathf.Deg2Rad(-90), Mathf.Deg2Rad(90));

            LockPick.Rotation = rot;
        }
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (MovingPick)
        {
            Vector3 rot = LockRotatingBody.Rotation;
            rot.y = Mathf.Max(rot.y - 0.05f, Mathf.Deg2Rad(-allowed));
            
            if (rot.y == Mathf.Deg2Rad(-90))
            {
                PuzzleSolved();
                return;
            }
            else if (rot.y == Mathf.Deg2Rad(-allowed))
            {
                GetNode<AnimationPlayer>("AnimationPlayer").Play("Twitch");
                if (!Sounds["StopLockSound"].Playing)
                    Sounds["StopLockSound"].Playing = true;
            }
            if (rot.y == LockRotatingBody.Rotation.y)
            {
                Sounds["OpenLockSound"].Playing = false;
            }
            else if (!Sounds["OpenLockSound"].Playing)
            {
                Sounds["OpenLockSound"].Playing = true;
            }
            LockRotatingBody.Rotation = rot;
        }
        else
        {
            Vector3 rot = LockRotatingBody.Rotation;
            rot.y = Mathf.Min(rot.y + 0.05f, 0);
            LockRotatingBody.Rotation = rot;
            if (rot.y != 0)
            {
                if (!Sounds["OpenLockSound"].Playing)
                    Sounds["OpenLockSound"].Playing = true;
            }
            else
            {
                Sounds["OpenLockSound"].Playing = false;
            }
        }
    }
    private void PuzzleSolved()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RemovePicks");
        SetProcess(false);
        SetProcessInput(false);
        Sounds["SolvedSound"].Play();
        Sounds["OpenLockSound"].Playing = false;
    }
    private void FinishGame(string anim)
    {
        if (anim == "RemovePicks")
            Finished(true);
    }
}
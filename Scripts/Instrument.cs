using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Instrument : Item
{
    [Signal]
	public delegate void OnSongEnded(Instrument inst);

	public AudioStreamPlayer3D Speaker;

    bool Playing = false;

    public virtual void ToggleMusic(bool Toggle)
	{
		Speaker.Playing = Toggle;
        Playing = Toggle;
        if (Toggle)
		{
            float pos = 0;
			Speaker.Stream = MusicManager.GetSong(this, out pos);
			Speaker.Play(pos);
		}
	}
    public void SongEnded()
    {
        EmitSignal("OnSongEnded", this);
        Playing = false;
    }
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
		Speaker = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
		
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        MusicManager.RegisterInstrument(this);
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        MusicManager.RemoveInstrument(this);
    }
    public bool IsPlaying()
    {
        return Playing;
    }
    public AudioStream GetCurrentSong()
	{
		return Speaker.Stream;
	}
    public float GetCurrentSongLoc()
    {
		return Speaker.GetPlaybackPosition() + (float)AudioServer.GetTimeSinceLastMix() + 0.05f;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

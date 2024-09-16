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
            float pos;
			Speaker.Stream = MusicManager.GetInstance().GetSong(this, out pos);
			Speaker.Play(pos);
		}
	}
    public void SongEnded()
    {
        EmitSignal("OnSongEnded", this);
        Playing = false;
    }
    public override void _Ready()
    {
        base._Ready();
		Speaker = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
		
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        MusicManager.GetInstance().RegisterInstrument(this);
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        MusicManager.GetInstance().RemoveInstrument(this);
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
}

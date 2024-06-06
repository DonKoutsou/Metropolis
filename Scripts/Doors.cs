using Godot;
using System;

public class Doors : Spatial
{
	AnimationPlayer anim;
	public override void _Ready()
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("CloseGate");
	}
	private void Entered_Area(Node body)
	{
		anim.Play("OpenGate");
	}
	private void Left_Area(Node body)
	{
		anim.Play("CloseGate");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Godot;
using System;


public class Sea : MeshInstance
{
	ShaderMaterial material;
	public override void _Ready()
	{
		material = (ShaderMaterial)GetActiveMaterial(0);
		//((Spatial)GetParent()).GlobalRotation = Vector3.Zero;
		AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.CurrentAnimation = "Wave";
		anim.Play();
	}
	/*private void _on_Sea_visibility_changed()
	{
		if (Visible)
			SetPhysicsProcess(true);
		else
			SetPhysicsProcess(false);
	}*/
	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		float str = DayNight.GetRainStr();
		if (str > 10)
		{
			material.SetShaderParam("IsRaining", true);
		}
		else
		{
			material.SetShaderParam("IsRaining", false);
		}
		//float thing = ((-360 - DayNight.GetWindDirection()) / 360) - 0.5f;
		//thing *= 2;
		//float theta = Mathf.Deg2Rad(-360 - DayNight.GetWindDirection());
		//Vector3 vec = Vector3.Forward;
		//Vector3 rotvec = vec.Rotated(new Vector3(0,1,0), -theta);
		//float cs = Mathf.Cos(theta);
		//float sn = Mathf.Sin(theta);
		//float x = 0;
		//float y = -0.0004f * DayNight.GetWindStr();
		//float px = x * cs - y * sn;
		//float py = x * sn + y * cs;
		//material.SetShaderParam("sampler_direction", new Vector2(rotvec.x, rotvec.z));
	}
}

using Godot;
using System;


public class Sea : StaticBody
{
	//ShaderMaterial material;
	public override void _Ready()
	{
		GlobalRotation = Vector3.Zero;
		//GetNode<MeshInstance>("Sea").QueueFree();
		//AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");
		//anim.Play("Wave");
	}
    //public override void _PhysicsProcess(float delta)
    //{
       //base._PhysicsProcess(delta);
		//ForceUpdateTransform();
    //}
    /*public override void _Process(float delta)
	{
		base._Process(delta);
		float str = DayNight.GetRainStr();
		float dir = DayNight.GetWindDirection();
		//((Spatial)GetParent()).GlobalRotation = Vector3.Zero;
		if (str > 10)
		{
			material.SetShaderParam("IsRaining", true);
		}
		else
		{
			material.SetShaderParam("IsRaining", false);
		}
		material.SetShaderParam("TextureRot", 180 - dir);
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
	}*/
}

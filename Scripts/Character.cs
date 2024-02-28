using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
public class Character : KinematicBody
{
	[Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

	[Export]
	public int FallAcceleration = 75;

	[Export]
	public int JumpImpulse = 20;

	[Export]
	public int BounceImpulse = 16;

	[Export]
	public int RunSpeed = 20;

	//public Character_Animations anims;

	[Export]
	public float m_HP = 100;

	
	[Export]
	public float m_Stamina = 100;

	[Export]
	public float m_StaminaRegen = 0.5f;

	[Export]
	public float m_RunCost = 1;

	public float startingstaming;

	public bool m_balive = true;

	public Vector3 origposition;

	public bool m_bEnabled = false;

	public CollisionShape collider;

	public Vector3 m_velocity = Vector3.Zero;


    public Vector3 _velocity = Vector3.Zero;

	[Export]
	public DialogueLine[] lines;

	[Export]
	float InventoryWeightOverride = -1;

	public Inventory CharacterInventory;

	public SpotLight NightLight;

	public Vector3 loctomove;

	public Character_Animations anim;

	public override void _Ready()
	{
		CharacterInventory = GetNode<Inventory>("Inventory");
		if (InventoryWeightOverride != -1)
		{
			CharacterInventory.OverrideWeight(InventoryWeightOverride);
		}
		//anims = GetNode<Character_Animations>("Character_Animations");
		collider = GetNode<CollisionShape>("CollisionShape");
		collider.Disabled = false;
		startingstaming = m_Stamina;
		

		GetNode<AudioStreamPlayer3D>("WalkingSound").Play();
		GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused = true;
		//GetNode<AudioStreamPlayer3D>("TiredSound").Play();
		//GetNode<AudioStreamPlayer3D>("TiredSound").StreamPaused = true;
		
		
		anim = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Character_Animations>("AnimationPlayer");
		NightLight = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Spatial>("rig").GetNode<Skeleton>("Skeleton").GetNode<BoneAttachment>("BoneAttachment").GetNode<SpotLight>("NightLight");
		if (this is Player)
			return;
		((Island)GetParent()).RegisterChar(this);
	}
	public Inventory GetCharacterInventory()
	{
		return CharacterInventory;
	}
	public bool HasLines()
	{
		return lines != null;
	}
	public DialogueLine GetLine()
	{
		return lines[0];
	}
	public override void _PhysicsProcess(float delta)
	{
		if (!m_bEnabled)
			return;
			
		if (origposition != Transform.origin)
			MoveTo(origposition);

		if (DayNight.IsDay())
			NightLight.LightEnergy = 0;
		else
			NightLight.LightEnergy = 1;
	}
    public void MoveTo(Vector3 loc)
	{
        if (!m_balive)
			return;
        _velocity = Vector3.Zero;
		var cloc = GlobalTransform.origin;
		var spd = RunSpeed;
		var direction = loc - GlobalTransform.origin;
		
		float dist = new Vector2(loctomove.x, loctomove.z).DistanceTo(new Vector2( GlobalTransform.origin.x, GlobalTransform.origin.z));

		if (dist < 1)
		{
			if (!GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused)
				GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused = true;
			anim.PlayAnimation(E_Animations.Idle);
		}
		else
		{
			direction = direction.Normalized();
			Vector3 lookloc = new Vector3(direction.x, 0, direction.z);
			GetNode<Spatial>("Pivot").LookAt(Translation - lookloc, Vector3.Up);
			if (GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused)
			{
				GetNode<AudioStreamPlayer3D>("WalkingSound").StreamPaused = false;
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 0.7f;
			}
			if (Input.IsActionPressed("Run"))
			{
				
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 0.5f;
				//GetNode<AudioStreamPlayer3D>("WalkingSound").db = 5f;
				anim.PlayAnimation(E_Animations.Walk);
			}
			else
			{
				GetNode<AudioStreamPlayer3D>("WalkingSound").PitchScale = 1f;
				anim.PlayAnimation(E_Animations.Run);
			}
				
		}
        _velocity.x = direction.x * spd;
		_velocity.z = direction.z * spd;
		//_velocity.y = direction.y * spd;
		// Vertical velocity
		_velocity.y -= FallAcceleration * 0.01f;
		// Moving the character
		_velocity = MoveAndSlide(_velocity, Vector3.Up);
	}
	public virtual void Start()
	{
		m_bEnabled = true;
		origposition = Transform.origin;
		//GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",false);
		SetProcess(true);
		SetPhysicsProcess(true);
		GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
	}
	public virtual void Stop()
	{
		m_bEnabled = false;
		//GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",true);
		SetProcess(false);
		SetPhysicsProcess(false);
		GetNode<CollisionShape>("CollisionShape").SetDeferred("disabled",false);
	}
	private void On_Body_Entered(object body)
	{
	}
	public virtual void Damage(float ammount, Character inst)
	{
		m_HP = m_HP - ammount;
		//Push(Mathf.Rad2Deg(GetAngleTo(inst.GlobalPosition)));

		if (m_HP <= 0)
			Kill();
	}
	public bool IsAlive()
	{
		return m_balive;
	}
	public float GetHP()
	{
		return m_HP;
	}
	public float GetStamina()
	{
		return m_Stamina;
	}
	public virtual void Kill()
	{
		//GetNode<Character_Animations>("Character_Animations").ForceAnimation(E_Animations.Die);
		//GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused = true;
		m_balive = false;
		origposition = GlobalTransform.origin;
		CharacterInventory.RemoveAllItems();
		//AppliedForce = AppliedForce * 0;
		Stop();
	}
	public void Push(float degr, float ammount = 500)
	{
		if (!m_balive)
			return;
		_velocity = Vector3.Zero;
		float adjdef = degr - 90;
		//left 
		if (adjdef < 10 && adjdef >= -170)
		{
			_velocity.x -= 1;
		}
		//right  
		if (degr > 80 || degr < -80)
		{
			_velocity.x += 1;
		}
		//up  
		if (degr > 10 && degr < 170)
		{
			_velocity.y -= 1;
		}
		//down    
		if (degr < -10 && degr > -170)
		{
			_velocity.y += 1;
		}
		//Rotation = 0;
		//AddForce(velocity * ammount, velocity * ammount);
		//AppliedForce = m_velocity * ammount;
	}
	private void On_DialogueButton_Button_Down()
	{
		
	}
}







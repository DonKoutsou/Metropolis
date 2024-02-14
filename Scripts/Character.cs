using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
public class Character : KinematicBody
{
	[Export]
	public int Speed { get; set; } = 14; // How fast the player will move (pixels/sec).

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

	NavigationAgent NavAgent;

	public Vector3 m_velocity = Vector3.Zero;

	public RID myrid;

    public Vector3 _velocity = Vector3.Zero;

	[Export]
	public DialogueLine[] lines;

	[Export]
	float InventoryWeightOverride = -1;

	public Inventory CharacterInventory;

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
		if (this is Player)
			return;
		
		NavAgent = GetNode<NavigationAgent>("NavigationAgent");

		myrid  = NavigationServer.AgentCreate();
        RID default_3d_map_rid  = GetWorld().NavigationMap;

        NavigationServer.AgentSetMap(myrid, default_3d_map_rid);
        NavigationServer.AgentSetRadius(myrid, 0.5f);

		((Island)GetParent()).RegisterChar(this);
	}
	public void UpdateMap()
    {
        RID default_3d_map_rid  = GetWorld().NavigationMap;

        Navigation2DServer.AgentSetMap(myrid, default_3d_map_rid);
        Navigation2DServer.AgentSetRadius(myrid, 0.5f);
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
		NavigationServer.AgentSetVelocity(myrid, _velocity);
    	NavigationServer.AgentSetTargetVelocity(myrid, _velocity);
	}
    public void MoveTo(Vector3 loc)
	{
        if (!m_balive)
			return;
        _velocity = Vector3.Zero;
		var cloc = GlobalTransform.origin;
        NavAgent.SetTargetLocation(loc);
        var nextloc = NavAgent.GetNextLocation();
        var newvel = (nextloc - cloc).Normalized() * 10;
		_velocity = newvel;
        LookAtFromPosition(cloc, loc, Vector3.Up);
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







using Godot;
using System;

public class Player : Character
{
    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public int FallAcceleration = 75;

    [Export]
    public int JumpImpulse = 20;

    [Export]
    public int BounceImpulse = 16;

    [Export]
	int RunSpeed = 20;

    [Signal]
    public delegate void Hit();

    Stamina_Bar Stamina_bar = null;

    HP_Bar hp_bar = null;

    DialoguePanel DiagPan;

    Character_Animations anim;

    public override void _Ready()
    {
        base._Ready();
        //Input.MouseMode = Input.MouseModeEnum.Visible;
        Control plUI = GetNode<Control>("PlayerUI");
        hp_bar = plUI.GetNode<HP_Bar>("HP_Bar");
		hp_bar.MaxValue = m_HP;
		Stamina_bar = plUI.GetNode<Stamina_Bar>("Stamina_Bar");
		Stamina_bar.MaxValue = m_Stamina;
		GetNode<AudioStreamPlayer2D>("WalkingSound").Play();
		GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused = true;
		var panels = GetTree().GetNodesInGroup("DialoguePanel");
		DiagPan = (DialoguePanel)panels[0];
		GetNode<AudioStreamPlayer2D>("TiredSound").Play();
		GetNode<AudioStreamPlayer2D>("TiredSound").StreamPaused = true;
        anim = GetNode<Spatial>("Pivot").GetNode<Spatial>("Guy").GetNode<Character_Animations>("AnimationPlayer");
        
    }
    public override void _PhysicsProcess(float delta)
	{
        var spd = Speed;
        var direction = Vector3.Zero;
        double stam = m_Stamina;
		if (Input.IsActionPressed("Move_Right"))
		{
			direction.x += 1;
		}

		if (Input.IsActionPressed("Move_Left"))
		{
			direction.x -= 1;
		}

		if (Input.IsActionPressed("Move_Down"))
		{
			direction.z += 1f;
		}

		if (Input.IsActionPressed("Move_Up"))
		{
            direction.z -= 1f;
		}
        if (Input.IsActionPressed("Run") && m_Stamina > 10 && _velocity != Vector3.Zero)
		{
			spd = RunSpeed;
			m_Stamina = m_Stamina - 1f;
		}
		else if (!Input.IsActionPressed("Run") && m_Stamina < startingstaming)
		{
			m_Stamina = m_Stamina + 0.5f;
				
			if (_velocity == Vector3.Zero )
				m_Stamina = m_Stamina + 1f;

		}
        if (direction == Vector3.Zero)
		{
			if (!GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused)
				GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused = true;
            anim.PlayAnimation(E_Animations.Idle);
		}
		else
		{
            direction = direction.Normalized();
            GetNode<Spatial>("Pivot").LookAt(Translation - direction, Vector3.Up);
			if (GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused)
            {
                GetNode<AudioStreamPlayer2D>("WalkingSound").StreamPaused = false;
                GetNode<AudioStreamPlayer2D>("WalkingSound").PitchScale = 0.7f;
            }
			if (Input.IsActionPressed("Run"))
			{
				GetNode<AudioStreamPlayer2D>("WalkingSound").PitchScale = 1f;
				GetNode<AudioStreamPlayer2D>("WalkingSound").VolumeDb = 5f;
			}
            anim.PlayAnimation(E_Animations.Walk);
		}
        Stamina_bar.Value = m_Stamina;

		if (m_Stamina < startingstaming / 10)
			GetNode<AudioStreamPlayer2D>("TiredSound").StreamPaused = false;
		else
			GetNode<AudioStreamPlayer2D>("TiredSound").StreamPaused = true;

		if (stam != m_Stamina)
			Stamina_bar.ChangeVisibility();
        // Ground velocity
        _velocity.x = direction.x * spd;
        _velocity.z = direction.z * spd;
        // Vertical velocity
        _velocity.y -= FallAcceleration * delta;
        // Moving the character
        _velocity = MoveAndSlide(_velocity, Vector3.Up);
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            anim.PlayAnimation(E_Animations.Jump);
            _velocity.y += JumpImpulse;
        }
        for (int index = 0; index < GetSlideCount(); index++)
        {
            // We check every collision that occurred this frame.
            KinematicCollision collision = GetSlideCollision(index);
            // If we collide with a monster...
            if (collision.Collider is Mob mob && mob.IsInGroup("mob"))
            {
                // ...we check that we are hitting it from above.
                if (Vector3.Up.Dot(collision.Normal) > 0.1f)
                {
                    // If so, we squash it and bounce.
                    mob.Squash();
                    _velocity.y = BounceImpulse;
                }
            }
        }
    }
    private void Die()
    {
        EmitSignal(nameof(Hit));
        QueueFree();
    }

        // We also specified this function name in PascalCase in the editor's connection window
    public void OnMobDetectorBodyEntered(Node body)
    {
        //Die();
    }
    public void OnDoorDetectorBodyEntered(Node body)
    {
        if (body is Wall)
		{
			Wall bod = (Wall)body;
			bod.Touch(this);
		}
    }

        // ...

        
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

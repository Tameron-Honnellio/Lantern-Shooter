using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Player walk speed, exported to editor
	[Export]
	public const float Speed = 15.0f;
	// Player jump velocity, exported to editor
	[Export]
	public const float JumpVelocity = 4.5f;
	// Camera sensitivity, exported to editor
	[Export]
	public const float lookSensitivity = 0.005f;

	// Gravity field for the player character, exported to editor
	[Export]
	public const float gravity = 9.8f;
	// Node3D field for controlling rotations from user input (left and right)
	Node3D head;
	// Camera3D field for controlling rotations from user input (up and down)
	Camera3D camera;
	// Helper field for clamping camera rotation
	Vector3 cameraRot;
	// AnimationPlayer field for playing the shoot animation
	AnimationPlayer shootAnim;
	// RayCast3D field for the lantern's bullet spawnpoint
	RayCast3D bulletSpawn;
	// Field for loading and holding the bullet scene
	PackedScene bulletLoad;
	// Field for instantiating and adding a new bullet to the scene tree
	bullet newBullet;
	// Field to help set global rotation of bullet
	Transform3D bulletRotHelp;

	// Called when node enters scene tree for the first time
    public override void _Ready()
    {
		// Lock mouse, and disable visible cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
		// Set head to Head node in player scene
		head = GetNode<Node3D>("Head");
		// Set camera to Camera3D node in player scene
		camera = GetNode<Camera3D>("Head/Camera3D");
		// Set shootAnim to AnimationPlayer node in lantern scene
		shootAnim = GetNode<AnimationPlayer>("Head/Camera3D/Lantern/AnimationPlayer");
		// Set bulletSpawn to RayCast3D node in the lantern scene
		bulletSpawn = GetNode<RayCast3D>("Head/Camera3D/Lantern/RayCast3D");
		// Store bullet.tscn PackedScene in bulletLoad field
		bulletLoad = (PackedScene)ResourceLoader.Load("res://Resources/Player/bullet.tscn");
    }
	// Called when input is detected
    public override void _Input(InputEvent @event)
	{
		// If mouse motion detected then rotate the camera to match it
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// Rotate head left and right about the y-axis
			head.RotateY(-mouseMotion.Relative.X * lookSensitivity);
			// Rotate camera up and down about the x-axis
			camera.RotateX(-mouseMotion.Relative.Y * lookSensitivity);
			// Clamp camera angle between -40 down and 70 degrees up
			cameraRot = camera.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -40, 70);
			camera.RotationDegrees = cameraRot;
		}

		// Check if player pressed escape
		if (Input.IsActionJustPressed("escape")) {
			// Quit game
			GetTree().Quit();
		}
	}
	// Called every frame
	public override void _PhysicsProcess(double delta)
	{
		// velocity = global velocity of characterbody
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		// Move with respect to the direction the head is facing
		Vector3 direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		// Global velocity = updated velocity
		Velocity = velocity;

		// Check if the player tried to shoot
		if (Input.IsActionJustPressed("shoot")) {
			// If shoot animation is not playing
			if (!shootAnim.IsPlaying()) {
				// Play shoot animation
				shootAnim.Play("Shoot");
				// Instantiate newBullet as type bullet
				newBullet = (bullet)bulletLoad.Instantiate();
				// Set rotation and position of newBullet to global bulletSpawn rotation and position (account for player movement)
				newBullet.Transform = bulletSpawn.GlobalTransform;
				// Add bullet to the parent scene of player (level / world)
				GetParent().AddChild(newBullet);
			}
		}

		// Move characterbody with respect to new velocity
		MoveAndSlide();
	}
}

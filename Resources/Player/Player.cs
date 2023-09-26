using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// player walk speed, exported to editor
	[Export]
	public const float Speed = 15.0f;
	// player jump velocity, exported to editor
	[Export]
	public const float JumpVelocity = 4.5f;
	// camera sensitivity, exported to editor
	[Export]
	public const float lookSensitivity = 0.005f;

	// gravity variable for the player character, exported to editor
	[Export]
	public const float gravity = 9.8f;
	// Node3D variable for controlling rotations from user input (left and right)
	Node3D head;
	// Camera3D variable for controlling rotations from user input (up and down)
	Camera3D camera;
	// helper variable for clamping camera rotation
	Vector3 cameraRot;
	// AnimationPlayer variable for playing the shoot animation
	AnimationPlayer shootAnim;

	// called when node enters scene tree for the first time
    public override void _Ready()
    {
		// lock mouse, and disable visible cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
		// set head to Head node in Godot scene
		head = GetNode<Node3D>("Head");
		// set camera to Camera3D node in Godot scene
		camera = GetNode<Camera3D>("Head/Camera3D");
		// set shootAnim to AnimationPlayer node in Godot scene
		shootAnim = GetNode<AnimationPlayer>("Head/Camera3D/Lantern/AnimationPlayer");
    }
	// called when input is detected
    public override void _Input(InputEvent @event)
	{
		// if mouse motion detected then rotate the camera to match it
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// rotate head left and right about the y-axis
			head.RotateY(-mouseMotion.Relative.X * lookSensitivity);
			// rotate camera up and down about the x-axis
			camera.RotateX(-mouseMotion.Relative.Y * lookSensitivity);
			// clamp camera angle between -40 down and 70 degrees up
			cameraRot = camera.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -40, 70);
			camera.RotationDegrees = cameraRot;
		}
	}
	// called every frame
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
		// Move in the direction the head is facing
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

		// global velocity = updated velocity
		Velocity = velocity;

		// check if the player tried to shoot
		if (Input.IsActionJustPressed("shoot")) {
			// if shoot animation is not playing
			if (!shootAnim.IsPlaying()) {
				// play shoot animation
				shootAnim.Play("Shoot");
			}
		}

		// move characterbody with respect to new velocity
		MoveAndSlide();
	}
}

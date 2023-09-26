using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// player walk speed
	[Export]
	public const float Speed = 15.0f;
	// player jump speed
	[Export]
	public const float JumpVelocity = 4.5f;
	[Export]
	public const float lookSensitivity = 0.005f;

	// gravity variable for the player character
	public float gravity = 9.8f;
	Node3D head;
	// camera variable for controlling rotations from user input
	Camera3D camera;

	// accumulators
	private float _rotationX = 0f;
	private float _rotationY = 0f;

    public override void _Ready()
    {
		// lock mouse, and disable visible cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
		head = GetNode<Node3D>("Head");
		camera = GetNode<Camera3D>("Head/Camera3D");
    }
    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// // modify accumulated mouse rotation
			// _rotationX += mouseMotion.Relative.X * lookSensitivity;
			// _rotationY += mouseMotion.Relative.Y * lookSensitivity;

			// // reset rotation
			// Transform3D transform = Transform;
			// transform.Basis = Basis.Identity;
			// Transform = transform;

			// RotateObjectLocal(Vector3.Up, _rotationX); // first rotate about Y
			// RotateObjectLocal(Vector3.Right, _rotationY); // then rotate about X
			head.RotateY(-mouseMotion.Relative.X * lookSensitivity);
			camera.RotateX(-mouseMotion.Relative.Y * lookSensitivity);
			//camera.Rotation.X = Mathf.Clamp();
			Vector3 cameraRot = camera.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -40, 70);
			camera.RotationDegrees = cameraRot;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
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

		Velocity = velocity;
		MoveAndSlide();
	}
}

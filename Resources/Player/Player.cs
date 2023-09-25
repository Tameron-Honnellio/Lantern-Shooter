using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float lookSensitivity = 0.02f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	Node3D head;
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
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
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

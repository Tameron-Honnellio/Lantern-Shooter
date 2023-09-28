using Godot;
using System;

public partial class bullet : Node3D
{
	// Bullet speed
	[Export]
	const float SPEED = 40f;
	// MeshInstance3D field for handling bullet collision
	MeshInstance3D mesh;
	// RayCast3D field for handling bullet collision
	RayCast3D ray;
	// Vector3 field for setting bullet's movement vector
	Vector3 bulletPosVector;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set mesh to MeshInstance3D in bullet scene
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		// Set ray to RayCast3D in bullet scene
		ray = GetNode<RayCast3D>("RayCast3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Set bulletPosVector to (X = 0, Y = 0, Z = -SPEED)
		// -Z is forward in Godot
		bulletPosVector = new Vector3(0, 0, -SPEED);
		// Update position of bullet with respect to delta time and rotation
		Position += Transform.Basis * bulletPosVector * (float)delta;
		// If bullet collides with something
		if (ray.IsColliding()) {
			// If object ray is colliding with is in the enemy group
			// ray.GetCollider is cast as type bodyPart because the 
				// IsInGroup and Hit methods are not inherited by RayCast3D in C#
			if (((bodyPart)ray.GetCollider()).IsInGroup("enemy")) {
				// The bodypart is hit and signal is emitted
				((bodyPart)ray.GetCollider()).Hit();
			}
			// Delete the bullet
			QueueFree();
		}
	}
	// Called when bullet despawn timer ends
	public void _on_bullet_despawn_timeout() {
		// Delete bullet
		QueueFree();
	}
}

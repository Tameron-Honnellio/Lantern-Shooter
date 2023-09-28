using Godot;
using System;

public partial class skeleton : CharacterBody3D
{
	// public const float Speed = 5.0f;
	// public const float JumpVelocity = 4.5f;
	// Skeleton health
	public int Health = 3;

	// Called when bodyPartHit signal is received from area3D node
	public void _on_area_3d_body_part_hit(int Damage){
		// Reduce skeleton's health
		Health -= Damage;
		// If health reaches zero or below
		if (Health <= 0) {
			// Delete skeleton
			QueueFree();
		}
	}
}

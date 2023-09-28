using Godot;
using System;

public partial class skeleton : CharacterBody3D
{
	// public const float Speed = 5.0f;
	// public const float JumpVelocity = 4.5f;

	// Skeleton health
	public int Health = 3;
	public AnimationPlayer dieAnim;

    public override void _Ready()
    {
        dieAnim = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    // Called when bodyPartHit signal is received from area3D node
    public async void _on_area_3d_body_part_hit(int Damage){
		// Reduce skeleton's health
		Health -= Damage;
		// If health reaches zero or below
		if (Health <= 0) {
			if (!dieAnim.IsPlaying()) {
				dieAnim.Play("Die");
			}		
			// Delete skeleton after one second timer
			await ToSignal(GetTree().CreateTimer(1.0), "timeout");
			QueueFree();
		}
	}
}

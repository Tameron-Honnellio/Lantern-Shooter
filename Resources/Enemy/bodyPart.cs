using Godot;
using System;

public partial class bodyPart : Area3D
{
	// Damage dealt to skeleton for hitting this body part
	// Exported to editor so different parts take more damage 
	// (i.e. headshot is 3 damage, body is 1 damage)
	[Export]
	public int damage = 1;
	// Create signal for when bodyPartHit
	[Signal]
	public delegate void bodyPartHitEventHandler(int Damage);

	// // Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// }

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	// Emits bodyPartHit signal when called
	public void Hit() {
		EmitSignal(SignalName.bodyPartHit, damage);
	}
}

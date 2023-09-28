using Godot;
using System;

public partial class bodyPart : Area3D
{
	// Damage dealt to skeleton for hitting this body part
	// Exported to editor so different parts take more damage 
	// (i.e. headshot is 3 damage, body is 1 damage)
	[Export]
	public int damage = 1;
	// Score value for hitting this body part
	// Exported to editor so different parts have different scores
	// (i.e. headshot is 30 points, body is 5 points)
	[Export]
	public int score;
	// Create signal for when bodyPartHit
	[Signal]
	public delegate void bodyPartHitEventHandler(int Damage);
	private CustomSignals scoreSignal;

    public override void _Ready()
    {
		// Gain access to global custom signal bus
        scoreSignal = GetNode<CustomSignals>("/root/CustomSignals");
    }

    // Emits bodyPartHit signal when called
    public void Hit() {
		EmitSignal(SignalName.bodyPartHit, damage);
		// Emits updateScore signal
		scoreSignal.EmitSignal(CustomSignals.SignalName.updateScore, score);
	}
}

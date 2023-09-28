using Godot;
using System;

public partial class CustomSignals : Node
{
	// Global custom signal bus
	[Signal]
	public delegate void updateScoreEventHandler(int scoreVal);
}

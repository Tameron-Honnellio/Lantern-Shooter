using Godot;
using System;

public partial class UI : Control
{
	// Field to keep track of and save player score
	public int playerScore = 0;
	// Field to set label text
	public String labelText;
	// Label field to update score text on UI
	public Label scorelabel;
	private CustomSignals scoreSignal;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Gain access to global custom signal bus
		scorelabel = GetNode<Label>("ScoreLabel");
		scoreSignal = GetNode<CustomSignals>("/root/CustomSignals");
		// Handle updateScore signal in updatePlayerScore()
		scoreSignal.updateScore += updatePlayerScore;
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Change text in scoreLabel to match playerScore
		labelText = "Score: " + GD.VarToStr(playerScore);
		scorelabel.Text = labelText;
	}

	public void updatePlayerScore(int scoreVal) {
		// Increase player score
		playerScore += scoreVal;
	}

	public double Save() {
		return (double)playerScore;
	}
}

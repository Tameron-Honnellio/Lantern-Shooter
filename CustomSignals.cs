using Godot;
using System;

public partial class CustomSignals : Node
{
	// Global custom signal bus
	[Signal]
	public delegate void updateScoreEventHandler(int scoreVal);
	[Signal]
	public delegate void playerWinsEventHandler(bool pWins);
	private bool gameWon;

	public void setWon(bool iswon){
		gameWon = iswon;
	}
	public bool isWon(){
		return gameWon;
	}
}

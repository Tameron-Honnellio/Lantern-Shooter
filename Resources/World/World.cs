using Godot;
using System;

public partial class world : Node
{
	private saverandloader saver;
	private CustomSignals playerWon;
	private PackedScene gameOver;
	private bool win = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameOver = (PackedScene)ResourceLoader.Load("res://Resources/World/gameovermenu.tscn");
		playerWon = GetNode<CustomSignals>("/root/CustomSignals");
		saver = GetNode<saverandloader>("/root/Saverandloader");
		// saver.SaveGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("endgame")) {
			playerWon.setWon(false);
			Input.MouseMode = Input.MouseModeEnum.Visible;
			// Save game when game ends, before transition to gameover
			saver.SaveGame();
			GetTree().ChangeSceneToPacked(gameOver);
		}
	}
}

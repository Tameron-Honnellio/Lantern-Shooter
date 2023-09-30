using Godot;
using System;

public partial class level_02 : Node3D
{
	PackedScene gameOverMenu;
	private CustomSignals playerWon;
	private saverandloader saver;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load game over menu packed scene
		gameOverMenu = (PackedScene)ResourceLoader.Load("res://Resources/World/gameovermenu.tscn");
		// Load autoloads into saver and playerWon
		playerWon = GetNode<CustomSignals>("/root/CustomSignals");
		saver = GetNode<saverandloader>("/root/Saverandloader");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// If no targets left in level
		if (!GetTree().HasGroup("target")) {
			// Save game
			saver.SaveGame();
			// Unlock mouse
			Input.MouseMode = Input.MouseModeEnum.Visible;
			// Set win to true
			playerWon.setWon(true);
			// Change scene to game over menu
			GetTree().ChangeSceneToPacked(gameOverMenu);
		}
	}
}

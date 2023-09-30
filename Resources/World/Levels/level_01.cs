using Godot;
using System;

public partial class level_01 : Node3D
{
	// Packed scene to load level 2
	PackedScene levelTwo;
	// Saverandloader to save game
	private saverandloader saver;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load packed scenes
		levelTwo = (PackedScene)ResourceLoader.Load("res://Resources/World/Levels/level_02.tscn");
		saver = GetNode<saverandloader>("/root/Saverandloader");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// If the current scene does not have any children of group target (no skeletons are left in the level)
		if (!GetTree().HasGroup("target")) {
			// Save game
			saver.SaveGame();
			// Change scene to level 2
			GetTree().ChangeSceneToPacked(levelTwo);
		}
	}
}

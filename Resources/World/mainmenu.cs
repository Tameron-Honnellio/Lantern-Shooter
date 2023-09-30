using Godot;
using System;

public partial class mainmenu : Control
{
	// Create highscore menu packed scene for menu transition
	private PackedScene highScoreMenu;
	// Create world packed scene for menu transition
	private PackedScene levelOne;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		highScoreMenu = (PackedScene)ResourceLoader.Load("res://Resources/World/highscoremenu.tscn");
		levelOne = (PackedScene)ResourceLoader.Load("res://Resources/World/Levels/level_01.tscn");
	}

	// If play pressed change scene to level one
	public void _on_play_pressed() {
		GetTree().ChangeSceneToPacked(levelOne);
	}
	// If highscores pressed change scene to high score menu
	public void _on_highscores_pressed() {
		GetTree().ChangeSceneToPacked(highScoreMenu);
	}
	// If quit button pressed quit game
	public void _on_quit_pressed() {
		GetTree().Quit();
	}
}

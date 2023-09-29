using Godot;
using System;

public partial class mainmenu : Control
{
	// Create highscore menu packed scene for menu transition
	private PackedScene highScoreMenu;
	// Create world packed scene for menu transition
	private PackedScene world;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		highScoreMenu = (PackedScene)ResourceLoader.Load("res://Resources/World/highscoremenu.tscn");
		world = (PackedScene)ResourceLoader.Load("res://Resources/World/world.tscn");
	}

	public void _on_play_pressed() {
		GetTree().ChangeSceneToPacked(world);
	}
	public void _on_highscores_pressed() {
		GetTree().ChangeSceneToPacked(highScoreMenu);
	}
	public void _on_quit_pressed() {
		GetTree().Quit();
	}
}

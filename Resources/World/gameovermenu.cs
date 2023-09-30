using Godot;
using System;

public partial class gameovermenu : Control
{
	// Labels to set and display different text depending on if player won
	private Label winText;
	private Label loseText;
	private CustomSignals playerWon;
	private CustomSignals globalScore;
	private PackedScene mainmenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load main menu packed scene for scene transition
		mainmenu = (PackedScene)ResourceLoader.Load("res://Resources/World/mainmenu.tscn");
		// Get custom signals to determine if playerWon, or value of globalScore
		playerWon = GetNode<CustomSignals>("/root/CustomSignals");
		globalScore = GetNode<CustomSignals>("/root/CustomSignals");
		// Load label text from gameovermenu scene
		winText = GetNode<Label>("CenterContainer/VBoxContainer/winText");
		loseText = GetNode<Label>("CenterContainer/VBoxContainer/loseText");
		// Make win/lose text invisible
		winText.Visible = false;
		loseText.Visible = false;
		// Display win or lose text depending on playerWon == true
		displayText();
	}

	// Display game over text
	public void displayText() {
		// if player won
		if (playerWon.isWon()) {
			// Add player score to label text
			winText.Text += globalScore.getScore().ToString();
			// Make win text visible
	 		winText.Visible = true;
		// Otherwise, make lose text visible
		} else if (!playerWon.isWon()){
	 		loseText.Visible = true;
		}
	}
	// On button pressed reset global score and change scene to main menu
	public void _on_main_menu_pressed() {
		globalScore.setScore(0);
		GetTree().ChangeSceneToPacked(mainmenu);
	}
	// Quit game if button pressed
	public void _on_quit_pressed() {
		GetTree().Quit();
	}
}

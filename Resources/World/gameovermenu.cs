using Godot;
using System;

public partial class gameovermenu : Control
{
	private Label winText;
	private Label loseText;
	private CustomSignals playerWon;
	private PackedScene mainmenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainmenu = (PackedScene)ResourceLoader.Load("res://Resources/World/mainmenu.tscn");
		playerWon = GetNode<CustomSignals>("/root/CustomSignals");
		winText = GetNode<Label>("CenterContainer/VBoxContainer/winText");
		loseText = GetNode<Label>("CenterContainer/VBoxContainer/loseText");
		winText.Visible = false;
		loseText.Visible = false;
		displayText();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void displayText() {
		if (playerWon.isWon()) {
	 		winText.Visible = true;
		} else if (!playerWon.isWon()){
	 		loseText.Visible = true;
		}
	}

	public void _on_main_menu_pressed() {
		GetTree().ChangeSceneToPacked(mainmenu);
	}
	public void _on_quit_pressed() {
		GetTree().Quit();
	}
}

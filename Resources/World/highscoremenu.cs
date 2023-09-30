using Godot;
using System;
using System.Collections.Generic;

public partial class highscoremenu : Control
{
	// Max capacity of list container
	private static int capacity = 5;
	// Helper variable to display numbered list
	private static int number;
	// Main menu packed scene
	private PackedScene mainMenu;
	// List to store and sort save data (highscores)
	private List<int> scoreList = new List<int>(capacity);
	// Label to dispay highscores
	private Label scores;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenu = (PackedScene)ResourceLoader.Load("res://Resources/World/mainmenu.tscn");
		scores = GetNode<Label>("CenterContainer/VBoxContainer/highscoreText");
		// Load highscore list from save file
		Loadfromfile();
		// set label text
		scores.Text += "HIGHSCORES:\n";
		for(int j = 0; j < capacity; j++) {
			number = j + 1;
			scores.Text += (number.ToString() + ".) " + scoreList[j].ToString() + "\n");
		}
	}

	// On button pressed, change scene to main menu
	public void _on_main_menu_pressed() {
		GetTree().ChangeSceneToPacked(mainMenu);
	}

	// Load data from file into list, and sort
	public void Loadfromfile(){
		// If no savedata exists, return
		if (!FileAccess.FileExists("user://savegame.save")){
			GD.Print("Save File Error: Cannot find 'savegame.save'");
			return;
		}
		// Using var savegame, this will free and close the file when the variable goes out of scope
		// Open file for reading
		using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
		// While there is data to read
		while (saveGame.GetPosition() < saveGame.GetLength()) {
			// Get line from file
			var jsonString = saveGame.GetLine();
			var json = new Json();
			// Parse line
			var parseResult = json.Parse(jsonString);
			// If parse was unsuccessful, print error
			if (parseResult != Error.Ok) {
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
			}
			// Add data from file to list
			scoreList.Add((int)json.Data);
		}
		// While list.size < capacity
		while (scoreList.Count < capacity) {
			// Fill empty space with 0
			scoreList.Add(0);
		}
		// Sort list
		scoreList.Sort();
		// Reverse for descending order
		scoreList.Reverse();
	}
}

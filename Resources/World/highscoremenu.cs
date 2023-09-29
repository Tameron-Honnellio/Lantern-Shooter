using Godot;
using System;
using System.Collections.Generic;

public partial class highscoremenu : Control
{
	private static int capacity = 5;
	private static int number;
	private PackedScene mainMenu;
	private List<int> scoreList = new List<int>(capacity);
	private Label scores;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenu = (PackedScene)ResourceLoader.Load("res://Resources/World/mainmenu.tscn");
		scores = GetNode<Label>("CenterContainer/VBoxContainer/highscoreText");
		Loadfromfile();
		// set label text
		scores.Text += "HIGHSCORES:\n";
		for(int j = 0; j < capacity; j++) {
			number = j + 1;
			scores.Text += (number.ToString() + ".) " + scoreList[j].ToString() + "\n");
		}
	}

	public override void _Process(double delta)
	{
	}

	public void _on_main_menu_pressed() {
		GetTree().ChangeSceneToPacked(mainMenu);
	}

	public void Loadfromfile(){
		if (!FileAccess.FileExists("user://savegame.save")){
			return;
		}
		using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
		while (saveGame.GetPosition() < saveGame.GetLength()) {
			var jsonString = saveGame.GetLine();
			var json = new Json();
			var parseResult = json.Parse(jsonString);
			if (parseResult != Error.Ok) {
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
			}
			GD.Print("JSON DATA: ", (int)json.Data);
			scoreList.Add((int)json.Data);
		}
		while (scoreList.Count < capacity) {
			scoreList.Add(0);
		}
		scoreList.Sort();
		scoreList.Reverse();
	}
}

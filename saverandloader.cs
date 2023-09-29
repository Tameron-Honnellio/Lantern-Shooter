using Godot;
using System;

public partial class saverandloader : Node
{
	private FileAccess saveGame;
	private FileAccess CreateSave;
	// Note: This can be called from anywhere inside the tree. This function is
	// path independent.
	// Go through everything in the persist category and ask them to return a
	// dict of relevant variables.
	public void SaveGame()
	{
		// If savefile doesn't exist, create one
		if (!FileAccess.FileExists("user://savegame.save")) {
			createSave();
		}

		// Open save file for writing
		saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.ReadWrite);

		// If the save file couldn't open, print error
		if (saveGame == null) {
			GD.Print("Save file error: ", FileAccess.GetOpenError());
		}

		var saveNodes = GetTree().GetNodesInGroup("Persist");
		foreach (Node saveNode in saveNodes)
		{
			// Check the node is an instanced scene so it can be instanced again during load.
			if (string.IsNullOrEmpty(saveNode.SceneFilePath))
			{
				GD.Print($"persistent node '{saveNode.Name}' is not an instanced scene, skipped");
				continue;
			}

			// Check the node has a save function.
			if (!saveNode.HasMethod("Save"))
			{
				GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
				continue;
			}

			// Call the node's save function.
			var nodeData = saveNode.Call("Save");

			// Json provides a static method to serialized JSON string.
			var jsonString = Json.Stringify(nodeData);
			// Move file cursor to end of file to avoid overwriting data
			saveGame.SeekEnd();
			// Store the save dictionary as a new line in the save file.
			saveGame.StoreLine(jsonString);
			// Write buffer to file and close it
			saveGame.Flush();
		}
	}

	// Create save at .../user/AppData/Godot/[this project]
	public void createSave() {
		CreateSave = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
		CreateSave.Flush();
	}
}

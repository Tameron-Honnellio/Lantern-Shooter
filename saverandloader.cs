using Godot;
using System;

public partial class saverandloader : Node
{
	// Note: This can be called from anywhere inside the tree. This function is
	// path independent.
	// Go through everything in the persist category and ask them to return a
	// dict of relevant variables.
	public void SaveGame()
	{
		using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.ReadWrite);
		if (saveGame == null) {
			GD.Print(FileAccess.GetOpenError());
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

			saveGame.SeekEnd();
			// Store the save dictionary as a new line in the save file.
			saveGame.StoreLine(jsonString);
		}
	}
}

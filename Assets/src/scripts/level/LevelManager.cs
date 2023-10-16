using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public List<TextAsset> levels;

	private Grid<char> grid;

	public void LoadLevel(int levelIndex) {
		// TODO: Clear the current level from the scene
		// [...]

		TextAsset levelToLoad = this.levels[levelIndex];
		LevelReader levelReader = new LevelReader();
		this.grid = levelReader.LoadLevel(levelToLoad.text);

		// TODO: Instantiate the new level on the scene
		// [...]
	}
}

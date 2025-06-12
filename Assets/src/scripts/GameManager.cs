using System.Collections.Generic;
using UnityEngine;

/*
	Game manager saves the state of the game across all the scenes
*/
public class GameManager : MonoBehaviour {
	public List<TextAsset> levels;
	public int LevelToLoad { get; set; }

	// UI related fields
	public readonly float VERTICAL_BUTTON_OFFSET = 10.0f;
}

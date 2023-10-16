using System;
using UnityEngine;

public class LevelReader {
	private static char EMPTY_CELL_CHARACTER = ' ';

	public Grid<char> LoadLevel(string levelText) {
		char[][] characters = this.ToCharacters(levelText);
		Grid<char> grid = this.GenerateGrid(characters);
		return grid;
	}

	// --------------------------------------------------

	private char[][] ToCharacters(string levelText) {
		string[] lines = levelText.Split(Environment.NewLine);

		int height = lines.Length;
		int width = lines[0].Length;

		char[][] characters = new char[height][];
		for(int h = 0; h < height; h++) {
			characters[h] = new char[width];
		}

		for(int h = 0; h < height; h++) {
			for(int w = 0; w < width; w++) {
				characters[h][w] = lines[h][w];
			}
		}

		return characters;
	}

	private Grid<char> GenerateGrid(char[][] characters) {
		Grid<char> grid = new Grid<char>();

		int height = characters.Length;
		int width = characters[0].Length;

		for(int h = 0; h < height; h++) {
			for(int w = 0; w < width; w++) {
				char character = characters[h][w];
				if(character == LevelReader.EMPTY_CELL_CHARACTER) { 
					continue;
				}

				Node<char> node = grid.AddNode(new Vector2(w, h), character);

				Node<char> nodeUp = grid.FindNodeByPosition(new Vector2(w, h - 1));
				if(nodeUp != null) {
					node.Connect(Direction.UP, nodeUp);
				}

				Node<char> nodeLeft = grid.FindNodeByPosition(new Vector2(w - 1, h));
				if(nodeLeft != null) {
					node.Connect(Direction.LEFT, nodeLeft);
				}
			}
		}

		return grid;
	}
}

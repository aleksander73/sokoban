using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelReader {
	private const char CHARACTER_CELL_EMPTY = ' ';
	private const char CHARACTER_CELL_BOX = 'B';
	private const char CHARACTER_CELL_FLOOR = 'F';
	private const char CHARACTER_CELL_PLAYER = 'P';
	private const char CHARACTER_CELL_TARGET = 'T';
	private const char CHARACTER_CELL_WALL = 'W';

	private readonly LevelReaderInput levelReaderInput;

	public LevelReader(LevelReaderInput levelReaderInput) {
		this.levelReaderInput = levelReaderInput;
	}

	public Level LoadLevel(string levelText) {
		char[][] characters = this.ToCharacterArray(levelText);
		Grid<Cell> grid = this.GenerateGrid(characters);

		// Center the grid at the player's position
		Vector2 playerPosition = this.GetPlayerPosition(grid);
		grid.Translate(-playerPosition);

		Level level = new Level(grid);
		return level;
	}

	// --------------------------------------------------

	private char[][] ToCharacterArray(string levelText) {
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

	private Grid<Cell> GenerateGrid(char[][] characters) {
		Grid<Cell> grid = new Grid<Cell>();

		int height = characters.Length;
		int width = characters[0].Length;

		for(int h = 0; h < height; h++) {
			for(int w = 0; w < width; w++) {
				Cell cell = this.CreateCell(characters[h][w]);
				if(cell == null) {
					continue;
				}

				Node<Cell> node = grid.AddNode(new Vector2(w, -h), cell);
				Node<Cell> nodeLeft = grid.FindNodeByPosition(new Vector2(w - 1, h));
				if(nodeLeft != null) {
					node.Connect(Direction.LEFT, nodeLeft);
				}
				Node<Cell> nodeUp = grid.FindNodeByPosition(new Vector2(w, h - 1));
				if(nodeUp != null) {
					node.Connect(Direction.UP, nodeUp);
				}
			}
		}

		return grid;
	}

	private Cell CreateCell(char character) {
		Cell cell = null;
		switch(character) {
			case CHARACTER_CELL_EMPTY: {
				break;
			}
			case CHARACTER_CELL_BOX: {
				cell = new Cell(CellType.FLOOR, this.levelReaderInput.GetFloor());
				cell.SetOccupier(this.levelReaderInput.GetBox());
				break;
			}
			case CHARACTER_CELL_FLOOR: {
				cell = new Cell(CellType.FLOOR, this.levelReaderInput.GetFloor());
				break;
			}
			case CHARACTER_CELL_PLAYER: {
				cell = new Cell(CellType.FLOOR, this.levelReaderInput.GetFloor());
				cell.SetOccupier(this.levelReaderInput.GetPlayer());
				break;
			}
			case CHARACTER_CELL_TARGET: {
				cell = new Cell(CellType.TARGET, this.levelReaderInput.GetTarget());
				break;
			}
			case CHARACTER_CELL_WALL: {
				cell = new Cell(CellType.WALL, this.levelReaderInput.GetWall());
				break;
			}
			default: {
				throw new Exception(string.Format("Unknown grid cell type: {0}", character));
			}
		}
		return cell;
	}

	private Vector2 GetPlayerPosition(Grid<Cell> grid) {
		Vector2 playerPosition = new Vector2();

		List<Node<Cell>> nodes = grid.GetNodes();
		for(int i = 0; i < nodes.Count; i++) {
			GameObject occupier = nodes[i].GetValue().GetOccupier();
			if(occupier != null) {
				if(occupier.CompareTag("Player")) {
					playerPosition = nodes[i].GetPosition();
					break;
				}
			}
		}

		return playerPosition;
	}
}

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

	private readonly GameObject floorPrefab;
	private readonly GameObject targetPrefab;
	private readonly GameObject wallPrefab;

	public LevelReader(GameObject floorPrefab, GameObject targetPrefab, GameObject wallPrefab) {
		this.floorPrefab = floorPrefab;
		this.targetPrefab = targetPrefab;
		this.wallPrefab = wallPrefab;
	}

	public Level LoadLevel(string levelText) {
		char[][] characters = this.ToCharacterArray(levelText);
		Level level = this.GenerateLevel(characters);
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

	private Level GenerateLevel(char[][] characters) {
		Grid<Cell> grid = new Grid<Cell>();
		Vector2 player = new Vector2();
		List<Vector2> boxes = new List<Vector2>();

		int height = characters.Length;
		int width = characters[0].Length;

		for(int h = 0; h < height; h++) {
			for(int w = 0; w < width; w++) {
				char character = characters[h][w];

				Cell cell = this.CreateCell(character);
				if(cell == null) {
					continue;
				}

				Vector2 nodePosition = new Vector2(w, -h);

				GridNode<Cell> node = grid.AddNode(nodePosition, cell);
				GridNode<Cell> nodeLeft = grid.FindNodeByPosition(nodePosition + Vector2.left);
				if(nodeLeft != null) {
					node.Connect(Direction.LEFT, nodeLeft);
				}
				GridNode<Cell> nodeUp = grid.FindNodeByPosition(nodePosition + Vector2.up);
				if(nodeUp != null) {
					node.Connect(Direction.UP, nodeUp);
				}

				// --------------------------------------------------

				if(character == CHARACTER_CELL_PLAYER) {
					player = nodePosition;
				} else if(character == CHARACTER_CELL_BOX) {
					boxes.Add(nodePosition);
				}
			}
		}

		Level level = new Level(grid, player, boxes);
		return level;
	}

	private Cell CreateCell(char character) {
		Cell cell = null;
		switch(character) {
			case CHARACTER_CELL_EMPTY: {
				break;
			}
			case CHARACTER_CELL_BOX:
			case CHARACTER_CELL_FLOOR:
			case CHARACTER_CELL_PLAYER: {
				cell = new Cell(CellType.FLOOR, this.floorPrefab);
				break;
			}
			case CHARACTER_CELL_TARGET: {
				cell = new Cell(CellType.TARGET, this.targetPrefab);
				break;
			}
			case CHARACTER_CELL_WALL: {
				cell = new Cell(CellType.WALL, this.wallPrefab);
				break;
			}
			default: {
				throw new Exception(string.Format("Unknown grid cell type: {0}", character));
			}
		}
		return cell;
	}
}

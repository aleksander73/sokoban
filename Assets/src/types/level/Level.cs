using System.Collections.Generic;
using UnityEngine;

public class Level {
	private readonly Grid<Cell> grid;
	private readonly Vector2 player;
	private readonly List<Vector2> boxes;

	public Level(Grid<Cell> grid, Vector2 player, List<Vector2> boxes) {
		this.grid = grid;
		this.player = player;
		this.boxes = boxes;
	}

	public Grid<Cell> GetGrid() {
		return this.grid;
	}

	public Vector2 GetPlayer() {
		return this.player;
	}

	public List<Vector2> GetBoxes() {
		return this.boxes;
	}
}

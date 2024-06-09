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

	public Rect GetBoundingRect() {
		float minX = 0, maxX = 0, minY = 0, maxY = 0;

		List<GridNode<Cell>> nodes = this.grid.GetNodes();
        foreach(GridNode<Cell> node in nodes) {
			Vector2 nodePos = node.GetPosition();
			if(nodePos.x < minX) {
				minX = nodePos.x;
			} else if(nodePos.x > maxX) {
				maxX = nodePos.x;
			}
			if(nodePos.y < minY) {
				minY = nodePos.y;
			} else if(nodePos.y > maxY) {
				maxY = nodePos.y;
			}
        }

		Rect boundingRect = new Rect(new Vector2(minX, minY), new Vector2(maxX - minX, maxY - minY));
		return boundingRect;
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

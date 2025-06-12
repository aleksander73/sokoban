using System.Collections.Generic;
using UnityEngine;

public class Grid<T> {
	private readonly List<GridNode<T>> nodes = new List<GridNode<T>>();

	public List<GridNode<T>> GetNodes() {
		return this.nodes;
	}

	public GridNode<T> FindNodeByPosition(Vector2 position) {
		return this.nodes.Find(node => node.GetPosition() == position);
	}

	public GridNode<T> AddNode(Vector2 position, T value) {
		GridNode<T> node = new GridNode<T>(position, value);
		this.nodes.Add(node);
		return node;
	}
}

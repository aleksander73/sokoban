using System.Collections.Generic;
using UnityEngine;

public class Grid<T> {
	private readonly List<GridNode<T>> nodes;

	public Grid() {
		this.nodes = new List<GridNode<T>>();
	}

	public List<GridNode<T>> GetNodes() {
		return this.nodes;
	}

	public GridNode<T> FindNodeByPosition(Vector2 position) {
		GridNode<T> foundNode = null;
		for(int i = 0; i < this.nodes.Count; i++) {
			GridNode<T> node = this.nodes[i];
			if(node.GetPosition() == position) {
				foundNode = node;
				break;
			}
		}
		return foundNode;
	}

	public GridNode<T> AddNode(Vector2 position, T value) {
		GridNode<T> node = new GridNode<T>(position, value);
		this.nodes.Add(node);
		return node;
	}
}

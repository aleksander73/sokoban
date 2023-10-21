using System.Collections.Generic;
using UnityEngine;

public class Grid<T> {
	private readonly List<Node<T>> nodes;

	public Grid() {
		this.nodes = new List<Node<T>>();
	}

	public List<Node<T>> GetNodes() {
		return this.nodes;
	}

	public Node<T> FindNodeByPosition(Vector2 position) {
		Node<T> foundNode = null;
		for(int i = 0; i < this.nodes.Count; i++) {
			Node<T> node = this.nodes[i];
			if(node.GetPosition() == position) {
				foundNode = node;
				break;
			}
		}
		return foundNode;
	}

	public Node<T> AddNode(Vector2 position, T value) {
		Node<T> node = new Node<T>(position, value);
		this.nodes.Add(node);
		return node;
	}
}

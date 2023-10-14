using System.Collections.Generic;

public class Grid<T> {
	private List<Node<T>> nodes;

	public Grid() {
		this.nodes = new List<Node<T>>();
	}

	public Node<T> AddNode(T value) {
		Node<T> node = new Node<T>(value);
		this.nodes.Add(node);
		return node;
	}

	public List<Node<T>> GetNodes() {
		return this.nodes;
	}
}

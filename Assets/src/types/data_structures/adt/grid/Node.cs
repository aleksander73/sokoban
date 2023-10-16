using UnityEngine;

public class Node<T> {
	private Vector2 position;
	private T value;
	private Node<T> up, right, down, left;

	public Node(Vector2 position, T value) {
		this.position = position;
		this.value = value;
	}

	public void Connect(Direction direction, Node<T> node) {
		switch(direction) {
			case Direction.UP: {
				this.SetUp(node);
				node.SetDown(this);
				break;
			}
			case Direction.RIGHT: {
				this.SetRight(node);
				node.SetLeft(this);
				break;
			}
			case Direction.DOWN: {
				this.SetDown(node);
				node.SetUp(this);
				break;
			}
			case Direction.LEFT: {
				this.SetLeft(node);
				node.SetRight(this);
				break;
			}
		}
	}

	// --------------------------------------------------

	public Vector2 GetPosition() {
		return this.position;
	}

	public T GetValue() {
		return this.value;
	}

	public void SetValue(T value) {
		this.value = value;
	}

	public Node<T> GetUp() {
		return this.up;
	}

	public void SetUp(Node<T> up) {
		this.up = up;
	}

	public Node<T> GetRight() {
		return this.right;
	}

	public void SetRight(Node<T> right) {
		this.right = right;
	}

	public Node<T> GetDown() {
		return this.down;
	}

	public void SetDown(Node<T> down) {
		this.down = down;
	}

	public Node<T> GetLeft() {
		return this.left;
	}

	public void SetLeft(Node<T> left) {
		this.left = left;
	}
}

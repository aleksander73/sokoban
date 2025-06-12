using UnityEngine;

public class GridNode<T> {
	private Vector2 position;
	private T value;
	private GridNode<T> up, right, down, left;

	public GridNode(Vector2 position, T value) {
		this.position = position;
		this.value = value;
	}

	public void Connect(Direction direction, GridNode<T> node) {
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

	public void SetPosition(Vector2 position) {
		this.position = position;
	}

	public T GetValue() {
		return this.value;
	}

	public void SetValue(T value) {
		this.value = value;
	}

	public GridNode<T> GetUp() {
		return this.up;
	}

	public void SetUp(GridNode<T> up) {
		this.up = up;
	}

	public GridNode<T> GetRight() {
		return this.right;
	}

	public void SetRight(GridNode<T> right) {
		this.right = right;
	}

	public GridNode<T> GetDown() {
		return this.down;
	}

	public void SetDown(GridNode<T> down) {
		this.down = down;
	}

	public GridNode<T> GetLeft() {
		return this.left;
	}

	public void SetLeft(GridNode<T> left) {
		this.left = left;
	}
}

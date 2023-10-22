using UnityEngine;

public class Player : MonoBehaviour {
	private LevelManager levelManager;
	private GameObject[] boxes;

	private void Start() {
		GameObject camera = GameObject.Find("camera");
		camera.transform.position = this.transform.position;
		camera.transform.SetParent(this.transform);

		GameObject levelManager = GameObject.Find("level_manager");
		this.levelManager = levelManager.GetComponent<LevelManager>();

		this.boxes = GameObject.FindGameObjectsWithTag("Box");
	}

	private void Update() {
		Direction direction = Direction.NONE;
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			direction = Direction.UP;
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			direction = Direction.RIGHT;
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			direction = Direction.DOWN;
		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			direction = Direction.LEFT;
		}

		// No arrows have been pressed
		if(direction == Direction.NONE) {
			return;
		}

		// --------------------------------------------------

		Node<Cell> nodeBelow = this.levelManager.GetGrid().FindNodeByPosition(this.transform.position);
		Node<Cell> targetNode = this.GetNodeInDirection(nodeBelow, direction);
		if(targetNode == null) {
			return;
		}

		// --------------------------------------------------

		if(this.NodeIsEmpty(targetNode)) {
			this.Move(this.gameObject, targetNode.GetPosition());
		} else {
			if(targetNode.GetValue().GetCellType() != CellType.WALL) {
				Node<Cell> nextTarget = this.GetNodeInDirection(targetNode, direction);
				if(nextTarget == null) {
					return;
				}

				if(this.NodeIsEmpty(nextTarget)) {
					GameObject box = this.GetBoxAtPosition(targetNode.GetPosition());
					this.Move(box, nextTarget.GetPosition());

					box.GetComponent<Box>().OnPositionChanged();
					this.levelManager.CheckForLevelComplete();
				}
			}
		}
	}

	private Node<Cell> GetNodeInDirection(Node<Cell> origin, Direction direction) {
		Node<Cell> node = null;

		if(direction == Direction.UP) {
			node = origin.GetUp();
		} else if(direction == Direction.RIGHT) {
			node = origin.GetRight();
		} else if(direction == Direction.DOWN) {
			node = origin.GetDown();
		} else if(direction == Direction.LEFT) {
			node = origin.GetLeft();
		}

		return node;
	}

	private bool NodeIsEmpty(Node<Cell> node) {
		return node.GetValue().GetCellType() != CellType.WALL && this.GetBoxAtPosition(node.GetPosition()) == null;
	}

	private GameObject GetBoxAtPosition(Vector2 position) {
		GameObject foundBox = null;
		foreach(var box in this.boxes) {
			Vector2 boxPosition = box.transform.position;
			if(boxPosition == position) {
				foundBox = box;
				break;
			}
		}
		return foundBox;
	}

	private void Move(GameObject go, Vector2 target) {
		float z = go.transform.position.z;
		go.transform.position = new Vector3(target.x, target.y, z);
	}
}

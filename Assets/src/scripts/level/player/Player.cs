using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private LevelManager levelManager;
	private bool movementDisabled;

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.levelManager = levelManager.GetComponent<LevelManager>();
		this.movementDisabled = false;
	}

	private void Update() {
		if(!this.movementDisabled) {
			this.HandleMovement();
		}
	}

	private void HandleMovement() {
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

		GridNode<Cell> nodeBelow = this.levelManager.GetGrid().FindNodeByPosition(this.transform.position);
		GridNode<Cell> destNode = this.GetNodeInDirection(nodeBelow, direction);
		if(destNode == null) {
			// No place to move the player to
			return;
		}

		// --------------------------------------------------

		if(this.NodeIsEmpty(destNode)) {
			this.Move(this.gameObject, destNode.GetPosition());
		} else {
			if(destNode.GetValue().GetCellType() == CellType.WALL) {
				// Wall ahead of the player
				return;
			}

			// Box ahead of the player
			GridNode<Cell> nextDestNode = this.GetNodeInDirection(destNode, direction);
			if(nextDestNode == null || !this.NodeIsEmpty(nextDestNode)) {
				// No place to move the box to
				return;
			}

			GameObject box = this.GetBoxAtPosition(destNode.GetPosition());
			this.Move(box, nextDestNode.GetPosition());
			box.GetComponent<Box>().OnPositionChanged();
			this.Move(this.gameObject, destNode.GetPosition());
			if(nextDestNode.GetValue().GetCellType() == CellType.TARGET) {
				// If the box was moved onto a target cell, check for level complete
				this.levelManager.CheckForLevelComplete();
			}
		}
	}

	private GridNode<Cell> GetNodeInDirection(GridNode<Cell> origin, Direction direction) {
		GridNode<Cell> node = null;

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

	private bool NodeIsEmpty(GridNode<Cell> node) {
		return node.GetValue().GetCellType() != CellType.WALL && this.GetBoxAtPosition(node.GetPosition()) == null;
	}

	private GameObject GetBoxAtPosition(Vector2 position) {
		List<GameObject> boxes = levelManager.GetBoxes();

		GameObject foundBox = null;
		foreach(var box in boxes) {
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

	public void SetMovementDisabled(bool movementDisabled) {
		this.movementDisabled = movementDisabled;
	}
}

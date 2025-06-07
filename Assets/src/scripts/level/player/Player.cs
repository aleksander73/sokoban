using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private LevelManager levelManager;
	private bool windowEnabled;

	private List<Transition> transitions;

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.levelManager = levelManager.GetComponent<LevelManager>();
		this.windowEnabled = false;
		this.transitions = new List<Transition>();
	}

	private void Update() {
		// Update or remove transitions
		List<Transition> transitionsToRemove = new List<Transition>();
		this.transitions.ForEach(transition => {
			if(transition.InProgres()) {
				transition.Update();
			} else {
				transitionsToRemove.Add(transition);
			}
		});
		transitionsToRemove.ForEach(transition => transitions.Remove(transition));

		bool activeTransitions = transitions.Count != 0;
		if(!this.windowEnabled && !activeTransitions) {
			// Handle user input
			this.HandleMovement();
		}
	}

	private void HandleMovement() {
		Direction direction = Direction.NONE;
		if(Input.GetKey(KeyCode.UpArrow)) {
			direction = Direction.UP;
		} else if(Input.GetKey(KeyCode.RightArrow)) {
			direction = Direction.RIGHT;
		} else if(Input.GetKey(KeyCode.DownArrow)) {
			direction = Direction.DOWN;
		} else if(Input.GetKey(KeyCode.LeftArrow)) {
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

		Vector3 playerOccupyPosition = this.ToOccupyPosition(destNode);
		float freeWalkDuration = 0.2f;

		if(this.NodeIsEmpty(destNode)) {
			this.Move(this.gameObject, playerOccupyPosition, freeWalkDuration, () => {});
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

			GameObject boxGO = this.GetBoxAtPosition(destNode.GetPosition());
			Box box = boxGO.GetComponent<Box>();
			float moveDuration = box.GetBoxMovingLength();

			this.Move(this.gameObject, playerOccupyPosition, moveDuration, () => {});
			Vector3 boxOccupyPosition = this.ToOccupyPosition(nextDestNode);
			box.GetComponent<Box>().OnMovingStarted();
			this.Move(boxGO, boxOccupyPosition, moveDuration, new Action(() => {
				box.GetComponent<Box>().OnMovingEnded();
				if(nextDestNode.GetValue().GetCellType() == CellType.TARGET) {
					// If the box was moved onto a target cell, check for level complete
					this.levelManager.CheckForLevelComplete();
				}
			}));
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
		foreach(GameObject box in boxes) {
			Vector2 boxPosition = box.transform.position;
			if(boxPosition == position) {
				foundBox = box;
				break;
			}
		}
		return foundBox;
	}

	private Vector3 ToOccupyPosition(GridNode<Cell> node) {
		Vector3 upperPosition = node.GetPosition();
		upperPosition.z = this.levelManager.GetPlayerLevel();
		return upperPosition;
	}

	private void Move(GameObject go, Vector3 target, float duration, Action onMoveFinished) {
		Transition transition = new Transition(go, target, duration, onMoveFinished);
		transition.Start();
		this.transitions.Add(transition);
	}

	public void SetWIndowEnabled(bool windowEnabled) {
		this.windowEnabled = windowEnabled;
	}

	// ==================================================

	public class Transition {
		private readonly GameObject gameObject;
		private readonly Vector3 origin;
		private readonly Vector3 target;
		private readonly float duration;
		private readonly Action onFinished;
		private float startTime;
		private bool inProgress;

		public Transition(GameObject gameObject, Vector3 target, float duration, Action onFinished) {
			this.gameObject = gameObject;
			this.origin = gameObject.transform.position;
			this.target = target;
			this.duration = duration;
			this.onFinished = onFinished;

			this.inProgress = false;
		}

		public void Start() {
			this.startTime = Time.time;
			this.inProgress = true;
		}

		public void Update() {
			if(!this.inProgress) {
				return;
			}

			if(Time.time > this.startTime + this.duration) {
				this.inProgress = false;
				this.gameObject.transform.position = this.target;
				this.onFinished();
				return;
			}

			float elapsedTime = Time.time - this.startTime;
			Vector3 r = this.target - this.origin;
			Vector3 newPosition = origin + r * (elapsedTime / this.duration);

			this.gameObject.transform.position = newPosition;
		}

		public bool InProgres() {
			return this.inProgress;
		}
	}
}

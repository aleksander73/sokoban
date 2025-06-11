using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private LevelManager levelManager;
	private bool windowOnScreen;

	private List<GameObjectAnimation<Vector3>> positionAnimations;
	private AudioSource playerMovingSfx;

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.levelManager = levelManager.GetComponent<LevelManager>();
		this.positionAnimations = new List<GameObjectAnimation<Vector3>>();
		this.playerMovingSfx = this.GetComponent<AudioSource>();
	}

	private void Update() {
		// Update or remove animations
		List<GameObjectAnimation<Vector3>> positionAnimationsToRemove = new List<GameObjectAnimation<Vector3>>();
		this.positionAnimations.ForEach(animation => {
			animation.Update();
			if(animation.IsFinished()) {
				positionAnimationsToRemove.Add(animation);
			}
		});
		positionAnimationsToRemove.ForEach(pa => this.positionAnimations.Remove(pa));

		bool activeAnimations = positionAnimations.Count != 0;
		if(!this.windowOnScreen && !activeAnimations) {
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

		if(this.NodeIsEmpty(destNode)) {
			this.Move(this.gameObject, playerOccupyPosition, this.playerMovingSfx.clip.length, player => this.OnMovingStarted(true), null);
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
			bool boxMovedOntoTarget = nextDestNode.GetValue().GetCellType() == CellType.TARGET;
			float moveDuration = !boxMovedOntoTarget ? box.GetBoxMoving().length : box.GetBoxOnTarget().length;

			this.Move(this.gameObject, playerOccupyPosition, moveDuration, player => this.OnMovingStarted(false), null);
			this.Move(
				boxGO,
				this.ToOccupyPosition(nextDestNode),
				moveDuration, 
				new Action<GameObject>(gameObject => {
					Box box = gameObject.GetComponent<Box>();
					box.OnMovingStarted(boxMovedOntoTarget);
				}),
				new Action<GameObject>(gameObject => {
					Box box = gameObject.GetComponent<Box>();
					box.OnMovingEnded();
					if(boxMovedOntoTarget) {
						// If the box was moved onto a target cell, check for level complete
						this.levelManager.CheckForLevelComplete();
					}
				})
			);
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
		Vector3 occupyPosition = node.GetPosition();
		occupyPosition.z = this.levelManager.GetPlayerLevel();
		return occupyPosition;
	}

	private void Move(GameObject go, Vector3 target, float duration, Action<GameObject> onMoveStarted, Action<GameObject> onMoveFinished) {
		Animation<Vector3> animation = new Animation<Vector3>(0, duration, Interpolators.GetVector3Linear(go.transform.position, target, duration));
		Action<Vector3, GameObject> applyResult = new Action<Vector3, GameObject>((result, gameObject) => {
			gameObject.transform.position = result;
		});

		GameObjectAnimation<Vector3> positionAnimation = new GameObjectAnimation<Vector3>(go, animation, applyResult);
		positionAnimation.onStarted += onMoveStarted;
		positionAnimation.onFinished += onMoveFinished;
		positionAnimation.Start();
		this.positionAnimations.Add(positionAnimation);
	}

	private void OnMovingStarted(bool playMovingSfx) {
		if(playMovingSfx) {
			this.playerMovingSfx.Play();
		}
	}

	public void SetWindowOnScreen(bool windowOnScreen) {
		this.windowOnScreen = windowOnScreen;
	}
}

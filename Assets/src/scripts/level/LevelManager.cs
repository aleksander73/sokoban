using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public GameObject boxPrefab;
	public GameObject floorPrefab;
	public GameObject playerPrefab;
	public GameObject targetPrefab;
	public GameObject wallPrefab;

	private static readonly float CELL_LEVEL = 0.0f;
	private static readonly float PLAYER_LEVEL = -1.0f;

	// --------------------------------------------------

	private GameManager gameManager;
	private LevelReader levelReader;
	private LevelUI levelUI;

	private Grid<Cell> grid;
	private GameObject player;
	private readonly List<GameObject> boxes = new List<GameObject>();

	private void Start() {
		this.gameManager = GameObject.Find("game_manager").GetComponent<GameManager>();
		this.levelReader = new LevelReader(this.floorPrefab, this.targetPrefab, this.wallPrefab);
		this.levelUI = GameObject.Find("ui").GetComponent<LevelUI>();
	}

	private void Update() {
		GameObject level = GameObject.Find("level");
		if(level == null) {
			this.LoadLevel(gameManager.LevelToLoad);
		}
	}

	public void LoadLevel(int levelIndex) {
		// Clear the level if one already exists
		this.ClearLevel();

		string levelText = gameManager.levels[levelIndex].text;
		Level level = this.levelReader.LoadLevel(levelText);
		this.grid = level.GetGrid();

		this.InstantiateLevel(level);

		// Position the camera at the center of the level
		Vector2 levelCenter = level.GetBoundingRect().center;
		GameObject camera = GameObject.Find("camera");
		camera.transform.position = new Vector3(levelCenter.x, levelCenter.y, PLAYER_LEVEL);
	}

	private void InstantiateLevel(Level level) {
		GameObject levelRoot = new GameObject("level");

		// Grid
		List<GridNode<Cell>> nodes = level.GetGrid().GetNodes();
		for(int i = 0; i < nodes.Count; i++) {
			GridNode<Cell> node = nodes[i];
			GameObject sprite = node.GetValue().GetSprite();

			Vector3 position = new Vector3(node.GetPosition().x, node.GetPosition().y, CELL_LEVEL);
			GameObject spriteInstance = GameObject.Instantiate(sprite, position, Quaternion.identity, levelRoot.transform);
			spriteInstance.name = Utility.SimplifyInstanceName(spriteInstance.name);
		}

		// Player
		Vector3 playerPosition = new Vector3(level.GetPlayer().x, level.GetPlayer().y, PLAYER_LEVEL);
		GameObject playerInstance = GameObject.Instantiate(this.playerPrefab, playerPosition, Quaternion.identity, levelRoot.transform);
		playerInstance.name = Utility.SimplifyInstanceName(playerInstance.name);
		this.player = playerInstance;

		// Boxes
		List<Vector2> boxes = level.GetBoxes();
		boxes.ForEach(box => {
			Vector3 boxPosition = new Vector3(box.x, box.y, PLAYER_LEVEL);
			GameObject boxInstance = GameObject.Instantiate(this.boxPrefab, boxPosition, Quaternion.identity, levelRoot.transform);
			boxInstance.name = Utility.SimplifyInstanceName(boxInstance.name);
			this.boxes.Add(boxInstance);
		});
	}

	public void CheckForLevelComplete() {
		bool levelComplete = this.IsLevelComplete();
		if(levelComplete) {
			this.OnLevelCompleted();
		}
	}

	private bool IsLevelComplete() {
		bool isCompleted = true;
		for(int i = 0; i < this.boxes.Count; i++) {
			GridNode<Cell> nodeBelow = this.grid.FindNodeByPosition(this.boxes[i].transform.position);
			if(nodeBelow.GetValue().GetCellType() != CellType.TARGET) {
				isCompleted = false;
				break;
			}
		};
		return isCompleted;
	}

	private void OnLevelCompleted() {
		// Disable player script
		this.player.GetComponent<Player>().SetMovementDisabled(true);

		levelUI.ToggleLevelCompletedWindow();
	}

	private void ClearLevel() {
		GameObject level = GameObject.Find("level");
		if(level == null) {
			return;
		}

		GameObject camera = GameObject.Find("camera");
		camera.transform.SetParent(null);
		camera.transform.position = new Vector3();

		// Reset the LevelManager state
		this.grid = null;
		this.player = null;
		this.boxes.Clear();

		GameObject.Destroy(level);
	}

	public void SetPlayerActive(bool active) {
		this.player.GetComponent<Player>().SetMovementDisabled(!active);
	}

	public Grid<Cell> GetGrid() {
		return this.grid;
	}

	public List<GameObject> GetBoxes() {
		return this.boxes;
	}
}

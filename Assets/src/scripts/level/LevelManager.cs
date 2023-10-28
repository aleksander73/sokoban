using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public List<TextAsset> levels;

	public GameObject boxPrefab;
	public GameObject floorPrefab;
	public GameObject playerPrefab;
	public GameObject targetPrefab;
	public GameObject wallPrefab;

	private static readonly float CELL_LEVEL = 0.0f;
	private static readonly float PLAYER_LEVEL = -1.0f;

	// --------------------------------------------------

	private LevelReader levelReader;
	private int currentLevelIndex = 0;

	private Grid<Cell> grid;
	private GameObject player;
	private readonly List<GameObject> boxes = new List<GameObject>();

	private void Start() {
		this.levelReader = new LevelReader(this.floorPrefab, this.targetPrefab, this.wallPrefab);
	}

	private void Update() {
		GameObject level = GameObject.Find("level");
		if(level == null) {
			if(this.currentLevelIndex < this.levels.Count) {
				this.LoadLevel(this.currentLevelIndex);
			} else {
				Debug.Log("Congratulations! You beat Sokoban!");
			}
		}
	}

	private void LoadLevel(int levelIndex) {
		string levelText = this.levels[levelIndex].text;
		Level level = this.levelReader.LoadLevel(levelText);
		this.grid = level.GetGrid();

		this.InstantiateLevel(level);
	}

	private void InstantiateLevel(Level level) {
		GameObject levelRoot = new GameObject("level");

		// Grid
		List<Node<Cell>> nodes = level.GetGrid().GetNodes();
		for(int i = 0; i < nodes.Count; i++) {
			Node<Cell> node = nodes[i];
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
			Node<Cell> nodeBelow = this.grid.FindNodeByPosition(this.boxes[i].transform.position);
			if(nodeBelow.GetValue().GetCellType() != CellType.TARGET) {
				isCompleted = false;
				break;
			}
		};
		return isCompleted;
	}

	private void OnLevelCompleted() {
		// Disable player script
		this.player.GetComponent<Player>().enabled = false;
		this.ClearLevel();
		this.currentLevelIndex++;
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

	public Grid<Cell> GetGrid() {
		return this.grid;
	}
}

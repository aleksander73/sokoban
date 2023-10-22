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

	private int currentLevelIndex = 0;

	private Grid<Cell> grid;
	private GameObject player;
	private readonly List<GameObject> boxes = new List<GameObject>();

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
		LevelReaderInput levelReaderInput = new LevelReaderInput(this.floorPrefab, this.targetPrefab, this.wallPrefab);
		LevelReader levelReader = new LevelReader(levelReaderInput);
		TextAsset levelToLoad = this.levels[levelIndex];
		Level level = levelReader.LoadLevel(levelToLoad.text);
		this.grid = level.GetGrid();

		// --------------------------------------------------

		GameObject levelRoot = new GameObject("level");

		// Instantiate grid
		Grid<Cell> grid = level.GetGrid();
		this.InstantiateGrid(levelRoot, grid);

		// Instantiate player
		Vector3 playerPosition = new Vector3(level.GetPlayer().x, level.GetPlayer().y, PLAYER_LEVEL);
		GameObject instance = GameObject.Instantiate(this.playerPrefab, playerPosition, Quaternion.identity, levelRoot.transform);
		instance.name = Utility.SimplifyInstanceName(instance.name);
		this.player = instance;

		// Instantiate boxes
		List<Vector2> boxes = level.GetBoxes();
		boxes.ForEach(box => {
			Vector3 boxPosition = new Vector3(box.x, box.y, PLAYER_LEVEL);
			instance = GameObject.Instantiate(this.boxPrefab, boxPosition, Quaternion.identity, levelRoot.transform);
			instance.name = Utility.SimplifyInstanceName(instance.name);
			this.boxes.Add(instance);
		});
	}

	public void CheckForLevelComplete() {
		bool levelComplete = this.IsLevelComplete();
		if(levelComplete) {
			this.OnLevelCompleted();
		}
	}

	private void OnLevelCompleted() {
		// Disable player script
		this.player.GetComponent<Player>().enabled = false;
		this.ClearLevel();
		this.currentLevelIndex++;
	}

	public void ClearLevel() {
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

	private void InstantiateGrid(GameObject levelRoot, Grid<Cell> grid) {
		List<Node<Cell>> nodes = grid.GetNodes();
		for(int i = 0; i < nodes.Count; i++) {
			Node<Cell> node = nodes[i];
			GameObject sprite = node.GetValue().GetSprite();

			Vector3 position = new Vector3(node.GetPosition().x, node.GetPosition().y, CELL_LEVEL);
			GameObject instance = GameObject.Instantiate(sprite, position, Quaternion.identity, levelRoot.transform);
			instance.name = Utility.SimplifyInstanceName(instance.name);
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

	public Grid<Cell> GetGrid() {
		return this.grid;
	}
}

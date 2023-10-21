using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public List<TextAsset> levels;

	public GameObject box;
	public GameObject floor;
	public GameObject player;
	public GameObject target;
	public GameObject wall;

	private static readonly float CELL_LEVEL = 0.0f;
	private static readonly float PLAYER_LEVEL = -1.0f;

	private Grid<Cell> grid;

	public void LoadLevel(int levelIndex) {
		LevelReaderInput levelReaderInput = new LevelReaderInput(
			this.floor,
			this.target,
			this.wall
		);
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
		GameObject instance = GameObject.Instantiate(this.player, playerPosition, Quaternion.identity, levelRoot.transform);
		instance.name = Utility.SimplifyInstanceName(instance.name);

		// Instantiate boxes
		List<Vector2> boxes = level.GetBoxes();
		boxes.ForEach(box => {
			Vector3 boxPosition = new Vector3(box.x, box.y, PLAYER_LEVEL);
			instance = GameObject.Instantiate(this.box, boxPosition, Quaternion.identity, levelRoot.transform);
			instance.name = Utility.SimplifyInstanceName(instance.name);
		});
	}

	public void ClearLevel() {
		GameObject level = GameObject.Find("level");
		if(level == null) {
			return;
		}

		GameObject camera = GameObject.Find("camera");
		camera.transform.SetParent(null);
		camera.transform.position = new Vector3();

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

	public Grid<Cell> GetGrid() {
		return this.grid;
	}
}

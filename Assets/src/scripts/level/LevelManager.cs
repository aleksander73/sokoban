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
		this.ClearLevel();

		// --------------------------------------------------

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

		// Instantiate grid
		Grid<Cell> grid = level.GetGrid();
		this.InstantiateGrid(grid);

		// Instantiate player
		Vector3 playerPosition = new Vector3(level.GetPlayer().x, level.GetPlayer().y, PLAYER_LEVEL);
		GameObject instance = GameObject.Instantiate(this.player, playerPosition, Quaternion.identity);
		instance.name = Utility.SimplifyInstanceName(instance.name);

		// Instantiate boxes
		List<Vector2> boxes = level.GetBoxes();
		boxes.ForEach(box => {
			Vector3 boxPosition = new Vector3(box.x, box.y, PLAYER_LEVEL);
			instance = GameObject.Instantiate(this.box, boxPosition, Quaternion.identity);
			instance.name = Utility.SimplifyInstanceName(instance.name);
		});
	}

	private void ClearLevel() {

	}

	private void InstantiateGrid(Grid<Cell> grid) {
		GameObject level = new GameObject("level");
		List<Node<Cell>> nodes = grid.GetNodes();
		for(int i = 0; i < nodes.Count; i++) {
			Node<Cell> node = nodes[i];
			GameObject sprite = node.GetValue().GetSprite();

			Vector3 position = new Vector3(node.GetPosition().x, node.GetPosition().y, CELL_LEVEL);
			GameObject instance = GameObject.Instantiate(sprite, position, Quaternion.identity, level.transform);
			instance.name = Utility.SimplifyInstanceName(instance.name);
		}
	}

	public Grid<Cell> GetGrid() {
		return this.grid;
	}
}

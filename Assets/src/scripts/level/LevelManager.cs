using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public List<TextAsset> levels;

	public GameObject box;
	public GameObject floor;
	public GameObject player;
	public GameObject target;
	public GameObject wall;

	private static readonly float FLOOR_LEVEL = 0.0f;
	private static readonly float PLAYER_LEVEL = -1.0f;

	private Level currentLevel;

	public void LoadLevel(int levelIndex) {
		this.ClearLevel();

		LevelReaderInput levelReaderInput = new LevelReaderInput(
			this.box,
			this.floor,
			this.player,
			this.target,
			this.wall
		);
		LevelReader levelReader = new LevelReader(levelReaderInput);
		TextAsset levelToLoad = this.levels[levelIndex];
		this.currentLevel = levelReader.LoadLevel(levelToLoad.text);

		this.InstantiateLevel();
	}

	private void ClearLevel() {

	}

	private void InstantiateLevel() {
		GameObject level = new GameObject("level");
		List<Node<Cell>> nodes = this.currentLevel.GetGrid().GetNodes();
		for(int i = 0; i < nodes.Count; i++) {
			Node<Cell> node = nodes[i];
			Cell cell = node.GetValue();

			Vector3 cellPosition = new Vector3(node.GetPosition().x, node.GetPosition().y, FLOOR_LEVEL);
			GameObject instance = GameObject.Instantiate(cell.GetSprite(), cellPosition, Quaternion.identity, level.transform);
			instance.name = Utility.SimplifyInstanceName(instance.name);

			GameObject occupier = cell.GetOccupier();
			if(occupier != null) {
				Vector3 occupierPosition = new Vector3(node.GetPosition().x, node.GetPosition().y, PLAYER_LEVEL);
				instance = GameObject.Instantiate(occupier, occupierPosition, Quaternion.identity);
				instance.name = Utility.SimplifyInstanceName(instance.name);
			}
		}
	}
}

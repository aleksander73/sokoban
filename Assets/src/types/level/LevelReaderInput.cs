using UnityEngine;

public class LevelReaderInput {
	private readonly GameObject box;
	private readonly GameObject floor;
	private readonly GameObject player;
	private readonly GameObject target;
	private readonly GameObject wall;

	public LevelReaderInput(GameObject box, GameObject floor, GameObject player, GameObject target, GameObject wall) {
		this.box = box;
		this.floor = floor;
		this.player = player;
		this.target = target;
		this.wall = wall;
	}

	public GameObject GetBox() {
		return this.box;
	}

	public GameObject GetFloor() {
		return this.floor;
	}
	public GameObject GetPlayer() {
		return this.player;
	}

	public GameObject GetTarget() {
		return this.target;
	}

	public GameObject GetWall() {
		return this.wall;
	}
}

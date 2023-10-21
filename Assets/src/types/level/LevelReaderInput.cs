using UnityEngine;

public class LevelReaderInput {
	private readonly GameObject floor;
	private readonly GameObject target;
	private readonly GameObject wall;

	public LevelReaderInput(GameObject floor, GameObject target, GameObject wall) {
		this.floor = floor;
		this.target = target;
		this.wall = wall;
	}

	public GameObject GetFloor() {
		return this.floor;
	}

	public GameObject GetTarget() {
		return this.target;
	}

	public GameObject GetWall() {
		return this.wall;
	}
}

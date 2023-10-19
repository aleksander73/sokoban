using UnityEngine;

public class Cell {
	private readonly CellType cellType;
	private readonly GameObject sprite;
	private GameObject occupier;

	public Cell(CellType cellType, GameObject sprite) {
		this.cellType = cellType;
		this.sprite = sprite;
	}

	public CellType GetCellType() {
		return this.cellType;
	}

	public GameObject GetSprite() {
		return this.sprite;
	}

	public GameObject GetOccupier() {
		return this.occupier;
	}

	public void SetOccupier(GameObject occupier) {
		this.occupier = occupier;
	}
}

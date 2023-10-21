using UnityEngine;

public class Cell {
	private readonly CellType cellType;
	private readonly GameObject sprite;

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
}

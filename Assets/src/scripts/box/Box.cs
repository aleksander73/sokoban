using UnityEngine;

public class Box : MonoBehaviour {
	private Grid<Cell> grid;
	private SpriteRenderer sr;
	private Color defaultColor;

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.grid = levelManager.GetComponent<LevelManager>().GetGrid();

		this.sr = this.GetComponent<SpriteRenderer>();
		this.defaultColor = this.sr.color;
	}

	public void OnPositionChanged() {
		Node<Cell> nodeBelow = this.grid.FindNodeByPosition(this.transform.position);
		this.sr.color = nodeBelow.GetValue().GetCellType() == CellType.TARGET ? new Color(1.0f, 1.0f, 1.0f, 0.85f) : this.defaultColor;
	}
}

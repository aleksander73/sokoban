using UnityEngine;

public class Box : MonoBehaviour {
	private Grid<Cell> grid;
	private SpriteRenderer sr;
	private Color defaultColor;
	private Color onTargetColor;
	private AudioSource boxMovingSfx;

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.grid = levelManager.GetComponent<LevelManager>().GetGrid();

		this.sr = this.GetComponent<SpriteRenderer>();
		this.defaultColor = this.sr.color;
		this.onTargetColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

		this.boxMovingSfx = this.GetComponent<AudioSource>();
	}

	public void OnPositionChanged() {
		this.boxMovingSfx.Play();

		GridNode<Cell> nodeBelow = this.grid.FindNodeByPosition(this.transform.position);
		this.sr.color = nodeBelow.GetValue().GetCellType() == CellType.TARGET ? this.onTargetColor : this.defaultColor;
	}
}

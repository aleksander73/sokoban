using UnityEngine;

public class Box : MonoBehaviour {
	private Grid<Cell> grid;
	private SpriteRenderer sr;
	private Color defaultColor;
	private Color onTargetColor;
	public AudioSource boxMovingSfx;
	public AudioSource boxOnTargetSfx;

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.grid = levelManager.GetComponent<LevelManager>().GetGrid();

		this.sr = this.GetComponent<SpriteRenderer>();
		this.defaultColor = this.sr.color;
		this.onTargetColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}

	public void OnMovingStarted(bool boxMovedOntoTarget) {
		this.sr.color = this.defaultColor;
		AudioSource sourceToPlay = !boxMovedOntoTarget ? boxMovingSfx : boxOnTargetSfx;
		sourceToPlay.Play();
	}

	public void OnMovingEnded() {
		GridNode<Cell> nodeBelow = this.grid.FindNodeByPosition(this.transform.position);
		if(nodeBelow.GetValue().GetCellType() == CellType.TARGET) {
			this.sr.color = this.onTargetColor;
		} else {
			this.sr.color = this.defaultColor;
		}
	}

	public AudioClip GetBoxMoving() {
		return this.boxMovingSfx.clip;
	}

	public AudioClip GetBoxOnTarget() {
		return this.boxMovingSfx.clip;
	}
}

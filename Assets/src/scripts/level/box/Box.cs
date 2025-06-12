using UnityEngine;

public class Box : MonoBehaviour {
	public AudioSource boxMovingSfx;
	public AudioSource boxOnTargetSfx;

	private Grid<Cell> grid;
	private SpriteRenderer sr;
	private Color onTargetColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

	private void Start() {
		GameObject levelManager = GameObject.Find("level_manager");
		this.grid = levelManager.GetComponent<LevelManager>().GetGrid();
		this.sr = this.GetComponent<SpriteRenderer>();
	}

	public void OnMovingStarted(bool boxMovedOntoTarget) {
		AudioSource sourceToPlay = !boxMovedOntoTarget ? boxMovingSfx : boxOnTargetSfx;
		sourceToPlay.Play();
	}

	public void OnMovingEnded() {
		GridNode<Cell> nodeBelow = this.grid.FindNodeByPosition(this.transform.position);
		if(nodeBelow.GetValue().GetCellType() == CellType.TARGET) {
			this.sr.color = this.onTargetColor;
		}
	}

	public AudioClip GetBoxMoving() {
		return this.boxMovingSfx.clip;
	}

	public AudioClip GetBoxOnTarget() {
		return this.boxMovingSfx.clip;
	}
}

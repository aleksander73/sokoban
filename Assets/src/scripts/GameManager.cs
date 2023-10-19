using UnityEngine;

public class GameManager : MonoBehaviour {
	public LevelManager levelManager;
	public int levelToLoad;

	public void Start() {
		this.levelManager.LoadLevel(this.levelToLoad);
	}
}

using UnityEngine;

public class GameManager : MonoBehaviour {
	public LevelManager levelManager;

    public void Start() {
		this.levelManager.LoadLevel(0);
    }
}

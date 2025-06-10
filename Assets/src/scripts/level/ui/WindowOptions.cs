using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowOptions : Window {
	private GameManager gameManager;
	private GameObject ui;
	private LevelManager levelManager;

	public Button resumeGameButton, restartLevelButton, mainMenuButton;

	private void Start() {
		this.gameManager = GameObject.Find("game_manager").GetComponent<GameManager>();;
		this.ui = GameObject.Find("ui");
		this.levelManager = GameObject.Find("level_manager").GetComponent<LevelManager>();

        Dictionary<Button, UnityAction> buttonClickHandlers = new Dictionary<Button, UnityAction> {
			{ resumeGameButton, () => this.OnResumeGame() },
			{ restartLevelButton, () => this.OnRestartLevel() },
			{ mainMenuButton, () => this.OnMainMenu() }
		};

        foreach(KeyValuePair<Button, UnityAction> entry in buttonClickHandlers) {
			Button button = entry.Key;
			UnityAction callback = entry.Value;
			button.GetComponent<Button>().onClick.AddListener(callback);
        }
    }

	private void OnResumeGame() {
		this.ui.GetComponent<LevelUI>().ToggleWindow(true, this.gameObject, _ => {});
	}

	private void OnRestartLevel() {
		this.ui.GetComponent<LevelUI>().ToggleWindow(true, this.gameObject, _ => {});

		CameraEffects cameraEffects = Camera.main.GetComponent<CameraEffects>();
		cameraEffects.Blink(overlay => {
			levelManager.LoadLevel(gameManager.LevelToLoad);
		});
	}

	private void OnMainMenu() {
		CameraEffects cameraEffects = Camera.main.GetComponent<CameraEffects>();
		cameraEffects.FadeOut(overlay => {
			SceneManager.LoadScene(Scenes.MAIN_MENU, LoadSceneMode.Single);
		});		
	}
}

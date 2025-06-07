using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsWindow : MonoBehaviour {
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

		// Set the color to `positive color`
		this.gameObject.GetComponent<Image>().color = gameManager.positiveColor;
    }

	private void OnResumeGame() {
		this.ui.GetComponent<LevelUI>().ToggleOptionsWindow();
	}

	private void OnRestartLevel() {
		levelManager.LoadLevel(gameManager.LevelToLoad);
		// Hide the options window
		this.ui.GetComponent<LevelUI>().ToggleOptionsWindow();
	}

	private void OnMainMenu() {
		SceneManager.LoadScene(Scenes.MAIN_MENU, LoadSceneMode.Single);
	}
}

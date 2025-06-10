using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowLevelCompleted : Window {
	private GameManager gameManager;
	private LevelManager levelManager;
	private GameObject ui;

	public Button mainMenuButton;
	public GameObject nextLevelButtonPrefab;

    private void Start() {
		this.gameManager = GameObject.Find("game_manager").GetComponent<GameManager>();
		this.levelManager = GameObject.Find("level_manager").GetComponent<LevelManager>();
		this.ui = GameObject.Find("ui");

		List<Button> visibleButtons = new List<Button>() {
			mainMenuButton
		};

		// ==================================================

		Dictionary<Button, UnityAction> buttonClickHandlers = new Dictionary<Button, UnityAction> {
			{ mainMenuButton, () => this.OnMainMenu() },
		};

		// Show next level button only when not playing the last level
		if(gameManager.LevelToLoad < gameManager.levels.Count - 1) {
			GameObject nextLevelButtonGO = GameObject.Instantiate<GameObject>(nextLevelButtonPrefab, this.gameObject.transform);
			Button nextLevelButton = nextLevelButtonGO.GetComponent<Button>();
			visibleButtons.Insert(0, nextLevelButton);
			buttonClickHandlers.Add(nextLevelButton, () => this.OnNextLevel());
		}

        foreach(KeyValuePair<Button, UnityAction> entry in buttonClickHandlers) {
			Button button = entry.Key;
			UnityAction callback = entry.Value;
			button.GetComponent<Button>().onClick.AddListener(callback);
        }

		// ==================================================

		// Position the buttons in the center of the screen
		float BUTTON_HEIGHT = nextLevelButtonPrefab.GetComponent<RectTransform>().rect.height;
		Vector2 origin = new Vector2(0, ((visibleButtons.Count - 1) * (BUTTON_HEIGHT + gameManager.VERTICAL_BUTTON_OFFSET)) / 2);
		for(int i = 0; i < visibleButtons.Count; i++) {
			Button button = visibleButtons[i];

			RectTransform rectTransform = button.GetComponent<RectTransform>();
			rectTransform.localPosition = origin - new Vector2(0, i * (BUTTON_HEIGHT + gameManager.VERTICAL_BUTTON_OFFSET));
		}
    }

	private void OnNextLevel() {
		CameraEffects cameraEffects = Camera.main.GetComponent<CameraEffects>();
		cameraEffects.Blink(overlay => {
			this.ui.GetComponent<LevelUI>().ToggleWindowLevelCompleted(true, _ => {});
			gameManager.LevelToLoad++;
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

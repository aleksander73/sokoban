using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {
	private LevelManager levelManager;
	private GameObject menu;
	public GameObject menuTogglePrefab;
	private GameObject menuToggle;
	private GameObject windowOptions;
	private GameObject windowLevelCompleted;

    void Start() {
		this.levelManager = GameObject.Find("level_manager").GetComponent<LevelManager>();
		this.menu = GameObject.Find("menu");

		// Position "menu_toggle" button in the upper left corner of the screen
		this.menuToggle = GameObject.Instantiate<GameObject>(this.menuTogglePrefab, this.menu.transform);
		RectTransform menuToggleRectTransform = menuToggle.GetComponent<RectTransform>();

		const float OFFSET = 20f;
		menuToggleRectTransform.localPosition = new Vector2(
			(-Screen.width + menuToggleRectTransform.rect.width) / 2  + OFFSET,
			(Screen.height - menuToggleRectTransform.rect.height) / 2  - OFFSET
		);

		Button menuToggleButton = menuToggle.GetComponent<Button>();
		menuToggleButton.onClick.AddListener(() => {
			this.ToggleWindow(false, windowOptions, _ => {});
		});

		// =========================

		this.windowOptions = GameObject.Find("window_options");
		this.windowLevelCompleted = GameObject.Find("window_level_completed");
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
			this.ToggleWindow(false, windowOptions, _ => {});
		}
    }

	// ==================================================

	public void ToggleWindow(bool immediately, GameObject targetWindow, Action<GameObject> onFinished) {
		GameObject[] windows = new GameObject[] { windowOptions, windowLevelCompleted };
		bool canToggleWindow = windows.All(windowGO => {
			Window window = windowGO.GetComponent<Window>();
			return windowGO == targetWindow || window.GetState() == WindowState.HIDDEN;
		});
		if(!canToggleWindow) {
			return;
		}

		// =========================

		Window window = targetWindow.GetComponent<Window>();
		this.levelManager.SetWindowOnScreen(true); // true because the window always begins transition when toggled
		window.Toggle(immediately, windowGO => {
			onFinished(windowGO);
			this.levelManager.SetWindowOnScreen(window.GetState() != WindowState.HIDDEN);
		});
	}

	public void ToggleWindowLevelCompleted(bool immediately, Action<GameObject> onFinished) {
		this.ToggleWindow(immediately, windowLevelCompleted, onFinished);
	}
}

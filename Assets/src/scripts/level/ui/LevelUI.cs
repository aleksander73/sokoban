using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {
	private LevelManager levelManager;
	public GameObject menuToggle;
	private GameObject optionsWindow;
	private GameObject levelCompletedWindow;

    void Start() {
		this.levelManager = GameObject.Find("level_manager").GetComponent<LevelManager>();

		// Position "menu_toggle" button in the upper left corner of the screen
		GameObject ui = GameObject.Find("ui");
		Rect uiRect = ui.GetComponent<RectTransform>().rect;

		GameObject menuToggleGO = GameObject.Instantiate<GameObject>(this.menuToggle, this.gameObject.transform);
		RectTransform menuToggleRectTransform = menuToggleGO.GetComponent<RectTransform>();

		const float OFFSET = 20f;
		menuToggleRectTransform.localPosition = new Vector2(
			(-uiRect.width + menuToggleRectTransform.rect.width) / 2  + OFFSET,
			(uiRect.height - menuToggleRectTransform.rect.height) / 2  - OFFSET
		);

		Button menuToggleButton = menuToggleGO.GetComponent<Button>();
		menuToggleButton.onClick.AddListener(() => {
			this.ToggleOptionsWindow();
		});

		// =========================

		// Deactivate the options window
		this.optionsWindow = GameObject.Find("options_window");
		this.optionsWindow.SetActive(false);

		// Deactivate the level completed window
		this.levelCompletedWindow = GameObject.Find("level_completed_window");
		this.levelCompletedWindow.SetActive(false);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
			this.ToggleOptionsWindow();
		}
    }

	// ==================================================

	private void OnWindowToggle(GameObject window) {
		this.levelManager.SetWindowEnabled(window.activeSelf);
	}

	public void ToggleOptionsWindow() {
		this.optionsWindow.SetActive(!optionsWindow.activeSelf);
		this.OnWindowToggle(optionsWindow);
	}

	public void ToggleLevelCompletedWindow() {
		this.levelCompletedWindow.SetActive(!levelCompletedWindow.activeSelf);
		this.OnWindowToggle(levelCompletedWindow);
	}
}

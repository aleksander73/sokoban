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
			this.ToggleWindowOptions();
		});

		// =========================

		// Deactivate the options window
		this.windowOptions = GameObject.Find("window_options");
		this.windowOptions.SetActive(false);

		// Deactivate the level completed window
		this.windowLevelCompleted = GameObject.Find("window_level_completed");
		this.windowLevelCompleted.SetActive(false);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
			this.menuToggle.GetComponent<Button>().onClick.Invoke();
		}
    }

	// ==================================================

	private void OnWindowToggle(GameObject window) {
		this.levelManager.SetWindowEnabled(window.activeSelf);
	}

	public void ToggleWindowOptions() {
		this.windowOptions.SetActive(!windowOptions.activeSelf);
		this.OnWindowToggle(windowOptions);
	}

	public void ToggleWindowLevelCompleted() {
		this.windowLevelCompleted.SetActive(!windowLevelCompleted.activeSelf);
		this.OnWindowToggle(windowLevelCompleted);
	}
}

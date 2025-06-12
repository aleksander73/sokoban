using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public Button levelSelectButton;
	public Button quitButton;
	
	private GameManager gameManager;
	private GameObject levelsMenu;


	private void Start() {
		this.gameManager = GameObject.Find("game_manager").GetComponent<GameManager>();
		this.levelsMenu = GameObject.Find("levels_menu");
		this.levelsMenu.SetActive(false);

		Dictionary<Button, UnityAction> buttonClickHandlers = new Dictionary<Button, UnityAction> {
			{ levelSelectButton, () => this.OnLevelSelect() },
			{ quitButton, () => this.OnQuit() }
		};

        foreach(KeyValuePair<Button, UnityAction> entry in buttonClickHandlers) {
			Button button = entry.Key;
			UnityAction callback = entry.Value;
			button.GetComponent<Button>().onClick.AddListener(callback);
        }

		GameObject.DontDestroyOnLoad(gameManager);
    }

	// --------------------------------------------------

	public void OnLevelSelect() {
		CameraEffects cameraEffects = Camera.main.GetComponent<CameraEffects>();
		cameraEffects.Blink(overlay => {
			this.gameObject.SetActive(false);
			levelsMenu.SetActive(true);
		});
	}

	public void OnQuit() {
		Application.Quit();
	}
}

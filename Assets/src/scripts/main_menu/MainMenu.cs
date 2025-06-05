using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	private GameObject gameManager;
	private GameObject levelsMenu;

	public Button levelSelectButton;
	public Button quitButton;

	public void Start() {
		gameManager = GameObject.Find("game_manager");
		levelsMenu = GameObject.Find("levels");
		levelsMenu.SetActive(false);

		 ColorBlock colorBlock = levelSelectButton.colors;
		 colorBlock.highlightedColor = gameManager.GetComponent<GameManager>().positiveColor;
		 levelSelectButton.colors = colorBlock;

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
		this.gameObject.SetActive(false);
		levelsMenu.SetActive(true);
	}

	public void OnQuit() {
		Application.Quit();
	}
}

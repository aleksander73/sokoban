using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public Button startButton;
	public Button quitButton;

	public void Start() {
		Dictionary<Button, UnityAction> buttonClickHandlers = new Dictionary<Button, UnityAction> {
			{ startButton, () => this.OnStart() },
			{ quitButton, () => this.OnQuit() }
		};

        foreach(KeyValuePair<Button, UnityAction> entry in buttonClickHandlers) {
			Button button = entry.Key;
			UnityAction callback = entry.Value;
			button.GetComponent<Button>().onClick.AddListener(callback);
        }
    }

	// --------------------------------------------------

	public void OnStart() {
		SceneManager.LoadScene(Scenes.LEVEL, LoadSceneMode.Single);
	}

	public void OnQuit() {
		Application.Quit();
	}
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour {
	private GameManager gameManager;
	private GameObject mainMenu;

	public GameObject buttonPrefab;
	public GameObject backButtonPrefab;
	public List<Button> levelButtons;

    private void Start() {
		this.gameManager = GameObject.Find("game_manager").GetComponent<GameManager>();
        this.mainMenu = GameObject.Find("main_menu");

		float BUTTON_HEIGHT = buttonPrefab.GetComponent<RectTransform>().rect.height;
		int nLevels = gameManager.levels.Count;
		Vector2 origin = new Vector2(0, ((nLevels - 1) * (BUTTON_HEIGHT + gameManager.VERTICAL_BUTTON_OFFSET)) / 2);
		for(int i = 0; i < nLevels; i++) {
			GameObject levelButtonGO = GameObject.Instantiate<GameObject>(buttonPrefab, this.gameObject.transform);
			levelButtonGO.transform.localPosition = origin - new Vector2(0, i * (BUTTON_HEIGHT + gameManager.VERTICAL_BUTTON_OFFSET));

			TextMeshProUGUI textMesh = levelButtonGO.GetComponentInChildren<TextMeshProUGUI>();
			textMesh.text = $"LEVEL {i + 1}";

			Button levelButton = levelButtonGO.GetComponent<Button>();
			ColorBlock colorBlock = levelButton.colors;
			colorBlock.highlightedColor = gameManager.positiveColor;
			levelButtonGO.GetComponent<Button>().colors = colorBlock;

			int levelToLoad = i;
			levelButton.onClick.AddListener(() => {
				gameManager.LevelToLoad = levelToLoad;
				SceneManager.LoadScene(Scenes.LEVEL, LoadSceneMode.Single);
			});

			levelButtons.Add(levelButton);
		}

		// ==================================================

		// Position "BACK" button in the lower left corner of the screen

		GameObject ui = GameObject.Find("ui");
		Rect uiRect = ui.GetComponent<RectTransform>().rect;

		GameObject backButtonGO = GameObject.Instantiate<GameObject>(backButtonPrefab, this.gameObject.transform);
		RectTransform backButtonRectTransform = backButtonGO.GetComponent<RectTransform>();

		const float OFFSET = 25f;
		backButtonRectTransform.localPosition = new Vector2(
			(-uiRect.width + backButtonRectTransform.rect.width) / 2  + OFFSET,
			(-uiRect.height + backButtonRectTransform.rect.height) / 2  + OFFSET
		);

		Button backButton = backButtonGO.GetComponent<Button>();
		backButton.onClick.AddListener(() => {
			mainMenu.SetActive(true);
			this.gameObject.SetActive(false);
		});
    }
}

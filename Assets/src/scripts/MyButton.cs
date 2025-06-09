using UnityEngine;
using UnityEngine.UI;

public class MyButton : MonoBehaviour {
    private void Start() {
        Button button = GetComponent<Button>();
        AudioSource clickSound = this.gameObject.GetComponent<AudioSource>();
        button.onClick.AddListener(() => clickSound.Play());
    }
}

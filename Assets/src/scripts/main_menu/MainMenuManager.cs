using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    private void Start() {
        CameraEffects cameraEffects = Camera.main.GetComponent<CameraEffects>();
        cameraEffects.FadeIn(null);
    }
}

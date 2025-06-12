using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    private GameObjectAnimation<float> volumeAnimation;

    private void Start() {
        this.volumeAnimation = new GameObjectAnimation<float>(
            this.gameObject,
            new Animation<float>(0f, 2f, Interpolators.GetFloatLinear(0f, 0.5f, 2f)),
            (soundVolume, gameObject) => {
                AudioSource backgroundMusic = gameObject.GetComponent<AudioSource>();
                backgroundMusic.volume = soundVolume;
            }
        );

        CameraEffects cameraEffects = Camera.main.GetComponent<CameraEffects>();
        cameraEffects.FadeIn(null);
    }

    private void Update() {
        this.volumeAnimation.Update();
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour {
    public GameObject overlay;

    private readonly float fadeDuration = 0.3f;
    private GameObjectAnimation<float> activeFadeAnimation;

    private void Update() {
        this.activeFadeAnimation?.Update();
    }

    private void SetActiveAnimation(GameObjectAnimation<float> activeAnimation) {
        this.activeFadeAnimation?.GetAnimation().Reset();
        this.activeFadeAnimation = activeAnimation;
        this.activeFadeAnimation.Start();
    }

    // ==================================================

    public void FadeIn(Action<GameObject> onFinished) {
        GameObjectAnimation<float> fadeIn = this.CreateFadeInAnimation(onFinished);
        this.SetActiveAnimation(fadeIn);
    }

    public void FadeOut(Action<GameObject> onFinished) {
        GameObjectAnimation<float> fadeOut = this.CreateFadeOutAnimation(onFinished);
        this.SetActiveAnimation(fadeOut);
    }

    public void Blink(Action<GameObject> middleAction) {
        this.FadeOut(overlay => {
            middleAction(overlay);
            this.FadeIn(null);
        });
    }

    // ==================================================

    // Helper function to create alpha channel animations
    private GameObjectAnimation<float> CreateAlphaChannelAnimation(float duration, Func<float, float> animationFunc, Action<GameObject> onFinished) {
        GameObjectAnimation<float> animation = new GameObjectAnimation<float>(
            this.overlay,
            new Animation<float>(0, duration, animationFunc),
            (alpha, overlay) => {
                Image image = overlay.GetComponent<Image>();
                Color color = image.color;
                Color newColor = new Color(color.r, color.g, color.b, alpha);
                image.color = newColor;
            }
        );
        animation.onFinished += onFinished;
        return animation;
    }

    private GameObjectAnimation<float> CreateFadeInAnimation(Action<GameObject> onFinished) {
        GameObjectAnimation<float> fadeIn = this.CreateAlphaChannelAnimation(fadeDuration, new Func<float, float>(t => {
            return 1 - t / fadeDuration;
        }), onFinished);
        return fadeIn;
    }

    private GameObjectAnimation<float> CreateFadeOutAnimation(Action<GameObject> onFinished) {
        GameObjectAnimation<float> fadeOut = this.CreateAlphaChannelAnimation(fadeDuration, new Func<float, float>(t => {
            return t / fadeDuration;
        }), onFinished);
        return fadeOut;
    }
}

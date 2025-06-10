using System;
using UnityEngine;

public class Window : MonoBehaviour {
    private float defaultFadeDuration;
    private GameObjectAnimation<float> activeFadeAnimation;
    private bool isVisible;
    private bool isTransitioning;

    private void Awake() {
        this.defaultFadeDuration = 0.1f;
    }

    private void Update() {
        this.activeFadeAnimation?.Update();
    }

    // ==================================================

    private void SetActiveAnimation(GameObjectAnimation<float> activeAnimation) {
        this.activeFadeAnimation?.GetAnimation().Reset();
        this.activeFadeAnimation = activeAnimation;
        this.activeFadeAnimation.Start();
    }

    // ==================================================

    public void Show(bool immediately, Action<GameObject> onFinished) {
        if(this.isTransitioning) {
            return;
        }

        float duration = immediately ? 0 : this.defaultFadeDuration;
        GameObjectAnimation<float> fadeIn = this.CreateFadeInAnimation(duration, onFinished);
        this.SetActiveAnimation(fadeIn);
    }

    public void Hide(bool immediately, Action<GameObject> onFinished) {
        if(this.isTransitioning) {
            return;
        }

        float duration = immediately ? 0 : this.defaultFadeDuration;
        GameObjectAnimation<float> fadeOut = this.CreateFadeOutAnimation(duration, onFinished);
        this.SetActiveAnimation(fadeOut);
    }

    public void Toggle(bool immediately, Action<GameObject> onFinished) {
        if(this.IsVisible()) {
			this.Hide(immediately, onFinished);
		} else {
			this.Show(immediately, onFinished);
		}
    }

    // ==================================================

    // Helper function to create alpha channel animations
    private GameObjectAnimation<float> CreateAlphaChannelAnimation(float duration, Func<float, float> animationFunc, Action<GameObject> onFinished) {
        GameObjectAnimation<float> animation = new GameObjectAnimation<float>(
            this.gameObject,
            new Animation<float>(0, duration, animationFunc),
            (alpha, window) => {
                CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
                canvasGroup.alpha = alpha;
            }
        );
        animation.onFinished += onFinished;
        return animation;
    }

    private GameObjectAnimation<float> CreateFadeInAnimation(float duration, Action<GameObject> onFinished) {
        GameObjectAnimation<float> fadeIn = this.CreateAlphaChannelAnimation(
            duration,
            new Func<float, float>(t => duration > 0 ? (t / duration) : 1),
            window => {
                CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                this.isVisible = true;
                this.isTransitioning = false;
                onFinished(window);
            }
        );
        fadeIn.onStarted += window => {
            this.isTransitioning = true;
        };
        return fadeIn;
    }

    private GameObjectAnimation<float> CreateFadeOutAnimation(float duration, Action<GameObject> onFinished) {
        GameObjectAnimation<float> fadeOut = this.CreateAlphaChannelAnimation(
            duration,
            new Func<float, float>(t => duration > 0 ? (1 - t / duration) : 0),
            window => {
                this.isTransitioning = false;
                onFinished(window);
            }
        );
        fadeOut.onStarted += window => {
            CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            this.isVisible = false;
            this.isTransitioning = true;
        };
        return fadeOut;
    }

    // ==================================================

    public bool IsVisible() {
        return this.isVisible;
    }

    public bool IsTransitioning() {
        return this.isTransitioning;
    }
}

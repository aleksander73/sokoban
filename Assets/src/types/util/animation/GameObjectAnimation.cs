using System;
using UnityEngine;

public class GameObjectAnimation<T> {
    private readonly GameObject gameObject;
    private readonly Animation<T> animation;
    private readonly Action<T, GameObject> applyResult;
    public Action<GameObject> onStarted;
    public Action<GameObject> onFinished;

    public GameObjectAnimation(GameObject gameObject, Animation<T> animation, Action<T, GameObject> applyResult) {
        this.gameObject = gameObject;
        this.animation = animation;
        this.applyResult = applyResult;
    }

    public void Start() {
        this.animation.Start();
        this.onStarted?.Invoke(this.gameObject);
    }

    public void Update() {
        if(this.IsFinished()) {
            return;
        }

        T result = this.animation.Update();
        this.applyResult(result, this.gameObject);

        if(this.animation.IsFinished()) {
            this.onFinished?.Invoke(this.gameObject);
        }
    }

    public bool IsFinished() {
        return this.animation.IsFinished();
    }
}

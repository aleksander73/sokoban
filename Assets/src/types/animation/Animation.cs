using System;
using UnityEngine;

public class Animation<T> {
    private readonly float t0;
    private readonly float t1;
    private readonly Func<float, T> getValue;
    private float startTime;
    private bool finished;


    public Animation(float t0, float t1, Func<float, T> getValue) {
        this.t0 = t0;
        this.t1 = t1;
        this.getValue = getValue;
    }

    public void Start() {
        this.startTime = Time.time;
    }

    public void Reset() {
        this.startTime = 0f;
        this.finished = false;
    }

    public T Update() {
        if (this.finished) {
            return this.getValue(t1);
        }

        float endTime = this.startTime + (t1 - t0);
        if(Time.time > endTime) {
            this.finished = true;
        }

        float elapsedTime = Time.time - this.startTime;
        float t = !this.finished ? t0 + elapsedTime : t1;
        return this.getValue(t);
    }

    public bool IsFinished() {
        return this.finished;
    }
}

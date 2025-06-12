using System;
using UnityEngine;

public class Interpolators {
    public static Func<float, float> GetFloatLinear(float startValue, float endValue, float duration) {
        Func<float, float> result = new Func<float, float>(t => {
            return startValue + (endValue - startValue) * (t / duration);
        });
        return result;
    }

    public static Func<float, Vector3> GetVector3Linear(Vector3 origin, Vector3 target, float duration) {
        Func<float, Vector3> result = new Func<float, Vector3>(t => {
            return origin + (target - origin) * (t / duration);
        });
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathStuff {

    /// <summary>
    /// Decays a value, but does it framerate-independently.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="smoothing"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static float Damp(float value, float smoothing, float dt) {
        return value * Mathf.Pow(smoothing, dt);
    }
    public static Vector3 Damp(Vector3 value, float smoothing, float dt) {
        return value * Mathf.Pow(smoothing, dt);
    }
    public static float Damp(float source, float target, float smoothing, float dt) {
        return Mathf.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }
    public static Vector3 Damp(Vector3 source, Vector3 target, float smoothing, float dt) {
        return Vector3.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }
    public static Quaternion Damp(Quaternion source, Quaternion target, float smoothing, float dt) {
        return Quaternion.Slerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }
}

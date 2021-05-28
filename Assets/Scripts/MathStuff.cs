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
    /// <summary>
    /// A function for easy implementation of spring-like easing. This is frame-rate independent,
    /// and is based on http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
    /// </summary>
    /// <param name="val">The current value of the spring. The function will modify the value.</param>
    /// <param name="vel">The current velocity of the spring. The function will modify the velocity.</param>
    /// <param name="target">The target value that the spring is easing towards.</param>
    /// <param name="damp">How much to dampen the "springiness".</param>
    /// <param name="freq">The oscillation-speed of the underlying wave.</param>
    /// <param name="dt">The current frame's delta-time. The function will use Time.deltaTime by default, but custom values can be passed in.</param>
    public static void Spring(
        ref float val,
        ref float vel,
        float target,
        float damp = .23f,
        float freq = 24,
        float dt = -1) {

        if (dt < 0) dt = Time.deltaTime;

        float k = 1 + 2 * dt * damp * freq;
        float tff = dt * freq * freq;
        float ttff = dt * tff;

        val = (k * val + dt * vel + ttff * target) / (k + ttff);
        vel = (vel + tff * (target - val)) / (k + ttff);
    }
    // 3D version
    public static void Spring(
        ref Vector3 val,
        ref Vector3 vel,
        Vector3 target,
        float damp = .23f,
        float freq = 24,
        float dt = -1) {

        if (dt < 0) dt = Time.deltaTime;

        float k = 1 + 2 * dt * damp * freq;
        float tff = dt * freq * freq;
        float ttff = dt * tff;

        val = (k * val + dt * vel + ttff * target) / (k + ttff);
        vel = (vel + tff * (target - val)) / (k + ttff);
    }
}
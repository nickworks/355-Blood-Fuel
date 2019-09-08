using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegisteredGlobalEffect {
    public EffectType type;
    public ParticleSystem system;
}
public enum EffectType {
    Dust,
    Sand
}
public class WorldParticles : MonoBehaviour
{

    [SerializeField]
    public RegisteredGlobalEffect[] particleSystems;

    static public WorldParticles obj { get; private set; }

    float time = 0;

    void Start() {
        obj = this;
        foreach (RegisteredGlobalEffect p in particleSystems) {
            p.system.Stop();
        }
    }
    public void FixedUpdate() {
        time = Time.fixedDeltaTime;
        foreach (RegisteredGlobalEffect p in particleSystems) {
            //p.system.Simulate(time, true, false, true);
        }
    }
    public void Emit(EffectType type, Vector3 pos, Vector3 vel) {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = pos;
        emitParams.velocity = vel * .75f;

        //particleSystems.TryGetValue(type, out ParticleSystem p);
        foreach (RegisteredGlobalEffect p in particleSystems) {
            if (p.type == type) {
                p.system.Emit(emitParams, 10);
                break;
            }
        }
    }

}

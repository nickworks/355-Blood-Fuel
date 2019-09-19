using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChance : MonoBehaviour
{
    //DONE: Setup a 1 in 100 chance that this object will play an animation when the player gets close.
    //TODO: Allow this script to "see" the distance from this object to any and all cars. If one gets close, play the animation.

    bool willAnim = false;

    // Start is called before the first frame update
    void Start() {
        if (Random.Range(1, 100) == 50) willAnim = true;
    }

    // Update is called once per frame
    void Update() {
        if (willAnim) {
            //Check the position of all of the cars. See if any is close enough. If so, play the animation.
        }
    }
}

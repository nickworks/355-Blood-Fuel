using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class allows the cliff in Ghost Town to spawn with a random scale on the z axis.
 */
public class RandomizeCliffScale : MonoBehaviour
{
    /*
     * This function randomizes the z scale when the cliff is spawned.
     */
    void Start()
    {
        var scale = transform.localScale;

        /*
         * Scales the cliff's z value to a random number between 3.5 and 4.
         */
        scale.z = Random.Range(3.5f, 4f);
        transform.localScale = scale;
    }
}

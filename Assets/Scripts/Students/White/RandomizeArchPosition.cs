using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class randomizes the x position of the last arch in Ghost Town when spawned.
 */
public class RandomizeArchPosition : MonoBehaviour
{
    /*
     * This function randomizes the x position.
     */
    void Start()
    {
        var xPosition = transform.position;
        xPosition.x = Random.Range(-200f, 200f);
        transform.position = xPosition;
    }
}

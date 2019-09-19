using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class randomizes the y rotation of the central tower in Ghost Town.
 */
public class White_Rotate : MonoBehaviour
{
    /*
     * This function randomizes the y rotation.
     */
    void Start()
    {
        var euler = transform.eulerAngles;
        euler.y = Random.Range(-90f, 90f);
        transform.eulerAngles = euler;
    }
}

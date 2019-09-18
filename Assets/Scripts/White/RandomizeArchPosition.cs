using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeArchPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var xPosition = transform.position;
        xPosition.x = Random.Range(-200f, 200f);
        transform.position = xPosition;
    }
}

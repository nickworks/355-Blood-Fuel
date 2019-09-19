using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeCliffScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var scale = transform.localScale;
        scale.z = Random.Range(3.5f, 4f);
        transform.localScale = scale;
    }
}

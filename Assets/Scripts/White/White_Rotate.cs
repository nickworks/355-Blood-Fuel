using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Rotate : MonoBehaviour
{
    void Start()
    {
        var euler = transform.eulerAngles;
        euler.y = Random.Range(0f, 360f);
        transform.eulerAngles = euler;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Rotate : MonoBehaviour
{
    void Start()
    {
        //float yAngle = Random.Range(-180, 180f);
        //transform.Rotate(0, yAngle, 0);

        var euler = transform.eulerAngles;
        euler.y = Random.Range(0f, 360f);
        transform.eulerAngles = euler;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatablePrefab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int yaw = Random.Range(0, 4);
        transform.rotation = Quaternion.Euler(0, yaw * 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webb_Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Rotates(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Rotates()
    {
        int rot = 0;
        rot = 0 + Random.Range(0,4);
       
        if (rot == 1)
        {
            transform.Rotate(Vector3.up * 90);
        }
        if (rot == 2)
        {
            transform.Rotate(Vector3.up * 180);
        }
        if (rot == 3)
        {
           
        transform.Rotate(Vector3.up * 270);
    } 
        }
        
}

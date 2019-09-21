using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingJohnson : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        RandomRotation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // rotates prefab by a random 90 degrees
    void RandomRotation()
    {
        float randPicker = Random.Range(0f, 1.3f);

        if (randPicker < .3f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (randPicker > .3f && randPicker < .6f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (randPicker > .6f && randPicker < .9f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (randPicker > 1.2f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 360, 0);
        }

    }
}

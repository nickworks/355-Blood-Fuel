using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
     int timer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
    }
    void GroundCheck()
    {
        timer += 1;
        if (timer >= 0)
        {
            if (GameObject.Find("groundStart") != null)
            {

                Destroy(gameObject);

            }
        }
    }
}

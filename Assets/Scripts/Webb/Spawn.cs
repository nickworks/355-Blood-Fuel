using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    int death = 0;
    // Start is called before the first frame update
    void Start()
    {
        death += Random.Range(0, 3);
        if (death > 0)
        {
            Destroy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Destroy()
    {

        Destroy(gameObject);
    }
}

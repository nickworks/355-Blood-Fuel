using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        float picker = Random.Range(1f, 10f);

        if(picker <= 5f)
        {
            gameObject.SetActive(true);
        }
        else if(picker >= 5f)
        {
            gameObject.SetActive(false);
        }

    }
}

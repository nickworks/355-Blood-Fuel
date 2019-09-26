using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnsonSpikeCode : MonoBehaviour
{

    public GameObject spike;
    bool triggered = false;
    float spikeTriggeredHeight;

    // Start is called before the first frame update
    void Start()
    {
        spikeTriggeredHeight = spike.transform.position.y + 13f;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            if (spike.transform.position.y >= spikeTriggeredHeight)
            {
                triggered = false;
            }
            else
            {
                spike.transform.Translate(0, 100 * Time.deltaTime, 0);
            }
            
            
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (spike.transform.position.y < spikeTriggeredHeight)
        {
            triggered = true;
        }
        else
        {
            triggered = false;
        }
    }

}

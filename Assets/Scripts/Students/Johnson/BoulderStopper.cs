using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderStopper : MonoBehaviour
{

    public Rigidbody[] rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Rigidbody rigid in rb)
        {
            rigid.isKinematic = true;

        }
    }
}

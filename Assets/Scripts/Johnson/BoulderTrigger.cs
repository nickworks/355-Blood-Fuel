using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{

    public Rigidbody[] rb;

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
            rigid.isKinematic = false;
            
        }
    }
}

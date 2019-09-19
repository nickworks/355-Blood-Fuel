using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{

    public Rigidbody[] rb;
    bool isCollided = false;

    void Start()
    {
        rb = GetComponents<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollided)
        {
            foreach (Rigidbody rigid in rb)
            {
                rigid.isKinematic = false;
            }
            Debug.Log("hello");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isCollided = true;
    }
}

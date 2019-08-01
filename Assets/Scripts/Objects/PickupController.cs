using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

    public float fuelAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
            Car car = other.GetComponent<Car>();
            if(car != null) car.AddFuel(10);
        }
    }
}

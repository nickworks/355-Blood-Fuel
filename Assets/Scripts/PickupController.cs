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
            DriverPlayer.main.car.AddFuel(10);
            DriverPlayer.score += 10;
        }
    }
}

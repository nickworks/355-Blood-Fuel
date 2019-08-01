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
            PlayerManager.playerOne.car.AddFuel(10);
            PlayerManager.playerOne.score += 10;
        }
    }
}

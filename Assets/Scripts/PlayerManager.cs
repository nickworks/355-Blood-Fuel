using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public DriverPlayer playerOne;

    public FollowTarget camera;

    public Car playerCarPrefab;

    void Start()
    {
        playerOne = new DriverPlayer();

        SpawnPlayer();
    }
    void SpawnPlayer() {
        Car car = Instantiate(playerCarPrefab);
        playerOne.TakeControl(car);

        camera.target = car.transform;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon {

    public HomingProjectile prefabRocket;
    public int amountOfRockets = 8;
    public float secondsBetweenLaunches = .1f;
    private float cooldown = 2;

    float secondsTilNextRound;
    int amountOfRocketsInChamber;

    override public void Update() {
        if (amountOfRocketsInChamber > 0) {
            secondsTilNextRound -= Time.deltaTime;
            if (secondsTilNextRound <= 0) {
                HomingProjectile rocket = Instantiate(prefabRocket, spawnPoint.position, Quaternion.identity);
                rocket.Launch(car.driver, car.state.up);
                secondsTilNextRound = secondsBetweenLaunches;
                amountOfRocketsInChamber--;
            }
        } else {
            base.Update(); // do regular cooldown
        }
    }
    public override bool FireWeapons() {

        // check if can fire:
        if (!base.FireWeapons()) return false;

        secondsTilNextRound = 0;
        amountOfRocketsInChamber = amountOfRockets;
        return true;
    }
}

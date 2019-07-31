using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    Car car;
    public Transform speedNeedle;
    public Image[] barrels;
    public Text score;

    public float speedometerMaxVelocity = 80;
    float previousAngle = 0;

    public void SetCar(Car car)
    {
        this.car = car;
    }
    void Update()
    {
        UpdateFuelGauge();
        UpdateSpeedometer();

        score.text = string.Join(" ", ((int)DriverPlayer.score).ToString().Split());
    }
    private void UpdateSpeedometer()
    {
        float vel = car ? car.ballBody.velocity.z : 0;

        float p = vel / speedometerMaxVelocity;
        p = Mathf.Clamp(p, 0, 1);
        float angle = Mathf.Lerp(120, -118, p);
        float finalAngle = (angle + previousAngle * 4) / 5;
        speedNeedle.eulerAngles = new Vector3(0, 0, finalAngle);
        previousAngle = finalAngle;
    }
    private void UpdateFuelGauge()
    {
        if (!car) return;

        float fuelPerBarrel = 10;
        float fuel = car.currentFuel;
        int fullBarrels = (int)(fuel / fuelPerBarrel);
        float percentOfLastBarrel = (fuel - fullBarrels * fuelPerBarrel) / fuelPerBarrel;

        for (int i = 0; i < barrels.Length; i++)
        {
            if (fullBarrels > i)
            {
                barrels[i].gameObject.SetActive(true);
                barrels[i].fillAmount = 1;
            }
            else if (fullBarrels == i)
            {
                barrels[i].gameObject.SetActive(true);
                barrels[i].fillAmount = percentOfLastBarrel;
            }
            else
            {
                barrels[i].gameObject.SetActive(false);
            }
        }
    }
}

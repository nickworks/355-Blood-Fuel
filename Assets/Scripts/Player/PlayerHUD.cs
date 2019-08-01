using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    DriverPlayer driver;
    public Transform speedNeedle;
    public Image[] barrels;
    public Text score;

    public float speedometerMaxVelocity = 80;
    float previousAngle = 0;

    public void SetPlayer(DriverPlayer player)
    {
        driver = player;
    }
    void Update()
    {
        UpdateFuelGauge();
        UpdateSpeedometer();

        score.text = string.Join(" ", ((int)driver.score).ToString().Split());
    }
    private void UpdateSpeedometer()
    {
        float vel = (driver.car == null) ? 0 : driver.car.ballBody.velocity.z;

        float p = vel / speedometerMaxVelocity;
        p = Mathf.Clamp(p, 0, 1);
        float angle = Mathf.Lerp(120, -118, p);
        float finalAngle = (angle + previousAngle * 4) / 5;
        speedNeedle.eulerAngles = new Vector3(0, 0, finalAngle);
        previousAngle = finalAngle;
    }
    private void UpdateFuelGauge()
    {
        if (driver.car == null) return;

        float fuelPerBarrel = 10;
        float fuel = driver.car.currentFuel;
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

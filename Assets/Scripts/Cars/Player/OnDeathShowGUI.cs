using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathShowGUI : MonoBehaviour {

    public GameObject prefabGUI;
    private bool spawned = false;
	void Explode()
    {
        SpawnGUI();
    }
    void Update()
    {
        if (DriverPlayer.main != null && DriverPlayer.main.car.currentFuel <= 0) SpawnGUI();
    }
    void SpawnGUI()
    {
        if (spawned) return;
        spawned = true;
        Instantiate(prefabGUI);
    }
}

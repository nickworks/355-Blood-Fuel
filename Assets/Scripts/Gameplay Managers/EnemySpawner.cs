using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public int maxEnemies = 5;
    static public List<DriverAI> activeAi = new List<DriverAI>();

    float spawnTimer = 0;

    public Car enemyPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

         if (activeAi.Count < maxEnemies && spawnTimer <= 0) {
            DriverPlayer player = PlayerManager.PickRandom();
            if(player != null && player.car != null) SpawnEnemyNear(player.car.transform);
            
        } else if (spawnTimer > 0) {
            spawnTimer -= Time.deltaTime;
        }
	}


    void SpawnEnemyNear(Transform transform) {
       
        float buffer = 5;
        float spawnWidth = 20;

        Vector3 position = transform.position;
        position.z -= Random.Range(10, 15);
        position.x += Random.Range(-spawnWidth, spawnWidth);
        if (position.x > 0 && position.x < buffer) position.x = buffer;
        if (position.x < 0 && position.x > -buffer) position.x = -buffer;

        Car car = Instantiate(enemyPrefab,position,Quaternion.identity);

        DriverAI ai = new DriverAI();
        ai.TakeControl(car);
        activeAi.Add(ai);

        spawnTimer = Random.Range(1, 2);
    }

    static public void Remove(DriverAI ai) {
        activeAi.Remove(ai);
        //print(" removing dead ai");
    }
}

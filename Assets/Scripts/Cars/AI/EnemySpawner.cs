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
            SpawnEnemy();
            
        } else if (spawnTimer > 0) {
            spawnTimer -= Time.deltaTime;
        }
	}


    void SpawnEnemy() {
        if (PlayerManager.playerOne == null) return;
        if (PlayerManager.playerOne.car == null) return;

        float buffer = 5;
        float spawnWidth = 20;

        Vector3 position = PlayerManager.playerOne.car.transform.position;
        position.z -= Random.Range(10, 15);
        position.x += Random.Range(-spawnWidth, spawnWidth);
        if (position.x > 0 && position.x < buffer) position.x = buffer;
        if (position.x < 0 && position.x > -buffer) position.x = -buffer;

        Car car = Instantiate(enemyPrefab,position,Quaternion.identity);

        DriverAI ai = new DriverAI();
        ai.TakeControl(car);

        spawnTimer = Random.Range(1, 2);
    }

    static public void Remove(DriverAI ai) {
        activeAi.Remove(ai);
    }
}

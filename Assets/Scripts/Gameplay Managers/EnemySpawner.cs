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


    void SpawnEnemyNear(Transform target) {

        Vector3 position = GetSpawnLocationNear(target);

        Car car = Instantiate(enemyPrefab,position,Quaternion.identity);

        DriverAI ai = new DriverAI();
        ai.TakeControl(car);
        activeAi.Add(ai);

        spawnTimer = Random.Range(1, 2);
    }
    Vector3 GetSpawnLocationNear(Transform target) {

        float spawnWidth = 20;
        float raycastLength = 10;

        Vector3 position = target.position;
        position.z -= Random.Range(10, 15); // 10 to 15 meters behind
        position.x += Random.Range(-spawnWidth, spawnWidth); // meters side-to-side
        position.y += raycastLength / 2;

        if (Physics.Raycast(position, Vector3.down * raycastLength, out RaycastHit hit)) {
            position = hit.point + new Vector3(0, 2, 0);
        }

        return position;
    }

    static public void Remove(DriverAI ai) {
        activeAi.Remove(ai);
        //print(" removing dead ai");
    }
}

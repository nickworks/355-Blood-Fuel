using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk : MonoBehaviour {

    public Transform endOfChunk;
    public float percentChanceOfFuelPickups = .5f;
    public float percentChanceOfObstacles = .5f;


    void Start () {
        Limit<TriggerVolume>(percentChanceOfFuelPickups); // 50% chance if spawning fuel at each node
        Limit<ObstacleSpawner>(percentChanceOfObstacles); // 50% chance of spawning an obstacle at each node
	}
    void Limit<T>(float percent)
    {
        List<T> objs = new List<T>(GetComponentsInChildren<T>());
        int numberOfBarrels = (int)(percent * objs.Count);

        while (objs.Count > numberOfBarrels)
        {
            int index = Random.Range(0, objs.Count);
            Destroy((objs[index] as Behaviour).gameObject);
            objs.RemoveAt(index);
        }
    }

	void Update () {
		
	}
}

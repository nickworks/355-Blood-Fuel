using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk : MonoBehaviour {

    public Transform endOfChunk;

    void Start () {
        
	}

    /// <summary>
    /// This method reduces a particular type of object to a specified percentage of the total that currently exist in the chunk.
    /// For example, Reduce&lt;Pickup&gt;(0.75f) reduces the number of Pickups to 75% the original number.
    /// </summary>
    /// <typeparam name="T">The Type of object to reduce.</typeparam>
    /// <param name="percent">The percentage of the original objects.</param>
    void Reduce<T>(float percent) {
        List<T> objs = new List<T>(GetComponentsInChildren<T>());
        int numberOfItems = (int)(percent * objs.Count);

        while (objs.Count > numberOfItems) {
            int index = Random.Range(0, objs.Count);
            Destroy((objs[index] as Behaviour).gameObject);
            objs.RemoveAt(index);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    public GameObject tree1;
    public GameObject rock1;

	void Start () {

        bool isTree = (Random.Range(0, 100) >= 50);

        tree1.SetActive(isTree);
        rock1.SetActive(!isTree);

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        transform.rotation = rot;

        transform.localScale = Vector3.one * Random.Range(2, 5);
	}
}

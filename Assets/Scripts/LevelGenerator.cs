using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Transform cam;
    public float chunkUnloadOffset = 5;

    public LevelChunk[] prefabChunks;
    private List<LevelChunk> loadedChunks = new List<LevelChunk>();
	
	void Start () {
        for (int i = 0; i < 4; i++)
        {
            SpawnRandomChunk();
        }
	}
	
	void Update () {
        CheckUnloadChunk();
	}
    void CheckUnloadChunk()
    {
        if (loadedChunks.Count <= 0) return;
        if (cam == null) return;

        if (loadedChunks[0].endOfChunk.position.z < cam.position.z - chunkUnloadOffset)
        {
            SpawnRandomChunk();
            Destroy(loadedChunks[0].gameObject);
            loadedChunks.RemoveAt(0);
        }
    }
    
    void SpawnRandomChunk()
    {
        int numOfPrefabs = prefabChunks.Length;
        if (numOfPrefabs <= 0) return;

        Vector3 pos = Vector3.zero;
        if (loadedChunks.Count > 0) pos = loadedChunks[loadedChunks.Count - 1].endOfChunk.position;

        LevelChunk prefab = prefabChunks[Random.Range(0, numOfPrefabs)];
        LevelChunk newChunk = Instantiate(prefab, pos, Quaternion.identity);
        // flip it:
        if(Random.Range(0,100) > 50) newChunk.transform.localScale = new Vector3(-1, 1, 1);

        loadedChunks.Add(newChunk);
    }
}

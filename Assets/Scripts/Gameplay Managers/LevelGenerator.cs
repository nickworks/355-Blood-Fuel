using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Transform cam;
    public int numberOfChunksToLoad = 2;
    /// <summary>
    /// The number of meters behind the camera where chunks are unload.
    /// </summary>
    public float thresholdForRemovingChunks = 5;
    /// <summary>
    /// The prefabs to consider loading as chunks.
    /// </summary>
    public LevelChunk[] prefabChunks;
    /// <summary>
    /// The currently loaded chunks.
    /// </summary>
    private List<LevelChunk> loadedChunks = new List<LevelChunk>();

    public static LevelChunk currentChunk { get; private set; }

	void Start () {
        for (int i = 0; i < numberOfChunksToLoad; i++) {
            SpawnRandomChunk();
        }
    }
	
	void Update () {
        CheckUnloadChunk();
        if (loadedChunks.Count < numberOfChunksToLoad) SpawnRandomChunk();
        currentChunk = loadedChunks[0];
	}
    /// <summary>
    /// This method unloads chunks that are off-screen.
    /// </summary>
    void CheckUnloadChunk() {
        if (loadedChunks.Count <= 0) return;
        if (cam == null) return;

        if (loadedChunks[0].endOfChunk.position.z < cam.position.z - thresholdForRemovingChunks) {
            Destroy(loadedChunks[0].gameObject);
            loadedChunks.RemoveAt(0);
        }

    }

    /// <summary>
    /// Spawn a random chunk and place it just beyond the last chunk.
    /// </summary>
    void SpawnRandomChunk() {
        int numOfPrefabs = prefabChunks.Length;
        if (numOfPrefabs <= 0) return;

        Vector3 pos = Vector3.zero;
        if (loadedChunks.Count > 0) pos = loadedChunks[loadedChunks.Count - 1].endOfChunk.position;

        LevelChunk prefab = prefabChunks[Random.Range(0, numOfPrefabs)];
        LevelChunk newChunk = Instantiate(prefab, pos, Quaternion.identity);
        // flip it:
        //if(Random.Range(0,100) > 50) newChunk.transform.localScale = new Vector3(-1, 1, 1);

        loadedChunks.Add(newChunk);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVolume : MonoBehaviour
{
    private Vector3 volumeBounds;
    GameObject volume;

    // Start is called before the first frame update
    void Start()
    {

        //TODO: Disable this object's mesh renderer and collider
        //TODO: Figure out this object's size
        //TODO: Figure out what other object(s) this object will spawn
        //TODO: Figure out the size of the ojects from the previous step
        //TODO: Spawn objects inside this object to fill up it's space but not clipping into each other. at least not too much

        volumeBounds = new Vector3();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

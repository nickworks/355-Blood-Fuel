using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
   public int death = 0;
    public int chance = 3;
    // Start is called before the first frame update
    void Start()
    {
        death += Random.Range(0, chance);
        if (death > 0)
        {
            Destroy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Destroy()
    {

        Destroy(gameObject);
    }
}

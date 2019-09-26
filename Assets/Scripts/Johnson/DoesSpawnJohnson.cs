using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesSpawnJohnson : MonoBehaviour
{
    public float pickerMax;
    public float pickerMin;
    public float threshold;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        if (pickerMax == 0 && pickerMin == 0)
        {
            pickerMax = 10f;
            pickerMin = 1;
        }

        float picker = Random.Range(pickerMin, pickerMax);

        if (threshold == 0) threshold = 5f;

        if (picker <= threshold)
        {
            gameObject.SetActive(true);
        }
        else if(picker >= threshold)
        {
            gameObject.SetActive(false);
        }

    }
}

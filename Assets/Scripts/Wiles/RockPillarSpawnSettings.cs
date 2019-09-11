using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPillarSpawnSettings : MonoBehaviour
{

    public Transform[] childTrans;
    public GameObject[] childObjects;

    // Start is called before the first frame update
    void Start()
    {
        //Get the transforms of Rock Pillar, bottom, collumn, and body.
        childTrans = GetComponentsInChildren<Transform>();

        //DONE: At instantiation, rotate the base, collumn and body of this object. (Also rotates it's root, but that's ok.)
        foreach (Transform trans in childTrans)
        {
            trans.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        }

        //DONE: Move Collumn a random distance down (localPosition 0.9 to -0.1)
        childTrans[2].localPosition = new Vector3(0, Random.Range(-0.1f, 0.9f), 0);

        //DONE: Move Collumn randomly in x and z based on how much it moved in y. 
        // (The closer to 0.4, the farther it could move. Max distance is 0.22, or -0.22 at 0.4 y and 0.11, or -0.11 at 0.8 or 0.0 y)
        float collumnY = childTrans[2].localPosition.y;
        if (collumnY >= 0.0 && collumnY <= 0.8)
        {
            //turns y position into a range between 0.0 to 1.0 (y=0.8 is now a 1.0)
            float testN = collumnY * 1.25f;
            float moveMax = (0.11f * testN) + 0.11f;
            float moveMin = -moveMax;
            childTrans[2].localPosition = new Vector3(Random.Range(moveMin, moveMax), collumnY, Random.Range(moveMin, moveMax));
        }

        //DONE: Move Body in the y direct based off of how much Collumn's y moved. 
        // (3.8-2.1 range when collumnY is 0.9, & no change when collumnY is at -0.1)
        float testM = collumnY + 0.1f;
        float bodyMax = 3.8f;
        float bodyMin = 3.8f - (2.1f * testM);
        childTrans[3].localPosition = new Vector3(0, Random.Range(bodyMin, bodyMax), 0);

        //DONE: Move the Body in the x and z space based on how much the body's been moved in the y direction.
        // (max move at no y change: 0.275. max move at 2.5 or lower: 1)
        float bodyY = childTrans[3].localPosition.y;
        if (bodyY <= 2.5)
        {
            childTrans[3].localPosition = new Vector3(Random.Range(0, 2.5f), bodyY, Random.Range(0, 2.5f));
        }
        else if (bodyY <= 3.8 && bodyY >= 2.5)
        {
            //turns y position into a range between 0.0 to 1.3 (y=3.8 is now a 1.3)
            float testP = bodyY - 1.3f;
            float moveMax2 = (-0.13f / 2.225f * testP) + 0.275f;
            float moveMin2 = -moveMax2;
            childTrans[3].localPosition = new Vector3(Random.Range(moveMin2, moveMax2), bodyY, Random.Range(moveMin2, moveMax2));
        }

        //TODO: Randomly decide if the pillar will have 0-2 extra limbs. 
        // The chance will be 1 in 20 for an extra, and another 1 in 20 for an additional. (Maybe make both these 1 in 10's)
        // If chosen to be created, Will make a cube, shpere, capsule, or cylendar a child of body.
        // Move the children to the edges of the body. Move up or down between 0-1.5. 
        // rotate all 3 axises randomly. Scale it between 0.75-1.5.
        int extraLimbChance = Random.Range(1, 20);
        if (true)
        {
            int pickALimb = Random.Range(1, 4);
            childObjects[3 + pickALimb].SetActive(true);
            childTrans[3 + pickALimb].localPosition = new Vector3(Random.Range(-1.5f, -1.5f), Random.Range(-1.5f, -1.5f), Random.Range(-1.5f, -1.5f));
            childTrans[3 + pickALimb].localScale = new Vector3(Random.Range(0.75f, 1.5f), Random.Range(0.75f, 1.5f), Random.Range(0.75f, 1.5f));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

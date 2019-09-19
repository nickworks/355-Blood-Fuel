using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockArchSpawnSettings : MonoBehaviour
{

    public float horizontalNudge;
    public float verticalNudge;
    public float rotateAmount;
    Transform tempTrans;

    // Start is called before the first frame update
    void Start()
    {
        tempTrans = transform;
        //tempTrans.localPosition = new Vector3(Random.Range(-horizontalNudge, horizontalNudge), 0, Random.Range(-verticalNudge, verticalNudge));
        tempTrans.localEulerAngles = new Vector3(0, Random.Range(-rotateAmount, rotateAmount), 11.195f);
        //Transform Transform = tempTrans;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

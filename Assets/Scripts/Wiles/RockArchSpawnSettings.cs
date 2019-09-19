using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockArchSpawnSettings : MonoBehaviour
{

    public float horizontalNudge;
    public float verticalNudge;
    public float rotateAmount;
    

    // Start is called before the first frame update
    void Start()
    {
        var tempPosition = transform.position;
        tempPosition.x = tempPosition.x + Random.Range(-horizontalNudge, horizontalNudge);
        tempPosition.z = tempPosition.z + Random.Range(-verticalNudge, verticalNudge);
        var tempAngle = transform.eulerAngles;
        tempAngle = new Vector3(0, Random.Range(-rotateAmount, rotateAmount), 11.195f);
        transform.position = tempPosition;
        transform.eulerAngles = tempAngle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

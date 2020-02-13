using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTank : MonoBehaviour
{
    public Transform targetTransform;
    public float movementSpeed, rotSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, targetTransform.position) < 5.0f) return;

        Vector3 targetPos = targetTransform.position;
        targetPos.y = transform.position.y;
        Vector3 dirRot = targetPos - transform.position;

        Quaternion targetRot = Quaternion.LookRotation(dirRot);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0,0,movementSpeed * Time.deltaTime));


    }
}

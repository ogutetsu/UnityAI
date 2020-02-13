using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAvoidance : MonoBehaviour
{
    public float speed = 20.0f;
    public float mass = 5.0f;
    public float force = 50.0f;
    public float minimumDistToAvoid = 20.0f;

    private float curSpeed;
    private Vector3 targetPosint;


    // Start is called before the first frame update
    void Start()
    {
        mass = 5.0f;
        targetPosint = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) &&
            Physics.Raycast(ray, out hit, 5000.0f))
        {
            targetPosint = hit.point;
        }

        Vector3 dir = (targetPosint - transform.position);
        dir.Normalize();

        AvoidObstacles(ref dir);

        if (Vector3.Distance(targetPosint, transform.position) < 3.0f) return;

        curSpeed = speed * Time.deltaTime;

        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        transform.position += transform.forward * curSpeed;

    }

    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;

        int layerMask = 1 << 8; //Obstacles

        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;

            dir = transform.forward + hitNormal * force;
        }

    }


    void OnGUI()
    {
        GUILayout.Label("Clock anywhere to move the vehicle.");
    }

}

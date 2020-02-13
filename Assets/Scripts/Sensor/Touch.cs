using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : Sense
{
    void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            if (aspect.aspectName == aspectName)
            {
                print("Enemy Touch Detected");
            }
        }
    }
}

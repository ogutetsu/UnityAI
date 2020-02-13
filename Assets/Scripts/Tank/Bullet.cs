﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject Explosion;
    [SerializeField]
    private float Speed = 600.0f;
    [SerializeField]
    private float LifeTime = 3.0f;

    public int damage = 50;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Instantiate(Explosion, contact.point, Quaternion.identity);
        Destroy(gameObject);

    }


}

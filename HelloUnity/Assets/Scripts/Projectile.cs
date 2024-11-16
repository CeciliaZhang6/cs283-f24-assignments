using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;       // Speed of the projectile
    public float lifetime = 5f;     // Time before the projectile gets destroyed

    void Start()
    {
        // Destroy the projectile after its lifetime to avoid clutter
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward based on its local forward direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}

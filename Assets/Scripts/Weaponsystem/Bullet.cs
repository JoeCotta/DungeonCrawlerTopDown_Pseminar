using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timeSelfDestroy;
    public float damage;

    void Update()
    {
        // destroys the bullet after a certain time
        Destroy(gameObject, timeSelfDestroy);
    }

    // if the bullet hits another object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // tries to apply damage to the object
        other.gameObject.SendMessage("hit", damage);

        // destroys itself
        Destroy(gameObject);
    }
}

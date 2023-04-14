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
        // if the collider is an enemy or the player it will apply damage to it
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") other.gameObject.SendMessage("hit", damage);

        // destroys itself
        if(other.gameObject.CompareTag("Map")||other.gameObject.CompareTag("Door")||other.gameObject.CompareTag("Doorvrt")) Destroy(gameObject);
    }
}

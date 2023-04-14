using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject owner;
    
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
        // if the owner is dead the bullet will be deleted
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }

        // if the collider is an enemy or the player it will apply damage to it
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){

            //only dmg if other gameobject isnt same type as object wielding the weapon (prevent friendly fire) by Cornell
            if(other.gameObject.tag != owner.tag){
                other.gameObject.SendMessage("hit", damage);
                // when hitting some1 it destroys itself
                Destroy(gameObject);
            }
        }

        // destroys itself; by Cornell
        if(other.gameObject.CompareTag("Map")||other.gameObject.CompareTag("Door")||other.gameObject.CompareTag("Doorvrt")) Destroy(gameObject);
        if(other.gameObject.CompareTag("Bullet")&&owner.tag != other.gameObject.GetComponent<Bullet>().owner.tag) Destroy(gameObject); //other condition so that enemy bullets dont delete eachother
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltBullet : MonoBehaviour
{
    private float dmg;
    private GameObject owner;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the owner is dead the bullet will be deleted
        if (!owner)
        {
            Destroy(gameObject);
            return;
        }

        // if the collider is an enemy or the player it will apply damage to it
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {

            //only dmg if other gameobject isnt same type as object wielding the weapon (prevent friendly fire) by Cornell
            if (other.gameObject.tag != owner.tag)
            {
                other.gameObject.SendMessage("hit", dmg);
                // when hitting some1 it destroys itself
                Destroy(gameObject);
            }
        }

        if (!gameObject || !other.gameObject || owner == null) return;
        // destroys itself; by Cornell
        if (other.gameObject.CompareTag("Map") || other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Doorvrt") || other.gameObject.CompareTag("DoorFix") || other.gameObject.CompareTag("DoorvrtFix")) Destroy(gameObject);
        if (other.gameObject.CompareTag("Bullet") && owner.tag != other.gameObject.GetComponent<Bullet>().owner.tag && gameObject != null) Destroy(gameObject); //other condition so that enemy bullets dont delete eachother
    }


    private void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.up);
    }

    public void assingVar(float dmg,GameObject owner)
    {
        this.dmg = dmg;
        this.owner = owner;
    }

}

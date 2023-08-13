using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltBullet : MonoBehaviour
{
    private float dmg;
    public float speed;
    private GameObject owner;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!owner) Destroy(gameObject);
        if (other.gameObject && other.gameObject.CompareTag(owner.tag) || (other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>().owner && other.gameObject.GetComponent<Bullet>().owner.tag == owner.tag)) return;

        HitSomething(other);
    }


    private void FixedUpdate()
    {
        if (!owner) Destroy(gameObject);
        if (owner && owner.CompareTag("Player")) speed = 2;
        else speed = 1;
        //cast ray to next position
        gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.up * speed * 0.75f);
    }

    private void HitSomething(Collider2D other)
    {
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
        if (other.gameObject.CompareTag("Map") || other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Doorvrt") || other.gameObject.CompareTag("DoorFix") || other.gameObject.CompareTag("DoorvrtFix") || other.gameObject.CompareTag("Bullet")) Destroy(gameObject);
    }

    public void assingVar(float dmg,GameObject owner)
    {
        this.dmg = dmg;
        this.owner = owner;
    }

}

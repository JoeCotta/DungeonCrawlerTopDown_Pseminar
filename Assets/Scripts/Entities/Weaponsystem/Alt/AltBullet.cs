using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltBullet : MonoBehaviour
{
    private float dmg;
    private float speed;
    private GameObject owner;

    private bool isBossBullet;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject || !other.gameObject || !owner) return;

        HitSomething(other);
    }


    private void FixedUpdate()
    {
        if (!owner) Destroy(gameObject);
        if (!isBossBullet)
        {
            if (owner && owner.CompareTag("Player"))    speed = 3f;
            else if (GameManager.hardcoreMode)          speed = 1f;
            else                                        speed = 0.5f;
        }
        //cast ray to next position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, speed * 15f * Time.deltaTime);
        if(hit.collider != null)
        {
            HitSomething(hit.collider);
        }
        gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.up * speed * 15f * Time.deltaTime);
    }

    private void HitSomething(Collider2D other)
    {
        if (!gameObject || !other.gameObject || !owner) return;

        //if character apply dmg
        if ((owner.CompareTag("Player") && other.CompareTag("Enemy")) || (owner.CompareTag("Enemy") && other.CompareTag("Player")) )
        {
            other.gameObject.SendMessage("hit", dmg);
            Destroy(gameObject);
        }

        //if bullet of enemy delete
        if (other.CompareTag("Bullet") && other.GetComponent<AltBullet>() && other.GetComponent<AltBullet>().owner != null)
        {
            if (!other.GetComponent<AltBullet>().owner.CompareTag(owner.tag)) Destroy(gameObject);
        }
        //if map object delete
        switch (other.tag)
        {
            case "Map":
                Destroy(gameObject);
                break;
            case "Door":
                Destroy(gameObject);
                break;
            case "Doorvrt":
                Destroy(gameObject);
                break;
            case "DoorFix":
                Destroy(gameObject);
                break;
            case "DoorvrtFix":
                Destroy(gameObject);
                break;
        }
    }

    public void assingVar(float dmg,GameObject owner)
    {
        this.dmg = dmg;
        this.owner = owner;
        this.isBossBullet = false;
    }
    public void assignBossVar(float speed, Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        this.isBossBullet = true;
        this.speed = speed;
    }
}



/* if the collider is an enemy or the player it will apply damage to it
if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
{

    //only dmg if other gameobject isnt same type as object wielding the weapon (prevent friendly fire) by Cornell
    if (other.gameObject.tag != owner.tag)
    {
        other.gameObject.SendMessage("hit", dmg);
        // when hitting some1 it destroys itself
        Destroy(gameObject);
    }
}*/

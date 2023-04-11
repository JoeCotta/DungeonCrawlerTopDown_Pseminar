using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Camera cam;
    public float maxMovementSpeed; // 7
    public float acceleration; // 50
    public float dashForce; // 1000
    public float dashCooldown; // 2
    public float dashTime; // 0.15
    public float dashDamage; // 10
    public float weaponPickUpRadius;
    public float weaponDropForce; // 500
    public float health;
    public GameObject dashEffect;
    public Animator camShake;
    public GameObject weaponPrefab;

    private Transform weaponSlot; 
    private GameObject weapon;
    private Vector2 inputMovement;
    private Vector2 mousePosition;
    private Vector2 lookDir;
    private float angleToMouse;
    private float dashCooldownLeft;
    private float dashTimeLeft;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool isDead;

    void Start()
    {
        weaponSlot = transform.GetChild(0);
        weapon = Instantiate(weaponPrefab, weaponSlot.position, weaponSlot.rotation);        

        cam.orthographic = true;
        dashCooldownLeft = 0;
        dashTimeLeft = dashTime;
        isDashing = false;
        isDead = false;
    }

    void Update()
    {
        // getting Keyboard Input
        inputMovement = Input.GetKey("w") ? Vector2.up : Vector2.zero;
        inputMovement += Input.GetKey("a") ? Vector2.left : Vector2.zero;
        inputMovement += Input.GetKey("s") ? Vector2.down : Vector2.zero;
        inputMovement += Input.GetKey("d") ? Vector2.right : Vector2.zero;

        // getting mouse Input
        // transform the screen mouse Position to a world point
        mousePosition =  cam.ScreenToWorldPoint(Input.mousePosition);

        // calculating the angle between the player and the mouse
        lookDir = rb.position - mousePosition;
        angleToMouse = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // set the rotation of the player
        rb.SetRotation(angleToMouse + 90f);

        // dash
        // if dash cooldown is finished and dash-Key is pressed
        if (dashCooldownLeft <= 0 && Input.GetKey(KeyCode.LeftShift))
        {
            isDashing = true;
            dashCooldownLeft = dashCooldown;
            
            // dash particles
            Instantiate(dashEffect, rb.position, Quaternion.identity);

            // dashes in "Mouse direction"
            dashDirection = -lookDir.normalized;

            // dashes in "Keyboard direction"
            // dashDirection = inputMovement.normalized;        

            // starts camera shake animation
            camShake.SetBool("isDashing", true);

        }
        // decrease the cooldown
        else if(dashCooldownLeft > 0){dashCooldownLeft -= Time.deltaTime;}

        // while dashing
        if(isDashing && dashTimeLeft > 0)
        {
            dash();
            dashTimeLeft -= Time.deltaTime;
        }
        // if dash is over
        else if(dashTimeLeft <= 0)
        {
            // stops camera shake animation
            camShake.SetBool("isDashing", false);

            dashTimeLeft = dashTime;
            isDashing = false;
        }

        // update the position and rotation of the weapon if the player has one
        if(weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = weaponSlot.rotation * Quaternion.Euler(0, 0, -90);;
        }

        // shoot if clicked
        if(Input.GetMouseButton(0)) shoot();

        // pickup / swap weapon
        if(Input.GetKeyDown("f")) swapWeapons();

    }

    void FixedUpdate()
    {
        // calculating the velocity the rb should have
        Vector2 targetVelocity = inputMovement * maxMovementSpeed;
        // difference between the velocity the rb should have and the actual one
        Vector2 velocityDifference = targetVelocity - rb.velocity;
        // F = m*a (m=1) and a = v/t (t=1) => F = v 
        // ==> force is the velocity difference multiplied by an optional factor to speed up and brake faster
        Vector2 force = velocityDifference * acceleration;
        rb.AddForce(force);
    }

    void dash(){
        rb.AddForce(dashForce * dashDirection);
    }

    void hit(float damage)
    {
        // player can't be hit while dashing
        if (isDashing) return;
        
        health -= damage;
        
        if(health <= 0)
        {
            isDead = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if player hits an enemy while dashing it deals damage
        if (collision.gameObject.tag == "Enemy" && isDashing)
        {
            collision.gameObject.SendMessage("hit", dashDamage);
        }
    }

    void shoot()
    {
        // if the player has no weapon you can't shoot
        if(!weapon) return;

        weapon.SendMessage("shoot");
    }

    void swapWeapons()
    {
        GameObject bestWeapon = null;
        // as a start value the set radius plus a little bit is good that you can find a weapon which is exactly on and not in the radius 
        float lowestWeaponDistance = weaponPickUpRadius + 10;

        // finds every weapon in the game
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("weapon");
        
        // finds the weapon which is the nearest to the player
        foreach(GameObject foundWeapon in weapons)
        {
            // if its not the players weapon
            if(foundWeapon == weapon) continue;

            float weaponDistance = (rb.position - new Vector2(foundWeapon.transform.position.x, foundWeapon.transform.position.y)).magnitude;
            if (weaponDistance < lowestWeaponDistance)
            {
                bestWeapon = foundWeapon;
                lowestWeaponDistance = weaponDistance;
            }
        }
        // gives the "old" weapon a force to kick to weapon away - a drop animation
        if (weapon) weapon.GetComponent<Rigidbody2D>().AddForce(-lookDir.normalized * weaponDropForce);

        // if there is no weapon to pickup
        if(bestWeapon == null || lowestWeaponDistance > weaponPickUpRadius) weapon = null;
        // change the weapons
        else weapon = bestWeapon;

    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D rb;
    public Rigidbody2D rbPlayer;
    public float maxMovementSpeed; // 7
    public float acceleration; // 50
    public float countdownFollowPlayerIfRayCastFails;
    public float checkAntiCollisionRadius;
    public float radiusEnemySeesPlayer;
    public GameObject weaponPrefab;

    private Transform weaponSlot;
    private Vector2 inputMovement;
    private Vector2 toPlayer;
    private Vector2 playerLastSeen;
    private float countdownLeft;
    private GameObject weapon;
    private RaycastHit2D checkCanSeePlayer;

    void Start()
    { 
        rb.rotation = 0;
        weaponSlot = transform.GetChild(0);
        weapon = Instantiate(weaponPrefab, weaponSlot.position, weaponSlot.rotation);        

    }

    void Update()
    { 

        checkCanSeePlayer = Physics2D.Raycast(transform.position, (rbPlayer.position - rb.position).normalized, radiusEnemySeesPlayer);

        

        moveDirection(rbPlayer.position);
        moveController();
        move();

        if(countdownLeft > 0) countdownLeft -= Time.deltaTime;
    
        // update the position and rotation of the weapon if the player has one
        if(weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = weaponSlot.rotation * Quaternion.Euler(0, 0, -90);
        }
    }

    void move()
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

    void moveDirection(Vector2 position)
    {        
        // calculates the Vector between the enemy and the player
        // if the player can't be seen and the countdown isn't over the enemy will walk to the last seen position of the player
        // if the countdown is over the enemy won't follow the player
        if (!checkCanSeePlayer || checkCanSeePlayer.collider.transform.gameObject.layer != LayerMask.NameToLayer("Map"))
        {
            toPlayer = (position - rb.position).normalized;
            playerLastSeen = position;
            countdownLeft = countdownFollowPlayerIfRayCastFails;
        }
        else if(countdownLeft > 0)
        {
            toPlayer = (playerLastSeen - rb.position).normalized;
        }
        else
        {
            toPlayer = Vector2.zero;
        }
    }

    void moveController()
    {
        // avoid hitting obstacles, enemies or the player
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, checkAntiCollisionRadius);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, checkAntiCollisionRadius);

        RaycastHit2D hitUpRight = Physics2D.Raycast(transform.position, transform.up + transform.right, checkAntiCollisionRadius);
        RaycastHit2D hitUpLeft = Physics2D.Raycast(transform.position, transform.up - transform.right, checkAntiCollisionRadius);

        inputMovement = Vector2.zero;

        // if there is an obstacle in the enemy's path it will move around
        if (hitRight) inputMovement -= new Vector2(transform.right.x, transform.right.y) * 0.5f;
        if (hitLeft) inputMovement += new Vector2(transform.right.x, transform.right.y) * 0.5f;
        if (hitUpRight) inputMovement -= new Vector2(transform.right.x, transform.right.y) * 0.25f;
        if (hitUpLeft) inputMovement += new Vector2(transform.right.x, transform.right.y) * 0.25f;

        // if the player is in a certain range the enemy will move to him
        if((rbPlayer.position - rb.position).sqrMagnitude >= 7 * 7)
        {
            inputMovement = toPlayer;
        }
        


        // manages the rotation of the enemy
        if(toPlayer == Vector2.zero) return;
        float angleToPlayer = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 7);

    }

}

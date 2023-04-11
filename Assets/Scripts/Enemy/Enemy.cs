using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float nextWaypointDistance;
    public GameObject weaponPrefab;

    private Transform weaponSlot; 
    private GameObject weapon;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    bool follow = true;

    void Start()
    {   
        // scans the Map to create Obstacles for the AI 1s after the program started
        InvokeRepeating("ScanMap", 1f, 0f);

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // updates the Path every half seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        // initialises the weaponSystem
        weaponSlot = transform.GetChild(0);
        weapon = Instantiate(weaponPrefab, weaponSlot.position, weaponSlot.rotation);
        
    }
    void ScanMap(){
        AstarPath.active.Scan();
    }

    void UpdatePath(){
        if(seeker.IsDone()) seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // if the next part of the path is generated
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void Update()
    {
        // update the position and rotation of the weapon if the enemy has one
        if(weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = weaponSlot.rotation * Quaternion.Euler(0, 0, -90);;
        }

        if(path == null) return;

        // check if enemy can hit the target (maybe a wall blocks the shot)
        RaycastHit2D hit = Physics2D.Raycast(rb.position, ((Vector2)target.position - rb.position).normalized);

        float DistanceToTarget = path.GetTotalLength();

        // if the Distance to the target is lower than 4 the enemy shouldn't follow the target, instead he should shoot at the target
        if (DistanceToTarget < 6 ) follow = false;
        // if the Distance to the target is grater than 8 the enemy should follow the target
        // or the enemy's shot is blocked -> should rather follow the target
        if (DistanceToTarget > 8 || hit.collider.gameObject.tag != "Player") follow = true;
    }

    void FixedUpdate()
    {
        // shoot if the target is close enough
        if (!follow) {shoot(); return;}
        if(path == null) return;
        
        // checks if the end of the path is reached
        if(currentWaypoint >= path.vectorPath.Count) return;
 
        // calculates the force to follow the path
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime * 10;

        rb.AddForce(force);

        // manages rotation while following target
        float angleNextWaypoint = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * angleNextWaypoint);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 7);

        // updates the current Waypoint        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }
    
    void shoot()
    {
        // manages rotation while shooting
        Vector2 PlayerDirection = ((Vector2)target.position - rb.position).normalized;
        float angleToPlayer = Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 7);


        // if the enemy has no weapon he can't shoot
        if(!weapon) return;

        weapon.SendMessage("shoot");
    }
    void hit(float damage){}

}

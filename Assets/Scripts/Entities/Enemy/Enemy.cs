using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{
    public int maxGold; //Cornell
    public RoomManagment manager;//""
    //public bool useAlternate;

    public Transform target;
    public float speed;
    public float nextWaypointDistance;
    public GameObject weaponPrefab;
    public float health;
    // level 0 is the worst armor and 10 is the best
    public int armorLevel = 0; 


    private Transform weaponSlot; 
    private GameObject weapon;
    public GameObject AWeapon; // alternate weapon

    private Path path;
    private int currentWaypoint = 0;

    private Seeker seeker;
    private Rigidbody2D rb;

    private bool follow = true;
    private bool outOfRange = true;
    public bool isDead = false;


    void Start()
    {   
        //get player Transform by Cornell
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameManager.player.transform;
        
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // updates the Path every half seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        // initialises the weaponSystem (random weapon)
        weaponSlot = transform.GetChild(0);

        // gets the amount of weapons which are available         
        int countWeapons = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().countWeapons;

        // selects a random weapon type
        int weaponType = Random.Range(0, countWeapons);

        // creates the weapon
        if (!GameManager.useAlt)
        {
            weapon = Instantiate(weaponPrefab, weaponSlot.position, weaponSlot.rotation);
            weapon.GetComponent<weaponsystem>().weaponType = weaponType; // sets the weaponType
            weapon.GetComponent<weaponsystem>().owner = gameObject;
        }
        else
        {
            weapon = Instantiate(AWeapon, weaponSlot.position, weaponSlot.rotation);
            weapon.GetComponent<AlternateWS>().weaponType = weaponType;
            weapon.GetComponent<AlternateWS>().owner = gameObject;
        }

        armorLevel = Random.Range(0, 11);

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
        if (GameManager.isPaused) return;

        // update the position and rotation of the weapon if the enemy has one
        if(weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = weaponSlot.rotation * Quaternion.Euler(0, 0, 90); // cornell same as with player
        }

        if(path == null) return;

        // check if enemy can hit the target (maybe a wall blocks the shot)
        RaycastHit2D hit = Physics2D.Raycast(rb.position, ((Vector2)target.position - rb.position).normalized);

        float DistanceToTarget = path.GetTotalLength();

        // if the player is too far away
        if (DistanceToTarget > 30) outOfRange = true;
        else outOfRange = false;

        // if the Distance to the target is grater than 8 the enemy should follow the target
        // or the enemy's shot is blocked -> should rather follow the target
        if (DistanceToTarget > 8 || hit.collider.gameObject.tag != "Player") follow = true;

        // if the Distance to the target is lower than 4 or out of the player's range the enemy shouldn't follow the target, instead he should shoot at the target
        // but only if the bullet will hit the Player
        if ((DistanceToTarget < 6 || outOfRange) && hit.collider.gameObject.tag == "Player") follow = false;
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
        // if the enemy has no weapon he can't shoot
        if(!weapon) return;

        // if the player is too far away
        if(outOfRange) return;

        // manages rotation while shooting
        Vector2 PlayerDirection = ((Vector2)target.position - rb.position).normalized;
        float angleToPlayer = Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 7);

        weapon.SendMessage("shoot");
    }
    void hit(float damage)
    {
        // this function -0.08x + 1 reduces the damage depending on the armor level
        damage *=  (float)-0.08 * armorLevel + 1;
        health -= damage;
        
        if(health <= 0)
        {
            isDead = true;
            onDeath();
        }
    }

    void onDeath()
    {
        Destroy(weapon);
        target.gameObject.GetComponent<Player>().playerGold += Mathf.Round(Random.Range(0,maxGold));//Cornell 
        target.gameObject.GetComponent<Player>().enemyKilled();
        manager.killEnemy(gameObject); //Cornell; manually deleting enemey elsewise error
    }

}
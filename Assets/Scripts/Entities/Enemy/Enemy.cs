using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using JetBrains.Annotations;

public class Enemy : MonoBehaviour
{
    public RoomManagment manager;//""
    public Boss boss;
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

    public GameObject boostPrefab;

    [SerializeField] private AudioSource hitSound;

    // sprites
    public Sprite sprite_front_left;
    public Sprite sprite_back;
    public Sprite sprite_front_right;

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


        if(path == null) return;

        // check if enemy can hit the target (maybe a wall blocks the shot)
        RaycastHit2D hit = Physics2D.Raycast(rb.position, ((Vector2)target.position - rb.position).normalized);

        float DistanceToTarget = path.GetTotalLength();

        // if the player is too far away
        if (DistanceToTarget > 30) outOfRange = true;
        else outOfRange = false;

        // if the Distance to the target is grater than 8 the enemy should follow the target
        // or the enemy's shot is blocked -> should rather follow the target
        if (DistanceToTarget > 8 || (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy")) ) follow = true;

        // if the Distance to the target is lower than 4 or out of the player's range the enemy shouldn't follow the target, instead he should shoot at the target
        // but only if the bullet will hit the Player
        if ((DistanceToTarget < 6 || outOfRange) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy")) ) follow = false;
    


        // manages rotation while following target
        Vector2 direction = ((Vector2)gameObject.GetComponent<BulletCalc>().CalcPath(15f,target.gameObject) - rb.position).normalized;
        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -180f;
        

        // update the position and rotation of the weapon if the enemy has one
        if(weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer + 180f); // cornell same as with player
        }

        // -30 - 90   front left
        // -30 - -150 back
        // 90 - -150 front-right
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (angleToPlayer > -30 && angleToPlayer < 90) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_left;
        else if ((angleToPlayer > 90 && angleToPlayer <= 180) || (angleToPlayer >= -180 && angleToPlayer < -150)) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_right;
        else if (angleToPlayer < -30 && angleToPlayer > -150){
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite_back;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }

        //flip weapon sprite
        if (weapon){
            // left side
            if (angleToPlayer < 90 && angleToPlayer >= -90) weapon.GetComponent<SpriteRenderer>().flipY = true;
            //right side
            else if((angleToPlayer >= 90 && angleToPlayer <= 180) || (angleToPlayer >= -180 && angleToPlayer < -90)) weapon.GetComponent<SpriteRenderer>().flipY = false;
        }

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
        Vector2 PlayerDirection = ((Vector2)gameObject.GetComponent<BulletCalc>().CalcPath(15f,target.gameObject) - rb.position).normalized;
        //Debug.Log(gameObject.GetComponent<BulletCalc>().CalcPath(15f, target.gameObject));
        float angleToPlayer = Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 7);

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
        else hitSound.Play();
    }

    void onDeath()
    {
        int dropWp = Random.Range(1, DataBase.weaponDropChance);  
        if (dropWp != 1)
        {
            Destroy(weapon);
            int dropBoost = Random.Range(1, DataBase.boostDropChance);
            if (dropBoost == 1) Instantiate(boostPrefab, transform.position, Quaternion.identity);
        }
        target.gameObject.GetComponent<Player>().playerGold += Mathf.Round(Random.Range(0,DataBase.maxGold));//Cornell 
        target.gameObject.GetComponent<Player>().enemyKilled();
        if (manager) manager.killEnemy(gameObject); //Cornell; manually deleting enemey elsewise error
        else if(boss) boss.killEnemy(gameObject);
        else Destroy(gameObject);

    }
}
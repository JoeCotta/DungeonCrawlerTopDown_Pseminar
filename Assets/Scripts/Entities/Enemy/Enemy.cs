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
    public bool EnemyUsePrediction;

    private Path path;
    private int currentWaypoint = 0;
    private float angleToPlayer;

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

    [SerializeField] private GameObject coinSpawnerPrefab;
    int weaponType;

    float shootRange;
    float followRange;

    void Start()
    {   
        if(GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>()){
            DataPersistenceManager dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();

            // difficulty
            health *=  DifficultyTracker.healthMultiplier;
            speed *= DifficultyTracker.speedMultiplier;
        }

        //get player Transform by Cornell
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameManager.player.transform;
        
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // updates the Path every half seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        // initialises the weaponSystem (random weapon)
        weaponSlot = transform.GetChild(0);

        // selects a random weapon type
        weaponType = Random.Range(0, 3);

        // creates the weapon
        weapon = Instantiate(AWeapon, weaponSlot.position, weaponSlot.rotation);
        weapon.GetComponent<AlternateWS>().weaponType = weaponType;
        weapon.GetComponent<AlternateWS>().owner = gameObject;

        if (Random.value <= 0.8f) EnemyUsePrediction = true;
        else EnemyUsePrediction = false;


        armorLevel = Random.Range(0, 11);

        // sets the shoot / follow range depending on the weapon
        switch(weaponType)
        {
            case 0:
                shootRange = 6;
                followRange = 8;
                break;
            case 1:
                shootRange = 9;
                followRange = 11;
                break;
            case 2:
                shootRange = 11;
                followRange = 13;
                break;
        }

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
        if (DistanceToTarget > followRange || (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy")) ) follow = true;

        // if the Distance to the target is lower than 6 or out of the player's range the enemy shouldn't follow the target, instead he should shoot at the target
        // but only if the bullet will hit the Player
        if ((DistanceToTarget < shootRange || outOfRange) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy")) ) follow = false;


        // manages rotation while following target
        Vector2 direction;
        if(EnemyUsePrediction) direction = ((Vector2)gameObject.GetComponent<BulletCalc>().CalcPath(15f,target.gameObject) - rb.position).normalized;
        else direction = ((Vector2)target.gameObject.transform.position - rb.position).normalized;
        angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -180f;
        

        // update the position and rotation of the weapon if the enemy has one
        if(weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer + 180f); // cornell same as with player
        }

        updateSprite();
                
        //flip weapon sprite
        if (weapon){
            // left side
            if ((angleToPlayer > -90 && angleToPlayer <= 0) || (angleToPlayer < -270 && angleToPlayer >= -360)) weapon.GetComponent<SpriteRenderer>().flipY = true;
            //right side
            else if(angleToPlayer <= -90 && angleToPlayer >= -270) weapon.GetComponent<SpriteRenderer>().flipY = false;
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

        // manages rotation while shooting 50 % chance to use prediction
        Vector2 PlayerDirection;
        PlayerDirection = ((Vector2)gameObject.GetComponent<BulletCalc>().CalcPath(7.5f * DifficultyTracker.bulletSpeedMultiplier, target.gameObject) - rb.position).normalized;

        float angleToPlayer = Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);

        weapon.SendMessage("shoot");
    }
    void hit(float damage)
    {
        // this function -0.08x + 1 reduces the damage depending on the armor level
        damage *=  (float)-0.08 * armorLevel + 1;
        damage *= DifficultyTracker.damageReduceFactor;
        health -= damage;

        
        if(health <= 0)
        {
            isDead = true;
            onDeath();
        }
        else hitSound.Play();
    }

    void updateSprite()
    {
        if(GameManager.enableSusMode)
        {
            updateSusSprite();
            return;
        }
        // -270 - -30   front left
        // -30 - -150 back
        // -270 - -150 front-right
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (angleToPlayer < -150 && angleToPlayer > -270) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_right;
        else if ((angleToPlayer > -30 && angleToPlayer <= 0) || (angleToPlayer <= -270 && angleToPlayer > -360)) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_left;
        else if (angleToPlayer < -30 && angleToPlayer > -150)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite_back;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }

    void updateSusSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (angleToPlayer < -90 && angleToPlayer > -270) gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.sus_Right;
        else if ((angleToPlayer > -90 && angleToPlayer <= 0) || (angleToPlayer <= -270 && angleToPlayer > -360)) gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.sus_Left;
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
        // sniper can't be dropped
        if(weapon.GetComponent<AlternateWS>().weaponType == 2) Destroy(weapon);
        
        // target.gameObject.GetComponent<Player>().playerGold += Mathf.Round(Random.Range(0,DataBase.maxGold));//Cornell 
        GameObject coinSpawner = Instantiate(coinSpawnerPrefab, transform.position, Quaternion.identity);
        coinSpawner.GetComponent<CoinSpawner>().amountCoins = (int)Mathf.Round(Random.Range(0,DataBase.maxGold));

        target.gameObject.GetComponent<Player>().enemyKilled();
        if (manager) manager.killEnemy(gameObject); //Cornell; manually deleting enemey elsewise error
        else if(boss) boss.killEnemy(gameObject);
        else Destroy(gameObject);

    }
}
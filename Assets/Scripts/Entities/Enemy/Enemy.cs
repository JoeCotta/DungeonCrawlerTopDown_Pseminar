using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using JetBrains.Annotations;

public class Enemy : MonoBehaviour
{
    public RoomManagment manager;//""
    public Boss boss;
    public int enemyTier;
    //1: Zombie
    //2: Normal
    //3: stronger

    public Transform target;
    public float speed, health;
    public float nextWaypointDistance;
    [SerializeField] float interval, timer, dmg;
    public GameObject weaponPrefab;
    // level 0 is the worst armor and 10 is the best
    public int armorLevel = 0;


    private Transform weaponSlot;
    public GameObject weapon;
    public GameObject AWeapon; // alternate weapon

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
    [SerializeField] private GameObject armorPrefab;
    [SerializeField] private float armorDropForce;

    void Start()
    {
        target = GameManager.player.transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // updates the Path every half seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        armorLevel = Random.Range(0, 11);

        // initialises the weaponSystem (random weapon)
        weaponSlot = transform.GetChild(0);

        /*if (Random.value <= 0.8f) EnemyUsePrediction = true;
        else EnemyUsePrediction = false;*/

        // selects a random weapon type
        weaponType = Random.Range(0, 3);

        if (enemyTier != 1)
        {
            // creates the weapon if not zombie
            weapon = Instantiate(AWeapon, weaponSlot.position, weaponSlot.rotation);
            weapon.GetComponent<AlternateWS>().weaponType = weaponType;
            weapon.GetComponent<AlternateWS>().owner = gameObject;
            interval = 1 / DataBase.weaponBase(weapon.GetComponent<AlternateWS>().weaponType, weapon.GetComponent<AlternateWS>().rarity)[3];
        }

        float[] assingValues = DataBase.enemyBase(enemyTier, weaponType);
        shootRange = assingValues[0];
        followRange = assingValues[1];
        health = assingValues[2];
        speed = assingValues[3];

        if (enemyTier == 1)
        {
            dmg = assingValues[4];
            interval = 1 / assingValues[5];
        }

        if (GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>())
        {
            DataPersistenceManager dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();

            // difficulty
            health *= DifficultyTracker.healthMultiplier;
            speed *= DifficultyTracker.speedMultiplier;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone()) seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // if the next part of the path is generated
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void Update()
    {
        if (GameManager.isPaused) return;


        if (path == null) return;

        // check if enemy can hit the target (maybe a wall blocks the shot)
        RaycastHit2D hit = Physics2D.Raycast(rb.position, ((Vector2)target.position - rb.position).normalized);

        float DistanceToTarget = path.GetTotalLength();

        // if the player is too far away
        if (DistanceToTarget > 30) outOfRange = true;
        else outOfRange = false;

        // if the Distance to the target is grater than 8 the enemy should follow the target
        // or the enemy's shot is blocked -> should rather follow the target
        if (DistanceToTarget > followRange || (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy"))) follow = true;

        // if the Distance to the target is lower than 6 or out of the player's range the enemy shouldn't follow the target, instead he should shoot at the target
        // but only if the bullet will hit the Player
        else if ((DistanceToTarget < shootRange || outOfRange) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))) follow = false;


        // manages weapon rotation while following target
        Vector2 direction;
        direction = ((Vector2)target.gameObject.transform.position - rb.position).normalized;
        angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;

        if (weapon)
        {
            weapon.transform.position = weaponSlot.position;
            if (follow || timer < interval) weapon.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer + 180f); // cornell same as with player
        }

        //flip weapon sprite
        if (weapon)
        {
            // left side
            if ((angleToPlayer > -90 && angleToPlayer <= 0) || (angleToPlayer < -270 && angleToPlayer >= -360)) weapon.GetComponent<SpriteRenderer>().flipY = true;
            //right side
            else if (angleToPlayer <= -90 && angleToPlayer >= -270) weapon.GetComponent<SpriteRenderer>().flipY = false;
        }


        timer += Time.deltaTime;

        updateSprite();
    }

    void FixedUpdate()
    {
        // shoot if the target is close enough
        if (!follow) { shoot(); return; }
        if (path == null) return;

        // checks if the end of the path is reached
        if (currentWaypoint >= path.vectorPath.Count) return;

        // calculates the force to follow the path
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime * 10;

        rb.AddForce(force);


        // updates the current Waypoint        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    void shoot()
    {
        // if the enemy has no weapon he can't shoot
        if (!weapon || outOfRange || enemyTier == 1) return;

        if (timer < interval) return;

        while (timer >= interval)
        {
            // manages rotation while shooting
            Vector2 direction;
            if (Random.value < 0.8f) direction = ((Vector2)gameObject.GetComponent<BulletCalc>().CalcPath(7.5f * DifficultyTracker.bulletSpeedMultiplier, target.gameObject) - rb.position).normalized;
            else direction = ((Vector2)target.gameObject.transform.position - rb.position).normalized;
            angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer);

            weapon.SendMessage("shoot");
            timer -= interval;
        }
    }
    void hit(float damage)
    {
        // this function -0.08x + 1 reduces the damage depending on the armor level
        damage *= (float)-0.08 * armorLevel + 1;
        damage *= DifficultyTracker.damageReduceFactor;
        health -= damage;


        if (health <= 0)
        {
            isDead = true;
            onDeath();
        }
        else hitSound.Play();
    }

    void updateSprite()
    {
        if (GameManager.enableSusMode)
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
        if (weapon && weapon.GetComponent<AlternateWS>().weaponType == 2) Destroy(weapon);

        // target.gameObject.GetComponent<Player>().playerGold += Mathf.Round(Random.Range(0,DataBase.maxGold));//Cornell 
        GameObject coinSpawner = Instantiate(coinSpawnerPrefab, transform.position, Quaternion.identity);
        coinSpawner.GetComponent<CoinSpawner>().amountCoins = (int)Mathf.Round(Random.Range(0, DataBase.maxGold));

        target.gameObject.GetComponent<Player>().enemyKilled();
        if (manager) manager.killEnemy(gameObject); //Cornell; manually deleting enemey elsewise error
        else if (boss) boss.killEnemy(gameObject);
        else Destroy(gameObject);


        // drops the armor if its level is not 0 and lower than 5
        // chance to drop: 1/7
        int random = Random.Range(0, 7);
        if (armorLevel != 0 && armorLevel < 5 && random == 0)
        {
            // creates the armour
            GameObject oldArmour = Instantiate(armorPrefab, transform.position, Quaternion.identity);

            // sets the level of the armour
            oldArmour.SendMessage("setLevel", armorLevel);

            // gives the "old" armour a force to kick the armour away - a drop animation
            oldArmour.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * armorDropForce);
        }


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (timer < interval || collision.gameObject.tag == gameObject.tag) return;
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.SendMessage("hit", dmg);
            timer = 0;
        }
    }
}
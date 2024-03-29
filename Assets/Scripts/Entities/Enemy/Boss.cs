using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Boss : MonoBehaviour
{

    // stats
    [Header("Stats")]
    [SerializeField] private float speed;
    public float maxHealth;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float bulletDamage;
    private float damageReduceFactor;
    private float bulletSpeedMultiplier;


    // references
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Sprite bulletSprite;
    [HideInInspector] public RoomManagment manager;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private GameObject UIBossBar;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject coinSpawnerPrefab;
    List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField] private GameObject armorPrefab;
    [SerializeField] private float armorDropForce;

    // Sounds
    [Header("Sounds")]
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource enemySpawnSound;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource killSound;

    // Textures
    [Header("Textures")]
    [SerializeField] private Sprite sprite_front_left;
    [SerializeField] private Sprite sprite_back;
    [SerializeField] private Sprite sprite_front_right;
    

    // pathfinding
    private Path path;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 1;
    private Seeker seeker;

    [Header("Attack Settings")]
    // attack
    [SerializeField] private float timeBetweenAttacks;
    private float currentTime;

    // attackSpawnEnemies
    [Header("attackSpawnEnemies")]
    [SerializeField] private int maxCountSpawnEnemiesAttack;

    // attackShootInCircle
    [Header("attackShootInCircle")]
    [SerializeField] private float  attackShootInCircleAngleBetweenBullets;
    [SerializeField] private float attackShootInCircleBulletSpeed;

    // attackSpamShootInCircle
    [Header("attackSpamShootInCircle")]
    [SerializeField] private int attackSpamShootInCircleCountWaves;
    [SerializeField] private float  attackSpamShootInCircleAngleBetweenBullets;
    [SerializeField] private float attackSpamShootInCircleBulletSpeed;
    [SerializeField] private float attackSpamShootInCircleAngleOffset;
    [SerializeField] private float attackSpamShootInCircleTimeBetweenWaves;
    private float attackSpamShootInCircleCurrentAngleOffset;
    private int attackSpamShootInCircleCount;
    

    // attackBurstShoot
    [Header("attackBurstShoot")]
    [SerializeField] private float attackBurstShootAngleOffsetToPlayer;
    [SerializeField] private float attackBurstShootBulletSpeed;
    [SerializeField] private float attackBurstShootTimeBetweenShots;
    [SerializeField] private int attackBurstShootCountBulletsPerAttack;
    private float attackBurstShootCurrentAngle;
    private int attackBurstShootCount;
    private float attackBurstShootAngleBetweenBullets;


    // attackBurstShootCircle
    [Header("attackBurstShootCircle")]
    [SerializeField] private int attackBurstShootCircleCountBulletsPerWave;
    [SerializeField] private float attackBurstShootCircleBulletSpeed;
    [SerializeField] private int attackBurstShootCircleCountWaves;
    
    [SerializeField] private float attackBurstShootCircleTimeBetweenWaves;
    private int attackBurstShootCircleCount;




    void Start()
    {
        // updates the Path every half seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        playerTransform = GameManager.player.transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        UIBossBar = GameObject.Find("Bossbar").transform.GetChild(0).gameObject;
        UIBossBar.GetComponent<BossBar>().boss = this;
        UIBossBar.SetActive(true);
        UIBossBar.GetComponent<BossBar>().isBoss = true;

        // difficulty
        if(GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>()){
            DataPersistenceManager dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();

            // difficulty
            bulletSpeedMultiplier = DifficultyTracker.bulletSpeedMultiplier;
            bulletDamage = DifficultyTracker.dmgMultiplier;
            damageReduceFactor = DifficultyTracker.damageReduceFactor;
            timeBetweenAttacks =  Mathf.Clamp(timeBetweenAttacks - DifficultyTracker.bossTimeBetweenAttacksReduceTime, 1.5f, timeBetweenAttacks);
            maxHealth *= DifficultyTracker.healthMultiplier;
        }
        currentHealth = maxHealth;
    }
    
    void UpdatePath()
    {
        if(seeker.IsDone()) seeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate() 
    {
        if(path == null) return;
        
        // checks if the end of the path is reached
        if(currentWaypoint >= path.vectorPath.Count) return;

        // calculates the force to follow the path
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime * 10000;

        rb.AddForce(force);

        
        // updates the current Waypoint        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void hit(float damage)
    {
        damage = Mathf.Max(damage * damageReduceFactor, 1);

        currentHealth -= damage;
        if (currentHealth <= 0) onDeath();
        else hitSound.Play();
    }

    void onDeath()
    {
        GameManager.isBossSpawned = false;
        UIBossBar.GetComponent<BossBar>().isBoss = true;
        UIBossBar.SetActive(false);
        Destroy(gameObject);

        // kill all spawned enemies
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            Destroy(spawnedEnemies[i].GetComponent<Enemy>().weapon);
            Destroy(spawnedEnemies[i]);
        }


        // loottable


        // 80/1000 rare
            // 60 / 100 pistol
            // 30 / 100 rifle
            // 10/100 sniper
        // 20/1000 epic
            // 60 / 100 pistol
            // 30 / 100 rifle
            // 10/100 sniper
        // 100/1000 legendary
            // 60 / 100 pistol
            // 30 / 100 rifle
            // 10/100 sniper

        // 200/1000 50 coins
        // 100/1000 100 coins
        // 50/1000  200 coins
        // 5/1000 500 coins

        // 200 / 1000 armor lvl 8
        // 100 / 1000 armor lvl 9
        // 50 / 1000 armor lvl 10

        int randomNumber = Random.Range(0, 1000);
        // 80/1000 rare
        if (randomNumber >= 0 && randomNumber < 80) 
        {
            int randomNumber2 = Random.Range(0, 100);
            AlternateWS weapon = Instantiate(weaponPrefab, manager.gameObject.transform.position, Quaternion.identity).GetComponent<AlternateWS>();
            weapon.rarity = 1;

            // 60 / 100 pistol
            if (randomNumber >= 0 && randomNumber < 60) weapon.weaponType = 0;
            // 30 / 100 rifle
            if (randomNumber >= 60 && randomNumber < 90) weapon.weaponType = 1;
            // 10/100 sniper
            if (randomNumber >= 90 && randomNumber < 100) weapon.weaponType = 2;
        }
        // 20/1000 epic
        if (randomNumber >= 80 && randomNumber < 100) 
        {
            int randomNumber2 = Random.Range(0, 100);
            AlternateWS weapon = Instantiate(weaponPrefab, manager.gameObject.transform.position, Quaternion.identity).GetComponent<AlternateWS>();
            weapon.rarity = 2;

            // 60 / 100 pistol
            if (randomNumber >= 0 && randomNumber < 60) weapon.weaponType = 0;
            // 30 / 100 rifle
            if (randomNumber >= 60 && randomNumber < 90) weapon.weaponType = 1;
            // 10/100 sniper
            if (randomNumber >= 90 && randomNumber < 100) weapon.weaponType = 2;
        }
        // 100/1000 legendary
        if (randomNumber >= 100 && randomNumber < 200) 
        {
            int randomNumber2 = Random.Range(0, 100);
            AlternateWS weapon = Instantiate(weaponPrefab, manager.gameObject.transform.position, Quaternion.identity).GetComponent<AlternateWS>();
            weapon.rarity = 3;

            // 40 / 100 pistol
            if (randomNumber >= 0 && randomNumber < 40) weapon.weaponType = 0;
            // 30 / 100 rifle
            if (randomNumber >= 40 && randomNumber < 60) weapon.weaponType = 1;
            // 20 / 100 golden ak
            if (randomNumber >= 60 && randomNumber < 80) weapon.weaponType = 3;
            // 10 / 100 minigun
            if (randomNumber >= 80 && randomNumber < 90) weapon.weaponType = 4;
            // 10/100 sniper
            if (randomNumber >= 90 && randomNumber < 100) weapon.weaponType = 2;
        }

        // 200/1000 50 coins
        if (randomNumber >= 200 && randomNumber < 405) 
        {
            GameObject coinSpawner = Instantiate(coinSpawnerPrefab, manager.gameObject.transform.position, Quaternion.identity);
            coinSpawner.GetComponent<CoinSpawner>().amountCoins = 50;
        }
        // 100/1000 100 coins
        if (randomNumber >= 405 && randomNumber < 505) 
        {
            GameObject coinSpawner = Instantiate(coinSpawnerPrefab, manager.gameObject.transform.position, Quaternion.identity);
            coinSpawner.GetComponent<CoinSpawner>().amountCoins = 100;
        }
        // 50/1000  200 coins
        if (randomNumber >= 505 && randomNumber < 555) 
        {
            GameObject coinSpawner = Instantiate(coinSpawnerPrefab, manager.gameObject.transform.position, Quaternion.identity);
            coinSpawner.GetComponent<CoinSpawner>().amountCoins = 200;
        }
        // 5/1000 500 coins
        if (randomNumber >= 555 && randomNumber < 560) 
        {
            GameObject coinSpawner = Instantiate(coinSpawnerPrefab, manager.gameObject.transform.position, Quaternion.identity);
            coinSpawner.GetComponent<CoinSpawner>().amountCoins = 500;
        }

        // 200 / 1000 armor lvl 8
        if (randomNumber >= 560 && randomNumber < 760) spawnArmor(8);

        // 100 / 1000 armor lvl 9
        if (randomNumber >= 760 && randomNumber < 860) spawnArmor(9);

        // 50 / 1000 armor lvl 10
        if (randomNumber >= 860 && randomNumber < 910) spawnArmor(10);

        manager.roomFinished();
    }

    void spawnArmor(int lvl)
    {
        // creates the armour
        GameObject oldArmour = Instantiate(armorPrefab, manager.gameObject.transform.position, Quaternion.identity);

        // sets the level of the armour
        oldArmour.SendMessage("setLevel", lvl);

        // gives the "old" armour a force to kick the armour away - a drop animation
        oldArmour.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * armorDropForce);
    }

    void Update()
    {
        if (currentTime < timeBetweenAttacks) currentTime += Time.deltaTime;
        else
        {
            attack();
            currentTime = 0;
        }

        updateSprite();
    }

    void updateSprite()
    {
        if(GameManager.enableSusMode)
        {
            updateSusSprite();
            return;
        }
        Vector2 vectorToPlayer = ((Vector2)playerTransform.position - rb.position).normalized;
        float angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;

        if (angleToPlayer > -90 && angleToPlayer < 30) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_right;
        else if ((angleToPlayer > 150 && angleToPlayer <= 180) || (angleToPlayer >= -180 && angleToPlayer <= -90)) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_left;
        else if (angleToPlayer > 30 && angleToPlayer < 150) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_back;
    }

    void updateSusSprite()
    {
        Vector2 vectorToPlayer = ((Vector2)playerTransform.position - rb.position).normalized;
        float angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;

        if (angleToPlayer > -90 && angleToPlayer < 90) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_right;
        else if ((angleToPlayer > 90 && angleToPlayer <= 180) || (angleToPlayer >= -180 && angleToPlayer <= -90)) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_left;
    }

    void attack()
    {
        int attack = Random.Range(0, 5);
        switch(attack)
        {
            case 0:
                attackSpawnEnemies();
                break;
            case 1:
                attackShootInCircle();
                break;
            case 2:
                attackSpamShootInCircle();
                break;
            case 3:
                attackBurstShoot();
                break;
            
            case 4:
                attackBurstShootCircle();
                break;
            
        }
    }

    void attackSpawnEnemies()
    {
        enemySpawnSound.Play();
        int countEnemies = Random.Range(0, maxCountSpawnEnemiesAttack);
        for (int i = -1; i < countEnemies; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            GameObject enemy = Instantiate(enemyPrefab, (Vector2)transform.position + randomDirection * 2, Quaternion.identity);
            enemy.GetComponent<Enemy>().health *= 0.5f;
            enemy.GetComponent<Enemy>().boss = this;
            Vector2 force =  randomDirection * 2000;
            enemy.GetComponent<Rigidbody2D>().AddForce(force);

            // add enemy to the array
            spawnedEnemies.Add(enemy);
        }

    }

    void attackShootInCircle()
    {
        shootSound.Play();
        for (int i = 0; i < 365/attackShootInCircleAngleBetweenBullets; i++)
        {
            float angelDeg = (i + 1) * attackShootInCircleAngleBetweenBullets;
            Vector2 direction = new Vector2(Mathf.Cos(angelDeg * Mathf.Deg2Rad), Mathf.Sin(angelDeg * Mathf.Deg2Rad)).normalized;
            GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.Euler(0, 0, angelDeg));
            bullet.GetComponent<AltBullet>().assingVar(bulletDamage, gameObject);
            bullet.GetComponent<AltBullet>().assignBossVar(attackShootInCircleBulletSpeed, bulletSprite); 
        }
    }

    void attackSpamShootInCircle()
    {
        shootSound.Play();
        for (int i = 0; i < 365/attackSpamShootInCircleAngleBetweenBullets; i++)
        {
            float angelDeg = (i + 1) * attackSpamShootInCircleAngleBetweenBullets + attackSpamShootInCircleCurrentAngleOffset;
            Vector2 direction = new Vector2(Mathf.Cos(angelDeg * Mathf.Deg2Rad), Mathf.Sin(angelDeg * Mathf.Deg2Rad)).normalized;
            GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.Euler(0, 0, angelDeg));
            bullet.GetComponent<AltBullet>().assingVar(bulletDamage, gameObject);
            bullet.GetComponent<AltBullet>().assignBossVar(attackSpamShootInCircleBulletSpeed, bulletSprite); 
        }
        attackSpamShootInCircleCurrentAngleOffset += attackSpamShootInCircleAngleOffset;
        attackSpamShootInCircleCount += 1;

        if (attackSpamShootInCircleCount < attackSpamShootInCircleCountWaves) Invoke("attackSpamShootInCircle", attackSpamShootInCircleTimeBetweenWaves);
        else{
            attackSpamShootInCircleCurrentAngleOffset = 0;
            attackSpamShootInCircleCount = 0;
        }
    }

    void attackBurstShoot()
    {      
        if (attackBurstShootCount == 0)
        {
            Vector2 vectorToPlayer = ((Vector2)playerTransform.position - rb.position).normalized;
            float angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
            attackBurstShootAngleBetweenBullets = (attackBurstShootAngleOffsetToPlayer * 2) / attackBurstShootCountBulletsPerAttack;
            attackBurstShootCurrentAngle = angleToPlayer + attackBurstShootAngleOffsetToPlayer;
            shootSound.Play();
        }

        GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.Euler(0, 0, attackBurstShootCurrentAngle - 90f));
        bullet.GetComponent<AltBullet>().assingVar(bulletDamage, gameObject);
        bullet.GetComponent<AltBullet>().assignBossVar(attackBurstShootBulletSpeed, bulletSprite); 

        attackBurstShootCurrentAngle -= attackBurstShootAngleBetweenBullets;
        attackBurstShootCount += 1;

        if(attackBurstShootCount < attackBurstShootCountBulletsPerAttack) Invoke("attackBurstShoot", attackBurstShootTimeBetweenShots);
        else attackBurstShootCount = 0;
    }
    void attackBurstShootCircle()
    {
        shootSound.Play();
        for (int i = 0; i < attackBurstShootCircleCountBulletsPerWave; i++)
        {
            float angleBetweenBullets = 365 / attackBurstShootCircleCountBulletsPerWave;

            float angelDeg = (i + 1) * angleBetweenBullets;
            Vector2 direction = new Vector2(Mathf.Cos(angelDeg * Mathf.Deg2Rad), Mathf.Sin(angelDeg * Mathf.Deg2Rad)).normalized;
            GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.Euler(0, 0, angelDeg));
            bullet.GetComponent<AltBullet>().assingVar(bulletDamage, gameObject);
            bullet.GetComponent<AltBullet>().assignBossVar(attackBurstShootCircleBulletSpeed, bulletSprite); 
        }

        attackBurstShootCircleCount += 1;

        if (attackBurstShootCircleCount < attackBurstShootCircleCountWaves) Invoke("attackBurstShootCircle", attackBurstShootCircleTimeBetweenWaves);
        else attackBurstShootCircleCount = 0;
    }

    public void killEnemy(GameObject enemy)
    {
        killSound.Play();
        Destroy(enemy);
        spawnedEnemies.Remove(enemy);
    }
}

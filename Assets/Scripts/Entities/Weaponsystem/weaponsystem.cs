using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsystem : MonoBehaviour
{
    public GameObject owner;//by cornell

    public Sprite[] weaponTexture;//cornell

    public int weaponType;
    public GameObject bulletPrefab;

    public float speedWhileWearing;
    public float FOVWhileWearing;

    private float fireCooldown;
    private float damage;
    private float shootForce;

    private Transform firePoint;
    private float fireCooldownLeft;

    void Start()
    {
        // getting the firePoint
        firePoint = transform.GetChild(0);

        // sets the weapons stats
        getWeaponStats();

        //set texture [Cornell]
        this.gameObject.GetComponent<SpriteRenderer>().sprite = weaponTexture[weaponType];
    }

    void Update()
    {
        if (GameManager.isPaused) return;
        // reduces the fire CoolDown
        if (fireCooldownLeft > 0) fireCooldownLeft -= Time.deltaTime;
    }

    void shoot()
    {
        // you can only shoot if the Cooldown is over
        if(fireCooldownLeft > 0) return;

        // Instantiates a new bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // adds a force to the bullet in the direction the player looks
        Rigidbody2D rb_bullet = bullet.GetComponent<Rigidbody2D>();
        rb_bullet.AddForce(firePoint.up * shootForce, ForceMode2D.Impulse);

        // sets the damage of the bullet
        bullet.GetComponent<Bullet>().damage = damage;

        // restarts the CoolDown
        fireCooldownLeft = fireCooldown;

        //save owner of weapon to prevent friendly fire
        bullet.GetComponent<Bullet>().owner = owner;
    }
    public void getWeaponStats()
    {
        weaponStats weaponStatsObject = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().weaponStatsList[weaponType];   

        // write the Stats in the variables
        fireCooldown = weaponStatsObject.fireCooldown;
        damage = weaponStatsObject.damage;
        shootForce = weaponStatsObject.shootForce;
        speedWhileWearing = weaponStatsObject.speedWhileWearing;
        FOVWhileWearing = weaponStatsObject.FOVWhileWearing;        

    }
}

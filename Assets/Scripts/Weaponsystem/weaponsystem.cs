using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsystem : MonoBehaviour
{
    public int weaponType;
    public GameObject bulletPrefab;

    private float fireCooldown;
    private float damage;
    private float shootForce;

    private Transform firePoint;
    private float fireCooldownLeft;

    void Start()
    {
        // getting the firePoint
        firePoint = transform.GetChild(0);

        // set the variables for the different types of weapons
        switch (weaponType)
        {
            // Pistol
            case 0:
                fireCooldown = 0.5f;
                damage = 2;
                shootForce = 10;
                break;
        }
    }

    void Update()
    {
        // reduces the fire CoolDown
        if(fireCooldownLeft > 0) fireCooldownLeft -= Time.deltaTime;
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
    }
}

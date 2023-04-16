using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public float maxHealth;// by Cornell
    public int dungeonFloor;
    
    public Rigidbody2D rb;
    public Camera cam;
    public float maxMovementSpeed; // 7
    public float acceleration; // 50
    public float dashForce; // 1000
    public float dashCooldown; // 2
    public float dashTime; // 0.15
    public float dashDamage; // 10
    public float weaponPickUpRadius;
    public float ItemDropForce; // 500
    public float health;
    public GameObject dashEffect;
    public Animator camShake;
    public GameObject weaponPrefab;
    // level 0 is the worst armor and 10 is the best
    public int armourLevel = 0; 
    // 1 is weapon; 2 is Armour
    public int selectedItem = 1;
    public GameObject armourPrefab;
    public float itemPickUpRadius;
    public float FOV;
    public float timeChangeFOV;
    public float t;
    public float FOVChangeWithoutWeapon = -1;

    private Transform weaponSlot; 
    public GameObject weapon;
    private Vector2 inputMovement;
    private Vector2 mousePosition;
    private Vector2 lookDir;
    private float angleToMouse;
    private float dashCooldownLeft;
    private float dashTimeLeft;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool isDead;
    public float playerGold;//treat as int by cornell
    private float speedBoostByWeapon;
    private float FOVChangeByWeapon;
    private bool isChangingFOV;
    private float lastFOV;


    void Start()
    {
        weaponSlot = transform.GetChild(0);
        weapon = Instantiate(weaponPrefab, weaponSlot.position, weaponSlot.rotation);  
        weapon.GetComponent<weaponsystem>().owner = gameObject; // by Cornell      

        cam.orthographic = true;
        dashCooldownLeft = 0;
        dashTimeLeft = dashTime;
        isDashing = false;
        isDead = false;

        changeSpeed();
        changeFOV();
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
        if(Input.GetKeyDown("f")) pickupItem();

        // updates the current selected Item
        if(Input.GetKeyDown("1")) selectedItem = 1;
        if(Input.GetKeyDown("2")) selectedItem = 2;

        // as long as the FOV has to change it will call changeFOV()
        if(!isChangingFOV) {
            // resets the last FOV and t
            lastFOV = cam.orthographicSize;
            t = 0;
        }
        else changeFOV();
    }

    void FixedUpdate()
    {
        // calculating the velocity the rb should have
        Vector2 targetVelocity = inputMovement * (maxMovementSpeed + speedBoostByWeapon);
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

        // this function -0.08x + 1 reduces the damage depending on the armor level
        damage *=  (float)-0.08 * armourLevel + 1;
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

    void pickupItem()
    {
        // find nearest Item
        GameObject nearestItem = null;

        float lowestItemDistance = itemPickUpRadius;

        // finds every Item in the game
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        // finds the Item which is the nearest to the player
        foreach(GameObject item in items)
        {
            // if its not the players weapon
            if(item == weapon) continue;

            float itemDistance = (rb.position - new Vector2(item.transform.position.x, item.transform.position.y)).magnitude;
            if (itemDistance < lowestItemDistance)
            {
                nearestItem = item;
                lowestItemDistance = itemDistance;
            }
        }

        // drops selected Item
        if(nearestItem == null)
        {
            switch (selectedItem)
            {
                case 1:
                    swapWeapon(null);
                    break;

                case 2:
                    swapArmour(null);
                    break;
            }

            isChangingFOV = true;
            changeSpeed();
            changeFOV();
        }    
        // switch items
        else
        {
            if(nearestItem.gameObject.name.StartsWith("Weapon")) swapWeapon(nearestItem);
            else swapArmour(nearestItem);

            isChangingFOV = true;
            changeSpeed();
            changeFOV();
        } 


    } 
    void swapArmour(GameObject armour)
    {
        if(armourLevel != 0)
        {
            // creates the "old" armour
            GameObject oldArmour = Instantiate(armourPrefab, transform.position, Quaternion.identity);

            // sets the level of the armour
            oldArmour.SendMessage("setLevel", armourLevel);

            // gives the "old" armour a force to kick the armour away - a drop animation
            oldArmour.GetComponent<Rigidbody2D>().AddForce(-lookDir.normalized * ItemDropForce);
        }

        // sets the Players armourLevel to 0
        armourLevel = 0;

        // if there is armour to pickup
        if(armour)
        {
            armourLevel = armour.GetComponent<Armour>().level;
            Destroy(armour);
        }
    }
    void swapWeapon(GameObject nearestWeapon)
    {
        // gives the "old" weapon a force to kick the weapon away - a drop animation
        if (weapon) weapon.GetComponent<Rigidbody2D>().AddForce(-lookDir.normalized * ItemDropForce);

        // if there is no weapon to pickup
        if(nearestWeapon == null) weapon = null;
        // change the weapons
        else weapon = nearestWeapon;
        if(weapon != null) weapon.GetComponent<weaponsystem>().owner = gameObject;//by Cornell
    }
    void changeSpeed()
    {
        if(!weapon) speedBoostByWeapon = 0;
        else speedBoostByWeapon = weapon.GetComponent<weaponsystem>().speedWhileWearing;        
    }
    void changeFOV() 
    {
        float checkFOVchange = FOVChangeByWeapon;

        // changes the FOVChangeByWeapon to the current one
        if(!weapon) FOVChangeByWeapon = FOVChangeWithoutWeapon;
        else FOVChangeByWeapon = weapon.GetComponent<weaponsystem>().FOVWhileWearing;  
                
        // FOV has changed
        if(checkFOVchange != FOVChangeByWeapon) lastFOV = cam.orthographicSize;

        // FOV is changing
        if(isChangingFOV)
        { 
            // as long as t is smaller than 1 it will raise (it has to be between 0 and 1 because of Mathf.Lerp)
            if(t < 1) t += Time.deltaTime / timeChangeFOV;
            else isChangingFOV = false;

            // sets the FOV smoothly
            cam.orthographicSize = Mathf.Lerp(lastFOV, FOV + FOVChangeByWeapon, t);
        }
    }





    public void LoadData(GameData data)
    {
        this.playerGold = data.currentCoins;
    }

    public void SaveData(ref GameData data)
    {
        data.currentCoins = this.playerGold;
    }
}

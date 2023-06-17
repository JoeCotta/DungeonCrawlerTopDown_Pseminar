using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    //Player Stats; part of resort & cleaning up code
    public float maxHealth;// by Cornell
    public float health;
    public int armourLevel = 0;// level 0 is the worst armor and 10 is the best
    public float playerGold;//treat as int by cornell
    public int revivesLeft;
    public int killedEnemys;//could be saved in GameManager

    public float maxMovementSpeed; // 7
    public float acceleration; // 50
    public float dashForce; // 1000
    public float dashCooldown; // 2
    public float dashTime; // 0.15
    public float dashDamage; // 10
    public float weaponPickUpRadius;
    public float ItemDropForce; // 500
    public float itemPickUpRadius;
    public int weaponType; // WHY THIS? das wird doch nie nach start benutzt

    public int dungeonFloor;
    public bool isDead;
    


    //temp variables for functions
    public int selectedItem = 1;// 1 is weapon; 2 is Armour

    public float FOV;
    public float timeChangeFOV;
    public float t;
    public float FOVChangeWithoutWeapon = -1;
    private float speedBoostByWeapon;
    private float FOVChangeByWeapon;
    private bool isChangingFOV;
    private float lastFOV;
    private bool changeFOVGameStart;

    private Vector2 inputMovement;
    private Vector2 mousePosition;
    private Vector2 lookDir;
    private float angleToMouse;

    private float dashCooldownLeft;
    private float dashTimeLeft;
    private Vector2 dashDirection;
    public bool isDashing;



    //References and Prefabs
    public GameManager gameManager; //by Cornell
    public GameObject weaponPrefab;
    public GameObject weapon;
    public GameObject armourPrefab;
    public GameObject dashEffect;
    public Camera cam;
    public Animator camShake;
    public Rigidbody2D rb;
    private Transform weaponSlot;



    void Start()
    {
        Invoke("assingWeapon", 0.1f);
    

        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        gameManager.references.Add(this.gameObject);

        cam.orthographic = true;
        dashCooldownLeft = 0;
        dashTimeLeft = dashTime;
        isDashing = false;
        isDead = false;

        // sets this to true that if the player spawns with a weapon with a FOV change that is not 0 the FOV is still changed
        changeFOVGameStart = true;

        // delays this that when calling the functions the weapon is properly loaded
        Invoke("changeSpeed", 0.5f);
        Invoke("changeFOV", 0.5f);
    }

    void assingWeapon()
    {
        // creates the weapon
        weaponSlot = transform.GetChild(0);
        weapon = Instantiate(weaponPrefab, weaponSlot.position, weaponSlot.rotation);
        // sets the weaponType
        weapon.GetComponent<weaponsystem>().weaponType = weaponType; // sets the weaponType
        weapon.GetComponent<weaponsystem>().owner = gameObject; // by Cornell
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


        //by cornell death
        if(health <= 0)
        {
            onPlayerDead();
        }
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
        
        //if(health <= 0) commented out for now, Cornell
        //{
            //isDead = true;
        //}
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
            // if you have a armour AND a weapon
            if (weapon && armourLevel != 0)
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
            }
            // if you only have a weapon 
            else if(weapon) swapWeapon(null);
            // if you only have a amour
            else if(armourLevel != 0) swapArmour(null);

            isChangingFOV = true;
            changeSpeed();
            changeFOV();
        }    
        // switch items
        else
        {
            print(nearestItem.gameObject.name.StartsWith("Weapon"));
            if(nearestItem.gameObject.name.StartsWith("Weapon")) swapWeapon(nearestItem);
            else if(nearestItem.gameObject.name.StartsWith("Armour")) swapArmour(nearestItem);

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
        // if the game is loaded changeFOV is called with a little delay
        if(changeFOVGameStart) 
        {
            isChangingFOV = true;
            changeFOVGameStart = false;
        }
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

    public void onPlayerDead()
    {
        //first check for Extra life(revive)
        if(revivesLeft == 0)
        {
            isDead = true;
            //Endgame Menu
        }else
        {
            revivesLeft--;
            health = maxHealth;
            //maybe add invincablility and pause game and ask if wanna revive
        }


    }

    public void enemyKilled()
    {
        killedEnemys++;
    }

    

    public void LoadData(GameData data)
    {
        this.isDead = data.isDead;
        this.playerGold = data.currentCoins;
        this.revivesLeft = Mathf.RoundToInt(data.revivesLeft);

        this.maxHealth = data.currentMaxHealth;
        this.health = data.currentHealth;
        this.armourLevel = Mathf.RoundToInt(data.currentArmor);
        this.weaponType = data.currentWeaponType;

        this.killedEnemys = data.enemysKilled;
    }

    public void SaveData(ref GameData data)
    {
        data.isDead = this.isDead;
        data.currentCoins = this.playerGold;
        data.revivesLeft = this.revivesLeft;

        data.currentMaxHealth = this.maxHealth;
        data.currentHealth = this.health;
        data.currentArmor = this.armourLevel;
        data.currentWeaponType = this.weapon.GetComponent<weaponsystem>().weaponType;

        data.enemysKilled = this.killedEnemys;
    }
}

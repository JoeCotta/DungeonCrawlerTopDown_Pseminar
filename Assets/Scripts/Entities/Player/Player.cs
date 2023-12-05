using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDataPersistence
{
    //Player Stats; part of resort & cleaning up code
    public float maxHealth, health, playerGold;
    public int armourLevel = 0, revivesLeft, killedEnemys;
    // level 0 is the worst armor and 10 is the best
    //playerGold treat as int by cornell

    public float maxMovementSpeed; // 7
    public float acceleration; // 50
    public float dashForce; // 150000
    public float dashCooldown; // 2
    public float dashTime; // 0.15
    public float dashDamage; // 10
    public float weaponPickUpRadius;
    public float ItemDropForce; // 500
    public float itemPickUpRadius;

    private int weaponType, oldRarity, second_weaponType, second_oldRarity;
    public float oldReserve, oldAmmo, second_oldReserve,second_oldAmmo;
    private bool hadSecondWeapon = false;


    public int dungeonFloor;
    public bool isDead;
    


    //temp variables for functions
    public int selectedItem = 1;// 1 is weapon; 2 is Armour

    public float FOV, timeChangeFOV;
    public float t;
    public float FOVChangeWithoutWeapon = -1;
    private float speedBoostByWeapon, FOVChangeByWeapon;
    private float lastFOV;
    private bool isChangingFOV, changeFOVGameStart;

    private Vector2 inputMovement, mousePosition, lookDir;
    private float angleToMouse;

    public float dashCooldownLeft;
    private float dashTimeLeft;
    private Vector2 dashDirection;
    public bool isDashing;



    //References and Prefabs
    public GameObject weaponPrefab, weapon, secondWeapon, armourPrefab, dashEffect, minmapUI;
    public Camera cam;
    public Animator camShake;
    public Rigidbody2D rb;
    private Transform weaponSlot, backSlot;

    // boosts
    public float speedBuff;
    public float healBuff;
    public float tHealBoost;

    // Sounds
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource weaponDropSound;
    [SerializeField] private AudioSource weaponPickupSound;

    // check walking
    private bool isMoving;
    private Vector2 lastPosition;
    private float movingCounter = 0;

    // sprites
    public Sprite sprite_front_left;
    public Sprite sprite_back;
    public Sprite sprite_front_right;

    void Start()
    {
        //Initiating all the delayed stuff
        StartCoroutine(DelayedAssing());
        cam.orthographic = true;
        dashCooldownLeft = 0;
        dashTimeLeft = dashTime;
        isDashing = false;
        isDead = false;

        // sets this to true that if the player spawns with a weapon with a FOV change that is not 0 the FOV is still changed
        changeFOVGameStart = true;


        // delays this that when calling the functions the weapon is properly loaded
        //Invoke("changeSpeed", 0.5f);
        //Invoke("changeFOV", 0.5f);

        tHealBoost = 0;
    }

    IEnumerator DelayedAssing()
    {
        yield return new WaitForSeconds(0.1f);
        assingWeapon();
        yield return new WaitForSeconds(0.4f);
        GameManager.references.Add(gameObject);
        changeSpeed();
        changeFOV();
    }

    void assingWeapon()
    {
        // creates the weapon
        weaponSlot = transform.GetChild(0);
        backSlot = transform.GetChild(2);
        weapon = Instantiate(GameManager.AWeapon, weaponSlot.position, weaponSlot.rotation);
        weapon.GetComponent<AlternateWS>().weaponType = weaponType; // sets the weaponType
        weapon.GetComponent<AlternateWS>().rarity = oldRarity;
        if(oldReserve != -1) weapon.GetComponent<AlternateWS>().loadOldInfo(oldReserve, oldAmmo);
        weapon.GetComponent<AlternateWS>().owner = gameObject; // by Cornell
        if (hadSecondWeapon)
        {
            secondWeapon = Instantiate(GameManager.AWeapon, backSlot.position, backSlot.rotation);
            secondWeapon.GetComponent<AlternateWS>().weaponType = second_weaponType;
            secondWeapon.GetComponent<AlternateWS>().rarity = second_oldRarity;
            if (second_oldReserve != -1) secondWeapon.GetComponent<AlternateWS>().loadOldInfo(second_oldReserve, second_oldAmmo);
            secondWeapon.GetComponent<AlternateWS>().owner = gameObject;
        }
    }

    void Update()
    {
        if (GameManager.isPaused) return;
        //if (currentReserve != -1 && weapon.GetComponent<AlternateWS>().reserve > 0&& !stopCheck) { weapon.GetComponent<AlternateWS>().reserve = currentReserve; stopCheck = true; }

        // getting Keyboard Input
        inputMovement = Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpKey"))) ? Vector2.up : Vector2.zero;
        inputMovement += Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftKey"))) ? Vector2.left : Vector2.zero;
        inputMovement += Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownKey"))) ? Vector2.down : Vector2.zero;
        inputMovement += Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightKey"))) ? Vector2.right : Vector2.zero;

        // check if moving
        if(lastPosition != rb.position) isMoving = true;
        else isMoving = false;

        // walking sound if getting keyboard input and position is actually changing
        if(isMoving && inputMovement != Vector2.zero) walkingSound.enabled = true;
        else walkingSound.enabled = false;

        // sets the postion of the last frame every second
        if(movingCounter >= 1){
            lastPosition = rb.position;
            movingCounter = 0;
        }
        movingCounter += Time.deltaTime;

        // getting mouse Input
        // transform the screen mouse Position to a world point
        mousePosition =  cam.ScreenToWorldPoint(Input.mousePosition);

        // calculating the angle between the player and the mouse
        lookDir = rb.position - mousePosition;
        angleToMouse = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        updatePlayerSprite();
        /*
        // -30 - 90   front left
        // -30 - -150 back
        // 90 - -150 front-right
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (angleToMouse > -30 && angleToMouse < 90) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_left;
        else if ((angleToMouse > 90 && angleToMouse <= 180) || (angleToMouse >= -180 && angleToMouse < -150)) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_right;
        else if (angleToMouse < -30 && angleToMouse > -150){
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite_back;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }*/

        //flip weapon sprite
        if (weapon){
            // left side
            if (angleToMouse < 90 && angleToMouse >= -90) weapon.GetComponent<SpriteRenderer>().flipY = true;
            //right side
            else if((angleToMouse >= 90 && angleToMouse <= 180) || (angleToMouse >= -180 && angleToMouse < -90)) weapon.GetComponent<SpriteRenderer>().flipY = false;
        }

        // dash
        // if dash cooldown is finished and dash-Key is pressed
        if (dashCooldownLeft <= 0 && Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DashKey"))))
        {
            isDashing = true;
            dashCooldownLeft = dashCooldown;
            
            // dash particles
            Instantiate(dashEffect, rb.position, Quaternion.identity);

            // dashes in "Mouse direction"
            if (GameManager.movementKeysForDash) dashDirection = inputMovement;
            else dashDirection = -lookDir.normalized;

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
            //dashTimeLeft -= Time.deltaTime;
        }

        // if dash is over
        else if(dashTimeLeft <= 0)
        {
            // stops camera shake animation
            camShake.SetBool("isDashing", false);

            //dashTimeLeft = dashTime;
            //isDashing = false;
        }

        //short invincibility even after dash
        if (isDashing && dashTimeLeft > -0.5f)
        {
            dashTimeLeft -= Time.deltaTime;
        }
        else if(isDashing && dashTimeLeft <= -0.5f)
        {
            dashTimeLeft = dashTime;
            isDashing = false;
        }

        // update the position and rotation of the weapon if the player has one
        /*if (weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = Quaternion.Euler(0, 0, angleToMouse + 180);

        }*/
        updateWeaponPosition();


        // shoot if clicked
        if(Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ShootKey")))) shoot();
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ReloadKey"))) && weapon.GetComponent<AlternateWS>()) weapon.GetComponent<AlternateWS>().Reload();

        // pickup / swap weapon
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DropKey")))) pickupItem();

        // updates the current selected Item
        if(Input.GetKeyDown("1")) selectedItem = 1;
        if(Input.GetKeyDown("2")) selectedItem = 2;
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SwitchWeaponKey")))) switchWeapon();

        // as long as the FOV has to change it will call changeFOV()
            if (!isChangingFOV) {
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

        
        // heal Buff
        if (healBuff != 0) {
            health += healBuff * Time.deltaTime * maxHealth/25;
        }

        // to open the map
        if(Input.GetKeyDown(KeyCode.CapsLock)) openMap();
        else if(Input.GetKeyUp(KeyCode.CapsLock)) closeMap();


        // calculating the velocity the rb should have
        Vector2 targetVelocity = inputMovement.normalized * (maxMovementSpeed + speedBoostByWeapon + speedBuff);
        // difference between the velocity the rb should have and the actual one
        Vector2 velocityDifference = targetVelocity - rb.velocity;
        // F = m*a (m=1) and a = v/t (t=1) => F = v 
        // ==> force is the velocity difference multiplied by an optional factor to speed up and brake faster
        Vector2 force = velocityDifference * acceleration;
        rb.AddForce(force * Time.deltaTime * 60);
    }

    void dash(){
        //added time.delta time to make movement always the same speed at any framerate
        rb.AddForce(dashForce * dashDirection * Time.deltaTime);
        dashSound.Play();
    }

    void hit(float damage)
    {
        // player can't be hit while dashing
        if (isDashing) return;

        // this function -0.08x + 1 reduces the damage depending on the armor level
        damage *=  (float)-0.08 * armourLevel + 1;
        health -= damage;
        
        // play sound
        hitSound.Play();

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

    void switchWeapon()
    {
        GameObject weaponStorage;
        weaponStorage = weapon;
        weapon = secondWeapon;
        if(weapon) weapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
        secondWeapon = weaponStorage;
        if(secondWeapon) secondWeapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        if (secondWeapon) secondWeapon.GetComponent<SpriteRenderer>().flipY = false;
        isChangingFOV = true;
        changeFOV();
        changeSpeed();
    }

    void updateWeaponPosition()
    {
        // update the position and rotation of the weapon if the player has one
        if (weapon)
        {
            weapon.transform.position = weaponSlot.position;
            weapon.transform.rotation = Quaternion.Euler(0, 0, angleToMouse + 180);
        }
        if (secondWeapon)
        {
            secondWeapon.transform.position = backSlot.position;
            secondWeapon.transform.rotation = backSlot.rotation;
        }
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
            if(item == weapon || item == secondWeapon) continue;

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
            //print(nearestItem.gameObject.name.StartsWith("Weapon"));
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
        //check for same weapontype + rarity 
        if(nearestWeapon && weapon && nearestWeapon.GetComponent<AlternateWS>().weaponType == weapon.GetComponent<AlternateWS>().weaponType && nearestWeapon.GetComponent<AlternateWS>().rarity == weapon.GetComponent<AlternateWS>().rarity)
        {
            AlternateWS weaponWS = weapon.GetComponent<AlternateWS>();
            weaponWS.reserve += nearestWeapon.GetComponent<AlternateWS>().reserve + nearestWeapon.GetComponent<AlternateWS>().ammo;
            nearestWeapon.GetComponent<AlternateWS>().reserve = 0;
            nearestWeapon.GetComponent<AlternateWS>().ammo = 0;
            return;
        }

        // gives the "old" weapon a force to kick the weapon away - a drop animation
        if (weapon){
            weapon.GetComponent<Rigidbody2D>().AddForce(-lookDir.normalized * ItemDropForce);
            // play weapon drop Sound
            weaponDropSound.Play();
        }

        // sets the owner of the dropped weapon to null
        if (weapon) weapon.GetComponent<AlternateWS>().owner = null;

        // you shouldn't be able to pick up a weapon another entity is holding
        if(nearestWeapon && nearestWeapon.GetComponent<AlternateWS>().owner) return;

        // if there is no weapon to pickup
        if(nearestWeapon == null) weapon = null;
        // change the weapons
        else {
            weapon = nearestWeapon;
            // play weapon pickup sound
            weaponPickupSound.Play();
        }
        if (weapon != null) weapon.GetComponent<AlternateWS>().owner = gameObject;
    }
    void changeSpeed()
    {
        if(!weapon) speedBoostByWeapon = 0;      
        else speedBoostByWeapon = weapon.GetComponent<AlternateWS>().speed;
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
        if (!weapon) FOVChangeByWeapon = FOVChangeWithoutWeapon;
        else FOVChangeByWeapon = weapon.GetComponent<AlternateWS>().fov;
                
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

    public void updatePlayerSprite()
    {
        //to enable easy setting of sprite by sus mode
        if (GameManager.enableSusMode)
        {
            susSprite();
            return;
        }
        int flipBackSlot = 0;
        // -30 - 90   front left
        // -30 - -150 back
        // 90 - -150 front-right
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (secondWeapon) secondWeapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        if (angleToMouse > -30 && angleToMouse < 90) { gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_left; flipBackSlot = 1; }
        else if ((angleToMouse > 90 && angleToMouse <= 180) || (angleToMouse >= -180 && angleToMouse < -150)) gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front_right;
        else if (angleToMouse < -30 && angleToMouse > -150)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite_back;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
            if (secondWeapon) secondWeapon.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
        if (backSlot)
        {
            backSlot.position = new Vector3(0.15f - (0.3f * flipBackSlot), 0.1f, 0) + transform.position;
            backSlot.rotation = Quaternion.Euler(0, (-180 * flipBackSlot), -135);
        }
    }

    public void susSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (angleToMouse > -90 && angleToMouse < 90) gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.sus_Left;
        else if ((angleToMouse > 90 && angleToMouse <= 180) || (angleToMouse >= -180 && angleToMouse < -90)) gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.sus_Right;
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
        this.oldRarity = data.currentRarity;
        this.oldReserve = data.currentReserve;
        this.oldAmmo = data.currentAmmo;
        if (data.hadSecondWeapon)
        {
            this.hadSecondWeapon = data.hadSecondWeapon;
            this.second_weaponType = data.second_currentWeaponType;
            this.second_oldRarity = data.second_currentRarity;
            this.second_oldReserve = data.second_currentReserve;
            this.second_oldAmmo = data.second_currentAmmo;
        }


        this.killedEnemys = data.enemysKilled;
    }

    public void SaveData(ref GameData data)
    {
        data.isDead = this.isDead;
        data.currentCoins = this.playerGold;
        data.revivesLeft = this.revivesLeft;
        data.reviveLevel = this.revivesLeft;

        data.currentMaxHealth = this.maxHealth;
        data.currentHealth = this.health;
        data.currentArmor = this.armourLevel;

        if (!weapon && !secondWeapon)
        {
            data.currentWeaponType = 0;
            data.currentRarity = 0;
            data.currentReserve = -1;
            data.currentAmmo = -1;
        }else if (!weapon && secondWeapon)
        {
            weapon = secondWeapon;
            secondWeapon = null;
        }

        if (weapon && weapon.GetComponent<AlternateWS>()) 
        { 
            data.currentWeaponType = this.weapon.GetComponent<AlternateWS>().weaponType; 
            data.currentRarity = this.weapon.GetComponent<AlternateWS>().rarity;
            data.currentReserve = this.weapon.GetComponent<AlternateWS>().reserve;
            data.currentAmmo = this.weapon.GetComponent<AlternateWS>().ammo;
            if (secondWeapon)
            {
                data.second_currentWeaponType = this.secondWeapon.GetComponent<AlternateWS>().weaponType;
                data.second_currentRarity = this.secondWeapon.GetComponent<AlternateWS>().rarity;
                data.second_currentReserve = this.secondWeapon.GetComponent<AlternateWS>().reserve;
                data.second_currentAmmo = this.secondWeapon.GetComponent<AlternateWS>().ammo;
                data.hadSecondWeapon = true;
            }
            else
            {
                data.second_currentWeaponType = 0;
                data.second_currentRarity = 0;
                data.second_currentReserve = -1;
                data.second_currentAmmo = -1;
                data.hadSecondWeapon = false;
            }
        }else if (weapon) data.currentWeaponType = this.weapon.GetComponent<AlternateWS>().weaponType;

        data.enemysKilled = this.killedEnemys;
    }

    void openMap()
    {
        // sets the position and size
        minmapUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, -200);
        minmapUI.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

        // makes it a little bit transparent
        minmapUI.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.7f);
    }

    void closeMap()
    {
        // sets the position and size
        minmapUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -50);
        minmapUI.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        // makes it a little bit transparent
        minmapUI.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
    }
}

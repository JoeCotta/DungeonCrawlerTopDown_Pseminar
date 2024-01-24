using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class AlternateWS : MonoBehaviour
{
    //INFO
    //TYPE
    //0 Pistol
    //1 AK
    //2 Sniper
    //
    //RARITY
    //0 common
    //1 rare
    //2 epic
    //3 legendary

    public int weaponType, rarity;
    private float dmg, rate, accuracy, chargedTime, maxChargeTime = 4, maxChargeDmgMultiplier = 4;
    public float mag, reserve, fov, ammo, dmgtest, speed, enemyDmgMultiplier, bulletcount;
    private bool blockLoadingInfo = false, isCharging = false;

    public Sprite[] textureEditor, magSprites;
    static public Sprite[] texture;

    public float rTime, rTimer;
    private float interval = 0;
    private bool reloading;
    private Transform firepoint;
    public GameObject bullet, owner, magPref, effekt;

    public float throwForceMag;
    private DataPersistenceManager dataPersistenceManager;

    // Sounds
    [SerializeField] private AudioSource[] shootSounds;
    [SerializeField] private AudioSource[] reloadSounds;
    [SerializeField] private AudioSource magDropSound;
    [SerializeField] private AudioSource emptyWeaponShootSound;

    //Animator
    [SerializeField] Animator legendary_Sniper;

    public void Start()
    {
        texture = textureEditor;
        firepoint = transform.GetChild(0);
        float[] temp = DataBase.weaponBase(weaponType, rarity);
        dmg         = temp[0];
        mag         = Mathf.RoundToInt(temp[1]);
        if (!blockLoadingInfo) reserve = Mathf.RoundToInt(temp[2]);
        rate        = temp[3];
        accuracy    = temp[4];
        fov         = temp[5];
        speed       = temp[6];
        bulletcount = temp[7];
        rTime       = temp[8];
        if (!blockLoadingInfo) ammo = mag;
        if (rarity != 3) gameObject.GetComponent<SpriteRenderer>().sprite = texture[weaponType];
        else gameObject.GetComponent<SpriteRenderer>().sprite = texture[weaponType + 4];
        rTimer = rTime;
        throwForceMag = 350f;
        if (GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>()) dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
    }

    public void shoot()
    {
        if (interval > 1 / rate && ammo > 0 && !reloading)
        {
            if (weaponType == 2 && rarity == 3 && owner.tag != "enemy" && !isCharging)
            {
                isCharging = true;
                chargedTime = 0;
                legendary_Sniper.enabled = true;
                return;
            }

            // shoot sound
            shootSounds[weaponType].Play();

            //make firerate more consistent and not framerate dependent
            while (interval > 1 / rate)
            {

                //loop for weapon like shotgun with multishot
                for (int i = 0; i < bulletcount; i++)
                {
                    Vector3 inAccuracy = new Vector3(0, 0, Random.Range(-45 + accuracy * 45, 45 - accuracy * 45 + 1));
                    GameObject temp = Instantiate(bullet, firepoint.position, firepoint.rotation.normalized * Quaternion.Euler(0, 0, inAccuracy.z));
                    if (temp != null)
                    {
                        if (owner.tag == "Player") temp.GetComponent<AltBullet>().assingVar(dmg * dataPersistenceManager.gameData.currentDamageMultiplier, owner);
                        if (owner.tag == "Enemy") temp.GetComponent<AltBullet>().assingVar(dmg * DifficultyTracker.dmgMultiplier, owner);
                    }
                }

                if (owner.tag != "Enemy") ammo--;
                interval -= 1 / rate;
            }
        }
        // no ammo
        else if (interval > 1 / rate && ammo <= 0 && !reloading)
        {
            Reload();
            emptyWeaponShootSound.Play();
        }
    }

    public void chargedShot()
    {
        if (chargedTime == 0) return;
        interval = 0;
        --ammo;
        GameObject temp = Instantiate(bullet, firepoint.position, firepoint.rotation.normalized);
        if (temp != null) temp.GetComponent<AltBullet>().assingVar(dmg * Mathf.Pow(maxChargeDmgMultiplier, chargedTime / maxChargeTime), owner);
        shootSounds[weaponType].Play();
        //Debug.Log(dmg * Mathf.Pow(maxChargeDmgMultiplier, chargedTime / maxChargeTime));
        isCharging = false;
        chargedTime = 0;
        legendary_Sniper.enabled = false;
    }

    public void Reload()
    {
        if (ammo == mag || reloading) return; if (reserve == 0) return;
        reloading = true; rTimer = 0;

        // reload sound 
        reloadSounds[weaponType].Play();

    }

    private void RealReload()
    {
        if (GameManager.player.weapon != gameObject) { reloading = false; return; }
        if (reserve >= mag - ammo)
        {
            reserve -= mag - ammo;
            ammo = mag;
        }
        else
        {
            ammo += reserve;
            reserve = 0;
        }
        GameObject thrownMag = Instantiate(magPref, firepoint.position, Quaternion.identity);
        if (rarity != 3) thrownMag.GetComponent<SpriteRenderer>().sprite = magSprites[weaponType];
        else thrownMag.GetComponent<SpriteRenderer>().sprite = magSprites[weaponType + 4];
        if (rarity != 3 || weaponType != 2) thrownMag.GetComponent<Rigidbody2D>().AddForce(firepoint.transform.up * throwForceMag);
        else thrownMag.GetComponent<Rigidbody2D>().AddForce(firepoint.transform.right * throwForceMag);
        reloading = false;

        // play throw sound
        magDropSound.Play();
    }

    public Vector2 GetAmmo()
    {
        return new Vector2(ammo, reserve);
    }

    private void Update()
    {
        if (interval < 1 / rate) interval += Time.deltaTime;
        if (rTimer < rTime) rTimer += Time.deltaTime;
        if (reloading && rTimer >= rTime) RealReload();
        if (!owner) { reloading = false; rTimer = rTime; }
        if (isCharging && Input.GetMouseButton(0) && owner.tag == "Player" && chargedTime < maxChargeTime)
        {
            chargedTime += Time.deltaTime; interval = 0;
            legendary_Sniper.speed = chargedTime / maxChargeTime * 2 + 0.5f;
        }
        else if (isCharging) chargedShot();
        if (!effekt.active && rarity != 0 && !owner)
        {
            effekt.SetActive(true);
            switch (rarity)
            {
                case 1:
                    effekt.GetComponent<ParticleSystem>().startColor = Color.cyan;
                    break;
                case 2:
                    effekt.GetComponent<ParticleSystem>().startColor = Color.magenta;
                    break;
                case 3:
                    effekt.GetComponent<ParticleSystem>().startColor = Color.red;
                    break;
                default:
                    effekt.SetActive(false);
                    break;
            }
        }
        else if (effekt.active && owner) effekt.SetActive(false);
    }

    public void loadOldInfo(float oldReserve, float oldAmmo)
    {
        blockLoadingInfo = true;
        reserve = oldReserve;
        ammo = oldAmmo;
    }
}

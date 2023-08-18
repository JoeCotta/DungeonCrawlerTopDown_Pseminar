using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private float dmg, rate, accuracy;
    public float mag, reserve, fov, ammo;
    private bool blockLoadingInfo = false;

    public Sprite[] textureEditor, magSprites;
    static public Sprite[] texture;

    public float rTime, rTimer;
    private float interval = 0;
    private bool reloading;
    private Transform firepoint;
    public GameObject bullet, owner, magPref;

    public float throwForceMag;
    void Start()
    { 
        texture = textureEditor;
        firepoint = transform.GetChild(0);
        float[] temp = DataBase.weaponBase(weaponType,rarity);
        dmg = temp[0];
        mag = Mathf.RoundToInt(temp[1]);
        if(!blockLoadingInfo) reserve = Mathf.RoundToInt(temp[2]);
        rate = temp[3];
        accuracy = temp[4];
        fov = temp[5];
        if (!blockLoadingInfo) ammo = mag;
        if (rarity != 3) gameObject.GetComponent<SpriteRenderer>().sprite = texture[weaponType];
        else gameObject.GetComponent<SpriteRenderer>().sprite = texture[weaponType + 3];
        rTime = 2.5f;//reload time default 2.5 sec
        rTimer = rTime;
        throwForceMag = 350f;
    }

    public void shoot()
    {
        if(interval > 1 / rate && ammo > 0 && !reloading)
        {
            Vector3 inAccuracy = new Vector3(0,0,Random.Range(-45+accuracy*45,45-accuracy*45+1));
            interval = 0;
            if(owner.tag != "Enemy")ammo--;
            GameObject temp = Instantiate(bullet, firepoint.position, firepoint.rotation.normalized*Quaternion.Euler(0,0,inAccuracy.z));
            if(temp != null) temp.GetComponent<AltBullet>().assingVar(dmg,owner);
        }
    }

    public void Reload()
    {
        if (ammo == mag || reloading) return; if (reserve == 0) return;
        reloading = true; rTimer = 0;
    }

    private void RealReload()
    {
        if (GameManager.player.weapon != gameObject) { reloading = false;  return;}
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
        if(rarity != 3) thrownMag.GetComponent<SpriteRenderer>().sprite = magSprites[weaponType];
        else thrownMag.GetComponent<SpriteRenderer>().sprite = magSprites[weaponType+3];
        if(rarity != 3 || weaponType != 2)thrownMag.GetComponent<Rigidbody2D>().AddForce(firepoint.transform.up * throwForceMag);
        else thrownMag.GetComponent<Rigidbody2D>().AddForce(firepoint.transform.right * throwForceMag);
        reloading = false;
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
    }

    public void loadOldInfo(float oldReserve, float oldAmmo)
    {
        blockLoadingInfo = true;
        reserve = oldReserve;
        ammo = oldAmmo;
    }
}

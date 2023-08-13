using System.Collections;
using System.Collections.Generic;
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
    private float dmg, mag, reserve, rate, accuracy, ammo;
    public float fov;

    public Sprite[] textureEditor;
    static public Sprite[] texture;

    private float interval = 0;
    private bool reloading;
    private Transform firepoint;
    public GameObject bullet, owner;
    void Start()
    {
        texture = textureEditor;
        firepoint = transform.GetChild(0);
        float[] temp = DataBase.weaponBase(weaponType,rarity);
        dmg = temp[0];
        mag = Mathf.RoundToInt(temp[1]);
        reserve = Mathf.RoundToInt(temp[2]);
        rate = temp[3];
        accuracy = temp[4];
        fov = temp[5];
        ammo = mag;
        gameObject.GetComponent<SpriteRenderer>().sprite = texture[weaponType];
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
        reloading = true;
        //play animation
        
        Invoke("RealReload", 2.5f);
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
        reloading = false;
    }

    public Vector2 GetAmmo()
    {
        return new Vector2(ammo, reserve);
    }

    private void Update()
    {
        if (interval < 1 / rate) interval += Time.deltaTime;
    }
}

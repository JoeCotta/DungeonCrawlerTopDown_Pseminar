using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateWS : MonoBehaviour
{
    public int weaponType;
    private float dmg;
    private float mag;
    private float reserve;
    private float rate;
    private float accuracy;
    public float fov;
    public Sprite[] texture;

    private float interval = 0;
    private bool reloading;
    private float ammo;
    private Transform firepoint;
    public GameObject bullet;
    public GameObject owner;
    void Start()
    {
        firepoint = transform.GetChild(0);
        float[] temp = DataBase.weaponBase(weaponType);
        dmg = temp[0];
        mag = temp[1];
        reserve = temp[2];
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

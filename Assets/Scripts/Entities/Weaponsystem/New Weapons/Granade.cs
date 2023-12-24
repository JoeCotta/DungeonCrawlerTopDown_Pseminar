using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField] float fuse; //time till explosion
    [SerializeField] float time;
    [SerializeField] float dmg;
    [SerializeField] float radius;
    [SerializeField] float duration; //How long effect lasts
    [SerializeField] GameObject dmgTrigger;
    [SerializeField] GameObject ExplosionEffekt;
    [SerializeField] bool dmgOverTime;
    private DataPersistenceManager dataManager; //i dont like this, but dont wnat to rework whole system rn
    public GameObject owner;

    [SerializeField] bool triggerExtraScript;
    [SerializeField] string functionName;
    [SerializeField] private AudioSource grenadeThrow;
    [SerializeField] private AudioSource grenadeExplosion;
    bool isExploded;
    private float rotationSpeed;

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        dmg *= dataManager.gameData.currentDamageMultiplier;
        grenadeThrow.Play();
        rotationSpeed = Random.Range(200, 300);
    }


    private void Update()
    {
        if(time >= fuse)
        {
            if (!isExploded) Explode();
            destroyObjectAfterSound();
        }
        time += Time.deltaTime;

        if (GetComponent<Rigidbody2D>()) GetComponent<Rigidbody2D>().SetRotation(GetComponent<Rigidbody2D>().rotation + rotationSpeed * Time.deltaTime);
        if (rotationSpeed > 0) rotationSpeed -= 200 * Time.deltaTime;
    }

    void destroyObjectAfterSound()
    {
        if(grenadeExplosion.isPlaying) return;
        Destroy(gameObject);
        Invoke("destroyObjectAfterSound", 0.5f);
    }

    void Explode()
    {
        DmgTrigger ExplosionRadius = Instantiate(dmgTrigger,transform.position,Quaternion.identity).GetComponent<DmgTrigger>();
        ExplosionRadius.assingVar(dmg,radius,duration,dmgOverTime,triggerExtraScript,functionName);
        Instantiate(ExplosionEffekt, transform.position, Quaternion.identity);
        isExploded = true;

        grenadeExplosion.Play();

        // make grenade vanish
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<SpriteRenderer>());
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(fuse / 10);
        GetComponent<Collider2D>().enabled = true;
    }
}

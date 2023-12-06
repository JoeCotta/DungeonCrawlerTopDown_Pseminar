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

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        dmg *= dataManager.gameData.currentDamageMultiplier;
    }

    private void Update()
    {
        if(time >= fuse)
        {
            Explode();
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }

    void Explode()
    {
        DmgTrigger ExplosionRadius = Instantiate(dmgTrigger,transform.position,Quaternion.identity).GetComponent<DmgTrigger>();
        ExplosionRadius.assingVar(dmg,radius,duration,dmgOverTime,triggerExtraScript,functionName);
        Instantiate(ExplosionEffekt, transform.position, Quaternion.identity);
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(fuse / 10);
        GetComponent<Collider2D>().enabled = true;
    }
}

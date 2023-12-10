using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Barrel : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject weapon;

    void Start()
    {
        updateAI(); 
    }

    private void hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (Random.value <= 0.1f) SpawnItem();
            Destroy(gameObject);
        }
    }

    void SpawnItem()
    {
        GameObject temp = Instantiate(weapon, transform.position, Quaternion.identity);
        temp.GetComponent<AlternateWS>().weaponType = DataBase.weaponType(-2);
        temp.GetComponent<AlternateWS>().rarity = DataBase.rarity(-2);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        updateAI(); 
    }

    void updateAI()
    {
        AstarPath.active.AddWorkItem(new AstarWorkItem(() => {
            // Safe to update graphs here

            Bounds bounds = GetComponent<Collider2D>().bounds;
            var guo = new GraphUpdateObject(bounds);

            // Set some settings
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject weapon;

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
}

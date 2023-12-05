using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().weapon.GetComponent<AlternateWS>().reserve += collision.GetComponent<Player>().weapon.GetComponent<AlternateWS>().ammo;
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<Player>().weapon)
        {
            AlternateWS alternateWS = collision.GetComponent<Player>().weapon.GetComponent<AlternateWS>();
            //If minigun not 3 mags
            if (alternateWS.mag != 500) alternateWS.reserve += alternateWS.mag * 3;
            else alternateWS.reserve += alternateWS.mag;
            Destroy(gameObject);
        }
    }
}

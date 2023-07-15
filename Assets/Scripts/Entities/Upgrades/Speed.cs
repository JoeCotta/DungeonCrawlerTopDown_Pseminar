using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player temp;
        if (collision.gameObject.CompareTag("Player"))
        {
            temp = collision.gameObject.GetComponent<Player>();

            //change stats
            temp.maxMovementSpeed += 0.5f;


            Destroy(gameObject);
        }
    }
    
}

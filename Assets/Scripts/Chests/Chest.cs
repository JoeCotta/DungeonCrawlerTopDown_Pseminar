using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject[] lootTable;
    public GameObject bean;
    public float  price; //maybe there will be a curse like 1.25x prices => decimals => have to round
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            if(other.gameObject.GetComponent<Player>().playerGold >= Mathf.Round(price)){
                other.gameObject.GetComponent<Player>().playerGold -= Mathf.Round(price);
                int item = Random.Range(0,lootTable.Length);
                Instantiate(lootTable[item],transform.position,Quaternion.identity);
                //if (Random.Range(1,4) == 1) Instantiate(bean, transform.position, Quaternion.identity); // secret you won't find
                Destroy(gameObject);
            }
            //Debug.Log("Player touched chest");
        }
    }
}

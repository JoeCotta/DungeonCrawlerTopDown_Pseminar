using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public GameObject text;
    public int chestLevel;
    public GameObject weaponPrefab; 
    public GameObject boostPrefab;

    private float  price; //maybe there will be a curse like 1.25x prices => decimals => have to round
    private List<int> lootTable = new List<int>();

    void Start()
    {
        // if this is an instance of the scene chest
        if(chestLevel == -1) return;

        // calculates the price of the chest
        price = chestLevel * 10 + 5;

        // set the info text when colliding with the chest
        text.GetComponent<TextMeshPro>().text = "press 'E' to pay " + price.ToString() + " coins to open the Level " + (chestLevel + 1).ToString() + " chest";
    }

    // spawns a weapon
    void spawnWeapon()
    {
        int weaponType = DataBase.weaponType(chestLevel);
        int rarity = DataBase.rarity(chestLevel);

        // creates the weapon
        GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);

        weapon.GetComponent<AlternateWS>().weaponType = weaponType; // sets the weaponType
        weapon.GetComponent<AlternateWS>().rarity = rarity;
    }

    // spawn boost
    void spawnBoost()
    {
        Instantiate(boostPrefab, transform.position, Quaternion.identity);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            text.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().playerGold >= Mathf.Round(price) && Input.GetKey("e"))
            {
                other.gameObject.GetComponent<Player>().playerGold -= Mathf.Round(price);

                // spawns a weapon or a boost orb
                // level 0 -> 30% weapon
                // level 1 -> 60% weapon
                // level 2 -> 90% weapon
                if (Random.value <= 0.3f * (chestLevel+1)) spawnWeapon();
                else spawnBoost();

                // Destroys the chest
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }
}

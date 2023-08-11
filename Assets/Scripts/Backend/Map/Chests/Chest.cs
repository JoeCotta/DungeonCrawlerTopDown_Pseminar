using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public GameObject text;
    public int chestLevel;
    public GameObject weaponPrefab; 

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

        // gets the weapon Stats
        weaponStats[] weapons = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().weaponStatsList;
        int countWeapons = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().countWeapons;

        // setting up loot table
        // creating a loot table with 1000 weapons -> if the chance is 0.256 then there will be 256 weapons of this type in the list to simulate the chance
        for (int i = 0; i < countWeapons; i++)
        {
            float chance = weapons[i].chestSpawnWeaponChances[chestLevel].chance * 1000;
            for (int j = 0; j < chance; j++) lootTable.Add(weapons[i].id);
        }

        if(lootTable.Count != 1000) Debug.Log("Weapon Chances of Chest Level " + chestLevel + " are wrong");

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

                // selects a random weaponType
                int item = Random.Range(0, lootTable.Count);
                int weaponType = lootTable[item];

                // creates the weapon
                GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);

                weapon.GetComponent<AlternateWS>().weaponType = weaponType; // sets the weaponType

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

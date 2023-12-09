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
    [SerializeField] private AudioSource spendMoney;
    [SerializeField] private GameObject armorPrefab;
    
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
    
    void spawnArmor()
    {
        float random = Random.Range(0, 100);
        int armorLevel = 0;

        // level 01  -> 25 %
        // level 02  -> 20 %
        // level 03  -> 15 %
        // level 04  -> 10 %
        // level 05  -> 08 %
        // level 06  -> 07 %
        // level 07  -> 06 %
        // level 08  -> 04 %
        // level 09  -> 03 %
        // level 10  -> 02 %
        
        if (random < 25) armorLevel = 1; 
        else if (random < 45) armorLevel = 2;
        else if (random < 60) armorLevel = 3;
        else if (random < 70) armorLevel = 4;
        else if (random < 78) armorLevel = 5;
        else if (random < 85) armorLevel = 6;
        else if (random < 91) armorLevel = 7;
        else if (random < 95) armorLevel = 8;
        else if (random < 98) armorLevel = 9;
        else if (random < 100) armorLevel = 10;


        // creates the armour
        GameObject oldArmour = Instantiate(armorPrefab, transform.position, Quaternion.identity);

        // sets the level of the armour
        oldArmour.SendMessage("setLevel", armorLevel);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            text.SetActive(true);
        }
    }

    void onAudioEnd()
    {
        if (!spendMoney.isPlaying) Destroy(gameObject);
        else Invoke("onAudioEnd", 0.1f);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().playerGold >= Mathf.Round(price) && Input.GetKey("e"))
            {
                other.gameObject.GetComponent<Player>().playerGold -= Mathf.Round(price);
                
                // play Buy Sound
                spendMoney.Play();

                // make the chest not noticeable until the sound is over
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                gameObject.GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                // Destroys the visible children
                foreach( Transform child in transform )
                {
                    child.gameObject.SetActive( false );
                } 
    
                
                onAudioEnd();

                // spawns a weapon or a boost orb
                // weapon   -> 30%
                // boost    -> 40%
                // armor    -> 30%

                float random = Random.value;
                
                if (random <= 0.3f * (chestLevel+1)) spawnWeapon();
                else if (random <= 0.7f) spawnBoost();
                else spawnArmor();
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

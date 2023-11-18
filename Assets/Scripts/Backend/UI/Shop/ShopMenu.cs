using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShopMenu : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    [SerializeField] private AudioSource buySound;

    [HideInInspector] public float maxHealthUpPrice;
    [HideInInspector] public float startArmourPrice;
    [HideInInspector] public float revivePrice;
    [HideInInspector] public float damageMultiplierPrice;


    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
    }

    void Update()
    {
        // max Health Up
        maxHealthUpPrice = Mathf.Pow(dataPersistenceManager.gameData.maxHealthlvl, 2) + 5;

        // Start Armour
        startArmourPrice = Mathf.Pow(dataPersistenceManager.gameData.armorLevel + 2, 2) + 5;

        // revives
        revivePrice = Mathf.Pow(dataPersistenceManager.gameData.reviveLevel + 1, 2) + 10;

        // damage Multiplier
        damageMultiplierPrice = Mathf.RoundToInt(0.25f * Mathf.Pow(dataPersistenceManager.gameData.damageMultiplierLevel + 1, 2)-15);
    }

    public void exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void buyMaxHealthUp()
    {
        if (dataPersistenceManager.gameData.currentCoins >= maxHealthUpPrice) 
        {
            dataPersistenceManager.gameData.maxHealthlvl += 1;
            dataPersistenceManager.gameData.currentCoins -= maxHealthUpPrice;
            dataPersistenceManager.gameData.currentMaxHealth = 5 * dataPersistenceManager.gameData.maxHealthlvl;
            dataPersistenceManager.gameData.currentHealth = dataPersistenceManager.gameData.currentMaxHealth;

            dataPersistenceManager.SaveGame();

            buySound.Play();
        }
    }

    public void buyStartArmour()
    {
        if(dataPersistenceManager.gameData.armorLevel >= 5) return;

        if (dataPersistenceManager.gameData.currentCoins >= startArmourPrice) 
        {
            dataPersistenceManager.gameData.armorLevel += 1;
            dataPersistenceManager.gameData.currentCoins -= startArmourPrice;
            dataPersistenceManager.gameData.startArmor = dataPersistenceManager.gameData.armorLevel;
            dataPersistenceManager.gameData.currentArmor = dataPersistenceManager.gameData.startArmor;

            dataPersistenceManager.SaveGame();

            buySound.Play();
        }
    }

    public void buyRevives()
    {
        if (dataPersistenceManager.gameData.currentCoins >= revivePrice) 
        {
            dataPersistenceManager.gameData.reviveLevel += 1;
            dataPersistenceManager.gameData.currentCoins -= revivePrice;
            dataPersistenceManager.gameData.revivesLeft = dataPersistenceManager.gameData.reviveLevel;

            dataPersistenceManager.SaveGame();

            buySound.Play();
        }
    }
    
    public void buyDamageMultiplier()
    {
        if (dataPersistenceManager.gameData.currentCoins >= damageMultiplierPrice) 
        {
            dataPersistenceManager.gameData.damageMultiplierLevel += 1;
            dataPersistenceManager.gameData.currentCoins -= damageMultiplierPrice;
            dataPersistenceManager.gameData.currentDamageMultiplier = dataPersistenceManager.gameData.damageMultiplierLevel / 10;
            dataPersistenceManager.gameData.permanentDamageMultiplier = dataPersistenceManager.gameData.damageMultiplierLevel / 10;

            dataPersistenceManager.SaveGame();

            buySound.Play();
        }
    }
}

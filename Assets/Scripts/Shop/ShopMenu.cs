using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShopMenu : MonoBehaviour
{
    public DataPersistenceManager dataPersistenceManager;
    public float priceMaxHealth = 10;
    public float priceArmor = 15;

    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        //priceArmor = Mathf.Pow(1.5f, dataPersistenceManager.gameData.startArmor) * priceArmor;
    }

    public void openMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void MaxHealthUp()
    {
        if(dataPersistenceManager.gameData.currentCoins >= priceMaxHealth)
        {
            dataPersistenceManager.gameData.currentCoins -= priceMaxHealth;
            dataPersistenceManager.gameData.maxHealth += 5;
            dataPersistenceManager.gameData.maxHealthlvl++;
            dataPersistenceManager.gameData.currentMaxHealth = dataPersistenceManager.gameData.maxHealth;
            dataPersistenceManager.gameData.currentHealth = dataPersistenceManager.gameData.maxHealth;
            dataPersistenceManager.SaveGame();
        }
    }

    public void startArmor()
    {
        if(dataPersistenceManager.gameData.currentCoins >= priceArmor && dataPersistenceManager.gameData.startArmor < 5)
        {
            dataPersistenceManager.gameData.currentCoins -= priceArmor;
            dataPersistenceManager.gameData.startArmor++;
            dataPersistenceManager.gameData.currentArmor = dataPersistenceManager.gameData.startArmor;
            dataPersistenceManager.SaveGame();
        }
    }
}

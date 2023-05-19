using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShopMenu : MonoBehaviour
{
    public DataPersistenceManager dataPersistenceManager;

    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
    }

    public void openMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void MaxHealthUp()
    {
        if(dataPersistenceManager.gameData.currentCoins >= 10)
        {
            dataPersistenceManager.gameData.currentCoins -= 10;
            dataPersistenceManager.gameData.maxHealth += 5;
            dataPersistenceManager.gameData.maxHealthlvl++;
            dataPersistenceManager.gameData.currentMaxHealth = dataPersistenceManager.gameData.maxHealth;
            dataPersistenceManager.gameData.currentHealth = dataPersistenceManager.gameData.maxHealth;
            dataPersistenceManager.SaveGame();
        }
        Debug.Log(dataPersistenceManager.gameData.maxHealth + "  " + dataPersistenceManager.gameData.currentCoins);
    }
}

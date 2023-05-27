using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShopMenu : MonoBehaviour
{
    private float dumpVariable; //only used for not used variables
    private int dumpVariableInt;


    public DataPersistenceManager dataPersistenceManager;
    public float basepriceMaxHealth;
    public float priceMaxHealth = 10;
    public float basepriceArmor;
    public float priceArmor = 15;
    public float basepriceRevive;
    public float priceRevive = 50;

    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        //priceArmor = Mathf.Pow(1.5f, dataPersistenceManager.gameData.startArmor) * basepriceArmor;
    }

    
    public void MaxHealthUp()
    {
        buy(ref dataPersistenceManager.gameData.maxHealth, true, ref dataPersistenceManager.gameData.maxHealthlvl, 5, priceArmor, true, ref dataPersistenceManager.gameData.currentMaxHealth, true, ref dataPersistenceManager.gameData.currentHealth);
    }

    public void startArmor()
    {
        if(dataPersistenceManager.gameData.startArmor > 5) buy(ref dataPersistenceManager.gameData.startArmor, false, ref dumpVariableInt, 1, priceArmor, true, ref dataPersistenceManager.gameData.currentArmor, false, ref dumpVariable);
        //priceArmor = Mathf.Pow(1.5f, dataPersistenceManager.gameData.startArmor) * basepriceArmor;
    }

    public void buyRevives()
    {
        //you wanted it soft coded =[           (its a mess)
        buy(ref dataPersistenceManager.gameData.revivesLeft, false, ref dumpVariableInt, 1, priceRevive, false, ref dumpVariable, false, ref dumpVariable);
    }



    
    public void buy(ref float stat, bool hasLVL,ref int level, int incrementPerUpgrade, float cost, bool setCurrentStat, ref float currentstat, bool setSubStat, ref float subStat)
    {
        //softcoded version
        if (dataPersistenceManager.gameData.currentCoins >= cost)
        {
            dataPersistenceManager.gameData.currentCoins -= cost;
            stat += 1 * incrementPerUpgrade;
            if (hasLVL) level++;
            if (setCurrentStat) currentstat = stat;
            if (setSubStat) subStat = stat;
            dataPersistenceManager.SaveGame();
        }
    }







    //Menu Managment
    public void openMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

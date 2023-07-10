using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isDead;

    
    //Player Data
    public float currentCoins;
    public float maxHealth;
    public float revivesLeft;
    public float startArmor;
    public int startWeaponType;

    // Levels
    public int maxHealthlvl;
    public int reviveLevel;
    public int armorLevel;


    //run specific data
    public float currentMaxHealth;
    public float currentHealth;
    public float currentArmor;
    public int currentWeaponType;

    private int dumpVariableInt;
    private float dumpVariableFloat;

    //statistics
    public int enemysKilled;

    public GameData()
    {
        isDead = false;
        currentCoins = 0;
        maxHealth = 20;
        revivesLeft = 0;
        startArmor = 0;
        startWeaponType = 0;

        maxHealthlvl = 0;
        reviveLevel = 0;
        armorLevel = 0;

        currentMaxHealth = 20;
        currentHealth = currentMaxHealth;
        currentArmor = 0;
        currentWeaponType = 0;

        enemysKilled = 0;
    }
    public ref int getLevel(string itemName)
    {
        switch (itemName.ToLower())
        {
            case "maxhealth":
                return ref maxHealthlvl;
            case "startarmor":
                return ref armorLevel;
            case "buyrevives":
                return ref reviveLevel;
        }
        return ref dumpVariableInt;
    }
    private ref float getMaxValue(string itemName)
    {
        switch (itemName.ToLower())
        {
            case "maxhealth":
                return ref currentMaxHealth;
            case "startarmor":
                return ref startArmor;
            case "buyrevives":
                return ref revivesLeft;
        }
        return ref dumpVariableFloat;
    }

    private ref float getValue(string itemName)
    {
        switch (itemName.ToLower())
        {
            case "maxhealth":
                return ref currentHealth;
            case "startarmor":
                return ref currentArmor;
            case "buyrevives":
                return ref revivesLeft;
        }
        return ref dumpVariableFloat;
    }

    public void updateValues()
    {
        ShopItems[] shopItemsList = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().shopItemsList;
        int countShopItems = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().countShopItems;

        for(int i = 0; i < countShopItems; i++)
        {
            string name = shopItemsList[i].name;
            getMaxValue(name) = getLevel(name) * shopItemsList[i].incrementPerUpgrade;
        }
    }

    public void setValuesToMax()
    {
        ShopItems[] shopItemsList = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().shopItemsList;
        int countShopItems = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().countShopItems;

        for(int i = 0; i < countShopItems; i++)
        {
            string name = shopItemsList[i].name;
            getValue(name) = getMaxValue(name);
        }
    }
}

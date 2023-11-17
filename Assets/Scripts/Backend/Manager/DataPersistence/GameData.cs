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
    public float currentDamageMultiplier;
    public float permanentDamageMultiplier;

    // Levels
    public int maxHealthlvl;
    public int reviveLevel;
    public int armorLevel;
    public int damageMultiplierLevel;


    //run specific data
    public float currentMaxHealth;
    public float currentHealth;
    public float currentArmor;
    public int currentWeaponType, currentRarity;
    public float currentReserve, currentAmmo;

    private int dumpVariableInt;
    private float dumpVariableFloat;

    //statistics
    public int enemysKilled;


    // options
    public float MasterVol;
    public float WeaponVol;
    public float PlayerVol;
    public float EnemyVol;
    public float UIVol;
    public float MapVol;
    public float MusicVol;
    public float BoostsVol;

    public GameData()
    {
        isDead = false;
        currentCoins = 0;
        maxHealth = 20;
        revivesLeft = 0;
        startArmor = 0;
        startWeaponType = 0;

        maxHealthlvl = 4;
        reviveLevel = 0;
        armorLevel = 0;
        damageMultiplierLevel = 10;

        currentMaxHealth = 20;
        currentHealth = currentMaxHealth;
        currentArmor = 0;
        currentWeaponType = 0;
        currentRarity = 0;
        currentReserve = -1;
        currentAmmo = -1;

        enemysKilled = 0;

        currentDamageMultiplier = damageMultiplierLevel / 10;
        permanentDamageMultiplier = damageMultiplierLevel / 10;

        MasterVol = 0;
        WeaponVol = 0;
        PlayerVol = 3;
        EnemyVol = -3;
        UIVol = 0;
        MapVol = 0;
        MusicVol = -12;
        BoostsVol = 0;
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
            case "damagemultiplier":
                return ref damageMultiplierLevel;
        }
        return ref dumpVariableInt;
    }
    ref float getMaxValue(string itemName)
    {
        switch (itemName.ToLower())
        {
            case "maxhealth":
                return ref currentMaxHealth;
            case "startarmor":
                return ref startArmor;
            case "buyrevives":
                return ref revivesLeft;
            case "damagemultiplier":
                return ref permanentDamageMultiplier;
        }
        return ref dumpVariableFloat;
    }
    public ref float getValue(string itemName)
    {
        switch (itemName.ToLower())
        {
            case "maxhealth":
                return ref currentHealth;
            case "startarmor":
                return ref currentArmor;
            case "buyrevives":
                return ref revivesLeft;
            case "damagemultiplier":
                return ref permanentDamageMultiplier;
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
            getMaxValue(name) = (float)getLevel(name) * shopItemsList[i].incrementPerUpgrade;
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

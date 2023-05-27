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
    public int maxHealthlvl;
    public float startArmor;
    public float revivesLeft;


    //run specific data
    public float currentMaxHealth;
    public float currentHealth;
    public float currentArmor;
    public GameObject weaponSaved;
    public GameObject weaponPrefabSaved;

    public GameData()
    {
        isDead = false;
        currentCoins = 0;
        maxHealth = 20;
        maxHealthlvl = 0;
        startArmor = 0;
        currentMaxHealth = 20;
        currentHealth = currentMaxHealth;
        currentArmor = 0;
    }
}

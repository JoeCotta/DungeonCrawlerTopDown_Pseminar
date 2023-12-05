using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool     isDead;

    
    //Player Data
    public float    currentCoins;
    public float    maxHealth;
    public float    revivesLeft;
    public float    startArmor;
    public int      startWeaponType;
    public float    currentDamageMultiplier;
    public float    permanentDamageMultiplier;

    // Levels
    public int  maxHealthlvl;
    public int  reviveLevel;
    public int  armorLevel;
    public int  damageMultiplierLevel;


    //run specific data
    public float    currentMaxHealth;
    public float    currentHealth;
    public float    currentArmor;
    public int      currentWeaponType, currentRarity;
    public float    currentReserve, currentAmmo;
    public int      second_currentWeaponType, second_currentRarity;
    public float    second_currentReserve, second_currentAmmo;
    public bool     hadSecondWeapon;
    public float    timeInRun;

    private int     dumpVariableInt;
    private float   dumpVariableFloat;

    //statistics
    public int  enemysKilled;


    // options
    public float    MasterVol;
    public float    WeaponVol;
    public float    PlayerVol;
    public float    EnemyVol;
    public float    UIVol;
    public float    MapVol;
    public float    MusicVol;
    public float    BoostsVol;
    public bool     susMode;
    public bool     hardcoreMode;
    public bool     movementKeysForDash;
    public bool     aimLine;

    public GameData()
    {
        isDead              = false;
        currentCoins        = 0;
        maxHealth           = 20;
        revivesLeft         = 0;
        startArmor          = 0;
        startWeaponType     = 0;

        maxHealthlvl            = 4;
        reviveLevel             = 0;
        armorLevel              = 0;
        damageMultiplierLevel   = 10;

        currentMaxHealth    = 20;
        currentHealth       = currentMaxHealth;
        currentArmor        = 0;
        currentWeaponType   = 0;
        currentRarity       = 0;
        currentReserve      = -1;
        currentAmmo         = -1;
        second_currentWeaponType = 0;
        second_currentRarity = 0;
        second_currentReserve = -1;
        second_currentAmmo = -1;
        hadSecondWeapon = false;

        enemysKilled = 0;

        currentDamageMultiplier     = damageMultiplierLevel / 10;
        permanentDamageMultiplier   = damageMultiplierLevel / 10;

        MasterVol       = 0;
        WeaponVol       = 0;
        PlayerVol       = 3;
        EnemyVol        = -3;
        UIVol           = 0;
        MapVol          = 0;
        MusicVol        = -12;
        BoostsVol       = 0;
        susMode         = false;
        hardcoreMode    = false;
        movementKeysForDash = false;
        aimLine         = true;
    }
}

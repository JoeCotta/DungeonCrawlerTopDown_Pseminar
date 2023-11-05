using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  

public class Explanations : MonoBehaviour
{

    public GameObject boosts;
    public GameObject weapons;
    public GameObject chests;
    
    
    private TMP_Text text;
    int stage = 0;
    float currentCoins;
    int startWeaponType;
    float currentMaxHealth;
    int currentWeaponType;
    int currentRarity;
    int enemysKilled;
    DataPersistenceManager dataPersistenceManager;

    void Start()
    { 
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        weapons.SetActive(false);
        boosts.SetActive(false);
        chests.SetActive(false);
        text = GetComponent<TMP_Text>();

        // Information
        this.text.text = "0/7 \nYou can get to the next step by pressing enter.";
        Invoke("getCurrentStats", 0.5f);
    }


    void getCurrentStats()
    {
        currentCoins = dataPersistenceManager.gameData.currentCoins;
        startWeaponType = dataPersistenceManager.gameData.startWeaponType;
        currentMaxHealth = dataPersistenceManager.gameData.currentMaxHealth;
        currentWeaponType = dataPersistenceManager.gameData.currentWeaponType;
        currentRarity = dataPersistenceManager.gameData.currentRarity;
        enemysKilled = dataPersistenceManager.gameData.enemysKilled;
    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Return))
        {
            stage += 1;
            nextStage();
        }
    }

    void nextStage()
    {
        switch(stage)
        {   
            // stage 1: movement
            //          - WASD
            //          - shift
            case 1:
                this.text.text = "1/7\n";
                this.text.text += "1) You can move around using W A S D.";
                break;
            case 2:
                this.text.text = "1/7\n";
                this.text.text += "You can dash towards your mouse by pressing LSHIFT.";
                break;

            // stage 2: UI
            //          - health
            //          - coins / dmg multiplier
            //          - magazine
            //          - minimap (CapsLock)
            case 3:
                this.text.text = "2/7\n";
                this.text.text += "At the top left corner you can see your current health.";
                break;
            case 4:
                this.text.text = "2/7\n";
                this.text.text += "At the bottom left corner you can see your coins and the current damage multiplier.";
                break;
            case 5:
                this.text.text = "2/7\n";
                this.text.text += "At the bottom right corner you can see your how much bullets are left in your weapon.";
                break;
            case 6:
                this.text.text = "2/7\n";
                this.text.text += "At the top right corner you can see the map. You can enlarge it by pressing CapsLock.";
                break;

            // stage 3: weapons
            //          - shoot/reload/pickup
            //          - weapon types (pistol/rifle/sniper)
            //          - rarities (from boss?)

            case 7:
                this.text.text = "3/7\n";
                this.text.text += "When holding a weapon you can shoot using the left mouse button.";
                break;
            case 8:
                this.text.text = "3/7\n";
                this.text.text += "You can reload your weapon by pressing R.";
                break;
            case 9:
                this.text.text = "3/7\n";
                this.text.text += "A weapon can be picked up or dropped with F.";
                break;
            case 10:
                this.text.text = "3/7\n";
                this.text.text += "There are three different types of weapons: pistols, Rifles and Snipers with different characteristics. Try them out!";
                weapons.SetActive(true);
                break;
            case 11:
                this.text.text = "3/7\n";
                this.text.text += "Furthermore there are four different rarities of the weapons: common, rare, epic and legendary.\n";
                this.text.text += "The higher the rarity the better the weapon is.\nLegendary weapons can only be dropped by the boss at the end of a dungeon.";
                break;

            // stage 4: slots
            //          - weapon
            //          - armor
            case 13:
                this.text.text = "4/7\n";
                this.text.text += "There are two types of slots: you can enter them by pressing one or two.";
                break;
            case 14:
                this.text.text = "4/7\n";
                this.text.text += "When the first slot is selected you can drop your weapon.";
                break;
            case 15:
                this.text.text = "4/7\n";
                this.text.text += "When the second slot is selected you can drop your armor.";
                break;

            // stage 5: enemies
            //          - different weapons & armor
            //          - can drop weapons or boosts
            case 16:
                this.text.text = "5/7\n";
                this.text.text += "Enemies can spawn with different weapons and armor: \nthey can potentially have any type of weapon. \nThere armor level is between one or ten (one is the worst and ten the best).";
                break;
            case 17:
                this.text.text = "5/7\n";
                this.text.text += "When you killed an enemy they can either drop their weapon, a boost or nothing.";
                break;


            // stage 6: boosts
            //          - health
            //          - speed
            //          - dmgMultiplier
            case 18:
                this.text.text = "6/7\n";
                this.text.text += "There are three types of boosts: the health,speed and the damage multiplier boost.\n Try them out!";
                boosts.SetActive(true);
                break;

            // stage 7: Map
            //          - enter room -> door closes, enemies spawning
            //          - chests (different levels) can drop weapons or boosts
            //          - complete dungeon when enter the portal (beating the boss)
            //          - if you die - you loose your weapon
            case 19:
                this.text.text = "7/7\n";
                this.text.text += "When entering a room all doors will close and they will only be opened when you have killed all the enemies.";
                break;
            case 20:
                this.text.text = "7/7\n";
                this.text.text += "After defeating a room there is a chance a chest will spawn. These have different levels (the higher the level the better the drops are).\nYou can open these chests by paying with your gathered coins.\nThe chests can either contain boosts or weapons.";
                chests.SetActive(true);
                break;
            case 21:
                this.text.text = "7/7\n";
                this.text.text += "In one room there will eventually spawn a portal after defeating the boss. This portal will bring you to the next dungeon";
                break;
            case 22:
                this.text.text = "7/7\n";
                this.text.text += "Be careful when playing the game - if you die your weapon is gone!";
                break;
            case 23:
                this.text.text = "Give the game a try!\nHave fun!!\nTo get to the main menu press enter";
                break;
            case 24:
                returnToMenu();
                break;
        }
    }

    void returnToMenu()
    {
        dataPersistenceManager.gameData.currentCoins = currentCoins;
        dataPersistenceManager.gameData.startWeaponType = startWeaponType;
        dataPersistenceManager.gameData.currentMaxHealth = currentMaxHealth;
        dataPersistenceManager.gameData.currentWeaponType = currentWeaponType;
        dataPersistenceManager.gameData.currentRarity = currentRarity;
        dataPersistenceManager.gameData.enemysKilled = enemysKilled;
        SceneManager.LoadScene("Menu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player; //script

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }























































    /*
    public void Save(){
        SaveSystem.SavePlayer(player);
    }
    
    public void Load(){
        PlayerData data = SaveSystem.LoadPlayer();
        player.health = data.currentHealth;
        player.maxHealth = data.currentMaxHealth;
        player.armourLevel = data.currentArmor ;
        player.playerGold = data.currentCoins;
        player.weapon = data.currentWeapon;
        player.dungeonFloor = data.currentDungeonFloor;
    }

    public void NextStage(){
        SceneManager.LoadScene("test room gen");
    }
    */
}

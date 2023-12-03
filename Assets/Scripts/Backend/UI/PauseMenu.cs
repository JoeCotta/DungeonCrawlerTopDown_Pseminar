using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    private Player player;

    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void QuitToMainMenu()
    {
        GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>().SaveGame();
        SceneManager.LoadScene("Menu");
    }

    public void QuickRestart()
    {
        if (dataPersistenceManager == null) return;
        player.health = dataPersistenceManager.gameData.currentMaxHealth;
        player.maxHealth = dataPersistenceManager.gameData.currentMaxHealth;
        player.armourLevel = Mathf.RoundToInt(dataPersistenceManager.gameData.startArmor);
        if (player.weapon && player.weapon.GetComponent<AlternateWS>())
        {
            player.weapon.GetComponent<AlternateWS>().weaponType = dataPersistenceManager.gameData.startWeaponType;
            player.weapon.GetComponent<AlternateWS>().reserve = -1; //to disable it loading old reserve
            player.weapon.GetComponent<AlternateWS>().rarity = 0;
        }



        player.isDead = false;

        dataPersistenceManager.SaveGame();
        SceneManager.LoadScene(2);
    }
}

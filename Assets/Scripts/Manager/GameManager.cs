using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> references;
    public DataPersistenceManager dataPersistenceManager;
    public Player player;
    private Image ffb;
    private float t;
    public float timeToFadeFromBlack;

    public bool isMenu;//set manually

    //remember to drag scene in files>build settings> here     and but the number next to it in here
    public int buildIndexOfSceneToLoad;

    private void Start()
    {
        Invoke("assingReferences", 0.1f);
    }

    private void Update()
    {
        if (player != null && player.isDead) resetRunData(); 

        if(t < timeToFadeFromBlack && ffb != null)
        {
            t += Time.deltaTime;
            ffb.color = new Color(0, 0, 0, 1-(t * 1 / timeToFadeFromBlack));
        }
    }

    private void assingReferences()
    {
        if(returnDataPersistence() != null)dataPersistenceManager = returnDataPersistence();
        if(returnPlayer() != null)player = returnPlayer();
        if(ffb != null)ffb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
    }

    public void resetRunData()
    {
        if(dataPersistenceManager == null) return;
        player.health = dataPersistenceManager.gameData.maxHealth;
        player.maxHealth = dataPersistenceManager.gameData.maxHealth;
        player.armourLevel = Mathf.RoundToInt(dataPersistenceManager.gameData.startArmor);
        player.weapon.GetComponent<weaponsystem>().weaponType = dataPersistenceManager.gameData.startWeaponType;



        player.isDead = false;

        dataPersistenceManager.SaveGame();
        SceneManager.LoadScene("Menu");
    }

    
    
    public void SaveGame()
    {
        dataPersistenceManager.SaveGame();
    }



    //functions for returning parts of the Reference List
    int playerTry; int dataTry;
    public Player returnPlayer()
    {
        //disable error msg in menu 
        if (isMenu) { return null; }
        foreach (GameObject reference in references)
        {
            if (reference.GetComponent<Player>())
            {
                return reference.GetComponent<Player>();
            }
        }
        Debug.Log("found no player; trying again");
        if (playerTry < 3) { Invoke("returnPlayer", 0.5f); playerTry++; } else { Debug.LogError("several trys NO PLAYER"); }
        return null;
    }

    public DataPersistenceManager returnDataPersistence()
    {
        //disable error msg in Menu
        if (isMenu) { return null; }
        foreach (GameObject reference in references)
        {
            if (reference.GetComponent<DataPersistenceManager>())
            {
                return reference.GetComponent<DataPersistenceManager>();
            }
        }
        Debug.Log("found no datapersistencemanager, trying again");
        if (dataTry < 3) { Invoke("returnDataPersistence", 0.5f); dataTry++; } else { Debug.LogError("several trys NO DATAManag"); }
        return null;
    }
}

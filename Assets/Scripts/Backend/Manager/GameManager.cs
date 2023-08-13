using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //private references
    private Image ffb;
    private GameObject hud, pause;
    public GameObject AWeaponAssing;

    //remember to drag scene in files>build settings> here     and but the number next to it in here
    private bool isMenu;//set manually

    //public vars
    static public List<GameObject> references;
    static public Player player;
    static public DataPersistenceManager dataPersistenceManager;
    public bool useAltInterface;
    static public bool useAlt, isPaused = false;
    static public int buildIndexOfSceneToLoad = 2;
    static public GameObject AWeapon;

    private void Start()
    {
        GameManager.references = new List<GameObject>(); //very important line
        useAlt = useAltInterface;
        AWeapon = AWeaponAssing;
        Invoke("assingReferences", 1f);
        if(GameObject.FindGameObjectWithTag("FTB")) ffb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
        if(GameObject.FindGameObjectWithTag("Hud")) hud = GameObject.FindGameObjectWithTag("Hud");
        if (GameObject.FindGameObjectWithTag("Pause")) { pause = GameObject.FindGameObjectWithTag("Pause"); pause.SetActive(false); }
    }

    float t; public float timeToFadeFromBlack;
    private void Update()
    {
        if (player != null && player.isDead) resetRunData(); 
        if(t < timeToFadeFromBlack && ffb != null)
        {
            t += Time.deltaTime;
            ffb.color = new Color(0, 0, 0, 1-(t * 1 / timeToFadeFromBlack));
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseSwitch();
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
        if(player.weapon.GetComponent<weaponsystem>())player.weapon.GetComponent<weaponsystem>().weaponType = dataPersistenceManager.gameData.startWeaponType;
        else if(player.weapon.GetComponent<AlternateWS>()) player.weapon.GetComponent<AlternateWS>().weaponType = dataPersistenceManager.gameData.startWeaponType;



        player.isDead = false;

        dataPersistenceManager.SaveGame();
        SceneManager.LoadScene("Menu");
    }

    
    
    public void SaveGame()
    {
        dataPersistenceManager.SaveGame();
    }


    public void PauseSwitch()
    {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;
                hud.SetActive(true);
                pause.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true;
                hud.SetActive(false);
                pause.SetActive(true);
            }
    }


    //functions for returning parts of the Reference List
    int playerTry; int dataTry;
    public Player returnPlayer()
    { 
        if (isMenu) { return null; }
        foreach (GameObject reference in GameManager.references)
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
        foreach (GameObject reference in GameManager.references)
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

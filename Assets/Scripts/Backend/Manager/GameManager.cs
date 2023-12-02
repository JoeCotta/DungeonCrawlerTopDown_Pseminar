using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour, IDataPersistence
{
    //private references
    private Image ffb;
    private GameObject hud, pause;
    public  GameObject AWeaponAssing;

    //remember to drag scene in files>build settings> here     and but the number next to it in here
    public  bool isMenu;//set manually

    //public vars
    //        public bool useAltInterface;
    //static  public bool useAlt, isPaused = false;

    static  public DataPersistenceManager dataPersistenceManager;
    static  public List<GameObject> references;
    static  public GameObject AWeapon;
            public Sprite susref_Right;
    static  public Sprite sus_Right;
            public Sprite susref_Left;
    static  public Sprite sus_Left;
            public Sprite susref_Small;
    static  public Sprite sus_Small;
    static  public Player player;
            public bool useAltInterface;
    static  public bool useAlt, isPaused = false;
            public bool susModeRef;
    static  public bool enableSusMode;
            public bool hardcoreModeRef = false;
    static  public bool hardcoreMode;
    static  public int buildIndexOfSceneToLoad = 2;

    public  int activeBoosts = 0;
    public  List<GameObject> boostList = new List<GameObject>();


    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private AudioSource startGameSound;
    private void Start()
    {
        GameManager.references = new List<GameObject>(); //very important line
        useAlt      = useAltInterface;
        AWeapon     = AWeaponAssing;
        sus_Right   = susref_Right;
        sus_Left    = susref_Left;
        sus_Small   = susref_Small;
        Invoke("assingReferences", 1f);
        if(GameObject.FindGameObjectWithTag("FTB"))         ffb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
        if(GameObject.FindGameObjectWithTag("Hud"))         hud = GameObject.FindGameObjectWithTag("Hud");
        if (GameObject.FindGameObjectWithTag("Pause")) {    pause = GameObject.FindGameObjectWithTag("Pause"); pause.SetActive(false); isPaused = false;  Time.timeScale = 1; }
    
        Invoke("updateVolume", 1f);

    }

    // update Volume
    void updateVolume()
    {
        masterMixer.SetFloat("MasterVol", dataPersistenceManager.gameData.MasterVol);
        masterMixer.SetFloat("WeaponVol", dataPersistenceManager.gameData.WeaponVol);
        masterMixer.SetFloat("PlayerVol", dataPersistenceManager.gameData.PlayerVol);
        masterMixer.SetFloat("EnemyVol", dataPersistenceManager.gameData.EnemyVol);
        masterMixer.SetFloat("UIVol", dataPersistenceManager.gameData.UIVol);
        masterMixer.SetFloat("MapVol", dataPersistenceManager.gameData.MapVol);
        masterMixer.SetFloat("MusicVol", dataPersistenceManager.gameData.MusicVol);
        masterMixer.SetFloat("BoostsVol", dataPersistenceManager.gameData.BoostsVol);
    }

    float t; public float timeToFadeFromBlack;
    private void Update()
    {
        if (player != null && player.isDead) resetRunData(); 
        if(t < timeToFadeFromBlack && ffb != null)
        {
            if(startGameSound && !startGameSound.isPlaying) startGameSound.Play();
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

    static public void resetRunData()
    {
        if(dataPersistenceManager == null) return;
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
        foreach (GameObject reference in references)
        {
            if (reference && reference.GetComponent<Player>())
            {
                return reference.GetComponent<Player>();
            }
        }
        Debug.Log("found no player; trying again");
        if (playerTry < 3) { Invoke("returnPlayer", 1f); playerTry++; } else { Debug.LogError("several trys NO PLAYER"); }
        return null;
    }

    public DataPersistenceManager returnDataPersistence()
    {
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

    public void HardCoreModeSwitch()
    {
        hardcoreMode = !hardcoreMode;
    }

    public void LoadData(GameData data)
    {
        this.susModeRef = data.susMode;
        enableSusMode = susModeRef;
        this.hardcoreModeRef = data.hardcoreMode;
        hardcoreMode = hardcoreModeRef;
    }

    public void SaveData(ref GameData data)
    {
        hardcoreModeRef = hardcoreMode;
        data.hardcoreMode = this.hardcoreModeRef;
    }
}

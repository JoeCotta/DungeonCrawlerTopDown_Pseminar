using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    public GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }




    private void Awake() {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene");
        }
        instance = this;
    }
    
    private void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private bool stop = false;
    private void Update()
    {
        if (!stop && GameObject.FindGameObjectWithTag("Manager") && GameManager.references != null) { GameManager.references.Add(this.gameObject); stop = true;}
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        PlayerPrefs.SetString("ReloadKey", "R");
        PlayerPrefs.SetString("DropKey", "F");
        PlayerPrefs.SetString("UpKey", "W");
        PlayerPrefs.SetString("LeftKey", "A");
        PlayerPrefs.SetString("DownKey", "S");
        PlayerPrefs.SetString("RightKey", "D");
        PlayerPrefs.SetString("DashKey", "LeftShift");
        PlayerPrefs.SetString("ShootKey", "Mouse0");
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        SaveGame();
    }

    public void LoadGame()
    {

        this.gameData = dataHandler.Load();

        if(this.gameData == null){
            Debug.Log("No data was found. Initializing data to default.");
            NewGame();
        }

        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if(dataPersistenceObj != null) dataPersistenceObj.LoadData(gameData);
        }

        // set difficulty
        int enemysKilled = gameData.enemysKilled;
        gameData.difficulty = (float)(0.03 * Mathf.Sqrt(enemysKilled)+1);
    }

    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if(dataPersistenceObj != null)dataPersistenceObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();    
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}

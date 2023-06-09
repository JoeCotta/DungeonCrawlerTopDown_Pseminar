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

    //remember to drag scene in files>build settings> here     and but the number next to it in here
    public int buildIndexOfSceneToLoad;

    private void Start()
    {
        Invoke("assingReferences", 1f);
        ffb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
    }

    private void Update()
    {
        if (player != null && player.isDead) resetRunData(); 
        if(t < timeToFadeFromBlack)
        {
            t += Time.deltaTime;
            ffb.color = new Color(0, 0, 0, 1-(t * 1 / timeToFadeFromBlack));
        }
    }

    private void assingReferences()
    {
        if(returnDataPersistence() != null)dataPersistenceManager = returnDataPersistence();
        if(returnPlayer() != null)player = returnPlayer();
    }

    public void resetRunData()
    {
        player.health = dataPersistenceManager.gameData.maxHealth;
        player.maxHealth = dataPersistenceManager.gameData.maxHealth;
        player.armourLevel = Mathf.RoundToInt(dataPersistenceManager.gameData.startArmor);

        player.isDead = false;

        dataPersistenceManager.SaveGame();
        SceneManager.LoadScene("Menu");
    }

    
    
    public void SaveGame()
    {
        dataPersistenceManager.SaveGame();
    }



    //functions for returning parts of the Reference List
    public Player returnPlayer()
    {
        foreach (GameObject reference in references)
        {
            if (reference.GetComponent<Player>())
            {
                return reference.GetComponent<Player>();
            }
        }
        Debug.Log("Gamemanager found no player");
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
        Debug.LogError("GameManager found no datapersistencemanager");
        return null;
    }
}

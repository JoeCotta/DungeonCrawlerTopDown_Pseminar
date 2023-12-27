using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    private Player player;
    [SerializeField] private GameObject quickRestartButton;

    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Invoke("setUpForExplanationsLevel", 0.5f);
    }

    void setUpForExplanationsLevel()
    {
        if (!dataPersistenceManager.gameData.isInTutorial) return;
        Destroy(quickRestartButton);
    }
    public void QuitToMainMenu()
    {
        if (dataPersistenceManager.gameData.isInTutorial) 
        {
            GameObject.Find("Explanation").GetComponent<Explanations>().returnToMenu();
        }
        else SceneManager.LoadScene("Menu");
        GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>().SaveGame();
    }

    public void QuickRestart()
    {
        GameManager.resetRunData();
        SceneManager.LoadScene(2);
    }
}

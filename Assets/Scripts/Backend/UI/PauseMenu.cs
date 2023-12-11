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
        GameManager.resetRunData();
        SceneManager.LoadScene(2);
    }
}

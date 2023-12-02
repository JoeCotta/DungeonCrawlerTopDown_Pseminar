using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>().SaveGame();
        SceneManager.LoadScene("Menu");
    }
}

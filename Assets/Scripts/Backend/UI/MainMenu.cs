using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //remember to drag scene in files>build settings> here     and but the number next to it in here
    public int buildIndexOfSceneToLoad;
    public void playGame(){
        //remember that button might be pointing to wrong menu(e. mainmenu/savemenu) if you save buildindex in scene
        //loadGame = true;
        GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>().SaveGame();
        SceneManager.LoadScene(buildIndexOfSceneToLoad);
    }

    public void quitGame(){
        Application.Quit();
    }

    public void openShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void openTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}

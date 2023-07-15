using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //remember to drag scene in files>build settings> here     and but the number next to it in here
    public int buildIndexOfSceneToLoad;
    /*float t = 0; float timeToFade = 3; bool loadGame; Image ftb;
    private void Start()
    {
        ftb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
    }
    private void Update()
    {
        if (loadGame)
        {
            if (t < timeToFade)
            {
                t += Time.deltaTime;
                ftb.color = new Color(0, 0, t * 1 / timeToFade);
            }
            else SceneManager.LoadScene(buildIndexOfSceneToLoad);
        }   
    }*/
    public void playGame(){
        //reme,ber that button might be pointing to wrong menu(e. mainmenu/savemenu) if you save buildindex in scene
        //loadGame = true;
        SceneManager.LoadScene(buildIndexOfSceneToLoad);
    }

    public void quitGame(){
        Application.Quit();
    }

    public void openShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    private GameManager gameManager;
    private Image ftb; //fade to black
    public float timeToFade;
    private float t = 0;
    public bool loadScene;
    public bool playSound;
    private int countEnterPortal;
    // Sound
    [SerializeField] private AudioSource levelCompleteSound;
    [SerializeField] private AudioSource startGameSound;
    private DataPersistenceManager dataPersistenceManager;

    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        GameManager.references.Add(gameObject);
        ftb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
        countEnterPortal = 0;
    }

    

    public void Update()
    {
        if (GameManager.isPaused) return;

        if (loadScene)
        {
            if (t < timeToFade)
            {
                t += Time.deltaTime;
                ftb.color = new Color(0, 0, 0, t * 1 / timeToFade);
            }
            else 
            {
                if (dataPersistenceManager.gameData.isInTutorial) SceneManager.LoadScene("Tutorial");
                else
                {
                    SceneManager.LoadScene(2);
                    gameManager.SaveGame();
                }
            }
        }
    }

    void playLevelCompleteSound()
    {
        if(countEnterPortal == 1){
            levelCompleteSound.Play();
            // updates floor
            GameManager.player.currentFloor += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            loadScene = true;
            countEnterPortal += 1;
            playLevelCompleteSound();
        }
    }
}

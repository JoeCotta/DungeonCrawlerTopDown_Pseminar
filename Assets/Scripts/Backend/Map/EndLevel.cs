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
    // Sound
    [SerializeField] private AudioSource levelCompleteSound;
    [SerializeField] private AudioSource startGameSound;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        GameManager.references.Add(gameObject);
        ftb = GameObject.FindGameObjectWithTag("FTB").GetComponent<Image>();
    }

    

    public void Update()
    {
        if (GameManager.isPaused) return;

        if (loadScene)
        {
            gameManager.SaveGame();
            if (t < timeToFade)
            {
                t += Time.deltaTime;
                ftb.color = new Color(0, 0, 0, t * 1 / timeToFade);
            }
            else{
                SceneManager.LoadScene(GameManager.buildIndexOfSceneToLoad);
                startGameSound.Play();
            }
        }
        if (playSound) {
            playSound = false;
            levelCompleteSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            loadScene = true;
            playSound = true;
        }
    }
}

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

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        gameManager.references.Add(gameObject);
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
            else SceneManager.LoadScene(gameManager.buildIndexOfSceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            loadScene = true;
        }
    }
}

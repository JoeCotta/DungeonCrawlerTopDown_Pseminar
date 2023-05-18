using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        gameManager.references.Add(gameObject);
    }

    public void loadNextLevel()
    {
        gameManager.SaveGame();
        SceneManager.LoadScene("test room gen");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.loadNextLevel();
        }
    }
}

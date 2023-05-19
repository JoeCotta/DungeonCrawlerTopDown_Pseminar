using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    public GameObject player;
    public DataPersistenceManager dataPersistenceManager;
    public Text text;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
    }

    void Update()
    {
        if (player != null)
        {
            text.text = "Coins: " + player.GetComponent<Player>().playerGold.ToString();
        }
        else
        {
            text.text = "Coins: " + dataPersistenceManager.gameData.currentCoins.ToString();
        }
    }
}

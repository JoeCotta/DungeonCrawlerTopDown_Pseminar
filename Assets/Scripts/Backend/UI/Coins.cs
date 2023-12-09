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
        if(GameObject.FindGameObjectWithTag("Player")) player = GameObject.FindGameObjectWithTag("Player");
        if(GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>()) dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
    }

    void Update()
    {
        if (player != null)
        {
            text.text = player.GetComponent<Player>().playerGold.ToString();
        }
        else
        {
            text.text = dataPersistenceManager.gameData.currentCoins.ToString();
        }
    }
}

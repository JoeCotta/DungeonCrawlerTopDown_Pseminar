using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public GameObject player;
    public Text text;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        text.text = "Coins: " + player.GetComponent<Player>().playerGold.ToString();
    }
}

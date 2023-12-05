using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCount : MonoBehaviour
{
    public GameObject player;
    public Text text;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        text.text = Mathf.Ceil(player.GetComponent<Player>().health).ToString() + "/" +  player.GetComponent<Player>().maxHealth;
    }
}

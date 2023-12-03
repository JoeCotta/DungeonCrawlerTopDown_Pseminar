using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject player;
    public Slider slider;
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
                                                                        slider.value = player.GetComponent<Player>().health / player.GetComponent<Player>().maxHealth;
        if (Mathf.Round(player.GetComponent<Player>().health) == 0)     slider.value = 1f / player.GetComponent<Player>().maxHealth;
    }
}

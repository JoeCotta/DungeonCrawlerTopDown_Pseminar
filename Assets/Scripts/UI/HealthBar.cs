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
        slider.value = player.GetComponent<Player>().health;
    }
}

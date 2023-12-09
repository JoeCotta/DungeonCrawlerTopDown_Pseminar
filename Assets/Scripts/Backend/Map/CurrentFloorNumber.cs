using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentFloorNumber : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    private GameManager gameManager;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if(GameManager.player) text.text = GameManager.player.currentFloor.ToString();
    }
}

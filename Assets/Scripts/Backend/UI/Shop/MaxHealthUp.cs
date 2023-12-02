using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MaxHealthUp : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    private ShopMenu shopMenu;
    private TextMeshProUGUI text;
    private int level;
    private float price;

    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").GetComponent<ShopMenu>();
        text = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        level = dataPersistenceManager.gameData.maxHealthlvl;
        price = shopMenu.maxHealthUpPrice;

        text.text = "Max Health\r\n";
        text.text += "Level " + (level - 4).ToString() + " -> " + (level - 4 + 1).ToString() + "\r\n";
        text.text += price.ToString() + " Coins";
    }

}

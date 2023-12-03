using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartArmor : MonoBehaviour
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
        level = dataPersistenceManager.gameData.armorLevel;
        price = shopMenu.startArmourPrice;

        text.text = "Start Armour\r\n";
        if (level < 5)
        {
            text.text += "Level " + level.ToString() + " -> " + (level + 1).ToString() + "\r\n";
            text.text += price.ToString() + " Coins\n";
        }
        if (level >= 5) text.text +=    "Level 5\r\n " +
                                        "Maxed";
    }
}

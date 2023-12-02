using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageMultiplier : MonoBehaviour
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
        level = dataPersistenceManager.gameData.damageMultiplierLevel;
        price = shopMenu.damageMultiplierPrice;

        text.text = "Damage Multiplier\r\n";
        text.text += "Level " + (level - 10).ToString() + " -> " + (level - 10 + 1).ToString() + "\r\n";
        text.text += price.ToString() + " Coins";
    }
}

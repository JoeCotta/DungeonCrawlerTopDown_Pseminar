using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrenadeShopItem : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    private ShopMenu shopMenu;
    private TextMeshProUGUI text;
    private int count;
    private float price;

    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").GetComponent<ShopMenu>();
        text = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        count = dataPersistenceManager.gameData.grenades;
        price = shopMenu.grenadesPrice;

        text.text = "Grenades\r\n";
        text.text += "Level " + count.ToString() + " -> " + (count + 1).ToString() + "\r\n";
        text.text += price.ToString() + " Coins";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartArmor : MonoBehaviour
{
    public DataPersistenceManager dataPersistenceManager;
    public ShopMenu shopMenu;
    public TextMeshProUGUI text;

    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").GetComponent<ShopMenu>();
        text = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "Starting Armor\r\n"+ shopMenu.priceArmor + " Coins" + "\r\n lvl: " + dataPersistenceManager.gameData.startArmor.ToString();
    }
}

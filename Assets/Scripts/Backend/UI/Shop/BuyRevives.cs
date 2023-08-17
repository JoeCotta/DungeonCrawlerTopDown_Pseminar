using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyRevives : MonoBehaviour
{
    public DataPersistenceManager dataPersistenceManager;
    public ShopMenu shopMenu;
    public TextMeshProUGUI text;

    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").GetComponent<ShopMenu>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "Onetime Revive\r\n"+ shopMenu.priceRevive + " Coins" + "\r\n left: " + dataPersistenceManager.gameData.revivesLeft.ToString();
    }
}

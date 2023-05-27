using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MaxHealthUp : MonoBehaviour
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
        text.text = "Max Health Up\r\n" + shopMenu.priceMaxHealth + " Coins"  +"\r\n lvl: " + dataPersistenceManager.gameData.maxHealthlvl.ToString() + " " + dataPersistenceManager.gameData.maxHealth.ToString();
    }
}

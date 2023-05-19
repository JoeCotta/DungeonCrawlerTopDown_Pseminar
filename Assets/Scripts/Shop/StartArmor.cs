using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartArmor : MonoBehaviour
{
    public DataPersistenceManager dataPersistenceManager;
    public TextMeshProUGUI text;

    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        text = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "Starting Armor\r\n15 Coins \r\n lvl: " + dataPersistenceManager.gameData.startArmor.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MaxHealthUp : MonoBehaviour
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
        text.text = "Max Health Up\r\n10 Coins \r\n lvl: " + dataPersistenceManager.gameData.maxHealthlvl.ToString() + " " + dataPersistenceManager.gameData.maxHealth.ToString();
    }
}

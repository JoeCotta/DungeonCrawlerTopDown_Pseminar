using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyRevives : MonoBehaviour
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
        text.text = "Onetime Revive\r\n50 Coins \r\n left: " + dataPersistenceManager.gameData.revivesLeft.ToString();
    }
}

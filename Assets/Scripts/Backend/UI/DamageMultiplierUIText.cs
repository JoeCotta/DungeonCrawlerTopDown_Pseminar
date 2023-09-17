using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DamageMultiplierUIText : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>()) dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        dataPersistenceManager.gameData.currentDamageMultiplier = dataPersistenceManager.gameData.permanentDamageMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if(dataPersistenceManager.gameData.permanentDamageMultiplier <= dataPersistenceManager.gameData.currentDamageMultiplier) text.text = dataPersistenceManager.gameData.currentDamageMultiplier.ToString() + "X";
        else dataPersistenceManager.gameData.currentDamageMultiplier = dataPersistenceManager.gameData.permanentDamageMultiplier;
    }
}

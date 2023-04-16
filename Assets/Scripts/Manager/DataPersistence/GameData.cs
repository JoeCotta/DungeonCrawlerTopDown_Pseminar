using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;

    //Player Data
    public float currentCoins;

    public GameData()
    {
        this.deathCount = 0;
        currentCoins = 0;
        Debug.Log("deleted");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour,IDataPersistence
{
    private int deathCount = 0;

    private void Start() {
        IncreaseDummyInt();
    }
    
    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = this.deathCount;
    }


    private void IncreaseDummyInt()
    {
        deathCount++;
    }
}

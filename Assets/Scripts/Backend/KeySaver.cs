using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySaver : MonoBehaviour, IDataPersistence
{
    private bool susMode;
    private string[] pressedKeys;

    private void Start()
    {
        pressedKeys = new string[6];
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            for(int i = 5; i > 0; i--)
            {
                pressedKeys[i] = pressedKeys[i - 1];
            }
            pressedKeys[0] = Input.inputString;
            CheckCode();
        }
    }
    
    void CheckCode()
    {
        string code = "";
        for(int i = 5; i >= 0; i--) code += pressedKeys[i];

        if (code == "amogus") susMode = !susMode;
    }

    public void LoadData(GameData data)
    {
        this.susMode = data.susMode;
    }

    public void SaveData(ref GameData data)
    {
        data.susMode = this.susMode;
        Debug.Log(data.susMode);
    }
}

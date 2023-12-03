using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySaver : MonoBehaviour, IDataPersistence
{
    private bool susMode;
    private string[] pressedKeys;
    public GameObject cheatWeapons;

    private void Start()
    {
        pressedKeys = new string[8];
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            for(int i = 7; i > 0; i--)
            {
                pressedKeys[i] = pressedKeys[i - 1];
            }
            pressedKeys[0] = Input.inputString;
            CheckCodeSus();
            CheckCodeCheat();
        }
    }
    
    void CheckCodeSus()
    {
        if (!GameManager.PublicGameManager.isMenu) return;
        string code = "";
        for(int i = 7; i >= 0; i--) code += pressedKeys[i];

        if (code == "20.12.22") susMode = !susMode;
    }

    void CheckCodeCheat()
    {
        if (GameManager.PublicGameManager.isMenu) return;
        string code = "";
        for (int i = 5; i >= 0; i--) code += pressedKeys[i];

        if (code == "amogus") Instantiate(cheatWeapons, transform);
    }

    public void LoadData(GameData data)
    {
        this.susMode = data.susMode;
    }

    public void SaveData(ref GameData data)
    {
        data.susMode = this.susMode;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KeyBindingsButton : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonLable;
    [SerializeField] public String playerPrefVarName;

    void Start()
    {
        buttonLable.text = PlayerPrefs.GetString(playerPrefVarName);
    }

    void Update()
    {
        if(buttonLable.text == "Awaiting Input")
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) 
            {
                if (Input.GetKey(keyCode))
                {
                    buttonLable.text = keyCode.ToString();
                    PlayerPrefs.SetString(playerPrefVarName, keyCode.ToString());
                    PlayerPrefs.Save();
                }
            }

        }
    }

    public void ChangeKey()
    {
        buttonLable.text = "Awaiting Input";
    }
}

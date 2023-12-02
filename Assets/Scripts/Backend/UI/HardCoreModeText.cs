using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HardCoreModeText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        updateText(); 
    }

    public void updateText()
    {
        text.text = "Hardcore-Mode: ";
        if (GameManager.hardcoreMode) text.text += "Active";
        else text.text += "Off";
    }
}

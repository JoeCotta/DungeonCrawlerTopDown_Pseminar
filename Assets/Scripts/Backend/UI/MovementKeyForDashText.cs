using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementKeyForDashText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        updateText();
    }

    public void updateText()
    {
        text.text = "Use Movement-Keys Dash: ";
        if (GameManager.movementKeysForDash) text.text += "Active";
        else text.text += "Off";
    }
}

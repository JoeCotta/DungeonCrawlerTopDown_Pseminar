using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimLineText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private void Awake()
    {
        updateText();
    }

    public void updateText()
    {
        text.text = "Aimline: \n";
        if (GameManager.aimLine) text.text += "Active";
        else text.text += "Off";
    }
}

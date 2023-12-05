using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    void Update()
    {
        text.text = DifficultyTracker.difficultyLevel;
    }
}

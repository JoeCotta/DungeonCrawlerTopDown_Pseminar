using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class grenadeCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    GranadeThrowing grenadeManager;

    void Start()
    {
        grenadeManager = GameObject.FindWithTag("Player").GetComponent<GranadeThrowing>();
    }

    void Update()
    {
        text.text = grenadeManager.grenades.ToString() + "X";
    }
}

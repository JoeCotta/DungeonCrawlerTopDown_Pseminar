using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    [HideInInspector] public bool isBoss;
    [HideInInspector] public Boss boss;

    // Update is called once per frame
    void Update()
    {
        if (!isBoss) return;

        float health = boss.currentHealth;
        slider.value = health / boss.maxHealth;
        text.text = Mathf.Round(health).ToString();
    }
}

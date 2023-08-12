using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownSlider : MonoBehaviour
{
    public Slider slider;
    public GameObject fill;
    public bool hideWhenFull;
    void Update()
    {
        if (GameManager.player == null) return;
        slider.value = 1 - GameManager.player.dashCooldownLeft / GameManager.player.dashCooldown;

        if (slider.value == 1 && hideWhenFull) fill.SetActive(false);
        else if (hideWhenFull)fill.SetActive(true);
    }
}

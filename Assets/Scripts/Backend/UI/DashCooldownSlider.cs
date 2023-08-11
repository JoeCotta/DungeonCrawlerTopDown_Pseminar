using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownSlider : MonoBehaviour
{
    public Slider slider;
    void Update()
    {
        if (GameManager.player == null) return;
        slider.value = 1 - GameManager.player.dashCooldownLeft / GameManager.player.dashCooldown;
    }
}

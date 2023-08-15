using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    private AlternateWS ws;
    public GameObject fill;
    void Update()
    {
        if (!GameManager.player) return;
        if(!GameManager.player.GetComponent<Player>().weapon) { fill.SetActive(false); return; }
        ws = GameManager.player.GetComponent<Player>().weapon.GetComponent<AlternateWS>();
        if (ws.rTimer / ws.rTime < 1) { fill.SetActive(true); gameObject.GetComponent<Slider>().value = ws.rTimer / ws.rTime; }
        else fill.SetActive(false);
    }
}

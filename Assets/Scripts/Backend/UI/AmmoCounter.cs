using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        Invoke("References", 1f);
    }
    void FixedUpdate()
    {
        if (player && GameManager.player.weapon != null && player.weapon.GetComponent<AlternateWS>()) gameObject.GetComponent<TextMeshProUGUI>().text = player.weapon.GetComponent<AlternateWS>().GetAmmo().x.ToString() + "/" + player.weapon.GetComponent<AlternateWS>().GetAmmo().y.ToString();
        else gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }

    void References()
    {
        player = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().returnPlayer();
    }
}
